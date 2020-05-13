using System.Windows;
using System.Windows.Controls;
using TiendaDeInformatica.Helpers;
using TiendaDeInformatica.Modelos;
using TiendaDeInformatica.Vistas.Caracteristicas;

namespace TiendaDeInformatica.Vistas
{
    /// <summary>
    /// Lógica de interacción para Clientes.xaml
    /// </summary>
    public partial class Productos : UserControl
    {
        private Principal _principal;

        public Productos(Principal principal, TipoProducto? tipoProducto)
        {
            InitializeComponent();
            _principal = principal;

            if (tipoProducto != null)
            {
                TituloTipoProducto_TextBlock.Text = tipoProducto.ToDescription();
                TituloTipoProducto_StackPanel.Visibility = Visibility.Visible;
            }
        }

        private void AgregarProducto_Button_Click(object sender, RoutedEventArgs e)
        {
            CaracteristicasProducto caracteristicasProducto = new CaracteristicasProducto(_principal, null);
            caracteristicasProducto.Owner = Application.Current.MainWindow;

            caracteristicasProducto.ShowDialog();
        }
    }
}
