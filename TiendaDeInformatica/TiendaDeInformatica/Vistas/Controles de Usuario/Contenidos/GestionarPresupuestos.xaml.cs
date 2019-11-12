using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TiendaDeInformatica.Controladores;
using TiendaDeInformatica.Modelos;
using TiendaDeInformatica.Vistas.Ventanas.Agregar_Presupuesto;
using TiendaDeInformatica.Vistas.Ventanas.Caracteristicas_Cliente;

namespace TiendaDeInformatica.Vistas.Controles_de_Usuario.Contenidos
{
    /// <summary>
    /// Lógica de interacción para GestionarClientes.xaml
    /// </summary>
    public partial class GestionarPresupuestos : System.Windows.Controls.UserControl
    {

        private bool VistaCargada = false;
        public GestionarPresupuestos()
        {
            InitializeComponent();
            RefrescarListBox();
            this.DataContext = this;
        }

        private void VistaGestionarPresupuestos_Loaded(object sender, RoutedEventArgs e)
        {
            VistaCargada = true;
        }

        private void AgregarPresupuesto_Button_Click(object sender, RoutedEventArgs e)
        {
            AgregarPresupuesto agregarPresupuesto = new AgregarPresupuesto(null);
            agregarPresupuesto.ShowDialog();
            RefrescarListBox();
        }

        private void BuscarPorCliente_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void RefrescarListBox()
        {
            Presupuestos_ListBox.Items.Clear();
            foreach (Presupuesto presupuesto in ControladorPresupuestos.ObtenerListaDePresupuestos())
            {
                Presupuestos_ListBox.Items.Add(presupuesto);
            }
        }

        private void ModificarPresupuesto(object sender, RoutedEventArgs e)
        {
            Presupuesto presupuesto = Presupuestos_ListBox.SelectedItem as Presupuesto;
            if (presupuesto != null)
            {
                AgregarPresupuesto agregarPresupuesto = new AgregarPresupuesto(presupuesto);
                agregarPresupuesto.ShowDialog();
                RefrescarListBox();
            }
        }

        private void EliminarPresupuesto(object sender, RoutedEventArgs e)
        {
            // Falta enviar alerta
            Presupuesto presupuesto = Presupuestos_ListBox.SelectedItem as Presupuesto;
            ControladorPresupuestos.EliminarPresupuesto(presupuesto);
            RefrescarListBox();
        }
    }
}
