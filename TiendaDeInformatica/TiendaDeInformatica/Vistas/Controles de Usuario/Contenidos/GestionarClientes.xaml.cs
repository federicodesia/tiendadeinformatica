using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TiendaDeInformatica.Controladores;
using TiendaDeInformatica.Modelos;
using TiendaDeInformatica.Vistas.Ventanas.Caracteristicas_Cliente;

namespace TiendaDeInformatica.Vistas.Controles_de_Usuario.Contenidos
{
    /// <summary>
    /// Lógica de interacción para GestionarClientes.xaml
    /// </summary>
    public partial class GestionarClientes : System.Windows.Controls.UserControl
    {
        private bool VistaCargada = false;
        public GestionarClientes()
        {
            InitializeComponent();
            RefrescarListBox();
        }

        private void RefrescarListBox()
        {
            if (FiltrarClientes_ComboBox.SelectedIndex == 0)
            {
                Personas_DataGrid.Items.Clear();
                foreach (Cliente cliente in (ControladorClientes.ObtenerListaDeClientes().Where(c => c.Tipo == "Persona")))
                {
                    Personas_DataGrid.Items.Add(cliente);
                }
            }
            else if (FiltrarClientes_ComboBox.SelectedIndex == 1)
            {
                Empresas_DataGrid.Items.Clear();
                foreach (Cliente cliente in (ControladorClientes.ObtenerListaDeClientes().Where(c => c.Tipo == "Empresa")))
                {
                    Empresas_DataGrid.Items.Add(cliente);
                }
            }
        }

        private void AgregarCliente_Button_Click(object sender, RoutedEventArgs e)
        {
            int CantidadDeClientes = ControladorClientes.ObtenerListaDeClientes().Count;
            CaracteristicasCliente agregarCliente = new CaracteristicasCliente(null);
            agregarCliente.ShowDialog();
            RefrescarListBox();

            if(ControladorClientes.ObtenerListaDeClientes().Count> CantidadDeClientes)
            {
                _ = MostrarSnackBar("Cliente agregado correctamente!");
            }
        }

        private void BuscarCliente_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (BuscarCliente_TextBox.Text != "")
            {
                string Busqueda = BuscarCliente_TextBox.Text.ToUpper();
                if (FiltrarClientes_ComboBox.SelectedIndex == 0)
                {
                    Personas_DataGrid.Items.Clear();
                    foreach (Cliente cliente in (ControladorClientes.ObtenerListaDeClientes().Where(c => c.Tipo == "Persona")))
                    {
                        if (cliente.Nombre.ToUpper().StartsWith(Busqueda)
                            || cliente.Apellido.ToUpper().StartsWith(Busqueda)
                            || cliente.CUIT.ToUpper().StartsWith(Busqueda)
                            || cliente.Telefono.ToUpper().StartsWith(Busqueda))
                        {
                            Personas_DataGrid.Items.Add(cliente);
                        }
                    }
                }
                else if (FiltrarClientes_ComboBox.SelectedIndex == 1)
                {
                    Empresas_DataGrid.Items.Clear();
                    foreach (Cliente cliente in (ControladorClientes.ObtenerListaDeClientes().Where(c => c.Tipo == "Empresa")))
                    {
                        if (cliente.Nombre.ToUpper().StartsWith(Busqueda)
                            || cliente.Apellido.ToUpper().StartsWith(Busqueda)
                            || cliente.CUIT.ToUpper().StartsWith(Busqueda)
                            || cliente.Telefono.ToUpper().StartsWith(Busqueda)
                            || cliente.NombreDeLaEmpresa.ToUpper().StartsWith(Busqueda))
                        {
                            Empresas_DataGrid.Items.Add(cliente);
                        }
                    }
                }
            }
            else
            {
                RefrescarListBox();
            }
        }

