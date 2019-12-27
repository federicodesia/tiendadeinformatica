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
    /// Lógica de interacción para CaracteristicasCliente.xaml
    /// </summary>
    public partial class CaracteristicasCliente : Window
    {
        private Principal _principal { get; set; }
        private Cliente _clienteModificar { get; set; }

        public string NombreDeLaEmpresa_TextBox_Text { get; set; }
        public string Nombre_TextBox_Text { get; set; }
        public string Apellido_TextBox_Text { get; set; }
        public string Telefono_TextBox_Text { get; set; }
        public string CUIT_TextBox_Text { get; set; }

        private bool _ejecutarOscurecerPantallaPrincipalAlCerrar { get; set; }

        public CaracteristicasCliente(Principal principal, Cliente clienteModificar, bool ejecutarOscurecerPantallaPrincipalAlCerrar)
        {
            InitializeComponent();
            this.DataContext = this;

            _principal = principal;
            _clienteModificar = clienteModificar;

            _ejecutarOscurecerPantallaPrincipalAlCerrar = ejecutarOscurecerPantallaPrincipalAlCerrar;
        }

        private void CaracteristicasCliente_Vista_Loaded(object sender, RoutedEventArgs e)
        {
            // Verificar si se va a crear o a modificar un cliente
            if (_clienteModificar != null)
            {
                // Cambiar el título y el botón
                Titulo_TextBlock.Text = "Modificar cliente";
                CrearModificar_Button.Content = "MODIFICAR";

                if (_clienteModificar.Tipo == "Empresa")
                {
                    // Seleccionar la pestaña y completar en nombre de la empresa
                    Empresa_RadioButton.IsChecked = true;
                    NombreDeLaEmpresa_TextBox.Text = _clienteModificar.NombreDeLaEmpresa;
                }

                // Completar los datos del cliente
                Nombre_TextBox.Text = _clienteModificar.Nombre;
                Apellido_TextBox.Text = _clienteModificar.Apellido;
                Telefono_TextBox.Text = _clienteModificar.Telefono;
                CUIT_TextBox.Text = _clienteModificar.CUIT;
            }

            _principal.OscurecerCompletamente(true);
            Contenido_DialogHost.IsOpen = true;
        }

        //
        // Cerrar ventana
        //

        private void Cancelar_Button_Click(object sender, RoutedEventArgs e)
        {
            MostrarAlerta();
        }

        private void MostrarAlerta()
        {
            if (_clienteModificar == null)
            {
                // Crear Cliente
                if (NombreDeLaEmpresa_TextBox_Text != null || Nombre_TextBox_Text != null || Apellido_TextBox_Text != null
                    || Telefono_TextBox_Text != null || CUIT_TextBox_Text != null)
                {
                    // Se realizaron cambios, y se mostrará la alerta
                    AlertaAlCerrar_Dialog.IsOpen = true;
                }
                else
                {
                    // No se realizaron cambios, y se cerrará
                    CerrarVentana();
                }
            }
            else
            {
                // Modificar Cliente
                if ((NombreDeLaEmpresa_TextBox_Text != null && _clienteModificar.NombreDeLaEmpresa == null)
                    || (NombreDeLaEmpresa_TextBox_Text != _clienteModificar.NombreDeLaEmpresa && _clienteModificar.Tipo == "Empresa")
                    || (Nombre_TextBox_Text != _clienteModificar.Nombre)
                    || (Apellido_TextBox_Text != _clienteModificar.Apellido)
                    || (Telefono_TextBox_Text != null && _clienteModificar.Telefono == null)
                    || (Telefono_TextBox_Text != _clienteModificar.Telefono)
                    || (CUIT_TextBox_Text != null && _clienteModificar.CUIT == null)
                    || (CUIT_TextBox_Text != _clienteModificar.CUIT))
                {
                    // Se realizaron cambios, y se mostrará la alerta
                    AlertaAlCerrar_Dialog.IsOpen = true;
                }
                else
                {
                    // No se realizaron cambios, y se cerrará
                    CerrarVentana();
                }
            }
        }

        private async void CerrarVentana()
        {
            if (_ejecutarOscurecerPantallaPrincipalAlCerrar)
            {
                _principal.OscurecerCompletamente(false);
            }
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
        // Crear o Modificar cliente
        //

        private void CrearModificar_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ObtenerResultadoReglasDeValidacion())
            {
                // No hay errores
                if (_clienteModificar == null)
                {
                    // Crear cliente
                    if (Persona_RadioButton.IsChecked == true)
                    {
                        // Persona
                        ControladorClientes.AgregarCliente(null, Nombre_TextBox_Text, Apellido_TextBox_Text, Telefono_TextBox_Text, CUIT_TextBox_Text);
                    }
                    else
                    {
                        // Empresa
                        ControladorClientes.AgregarCliente(NombreDeLaEmpresa_TextBox_Text, Nombre_TextBox_Text, Apellido_TextBox_Text, Telefono_TextBox_Text, CUIT_TextBox_Text);
                    }
                    _ = _principal.MostrarMensajeEnSnackbar("Cliente agregado correctamente!");
                }
                else
                {
                    // Modificar cliente
                    if (Persona_RadioButton.IsChecked == true)
                    {
                        // Persona
                        ControladorClientes.ModificarCliente(_clienteModificar, null, Nombre_TextBox_Text, Apellido_TextBox_Text, Telefono_TextBox_Text, CUIT_TextBox_Text);
                    }
                    else
                    {
                        // Empresa
                        ControladorClientes.ModificarCliente(_clienteModificar, NombreDeLaEmpresa_TextBox_Text, Nombre_TextBox_Text, Apellido_TextBox_Text, Telefono_TextBox_Text, CUIT_TextBox_Text);
                    }
                    _ = _principal.MostrarMensajeEnSnackbar("Cliente modificado correctamente!");
                }
                CerrarVentana();
            }
            else
            {
                // Hay errores. Actualizar los mensajes de error
                NombreDeLaEmpresa_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                Nombre_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                Apellido_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                Telefono_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                CUIT_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }

        private bool ObtenerResultadoReglasDeValidacion()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            if (((Empresa_RadioButton.IsChecked == true && new CampoVacio().Validate(NombreDeLaEmpresa_TextBox_Text, currentCulture) == new ValidationResult(true, null)) || Empresa_RadioButton.IsChecked == false)
                && new SoloLetras().Validate(Nombre_TextBox_Text, currentCulture) == new ValidationResult(true, null)
                && new SoloLetras().Validate(Apellido_TextBox_Text, currentCulture) == new ValidationResult(true, null)
                && new Telefono().Validate(Telefono_TextBox_Text, currentCulture) == new ValidationResult(true, null)
                && new CUIT().Validate(CUIT_TextBox_Text, currentCulture) == new ValidationResult(true, null))
            {
                // No hay errores
                return true;
            }
            // Hay errores
            return false;
        }
    }
}
