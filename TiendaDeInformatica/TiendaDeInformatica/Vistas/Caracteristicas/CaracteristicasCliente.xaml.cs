using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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
        private bool _tipoCliente { get; set; }

        public CaracteristicasCliente(Principal principal, Cliente clienteModificar, bool ejecutarOscurecerPantallaPrincipalAlCerrar, bool? tipoCliente)
        {
            InitializeComponent();
            this.DataContext = this;

            _principal = principal;
            _clienteModificar = clienteModificar;

            _ejecutarOscurecerPantallaPrincipalAlCerrar = ejecutarOscurecerPantallaPrincipalAlCerrar;

            if(tipoCliente!=null)
                _tipoCliente = (bool)tipoCliente;
        }

        private void CaracteristicasCliente_Vista_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Verificar si se va a crear o a modificar un cliente
                if (_clienteModificar != null)
                {
                    // Cambiar el título y el botón
                    Titulo_TextBlock.Text = "Modificar cliente";
                    AgregarModificar_Button.Content = "MODIFICAR";
                    AgregarModificarClienteDuplicado_Button.Content = "MODIFICAR IGUAL";

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
                else if (!_tipoCliente)
                    Empresa_RadioButton.IsChecked = true;

                _principal.OscurecerCompletamente(true);
                Contenido_DialogHost.IsOpen = true;
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        // ------------------------------------------------------ //
        //              Agregar o modificar un cliente            //
        // ------------------------------------------------------ //

        private void AgregarModificar_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ObtenerResultadoReglasDeValidacion())
                {
                    // No hay errores
                    bool clienteDuplicado = false;
                    string nombre;
                    List<Cliente> clientes = new List<Cliente>();

                    if (Persona_RadioButton.IsChecked.Value)
                    {
                        nombre = Nombre_TextBox.Text.ToUpper() + " " + Apellido_TextBox_Text.ToUpper();
                        clientes = ControladorClientes.ObtenerListaDeClientes().Where(c => c.Tipo == "Persona").ToList();
                        Titulo_AlertaClienteDuplicado_TextBlock.Text = "Se encontró un cliente con el mismo nombre y apellido";
                    }
                    else
                    {
                        nombre = NombreDeLaEmpresa_TextBox.Text.ToUpper();
                        clientes = ControladorClientes.ObtenerListaDeClientes().Where(c => c.Tipo == "Empresa").ToList();
                        Titulo_AlertaClienteDuplicado_TextBlock.Text = "Se encontró una empresa con el mismo nombre";
                    }

                    nombre = QuitarTildes(nombre);
                    foreach (Cliente cliente in clientes)
                    {
                        if (((_clienteModificar != null) && (_clienteModificar.Id != cliente.Id) && (QuitarTildes(cliente.MostrarNombre).ToUpper() == nombre))
                            || (_clienteModificar == null && (QuitarTildes(cliente.MostrarNombre).ToUpper() == nombre)))
                        {
                            clienteDuplicado = true;
                            AlertaClienteDuplicado_Dialog.IsOpen = true;
                            break;
                        }
                    }

                    if (!clienteDuplicado)
                        AgregarModificarCliente();
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
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private bool ObtenerResultadoReglasDeValidacion()
        {
            try
            {
                CultureInfo currentCulture = CultureInfo.CurrentCulture;
                if (((Empresa_RadioButton.IsChecked == true && new CampoVacio().Validate(NombreDeLaEmpresa_TextBox.Text, currentCulture) == new ValidationResult(true, null)) || Empresa_RadioButton.IsChecked == false)
                    && new SoloLetras().Validate(Nombre_TextBox.Text, currentCulture) == new ValidationResult(true, null)
                    && new SoloLetras().Validate(Apellido_TextBox.Text, currentCulture) == new ValidationResult(true, null)
                    && new Telefono().Validate(Telefono_TextBox.Text, currentCulture) == new ValidationResult(true, null)
                    && new CUIT().Validate(CUIT_TextBox.Text, currentCulture) == new ValidationResult(true, null))
                {
                    // No hay errores
                    return true;
                }
                // Hay errores
                return false;
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        public string QuitarTildes(string texto)
        {
            try
            {
                return new String(texto.Normalize(NormalizationForm.FormD).Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray()).Normalize(NormalizationForm.FormC);
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        // ------------------------------------------------------ //
        //        Agregar o modificar cliente duplicado           //
        // ------------------------------------------------------ //

        private void AgregarModificarClienteDuplicado_Button_Click(object sender, RoutedEventArgs e)
        {
            AlertaClienteDuplicado_Dialog.IsOpen = false;
            AgregarModificarCliente();
        }

        private void AgregarModificarCliente()
        {
            try
            {
                if (_clienteModificar == null)
                {
                    // Crear cliente
                    if (Persona_RadioButton.IsChecked == true)
                        ControladorClientes.AgregarCliente(null, Nombre_TextBox.Text, Apellido_TextBox.Text, Telefono_TextBox.Text, CUIT_TextBox.Text);
                    else
                        ControladorClientes.AgregarCliente(NombreDeLaEmpresa_TextBox.Text, Nombre_TextBox.Text, Apellido_TextBox.Text, Telefono_TextBox.Text, CUIT_TextBox.Text);
                    _ = _principal.MostrarMensajeEnSnackbar("Cliente agregado correctamente!");
                }
                else
                {
                    // Modificar cliente
                    if (Persona_RadioButton.IsChecked == true)
                        ControladorClientes.ModificarCliente(_clienteModificar, null, Nombre_TextBox.Text, Apellido_TextBox.Text, Telefono_TextBox.Text, CUIT_TextBox.Text);
                    else
                        ControladorClientes.ModificarCliente(_clienteModificar, NombreDeLaEmpresa_TextBox.Text, Nombre_TextBox.Text, Apellido_TextBox.Text, Telefono_TextBox.Text, CUIT_TextBox.Text);
                    _ = _principal.MostrarMensajeEnSnackbar("Cliente modificado correctamente!");
                }
                CerrarVentana();
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        // ------------------------------------------------------ //
        //                    Cerrar ventana                      //
        // ------------------------------------------------------ //

        private void Cancelar_Button_Click(object sender, RoutedEventArgs e)
        {
            VerificarCambiosAlCerrar();
        }

        private async void CerrarVentana()
        {
            if (_ejecutarOscurecerPantallaPrincipalAlCerrar)
                _principal.OscurecerCompletamente(false);
            Contenido_DialogHost.IsOpen = false;
            await Task.Delay(300);
            this.Close();
        }

        // ------------------------------------------------------ //
        //   Alerta al cerrar la ventana sin guardar los cambios  //
        // ------------------------------------------------------ //

        private void VerificarCambiosAlCerrar()
        {
            try
            {
                if (_clienteModificar == null)
                {
                    // Crear Cliente
                    if (NombreDeLaEmpresa_TextBox.Text != "" || Nombre_TextBox.Text != "" || Apellido_TextBox.Text != ""
                        || Telefono_TextBox.Text != "" || CUIT_TextBox.Text != "")
                    {
                        // Se realizaron cambios, y se mostrará la alerta
                        AlertaAlCerrar_Dialog.IsOpen = true;
                    }
                    else
                        CerrarVentana();
                }
                else
                {
                    // Modificar Cliente
                    if ((NombreDeLaEmpresa_TextBox.Text != "" && _clienteModificar.NombreDeLaEmpresa == null)
                        || (NombreDeLaEmpresa_TextBox.Text != _clienteModificar.NombreDeLaEmpresa && _clienteModificar.Tipo == "Empresa")
                        || (Nombre_TextBox.Text != _clienteModificar.Nombre)
                        || (Apellido_TextBox.Text != _clienteModificar.Apellido)
                        || (Telefono_TextBox.Text != "" && _clienteModificar.Telefono == null)
                        || (Telefono_TextBox.Text != _clienteModificar.Telefono)
                        || (CUIT_TextBox.Text != "" && _clienteModificar.CUIT == null)
                        || (CUIT_TextBox.Text != _clienteModificar.CUIT))
                    {
                        // Se realizaron cambios, y se mostrará la alerta
                        AlertaAlCerrar_Dialog.IsOpen = true;
                    }
                    else
                        CerrarVentana();
                }
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private void CerrarIgual_Button_Click(object sender, RoutedEventArgs e)
        {
            AlertaAlCerrar_Dialog.IsOpen = false;
            CerrarVentana();
        }
    }
}
