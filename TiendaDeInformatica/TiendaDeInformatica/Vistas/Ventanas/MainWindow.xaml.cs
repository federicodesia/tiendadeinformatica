using MahApps.Metro.Controls;
using TiendaDeInformatica.Controladores;
using TiendaDeInformatica.Modelos;
using TiendaDeInformatica.Vistas.Controles_de_Usuario.Contenidos;
using TiendaDeInformatica.Vistas.Ventanas.Agregar_Presupuesto;
using TiendaDeInformatica.Vistas.Ventanas.Agregar_Producto;

namespace TiendaDeInformatica
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent(); 
        }

        private void NuevoPresupuesto_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AgregarPresupuesto agregarPresupuesto = new AgregarPresupuesto();
            agregarPresupuesto.ShowDialog();
        }

        private void NuevoProducto_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AgregarProducto agregarProducto = new AgregarProducto();
            agregarProducto.ShowDialog();
        }

        private void MetroWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Contenido.Children.Add(new GestionarClientes());
        }
    }
}
