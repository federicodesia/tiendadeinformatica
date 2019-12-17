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
    /// Lógica de interacción para ConfiguracionGuiada.xaml
    /// </summary>
    public partial class ConfiguracionGuiada : UserControl
    {
        public ConfiguracionGuiada()
        {
            InitializeComponent();
            RefrescarListBox();
        }
        private void RefrescarListBox()
        {
            int marcas = ControladorMarcas.ObtenerListaDeMarcas().Count();
            if (marcas > 0)
            { 
                ConfiguracionGuiada_ListBox.Items.Clear();
                ConfiguracionGuiada_ListBox.Visibility = Visibility.Visible;
                NingunaMarca_Label.Visibility = Visibility.Hidden;
                foreach (Marca marca in ControladorMarcas.ObtenerListaDeMarcas())
                {
                    ConfiguracionGuiada_ListBox.Items.Add(marca);
                }
            }
            else
            {
                ConfiguracionGuiada_ListBox.Visibility = Visibility.Hidden;
                NingunaMarca_Label.Visibility = Visibility.Visible;
            }
        }
    }
}
