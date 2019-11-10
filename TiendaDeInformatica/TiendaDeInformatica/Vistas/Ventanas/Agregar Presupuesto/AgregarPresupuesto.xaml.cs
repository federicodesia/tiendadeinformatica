using MahApps.Metro.Controls;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TiendaDeInformatica.Controladores;
using TiendaDeInformatica.Modelos;
using TiendaDeInformatica.Vistas.Ventanas.Agregar_Cliente;
using TiendaDeInformatica.Vistas.Ventanas.Agregar_Presupuesto.Reglas_de_Validacion;

namespace TiendaDeInformatica.Vistas.Ventanas.Agregar_Presupuesto
{
    /// <summary>
    /// Lógica de interacción para AgregarPresupuesto.xaml
    /// </summary>
    public partial class AgregarPresupuesto : MetroWindow
    {
        public Cliente BuscarCliente_ComboBox_SelectedItem { get; set; }
        public string FechaDeExpiracion_DatePicker_SelectedDate { get; set; }


        public AgregarPresupuesto()
        {
            InitializeComponent();

            FechaDeExpiracion_DatePicker.BlackoutDates.AddDatesInPast();
            ActualizarComboBoxClientes();
            this.DataContext = this;
        }

        private void AgregarCliente_Button_Click(object sender, RoutedEventArgs e)
        {
            int CantidadDeClientes = ControladorClientes.ObtenerListaDeClientes().Count;
            AgregarCliente agregarCliente = new AgregarCliente();
            agregarCliente.ShowDialog();

            ActualizarComboBoxClientes();
            if (ControladorClientes.ObtenerListaDeClientes().Count > CantidadDeClientes)
            {
                _ = MostrarPopupAsync();
            }
        }

        private void FechaDeExpiracion_CheckBox_Click(object sender, RoutedEventArgs e)
        {
            _ObtenerEstadoFechaExpiracion = FechaDeExpiracion_CheckBox.IsChecked.Value;
            if (FechaDeExpiracion_DatePicker.SelectedDate != null || FechaDeExpiracion_CheckBox.IsChecked == false)
            {
                FechaDeExpiracion_DatePicker.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource();
            }
        }

        private void Cancelar_Button_Click(object sender, RoutedEventArgs e)
        {
            MostrarDialog();
        }

        private void Crear_Button_Click(object sender, RoutedEventArgs e)
        {
            Cliente cliente = BuscarCliente_ComboBox.SelectedItem as Cliente;

            if (new ClienteSeleccionado().Validate(cliente, CultureInfo.CurrentCulture) == new ValidationResult(true, null)
                && ((FechaDeExpiracion_CheckBox.IsChecked == true && new FechaExpiracion().Validate(FechaDeExpiracion_DatePicker.SelectedDate, CultureInfo.CurrentCulture) == new ValidationResult(true, null)) || FechaDeExpiracion_CheckBox.IsChecked == false))
            {
                if (FechaDeExpiracion_CheckBox.IsChecked == false)
                {
                    ControladorPresupuestos.AgregarPresupuesto(cliente, null);
                }
                else
                {
                    ControladorPresupuestos.AgregarPresupuesto(cliente, FechaDeExpiracion_DatePicker.SelectedDate.GetValueOrDefault());
                }
                this.Close();
            }
            else
            {
                BuscarCliente_ComboBox.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
                FechaDeExpiracion_DatePicker.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource();
            }
        }




        private void ActualizarComboBoxClientes()
        {
            BuscarCliente_ComboBox.Items.Clear();
            foreach (Cliente cliente in ControladorClientes.ObtenerListaDeClientes())
            {
                BuscarCliente_ComboBox.Items.Add(cliente);
            }
        }

        private async Task MostrarPopupAsync()
        {
            await Task.Delay(500);
            ClienteCreado_Snackbar.IsActive = true;
            await Task.Delay(5000);
            ClienteCreado_Snackbar.IsActive = false;
        }



        private static bool _ObtenerEstadoFechaExpiracion;
        public static bool ObtenerEstadoFechaExpiracion()
        {
            return _ObtenerEstadoFechaExpiracion;
        }


        private void ClienteCreado_Snackbar_ActionClick(object sender, RoutedEventArgs e)
        {
            ClienteCreado_Snackbar.IsActive = false;
        }

        private void MostrarDialog()
        {
            if (BuscarCliente_ComboBox.SelectedItem != null || FechaDeExpiracion_DatePicker.SelectedDate != null)
            {
                Dialog.IsOpen = true;
            }
            else
            {
                this.Close();
            }
        }

        private void BuscarCliente_ComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Cliente clienteSeleccionado = BuscarCliente_ComboBox.SelectedItem as Cliente;
            if (clienteSeleccionado == null)
            {
                string Busqueda = BuscarCliente_ComboBox.Text.ToUpper();
                string empresa = "";
                BuscarCliente_ComboBox.Items.Clear();
                foreach (Cliente cliente in ControladorClientes.ObtenerListaDeClientes())
                {                    
                    if (!string.IsNullOrWhiteSpace(cliente.NombreDeLaEmpresa))
                        empresa = cliente.NombreDeLaEmpresa;
                    if (empresa.ToUpper().StartsWith(Busqueda) || cliente.Nombre.ToUpper().StartsWith(Busqueda) || cliente.Apellido.ToUpper().StartsWith(Busqueda) || cliente.Telefono.ToUpper().StartsWith(Busqueda) || cliente.CUIT.ToUpper().StartsWith(Busqueda))
                    {
                        BuscarCliente_ComboBox.Items.Add(cliente);
                    }
                }
            }
        }
    }
}