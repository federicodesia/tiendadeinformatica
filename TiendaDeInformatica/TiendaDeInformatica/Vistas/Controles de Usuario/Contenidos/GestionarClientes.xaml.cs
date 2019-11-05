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
            Clientes_ListBox.Items.Clear();
            foreach(Cliente cliente in ControladorClientes.ObtenerListaDeClientes())
            {
                Clientes_ListBox.Items.Add(cliente);
            }
        }
    }
}
