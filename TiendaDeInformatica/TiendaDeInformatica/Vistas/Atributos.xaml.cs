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

namespace TiendaDeInformatica.Vistas
{
    /// <summary>
    /// Lógica de interacción para Atributos.xaml
    /// </summary>
    public partial class Atributos : UserControl
    {
        public Atributos()
        {
            InitializeComponent();
        }

        private void Atributos_Vista_Loaded(object sender, RoutedEventArgs e)
        {
            RefrescarListBox();
        }
        private void RefrescarListBox()
        {
            Atributos_ListBox.Items.Clear();
            foreach (Atributo atributo in ControladorAtributos.ObtenerListaDeAtributos())
            {
                Atributos_ListBox.Items.Add(atributo);
            }
        }

        private void Agregar_Button_Click(object sender, RoutedEventArgs e)
        {
            ControladorAtributos.AgregarAtributo(Atributos_TextBox.Text);
            RefrescarListBox();
        }
    }
}
