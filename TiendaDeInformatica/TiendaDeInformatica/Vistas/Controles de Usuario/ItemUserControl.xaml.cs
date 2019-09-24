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
using TiendaDeInformatica.Modelos.Menu;

namespace TiendaDeInformatica.Vistas.Controles_de_Usuario
{
    /// <summary>
    /// Lógica de interacción para ItemUserControl.xaml
    /// </summary>
    public partial class ItemUserControl : UserControl
    {
        public ItemUserControl(Item item)
        {
            InitializeComponent();

            if (item.SubItems.Count == 0)
            {
                ExpanderMenu.Visibility = Visibility.Hidden;
            }
            else
            {
                ExpanderMenu.Visibility = item.SubItems == null ? Visibility.Collapsed : Visibility.Visible;
                ListViewItemMenu.Visibility = item.SubItems == null ? Visibility.Visible : Visibility.Collapsed;
            }

            this.DataContext = item;
        }
    }
}
