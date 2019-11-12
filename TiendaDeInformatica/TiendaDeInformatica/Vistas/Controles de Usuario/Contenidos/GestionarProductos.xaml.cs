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
using TiendaDeInformatica.Vistas.Ventanas.Agregar_Producto;

namespace TiendaDeInformatica.Vistas.Controles_de_Usuario.Contenidos
{
    /// <summary>
    /// Lógica de interacción para GestionarProductos.xaml
    /// </summary>
    public partial class GestionarProductos : UserControl
    {
        private bool VistaCargada = false;
        public TipoProducto FiltarProductosPorTipo_ComboBox_SelectedItem { get; set; }
        public Marca FiltrarProductosPorMarca_ComboBox_SelectedItem { get; set; } 
        public GestionarProductos()
        {
            InitializeComponent();
            TipoProducto[] tipoProductos = (TipoProducto[])Enum.GetValues(typeof(TipoProducto));
            IEnumerable<TipoProducto> tipoProductosOrdenados = tipoProductos.OrderBy(v => v.ToString());
            FiltrarProductosPorTipo_ComboBox.ItemsSource = tipoProductosOrdenados;
            ActualizarComboBoxMarcas();
            this.DataContext = this;

            foreach(Marca marca in ControladorMarcas.ObtenerListaDeMarcas())
            {
                FiltrarProductosPorMarca_ComboBox.Items.Add(marca);
            }
        }

        private void AgregarProducto_Button_Click(object sender, RoutedEventArgs e)
        {
            int CantidadDeProductos = ControladorProductos.ObtenerListaDeProductos().Count;
            AgregarProducto agregarProducto = new AgregarProducto();
            agregarProducto.ShowDialog();
            RefrescarListBox();
        }

        private void BuscarPorModelo_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RefrescarListBox()
        {
            Productos_ListBox.Items.Clear();
            foreach (Producto producto in ControladorProductos.ObtenerListaDeProductos())
            {
                Productos_ListBox.Items.Add(producto);
            }
        }
        private void ActualizarComboBoxMarcas()
        {
            FiltrarProductosPorMarca_ComboBox.Items.Clear();
            foreach (Marca marca in ControladorMarcas.ObtenerListaDeMarcas().OrderBy(v => v.Nombre).ToList())
            {
                FiltrarProductosPorMarca_ComboBox.Items.Add(marca);
            }
        }

        private void FiltrarProductosPorMarca_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Productos_ListBox.Items.Clear();
            foreach (Producto producto in ControladorProductos.ObtenerListaDeProductos())
            {
                if (producto.Marca == FiltrarProductosPorMarca_ComboBox.SelectedItem)
                {
                    Productos_ListBox.Items.Add(producto);
                }
            }
        }

        private void FiltrarProductosPorTipo_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }

        private void VistaGestionarProductos_Loaded(object sender, RoutedEventArgs e)
        {
            VistaCargada = true;
        }

        private void FiltrarProductosPorTipo_ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            FiltrarProductosPorTipo_ComboBox.SelectedIndex = -1;
        }

        private void FiltrarProductosPorMarca_ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            FiltrarProductosPorMarca_ComboBox.SelectedIndex = -1;
        }
    }
}
