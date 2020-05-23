using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TiendaDeInformatica.Controladores;
using TiendaDeInformatica.Helpers;
using TiendaDeInformatica.Modelos;
using TiendaDeInformatica.Vistas.Caracteristicas;

namespace TiendaDeInformatica.Vistas
{
    /// <summary>
    /// Lógica de interacción para Clientes.xaml
    /// </summary>
    public partial class ResumenPresupuesto : UserControl
    {
        private Principal _principal;
        private int _presupuestoId;

        public ResumenPresupuesto(Principal principal, int presupestoId)
        {
            InitializeComponent();
            this.DataContext = ControladorPresupuestos.ObtenerPresupuesto(presupestoId);
            _principal = principal;
            _presupuestoId = presupestoId;

            principal.PresupuestosBackground_Grid.Visibility = Visibility.Visible;
            principal.MenuIzquierdo.SelectedIndex = -1;

            TituloPresupuesto_TextBlock.Text = $"Presupuesto #{_presupuestoId.ToString()}";
        }

        private void ResumenPresupuesto_Vista_Loaded(object sender, RoutedEventArgs e)
        {
            RefrescarListaPresupuestoProducto();
        }

        private void Presupuestos_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            _principal.MenuIzquierdo.SelectedIndex = 0;
        }

        // ------------------------------------------------------ //
        //           Refrescar la lista de los productos          //
        // ------------------------------------------------------ //

        public void RefrescarListaPresupuestoProducto()
        {
            Presupuesto presupuesto = ControladorPresupuestos.ObtenerPresupuesto(_presupuestoId);
            this.DataContext = presupuesto;
            Productos_ListBox.Items.Clear();
            foreach (PresupuestoProducto presupuestoProducto in presupuesto.Productos)
                Productos_ListBox.Items.Add(presupuestoProducto);
        }

        // ------------------------------------------------------ //
        //                   Opciones de cada item                //
        // ------------------------------------------------------ //

        private void Eliminar_TextBlock_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TextBlock textBlockSender = (TextBlock)sender;
            if (textBlockSender.DataContext is PresupuestoProducto)
            {
                ControladorPresupuestos.EliminarPresupuestoProducto((PresupuestoProducto)textBlockSender.DataContext);
                RefrescarListaPresupuestoProducto();
            }
        }

        private void AgregarCantidadPresupuestoProducto_Button_Click(object sender, RoutedEventArgs e)
        {
            Button buttonSender = (Button)sender;
            if (buttonSender.DataContext is PresupuestoProducto)
            {
                PresupuestoProducto presupuestoProducto = (PresupuestoProducto)buttonSender.DataContext;
                if (presupuestoProducto.Cantidad < 99)
                {
                    ControladorPresupuestos.ModificarCantidadPresupuestoProducto(presupuestoProducto, presupuestoProducto.Cantidad += 1);
                    RefrescarListaPresupuestoProducto();
                }
            }
        }

        private void EliminarCantidadPresupuestoProducto_Button_Click(object sender, RoutedEventArgs e)
        {
            Button buttonSender = (Button)sender;
            if (buttonSender.DataContext is PresupuestoProducto)
            {
                PresupuestoProducto presupuestoProducto = (PresupuestoProducto)buttonSender.DataContext;
                if (presupuestoProducto.Cantidad > 1)
                {
                    ControladorPresupuestos.ModificarCantidadPresupuestoProducto(presupuestoProducto, presupuestoProducto.Cantidad -= 1);
                    RefrescarListaPresupuestoProducto();
                }
            }
        }

        private async void CantidadPresupuestoProducto_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBoxSender = (TextBox)sender;
            string texto = textBoxSender.Text;

            await Task.Delay(100);
            if (texto == textBoxSender.Text)
            {
                if (textBoxSender.DataContext is PresupuestoProducto)
                {
                    bool esInt = int.TryParse(((TextBox)sender).Text, out int cantidad);
                    if (esInt && cantidad > 0 && cantidad <= 99)
                    {
                        ControladorPresupuestos.ModificarCantidadPresupuestoProducto((PresupuestoProducto)textBoxSender.DataContext, cantidad);
                        RefrescarListaPresupuestoProducto();
                    }
                }
            }
        }
    }
}
