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
            ConfiguracionGuiada_ListBox.Items.Clear();
            foreach (Marca marca in ControladorMarcas.ObtenerListaDeMarcas())
            {
                ConfiguracionGuiada_ListBox.Items.Add(marca);
            }
        }
    }
}
