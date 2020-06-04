using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TiendaDeInformatica.Controladores;
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
            if (ResumenPresupuesto_Vista.IsLoaded)
            {
                Presupuesto presupuesto = ControladorPresupuestos.ObtenerPresupuesto(_presupuestoId);
                this.DataContext = presupuesto;

                List<PresupuestoProducto> productos = OrdenarProductos(presupuesto.Productos);
                Productos_ListBox.Items.Clear();
                foreach (PresupuestoProducto presupuestoProducto in productos)
                    Productos_ListBox.Items.Add(presupuestoProducto);
            }
        }

        // ------------------------------------------------------ //
        //                     Ordenar productos                  //
        // ------------------------------------------------------ //

        private void OrdenarProductos_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefrescarListaPresupuestoProducto();
        }

        private void OrdenarProductos_AscDesc_ToggleButton_CheckedUnchecked(object sender, RoutedEventArgs e)
        {
            RefrescarListaPresupuestoProducto();
        }

        private List<PresupuestoProducto> OrdenarProductos(List<PresupuestoProducto> productos)
        {
            if (OrdenarProductos_ComboBox.SelectedIndex == 0)
            {
                // Incorporación
                productos.Sort((p1, p2) => p1.Id.CompareTo(p2.Id));
                productos.Reverse();
            }
            else if (OrdenarProductos_ComboBox.SelectedIndex == 1)
            {
                // Precio
                productos.Sort((p1, p2) => p1.MostrarPrecioProducto.CompareTo(p2.MostrarPrecioProducto));
            }
            else if (OrdenarProductos_ComboBox.SelectedIndex == 2)
            {
                // Unidades
                productos.Sort((p1, p2) => p1.Cantidad.CompareTo(p2.Cantidad));
            }

            if (!OrdenarProductos_AscDesc_ToggleButton.IsChecked.Value)
                productos.Reverse();

            return productos;
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

        private void ModificarPresupuesto_Button_Click(object sender, RoutedEventArgs e)
        {
            CaracteristicasPresupuesto caracteristicasPresupuesto = new CaracteristicasPresupuesto(_principal, ControladorPresupuestos.ObtenerPresupuesto(_presupuestoId));
            caracteristicasPresupuesto.Owner = Application.Current.MainWindow;

            caracteristicasPresupuesto.ShowDialog();
            this.DataContext = ControladorPresupuestos.ObtenerPresupuesto(_presupuestoId);
        }
    }
}
