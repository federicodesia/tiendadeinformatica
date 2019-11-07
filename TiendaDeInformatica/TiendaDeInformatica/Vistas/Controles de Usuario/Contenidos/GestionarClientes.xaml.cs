using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TiendaDeInformatica.Controladores;
using TiendaDeInformatica.Modelos;
using TiendaDeInformatica.Vistas.Ventanas.Agregar_Cliente;

namespace TiendaDeInformatica.Vistas.Controles_de_Usuario.Contenidos
{
    /// <summary>
    /// Lógica de interacción para GestionarClientes.xaml
    /// </summary>
    public partial class GestionarClientes : UserControl
    {
        public GestionarClientes()
        {
            InitializeComponent();
            RefrescarListBox();
        }

        private void RefrescarListBox()
        {
            Personas_DataGrid.Items.Clear();
            foreach(Cliente cliente in ControladorClientes.ObtenerListaDeClientes())
            {
                Personas_DataGrid.Items.Add(cliente);
            }
        }

        private void AgregarCliente_Button_Click(object sender, RoutedEventArgs e)
        {
            int CantidadDeClientes = ControladorClientes.ObtenerListaDeClientes().Count;
            AgregarCliente agregarCliente = new AgregarCliente();
            agregarCliente.ShowDialog();
            RefrescarListBox();
        }
    }
}
