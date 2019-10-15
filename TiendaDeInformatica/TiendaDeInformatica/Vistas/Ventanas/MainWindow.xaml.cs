using MahApps.Metro.Controls;
using TiendaDeInformatica.Modelos;
using TiendaDeInformatica.Vistas.Ventanas.Agregar_Presupuesto;

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
    }
}
