using System.Windows;
using System.Windows.Controls;

namespace TiendaDeInformatica.Vistas
{
    /// <summary>
    /// Lógica de interacción para Clientes.xaml
    /// </summary>
    public partial class EquiposArmados : UserControl
    {
        private Principal _principal;

        public EquiposArmados(Principal principal)
        {
            InitializeComponent();
            _principal = principal;
        }

        private void EquiposArmados_Vista_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
