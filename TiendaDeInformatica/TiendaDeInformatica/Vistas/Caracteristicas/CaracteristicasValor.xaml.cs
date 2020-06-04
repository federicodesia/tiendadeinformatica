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
    /// Lógica de interacción para CaracteristicasMarca.xaml
    /// </summary>
    public partial class CaracteristicasValor : Window
    {
        public static Atributo _atributo { get; set; }
        public static Valor _valorModificar { get; set; }

        private Principal _principal { get; set; }
        private CaracteristicasProducto _caracteristicasProducto { get; set; }
        private Configuracion _configuracion { get; set; }

        public string Nombre_TextBox_Text { get; set; }

        public CaracteristicasValor(Principal principal, CaracteristicasProducto caracteristicasProducto, Configuracion configuracion, Atributo atributo, Valor valor)
        {
            InitializeComponent();
            this.DataContext = this;

            _principal = principal;
            _caracteristicasProducto = caracteristicasProducto;
            _configuracion = configuracion;
            _valorModificar = valor;
            _atributo = atributo;
        }

        private void CaracteristicasValor_Vista_Loaded(object sender, RoutedEventArgs e)
        {
            // Verificar si se va a crear o a modificar un valor
            if (_valorModificar != null)
            {
                // Cambiar el título y el botón
                Titulo_TextBlock.Text = "Modificar valor del atributo " + _atributo.Nombre;
                AgregarModificar_Button.Content = "MODIFICAR";

                // Cargar los datos del valor
                Nombre_TextBox.Text = _valorModificar.Nombre;
            }
            else
                Titulo_TextBlock.Text = "Agregar valor al atributo " + _atributo.Nombre;

            if (_configuracion != null)
                _configuracion.OscurecerFondoValores(true);
            Contenido_DialogHost.IsOpen = true;
        }

        // ------------------------------------------------------ //
        //              Agregar o Modificar un valor              //
        // ------------------------------------------------------ //

        private void AgregarModificar_Button_Click(object sender, RoutedEventArgs e)
        {
            if (new ValorValidationRule().Validate(Nombre_TextBox.Text, CultureInfo.CurrentCulture) == new ValidationResult(true, null))
            {
                if (_valorModificar == null)
                {
                    ControladorAtributos.AgregarValor(_atributo, Nombre_TextBox.Text);
                    _ = _principal.MostrarMensajeEnSnackbar("Valor agregado correctamente!");
                }
                else
                {
                    ControladorAtributos.ModificarValor(_valorModificar, Nombre_TextBox.Text);
                    _ = _principal.MostrarMensajeEnSnackbar("Valor modificado correctamente!");
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
            if (_caracteristicasProducto != null)
                _caracteristicasProducto.OscurecerFondo(false);
            else if (_configuracion != null)
                _configuracion.OscurecerFondoValores(false);

            Contenido_DialogHost.IsOpen = false;
            await Task.Delay(300);
            this.Close();
        }
    }
}
