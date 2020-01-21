using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using TiendaDeInformatica.Controladores;
using TiendaDeInformatica.Modelos;
using TiendaDeInformatica.Vistas.Caracteristicas;

namespace TiendaDeInformatica.Vistas
{
    /// <summary>
    /// Lógica de interacción para Clientes.xaml
    /// </summary>
    public partial class Clientes : System.Windows.Controls.UserControl
    {
        private Principal _principal;

        public Clientes(Principal principal)
        {
            InitializeComponent();
            _principal = principal;

            RefrescarListaDeClientes(true);
        }

        //
        // Agregar cliente
        //

        private void AgregarCliente_Button_Click(object sender, RoutedEventArgs e)
        {
            CaracteristicasCliente caracteristicasCliente = new CaracteristicasCliente(_principal, null, true);
            caracteristicasCliente.Owner = System.Windows.Application.Current.MainWindow;

            caracteristicasCliente.ShowDialog();
            RefrescarListaDeClientes(false);
        }

        //
        // Opciones al hacer click derecho sobre un cliente
        //

        private void EliminarCliente_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _principal.OscurecerCompletamente(true);
            AlertaBorrarCliente_DialogHost.IsOpen = true;
        }

        // Modificar
        private void ModificarCliente_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Cliente cliente = new Cliente();
            if (FiltroPersona_RadioButton.IsChecked.Value)
                cliente = Personas_DataGrid.SelectedItem as Cliente;
            else
                cliente = Empresas_DataGrid.SelectedItem as Cliente;

            if (cliente != null)
            {
                CaracteristicasCliente caracteristicasCliente = new CaracteristicasCliente(_principal, cliente, true);
                caracteristicasCliente.Owner = System.Windows.Application.Current.MainWindow;

                caracteristicasCliente.ShowDialog();
                RefrescarListaDeClientes(false);
            }
        }

        // Eliminar
        private void EliminarCliente_Button_Click(object sender, RoutedEventArgs e)
        {
            Cliente cliente = new Cliente();
            if (FiltroPersona_RadioButton.IsChecked.Value)
                cliente = Personas_DataGrid.SelectedItem as Cliente;
            else
                cliente = Empresas_DataGrid.SelectedItem as Cliente;

            if (cliente != null)
            {
                ControladorClientes.EliminarCliente(cliente);
                RefrescarListaDeClientes(false);

                AlertaBorrarCliente_DialogHost.IsOpen = false;
                _principal.OscurecerCompletamente(false);
                _ = _principal.MostrarMensajeEnSnackbar("Cliente eliminado correctamente!");
            }
            
        }

        private void Cancelar_Button_Click(object sender, RoutedEventArgs e)
        {
            _principal.OscurecerCompletamente(false);
            AlertaBorrarCliente_DialogHost.IsOpen = false;
        }

        //
        // Buscar
        //

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefrescarListaDeClientes(false);
        }

        //
        // Filtros
        //

        private void FiltroTipoCliente_RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RefrescarListaDeClientes(false);
        }

        private void RefrescarListaDeClientes(bool saltarVerificacion)
        {
            if (Clientes_Vista.IsLoaded || saltarVerificacion)
            {
                List<Cliente> clientes = ControladorClientes.ObtenerListaDeClientes().ToList();
                List<Cliente> resultados = BuscarCliente(FiltrarPorTipoDeCliente(clientes), BuscarCliente_TextBox.Text);

                if (FiltroPersona_RadioButton.IsChecked.Value)
                {
                    Personas_DataGrid.Items.Clear();
                    foreach (Cliente cliente in resultados)
                    {
                        Personas_DataGrid.Items.Add(cliente);
                    }
                    CantidadDeResultados_TextBlock.Text = Personas_DataGrid.Items.Count.ToString();
                }
                else
                {
                    Empresas_DataGrid.Items.Clear();
                    foreach (Cliente cliente in resultados)
                    {
                        Empresas_DataGrid.Items.Add(cliente);
                    }
                    CantidadDeResultados_TextBlock.Text = Empresas_DataGrid.Items.Count.ToString();
                }
            }
        }

        private List<Cliente> BuscarCliente(List<Cliente> clientes, string busqueda)
        {
            if (busqueda != "")
            {
                busqueda = busqueda.ToUpper();
                List<Cliente> resultado = new List<Cliente>();
                foreach (Cliente cliente in clientes)
                {
                    if ((cliente.NombreDeLaEmpresa != null && cliente.NombreDeLaEmpresa.ToUpper().StartsWith(busqueda))
                        || (cliente.NombreDelResponsable != null && cliente.NombreDelResponsable.ToUpper().StartsWith(busqueda))
                        || (cliente.CUIT != null && cliente.CUIT.StartsWith(busqueda))
                        || (cliente.Telefono != null && cliente.Telefono.StartsWith(busqueda)
                        || cliente.MostrarNombre.ToUpper().StartsWith(busqueda)
                        || cliente.Apellido.ToUpper().StartsWith(busqueda)))
                    {
                        resultado.Add(cliente);
                    }
                }
                return resultado;
            }
            return clientes;
        }

        private List<Cliente> FiltrarPorTipoDeCliente(List<Cliente> clientes)
        {
            if (FiltroPersona_RadioButton.IsChecked.Value == true)
            {
                return clientes.Where(c => c.Tipo == "Persona").ToList();
            }
            return clientes.Where(c => c.Tipo == "Empresa").ToList();
        }

        //
        // Exporar lista de clientes
        // 

        private void ExporarLista_Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Archivos de texto (*.txt)|*.txt"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter escritor = new StreamWriter(dialog.FileName))
                {
                    escritor.Write(ControladorClientes.ExportarListaDeClientes());
                }
                _ = _principal.MostrarMensajeEnSnackbar("Lista exportada correctamente!");
            }
        }
    }
}
