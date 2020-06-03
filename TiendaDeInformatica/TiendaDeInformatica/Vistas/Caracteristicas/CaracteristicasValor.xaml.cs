using System.Globalization;
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
    /// Lógica de interacción para CaracteristicasMarca.xaml
    /// </summary>
    public partial class CaracteristicasValor : Window
    {
        private Atributo _atributo { get; set; }
        private Valor _valorModificar { get; set; }

        private CaracteristicasProducto _caracteristicasProducto { get; set; }

        public string Nombre_TextBox_Text { get; set; }

        public CaracteristicasValor(Atributo atributo, Valor valor, CaracteristicasProducto caracteristicasProducto)
        {
            InitializeComponent();
            this.DataContext = this;

            _valorModificar = valor;
            _atributo = atributo;
            _caracteristicasProducto = caracteristicasProducto;
        }

        private void CaracteristicasValor_Vista_Loaded(object sender, RoutedEventArgs e)
        {
            // Verificar si se va a crear o a modificar un valor
            if (_valorModificar != null)
            {
                // Cambiar el título y el botón
                Titulo_TextBlock.Text = "Modificar valor del atributo " + _valorModificar.Atributo.Nombre;
                AgregarModificar_Button.Content = "MODIFICAR";

                // Cargar los datos del valor
                Nombre_TextBox.Text = _valorModificar.Nombre;
            }
            Contenido_DialogHost.IsOpen = true;
        }

        // ------------------------------------------------------ //
        //              Agregar o Modificar un valor              //
        // ------------------------------------------------------ //

        private void AgregarModificar_Button_Click(object sender, RoutedEventArgs e)
        {
            if (new CampoVacio().Validate(Nombre_TextBox_Text, CultureInfo.CurrentCulture) == new ValidationResult(true, null))
            {
                // No hay errores
                bool valorDuplicado = false;
                string nombre = TextHelper.QuitarTildes(Nombre_TextBox.Text).ToUpper();

                foreach (Valor valor in _atributo.Valores)
                {
                    if (((_valorModificar != null) && (_valorModificar.Id != valor.Id) && (TextHelper.QuitarTildes(valor.Nombre).ToUpper() == nombre))
                        || (_valorModificar == null && (TextHelper.QuitarTildes(valor.Nombre).ToUpper() == nombre)))
                    {
                        valorDuplicado = true;
                        break;
                    }
                }

                if (!valorDuplicado)
                    AgregarModificarValor();
            }
            else
            {
                // Hay errores. Actualizar los mensajes de error
                Nombre_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }

        private void AgregarModificarValor()
        {
            if (_valorModificar == null)
                ControladorAtributos.AgregarValor(_atributo, Nombre_TextBox.Text);
            else
                ControladorAtributos.ModificarValor(_valorModificar, Nombre_TextBox.Text);
            CerrarVentana();
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

            Contenido_DialogHost.IsOpen = false;
            await Task.Delay(300);
            this.Close();
        }
    }
}
