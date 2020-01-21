using System;
using System.Globalization;
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
                CrearModificar_Button.Content = "MODIFICAR";

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

        //
        // Cerrar
        //

        private void Cancelar_Button_Click(object sender, RoutedEventArgs e)
        {
            if (_marcaModificar == null)
            {
                // Crear Marca
                if (Nombre_TextBox.Text != "" || ImagenSeleccionada != null)
                {
                    // Se realizaron cambios
                    AlertaAlCerrar_Dialog.IsOpen = true;
                }
                else
                {
                    CerrarVentana();
                }
            }
            else
            {
                // Modificar Marca
                if ((Nombre_TextBox.Text != _marcaModificar.Nombre)
                    || (_marcaModificar.Imagen != ImagenSeleccionada))
                {
                    // Se realizaron cambios
                    AlertaAlCerrar_Dialog.IsOpen = true;
                }
                else
                {
                    CerrarVentana();
                }
            }
        }

        private async void CerrarVentana()
        {
            _principal.OscurecerCompletamente(false);
            Contenido_DialogHost.IsOpen = false;
            await Task.Delay(300);
            this.Close();
        }

        private void CerrarIgual_Button_Click(object sender, RoutedEventArgs e)
        {
            AlertaAlCerrar_Dialog.IsOpen = false;
            CerrarVentana();
        }

        //
        // Agregar o modificar marca
        //

        private void CrearModificar_Button_Click(object sender, RoutedEventArgs e)
        {
            if (new CampoVacio().Validate(Nombre_TextBox_Text, CultureInfo.CurrentCulture) == new ValidationResult(true, null))
            {
                // No hay errores
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
            else
            {
                // Hay errores. Actualizar los mensajes de error
                Nombre_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }

        //
        // Seleccionar imagen
        //

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
