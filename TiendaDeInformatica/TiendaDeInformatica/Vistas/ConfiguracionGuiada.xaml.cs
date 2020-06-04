using System.Windows;
using System.Windows.Controls;

namespace TiendaDeInformatica.Vistas
{
    /// <summary>
    /// Lógica de interacción para Clientes.xaml
    /// </summary>
    public partial class ConfiguracionGuiada : UserControl
    {
        private Principal _principal;

        public ConfiguracionGuiada(Principal principal)
        {
            InitializeComponent();
            _principal = principal;
        }

        private void ConfiguracionGuiada_Vista_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
