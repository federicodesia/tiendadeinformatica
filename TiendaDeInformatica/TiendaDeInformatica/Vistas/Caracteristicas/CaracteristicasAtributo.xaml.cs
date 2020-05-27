using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TiendaDeInformatica.Controladores;
using TiendaDeInformatica.Helpers;
using TiendaDeInformatica.Modelos;
using TiendaDeInformatica.Vistas.Reglas_de_Validacion;

namespace TiendaDeInformatica.Vistas.Caracteristicas
{
    /// <summary>
    /// Lógica de interacción para CaracteristicasAtributo.xaml
    /// </summary>
    public partial class CaracteristicasAtributo : Window
    {
        private Principal _principal { get; set; }
        private Atributo _atributoModificar { get; set; }

        public string Nombre_TextBox_Text { get; set; }

        public CaracteristicasAtributo(Principal principal, Atributo atributo)
        {
            InitializeComponent();
            this.DataContext = this;

            _principal = principal;
            _atributoModificar = atributo;
        }

        private void CaracteristicasAtributo_Vista_Loaded(object sender, RoutedEventArgs e)
        {
            // Verificar si se va a crear o a modificar una marca
            if (_atributoModificar != null)
            {
                Titulo_TextBlock.Text = "Modificar marca";
                AgregarModificar_Button.Content = "MODIFICAR";

                Nombre_TextBox.Text = _atributoModificar.Nombre;
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
                bool atributoDuplicado = false;
                string nombre = TextHelper.QuitarTildes(Nombre_TextBox.Text).ToUpper();

                foreach (Atributo atributo in ControladorAtributos.ObtenerListaDeAtributos())
                {
                    if (((_atributoModificar != null) && (atributo.Id != _atributoModificar.Id) && (TextHelper.QuitarTildes(atributo.Nombre).ToUpper() == nombre))
                        || (_atributoModificar == null && (TextHelper.QuitarTildes(atributo.Nombre).ToUpper() == nombre)))
                    {
                        atributoDuplicado = true;
                        AlertaAtributoDuplicado_Dialog.IsOpen = true;
                        break;
                    }
                }

                if (!atributoDuplicado)
                    AgregarModificarAtributo();
            }
            else
            {
                // Hay errores. Actualizar los mensajes de error
                Nombre_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }

        private void AgregarModificarAtributo()
        {
            if (_atributoModificar == null)
            {
                // Crear atributo
                ControladorAtributos.AgregarAtributo(Nombre_TextBox.Text);
                _ = _principal.MostrarMensajeEnSnackbar("Atributo agregado correctamente!");
            }
            else
            {
                // Modificar atributo
                ControladorAtributos.ModificarAtributo(_atributoModificar, Nombre_TextBox.Text);
                _ = _principal.MostrarMensajeEnSnackbar("Atributo modificado correctamente!");
            }
            CerrarVentana();
        }

        // ------------------------------------------------------ //
        //                     Cerrar ventana                     //
        // ------------------------------------------------------ //

        private void Cancelar_Button_Click(object sender, RoutedEventArgs e)
        {
            if (_atributoModificar == null)
            {
                // Se iba a crear un atributo
                if (Nombre_TextBox.Text != "")
                    AlertaAlCerrar_Dialog.IsOpen = true;
                else
                    CerrarVentana();
            }
            else
            {
                // Se iba a modificar un atributo
                if (Nombre_TextBox.Text != _atributoModificar.Nombre)
                    AlertaAlCerrar_Dialog.IsOpen = true;
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
    }
}
