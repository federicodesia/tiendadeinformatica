﻿using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TiendaDeInformatica.Controladores;
using TiendaDeInformatica.Helpers;
using TiendaDeInformatica.Modelos;
using TiendaDeInformatica.Vistas.Reglas_de_Validacion;

namespace TiendaDeInformatica.Vistas.Caracteristicas
{
    /// <summary>
    /// Lógica de interacción para CaracteristicasMarca.xaml
    /// </summary>
    public partial class CaracteristicasMarca : Window
    {
        private Principal _principal { get; set; }
        private Marca _marcaModificar { get; set; }

        public string Nombre_TextBox_Text { get; set; }
        public byte[] ImagenSeleccionada { get; set; }

        public CaracteristicasMarca(Principal principal, Marca marcaModificar)
        {
            InitializeComponent();
            this.DataContext = this;

            _principal = principal;
            _marcaModificar = marcaModificar;
        }

        private void CaracteristicasMarca_Vista_Loaded(object sender, RoutedEventArgs e)
        {
            // Verificar si se va a crear o a modificar una marca
            if (_marcaModificar != null)
            {
                // Cambiar el título y el botón
                Titulo_TextBlock.Text = "Modificar marca";
                AgregarModificar_Button.Content = "MODIFICAR";
                AgregarModificarMarcaDuplicada_Button.Content = "MODIFICAR IGUAL";

                // Cargar los datos de la marca
                Nombre_TextBox.Text = _marcaModificar.Nombre;
                if (_marcaModificar.Imagen != null)
                {
                    ImagenSeleccionada = _marcaModificar.Imagen;
                    Imagen_Image.Source = ConvertirImagen.ConvertByteArrayToImage(ImagenSeleccionada);
                }
            }

            _principal.OscurecerCompletamente(true);
            Contenido_DialogHost.IsOpen = true;
        }

        // ------------------------------------------------------ //
        //              Agregar o Modificar una marca             //
        // ------------------------------------------------------ //

        private void AgregarModificar_Button_Click(object sender, RoutedEventArgs e)
        {
            if (new CampoVacio().Validate(Nombre_TextBox_Text, CultureInfo.CurrentCulture) == new ValidationResult(true, null))
            {
                // No hay errores
                bool marcaDuplicada = false;
                string nombre = QuitarTildes(Nombre_TextBox.Text).ToUpper();

                foreach (Marca marca in ControladorMarcas.ObtenerListaDeMarcas())
                {
                    if (QuitarTildes(marca.Nombre).ToUpper() == nombre)
                    {
                        marcaDuplicada = true;
                        AlertaMarcaDuplicada_Dialog.IsOpen = true;
                        break;
                    }
                }

                if (!marcaDuplicada)
                    AgregarModificarMarca();
            }
            else
            {
                // Hay errores. Actualizar los mensajes de error
                Nombre_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }

        public string QuitarTildes(string texto)
        {
            return new String(texto.Normalize(NormalizationForm.FormD).Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray()).Normalize(NormalizationForm.FormC);
        }

        private void AgregarModificarMarca()
        {
            if (_marcaModificar == null)
            {
                // Crear marca
                ControladorMarcas.AgregarMarca(Nombre_TextBox.Text, ImagenSeleccionada);
                _ = _principal.MostrarMensajeEnSnackbar("Marca agregada correctamente!");
            }
            else
            {
                // Modificar marca
                ControladorMarcas.ModificarMarca(_marcaModificar, Nombre_TextBox.Text, ImagenSeleccionada);
                _ = _principal.MostrarMensajeEnSnackbar("Marca modificada correctamente!");
            }
            CerrarVentana();
        }

        // ------------------------------------------------------ //
        //          Agregar o modificar marca duplicada           //
        // ------------------------------------------------------ //

        private void AgregarModificarMarcaDuplicada_Button_Click(object sender, RoutedEventArgs e)
        {
            AlertaMarcaDuplicada_Dialog.IsOpen = false;
            AgregarModificarMarca();
        }

        // ------------------------------------------------------ //
        //                     Cerrar ventana                     //
        // ------------------------------------------------------ //

        private void Cancelar_Button_Click(object sender, RoutedEventArgs e)
        {
            if (_marcaModificar == null)
            {
                // Se iba a crear una marca
                if (Nombre_TextBox.Text != "" || ImagenSeleccionada != null)
                {
                    // Se realizaron cambios
                    AlertaAlCerrar_Dialog.IsOpen = true;
                }
                else
                    CerrarVentana();
            }
            else
            {
                // Se iba a modificar una marca
                if ((Nombre_TextBox.Text != _marcaModificar.Nombre)
                    || (_marcaModificar.Imagen != ImagenSeleccionada))
                {
                    // Se realizaron cambios
                    AlertaAlCerrar_Dialog.IsOpen = true;
                }
                else
                    CerrarVentana();
            }
        }

        private async void CerrarVentana()
        {
            _principal.OscurecerCompletamente(false);
            Contenido_DialogHost.IsOpen = false;
            await Task.Delay(300);
            this.Close();
        }

        // ------------------------------------------------------ //
        //        Alerta al cerrar sin guardar los cambios        //
        // ------------------------------------------------------ //

        private void CerrarIgual_Button_Click(object sender, RoutedEventArgs e)
        {
            AlertaAlCerrar_Dialog.IsOpen = false;
            CerrarVentana();
        }

        // ------------------------------------------------------ //
        //                 Seleccionar una imagen                //
        // ------------------------------------------------------ //

        private void SeleccionarImagen_Button_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "PNG (*.png)|*.png";
            var result = openFileDialog.ShowDialog();

            if (result == false) return;

            Imagen_Image.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            ImagenSeleccionada = ConvertirImagen.ConvertImageToByteArray(openFileDialog.FileName);
        }
    }
}
