using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class Productos : UserControl
    {
        private Principal _principal;
        TipoProducto? _tipoProducto;

        public Productos(Principal principal, TipoProducto? tipoProducto)
        {
            InitializeComponent();
            _principal = principal;
            _tipoProducto = tipoProducto;

            if (tipoProducto != null)
            {
                TituloTipoProducto_TextBlock.Text = tipoProducto.GetEnumDescription();
                TituloTipoProducto_StackPanel.Visibility = Visibility.Visible;
            }
        }

        private void Vista_Productos_Loaded(object sender, RoutedEventArgs e)
        {
            RefrescarListaDeProductos();
        }

        private void TituloProductos_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (_tipoProducto != null)
                _principal.MenuIzquierdo.SelectedIndex = 6;
        }

        private void AgregarProducto_Button_Click(object sender, RoutedEventArgs e)
        {
            CaracteristicasProducto caracteristicasProducto = new CaracteristicasProducto(_principal, null, _tipoProducto);
            caracteristicasProducto.Owner = Application.Current.MainWindow;

            caracteristicasProducto.ShowDialog();
            RefrescarListaDeProductos();
        }


        // ------------------------------------------------------ //
        //    Opciones al hacer click derecho sobre un producto   //
        // ------------------------------------------------------ //

        private void ModificarProducto_Click(object sender, RoutedEventArgs e)
        {
            Producto producto = Productos_ListBox.SelectedItem as Producto;
            if (producto != null)
            {
                CaracteristicasProducto caracteristicasProducto = new CaracteristicasProducto(_principal, producto, null);
                caracteristicasProducto.Owner = Application.Current.MainWindow;

                caracteristicasProducto.ShowDialog();
                RefrescarListaDeProductos();
            }
        }

        private void EliminarProducto_Click(object sender, RoutedEventArgs e)
        {
            _principal.OscurecerCompletamente(true);
            AlertaEliminarProducto_DialogHost.IsOpen = true;
        }

        //
        // Alerta al eliminar un producto
        //

        private void CancelarEliminarProducto_Button_Click(object sender, RoutedEventArgs e)
        {
            _principal.OscurecerCompletamente(false);
            AlertaEliminarProducto_DialogHost.IsOpen = false;
        }

        private void EliminarProducto_Button_Click(object sender, RoutedEventArgs e)
        {
            Producto producto = Productos_ListBox.SelectedItem as Producto;
            if (producto != null)
            {
                ControladorProductos.EliminarProducto(producto);
                RefrescarListaDeProductos();

                AlertaEliminarProducto_DialogHost.IsOpen = false;
                _principal.OscurecerCompletamente(false);
                _ = _principal.MostrarMensajeEnSnackbar("Producto eliminado correctamente!");
            }
        }


        // ------------------------------------------------------ //
        //   Ajustar las columnas y filas de la lista de marcas   //
        // ------------------------------------------------------ //

        private UniformGrid itemsGrid;

        private void Items_UniformGrid_Loaded(object sender, RoutedEventArgs e)
        {
            itemsGrid = sender as UniformGrid;
            AjustarFilasColumnas();
        }

        private void Vista_Productos_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AjustarFilasColumnas();
        }

        private void AjustarFilasColumnas()
        {
            if (itemsGrid != null)
            {
                // Cantidad de columnas a partir del ancho
                itemsGrid.Columns = (int)(Contenido_Grid.ActualWidth / 190);

                if (itemsGrid.Columns > 0)
                    // Calcular la cantidad de filas dependiendo de la cantidad de columnas y productos
                    itemsGrid.Rows = (int)Math.Ceiling((decimal)Productos_ListBox.Items.Count / (decimal)itemsGrid.Columns);
                else
                    // Hay una sola columna. Filas iguales a la cantidad de productos
                    itemsGrid.Rows = Productos_ListBox.Items.Count;

                // Alto de las filas para ajustar el ScrollBar vertical
                itemsGrid.Height = itemsGrid.Rows * 298;
            }
        }

        // ------------------------------------------------------ //
        //             Refrescar la lista de productos            //
        // ------------------------------------------------------ //

        private void RefrescarListaDeProductos()
        {
            if (Vista_Productos.IsLoaded)
            {
                Productos_ListBox.Items.Clear();

                List<Producto> productos = new List<Producto>();
                if (_tipoProducto==null)
                    productos = ControladorProductos.ObtenerListaDeProductos();
                else
                    productos = ControladorProductos.ObtenerListaDeProductos().Where(p => p.Tipo == _tipoProducto).ToList();

                List<Producto> resultados = BuscarProducto(productos, BuscarProducto_TextBox.Text);
                foreach (Producto producto in resultados)
                    Productos_ListBox.Items.Add(producto);

                CantidadDeResultados_TextBlock.Text = Productos_ListBox.Items.Count.ToString();
                AjustarFilasColumnas();
            }
        }

        // ------------------------------------------------------ //
        //                     Buscar producto                    //
        // ------------------------------------------------------ //

        private void BuscarProducto_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefrescarListaDeProductos();
        }

        private List<Producto> BuscarProducto(List<Producto> productos, string busqueda)
        {
            if (busqueda != "")
            {
                busqueda = TextHelper.QuitarTildes(busqueda).ToUpper();
                List<Producto> resultado = new List<Producto>();
                foreach (Producto producto in productos)
                {
                    if (TextHelper.QuitarTildes(producto.Modelo).ToUpper().Contains(busqueda))
                        resultado.Add(producto);
                }
                return resultado;
            }
            return productos;
        }

        // ------------------------------------------------------ //
        //              Agregar Producto a Presupuesto            //
        // ------------------------------------------------------ //

        private void Productos_ListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Productos_ListBox.SelectedItem is Producto)
                _principal.AgregarProductoAPresupuesto(Productos_ListBox.SelectedItem as Producto);
        }
    }
}
