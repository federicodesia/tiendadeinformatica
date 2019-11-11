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
        private bool VistaCargada = false;
        public MainWindow()
        {
            InitializeComponent(); 
        }

        private void MetroWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            VistaCargada = true;
            Contenido.Children.Add(new GestionarClientes());
        }

        private void MenuIzquierdo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (VistaCargada)
            {
                int index = MenuIzquierdo.SelectedIndex;
                Contenido.Children.Clear();
                switch (index)
                {
                    case 0:
                        Contenido.Children.Add(new GestionarPresupuestos());
                        break;
                    case 1:
                        Contenido.Children.Add(new GestionarClientes());
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