        private void FiltrarClientes_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VistaCargada)
            {
                if (FiltrarClientes_ComboBox.SelectedIndex == 0)
                {
                    Empresas_DataGrid.Visibility = Visibility.Hidden;
                    Personas_DataGrid.Visibility = Visibility.Visible;
                }
                else if (FiltrarClientes_ComboBox.SelectedIndex == 1)
                {
                    Personas_DataGrid.Visibility = Visibility.Hidden;
                    Empresas_DataGrid.Visibility = Visibility.Visible;
                }
                RefrescarListBox();
            }
        }

        private void VistaGestionarClientes_Loaded(object sender, RoutedEventArgs e)
        {
            VistaCargada = true;
        }

        private void EliminarCliente(object sender, RoutedEventArgs e)
        {
            if (FiltrarClientes_ComboBox.SelectedIndex == 0)
            {
                Cliente cliente = Personas_DataGrid.SelectedItem as Cliente;
                if (cliente != null)
                {
                    ControladorClientes.EliminarCliente(cliente);
                }
            }
            else if (FiltrarClientes_ComboBox.SelectedIndex == 1)
            {
                Cliente cliente = Empresas_DataGrid.SelectedItem as Cliente;
                if (cliente != null)
                {
                    ControladorClientes.EliminarCliente(cliente);
                }
            }
            RefrescarListBox();
            _ = MostrarSnackBar("Cliente eliminado correctamente!");
        }

        private void ModificarCliente(object sender, RoutedEventArgs e)
        {
            if (FiltrarClientes_ComboBox.SelectedIndex == 0)
            {
                Cliente cliente = Personas_DataGrid.SelectedItem as Cliente;
                if (cliente != null)
                {
                    CaracteristicasCliente agregarCliente = new CaracteristicasCliente(cliente);
                    agregarCliente.ShowDialog();

                    if (cliente != ControladorClientes.ObtenerListaDeClientes().Where(c => c.Id == cliente.Id))
                    {
                        _ = MostrarSnackBar("Cliente modificado correctamente!");
                    }
                }
            }
            else if (FiltrarClientes_ComboBox.SelectedIndex == 1)
            {
                Cliente cliente = Empresas_DataGrid.SelectedItem as Cliente;
                if (cliente != null)
                {
                    CaracteristicasCliente agregarCliente = new CaracteristicasCliente(cliente);
                    agregarCliente.ShowDialog();

                    if (cliente != ControladorClientes.ObtenerListaDeClientes().Where(c => c.Id == cliente.Id))
                    {
                        _ = MostrarSnackBar("Cliente modificado correctamente!");
                    }
                }
            }
            RefrescarListBox();
        }

        private void Exportar_Button_Click(object sender, RoutedEventArgs e)
        {
            string textoGuardar = "Lista de clientes:\r\n";
            if (FiltrarClientes_ComboBox.SelectedIndex == 0)
            {
                if ((ControladorClientes.ObtenerListaDeClientes().Where(c => c.Tipo == "Persona")).Count() > 0)
                {
                    foreach (Cliente cliente in (ControladorClientes.ObtenerListaDeClientes().Where(c => c.Tipo == "Persona")))
                    {
                        textoGuardar = textoGuardar + $"\r\n- {cliente.Nombre} {cliente.Apellido}";
                        if (cliente.Telefono != "")
                        {
                            textoGuardar = textoGuardar + $", Teléfono: {cliente.Telefono}";
                        }
                        if (cliente.CUIT != "")
                        {
                            textoGuardar = textoGuardar + $", CUIT: {cliente.CUIT}";
                        }
                    }
                }
                else
                {
                    textoGuardar = textoGuardar = "\r\nNo hay ninguna persona en la lista de clientes";
                }

            }
            else if (FiltrarClientes_ComboBox.SelectedIndex == 1)
            {
                if((ControladorClientes.ObtenerListaDeClientes().Where(c => c.Tipo == "Empresa")).Count() > 0)
                {
                    foreach (Cliente cliente in (ControladorClientes.ObtenerListaDeClientes().Where(c => c.Tipo == "Empresa")))
                    {
                        textoGuardar = textoGuardar + $"\r\n- {cliente.NombreDeLaEmpresa} (Responsable: {cliente.NombreDelResponsable})";
                        if (cliente.Telefono != "")
                        {
                            textoGuardar = textoGuardar + $", Teléfono: {cliente.Telefono}";
                        }
                        if (cliente.CUIT != "")
                        {
                            textoGuardar = textoGuardar + $", CUIT: {cliente.CUIT}";
                        }
                    }
                }
                else
                {
                    textoGuardar = textoGuardar = "\r\nNo hay ninguna empresa en la lista de clientes";
                }
            }

            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Archivos de texto (*.txt)|*.txt"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter escritor = new StreamWriter(dialog.FileName))
                {
                    escritor.Write(textoGuardar);
                }
                _ = MostrarSnackBar("Lista exportada correctamente!");
            }
        }

        private void SnackbarMessage_ActionClick(object sender, RoutedEventArgs e)
        {
            Mensaje_Snackbar.IsActive = false;
        }

        private async Task MostrarSnackBar(string mensaje)
        {
            Mensaje_Snackbar_Contenido.Content = mensaje;
            await Task.Delay(500);
            Mensaje_Snackbar.IsActive = true;
            await Task.Delay(5000);
            Mensaje_Snackbar.IsActive = false;
        }
    }
}
