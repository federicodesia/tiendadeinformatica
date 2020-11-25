using System;
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
    /// Lógica de interacción para CaracteristicasPresupuesto.xaml
    /// </summary>
    public partial class CaracteristicasPresupuesto : Window
    {
        private Principal _principal { get; set; }
        private Presupuesto _presupuestoModificar { get; set; }

        public Cliente BuscarCliente_ComboBox_SelectedItem { get; set; }
        public string FechaDeExpiracion_DatePicker_SelectedDate { get; set; }

        public CaracteristicasPresupuesto(Principal principal, Presupuesto presupuestoModificar)
        {
            InitializeComponent();
            this.DataContext = this;

            _principal = principal;
            _presupuestoModificar = presupuestoModificar;

            // No permitir ingresar fechas pasadas
            FechaDeExpiracion_DatePicker.BlackoutDates.AddDatesInPast();
        }

        private void CaracteristicasPresupuesto_Vista_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Verificar si se va a crear o a modificar un presupuesto
                if (_presupuestoModificar != null)
                {
                    // Cambiar el título y el botón
                    Titulo_TextBlock.Text = "Modificar presupuesto";
                    AgregarModificar_Button.Content = "MODIFICAR";

                    // Cargar el cliente del presupuesto
                    foreach (Cliente cliente in ControladorClientes.ObtenerListaDeClientes())
                    {
                        BuscarCliente_ComboBox.Items.Add(cliente);
                        if (cliente.Id == _presupuestoModificar.ClienteId)
                            BuscarCliente_ComboBox.SelectedItem = cliente as Cliente;
                    }

                    if (_presupuestoModificar.FechaDeExpiracion != null)
                    {
                        // Cargar, si tiene, la fecha de expiración del presupuesto
                        FechaDeExpiracion_CheckBox.IsChecked = true;
                        FechaDeExpiracion_DatePicker.SelectedDate = _presupuestoModificar.FechaDeExpiracion;
                    }
                }
                else
                    ActualizarListaDeClientes();

                _principal.OscurecerCompletamente(true);
                Contenido_DialogHost.IsOpen = true;
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        // ------------------------------------------------------ //
        //              Agregar o Modificar un presupuesto        //
        // ------------------------------------------------------ //

        private void AgregarModificar_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ObtenerResultadoReglasDeValidacion())
                {
                    // No hay errores
                    if (_presupuestoModificar == null)
                    {
                        // Crear cliente
                        if (FechaDeExpiracion_CheckBox.IsChecked == false)
                            ControladorPresupuestos.AgregarPresupuesto(BuscarCliente_ComboBox_SelectedItem, null);
                        else
                            ControladorPresupuestos.AgregarPresupuesto(BuscarCliente_ComboBox_SelectedItem, FechaDeExpiracion_DatePicker.SelectedDate.GetValueOrDefault());
                        _ = _principal.MostrarMensajeEnSnackbar("Presupuesto agregado correctamente!");
                    }
                    else
                    {
                        // Modificar cliente
                        if (FechaDeExpiracion_CheckBox.IsChecked == false)
                            ControladorPresupuestos.ModificarPresupuesto(_presupuestoModificar, BuscarCliente_ComboBox_SelectedItem, null);
                        else
                            ControladorPresupuestos.ModificarPresupuesto(_presupuestoModificar, BuscarCliente_ComboBox_SelectedItem, FechaDeExpiracion_DatePicker.SelectedDate.GetValueOrDefault());
                        _ = _principal.MostrarMensajeEnSnackbar("Presupuesto modificado correctamente!");
                    }
                    CerrarVentana();
                }
                else
                {
                    // Hay errores. Actualizar los mensajes de error
                    BuscarCliente_ComboBox.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
                    FechaDeExpiracion_DatePicker.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource();
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
                if (new ClienteSeleccionado().Validate(BuscarCliente_ComboBox_SelectedItem, currentCulture) == new ValidationResult(true, null)
                    && (FechaDeExpiracion_CheckBox.IsChecked.Value == true && new FechaDeExpiracion().Validate(FechaDeExpiracion_DatePicker.SelectedDate, currentCulture) == new ValidationResult(true, null) || FechaDeExpiracion_CheckBox.IsChecked.Value == false))
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
                return false;
            }
        }

        // ------------------------------------------------------ //
        //             Actualizar la lista de clientes            //
        // ------------------------------------------------------ //

        private void ActualizarListaDeClientes()
        {
            try
            {
                BuscarCliente_ComboBox.Items.Clear();
                foreach (Cliente cliente in ControladorClientes.ObtenerListaDeClientes())
                    BuscarCliente_ComboBox.Items.Add(cliente);
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        // ------------------------------------------------------ //
        //            Estado de la fecha de expiración            //
        // ------------------------------------------------------ //

        private static bool _estadoFechaExpiracion;
        public static bool ObtenerEstadoFechaExpiracion()
        {
            return _estadoFechaExpiracion;
        }

        private void FechaDeExpiracion_CheckBox_CheckedUnchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                _estadoFechaExpiracion = FechaDeExpiracion_CheckBox.IsChecked.Value;
                if (FechaDeExpiracion_CheckBox.IsChecked.Value == false)
                {
                    // Eliminar el mensaje de error
                    FechaDeExpiracion_DatePicker.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource();
                }
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        // ------------------------------------------------------ //
        //  Alerta al cerrar la ventana sin guardar los cambios   //
        // ------------------------------------------------------ //

        private void Cancelar_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_presupuestoModificar == null)
                {
                    // Crear Presupuesto
                    if (BuscarCliente_ComboBox_SelectedItem != null || FechaDeExpiracion_DatePicker_SelectedDate != null || FechaDeExpiracion_CheckBox.IsChecked.Value == true)
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
                    // Modificar Presupuesto
                    Cliente clienteSeleccionado = BuscarCliente_ComboBox.SelectedItem as Cliente;
                    if (clienteSeleccionado != null)
                    {
                        if ((BuscarCliente_ComboBox_SelectedItem == null || (clienteSeleccionado.Id != _presupuestoModificar.ClienteId))
                        || (FechaDeExpiracion_CheckBox.IsChecked.Value == true && _presupuestoModificar.FechaDeExpiracion == null)
                        || (FechaDeExpiracion_CheckBox.IsChecked.Value == false && _presupuestoModificar.FechaDeExpiracion != null)
                        || (FechaDeExpiracion_DatePicker.SelectedDate != _presupuestoModificar.FechaDeExpiracion))
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
                        // Se realizaron cambios, y se mostrará la alerta
                        AlertaAlCerrar_Dialog.IsOpen = true;
                    }
                }
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
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

        // ------------------------------------------------------ //
        //             Acceso directo Agregar Cliente             //
        // ------------------------------------------------------ //

        private void AgregarCliente_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CaracteristicasCliente caracteristicasCliente = new CaracteristicasCliente(_principal, null, false, true);
                caracteristicasCliente.Owner = Application.Current.MainWindow;

                caracteristicasCliente.ShowDialog();
                if (BuscarCliente_ComboBox.Items.Count != ControladorClientes.ObtenerListaDeClientes().Count)
                    ActualizarListaDeClientes();
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }
    }
}
