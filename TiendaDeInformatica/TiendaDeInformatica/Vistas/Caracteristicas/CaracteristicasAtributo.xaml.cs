using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TiendaDeInformatica.Controladores;
using TiendaDeInformatica.Modelos;
using TiendaDeInformatica.Vistas.Reglas_de_Validacion;

namespace TiendaDeInformatica.Vistas.Caracteristicas
{
    /// <summary>
    /// Lógica de interacción para CaracteristicasAtributo2.xaml
    /// </summary>
    public partial class CaracteristicasAtributo : Window
    {
        public static Atributo _atributoModificar { get; set; }
        private Principal _principal { get; set; }
        private Configuracion _configuracion { get; set; }

        public string Nombre_TextBox_Text { get; set; }

        public CaracteristicasAtributo(Principal principal, Configuracion configuracion, Atributo atributo)
        {
            InitializeComponent();
            this.DataContext = this;

            _principal = principal;
            _atributoModificar = atributo;
            _configuracion = configuracion;
        }

        private void CaracteristicasAtributo_Vista_Loaded(object sender, RoutedEventArgs e)
        {
            // Verificar si se va a crear o a modificar un atributo
            if (_atributoModificar != null)
            {
                // Cambiar el título y el botón
                Titulo_TextBlock.Text = "Modificar el atributo " + _atributoModificar.Nombre;
                AgregarModificar_Button.Content = "MODIFICAR";

                // Cargar los datos del valor
                Nombre_TextBox.Text = _atributoModificar.Nombre;
            }

            _configuracion.OscurecerFondoAtributos(true);
            Contenido_DialogHost.IsOpen = true;
        }

        // ------------------------------------------------------ //
        //             Agregar o Modificar un atributo            //
        // ------------------------------------------------------ //

        private void AgregarModificar_Button_Click(object sender, RoutedEventArgs e)
        {
            if (new AtributoValidationRule().Validate(Nombre_TextBox.Text, CultureInfo.CurrentCulture) == new ValidationResult(true, null))
            {
                if (_atributoModificar == null)
                {
                    ControladorAtributos.AgregarAtributo(Nombre_TextBox.Text);
                    _ = _principal.MostrarMensajeEnSnackbar("Atributo agregado correctamente!");
                }
                else
                {
                    ControladorAtributos.ModificarAtributo(_atributoModificar, Nombre_TextBox.Text);
                    _ = _principal.MostrarMensajeEnSnackbar("Atributo modificado correctamente!");
                }
                CerrarVentana();
            }
            else
            {
                // Hay errores. Actualizar los mensajes de error
                Nombre_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }

        // ------------------------------------------------------ //
        //                       Cerrar ventana                   //
        // ------------------------------------------------------ //

        private void Cancelar_Button_Click(object sender, RoutedEventArgs e)
        {
            CerrarVentana();
        }

        private async void CerrarVentana()
        {
            if (_configuracion != null)
                _configuracion.OscurecerFondoAtributos(false);

            Contenido_DialogHost.IsOpen = false;
            await Task.Delay(300);
            this.Close();
        }
    }
}
