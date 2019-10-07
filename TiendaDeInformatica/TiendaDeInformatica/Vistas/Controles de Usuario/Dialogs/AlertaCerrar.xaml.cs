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

namespace TiendaDeInformatica.Vistas.Controles_de_Usuario.Dialogs
{
    /// <summary>
    /// Lógica de interacción para AlertaCerrar.xaml
    /// </summary>
    public partial class AlertaCerrar : UserControl
    {
        public AlertaCerrar()
        {
            InitializeComponent();
        }

        private void CerrarDeIgualManera_Click(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }
    }
}
