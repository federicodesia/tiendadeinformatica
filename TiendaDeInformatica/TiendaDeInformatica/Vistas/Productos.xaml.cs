﻿using System;
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
            // Colocar el Slider del filtro de precio al valor máximo
            List<Producto> productos = new List<Producto>();
            if (_tipoProducto == null)
                productos = ControladorProductos.ObtenerListaDeProductos();
            else
                productos = ControladorProductos.ObtenerListaDeProductos().Where(p => p.Tipo == _tipoProducto).ToList();

            if (productos.Count > 0)
            {
                double productoMasAlto = double.Parse(productos.Max(p => p.Precio.ToString()));
                if (productoMasAlto > 100000)
                {
                    FiltroPrecio_RangeSlider.Maximum = productoMasAlto;
                    FiltroPrecio_RangeSlider.UpperValue = productoMasAlto;
                }
            }

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

        private void AgregarPresupuestoProducto_Click(object sender, RoutedEventArgs e)
        {
            if (Productos_ListBox.SelectedItem is Producto)
                _principal.AgregarProductoAPresupuesto(Productos_ListBox.SelectedItem as Producto);
        }

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

                List<Producto> resultados = OrdenarProductos(FiltrarProductosPorPrecio(BuscarProducto(productos, BuscarProducto_TextBox.Text)));
                foreach (Producto producto in resultados)
                    Productos_ListBox.Items.Add(producto);

                CantidadDeResultados_TextBlock.Text = Productos_ListBox.Items.Count.ToString();
                AjustarFilasColumnas();

                // Cambiar el valor máximo del filtro de precio
                if (productos.Count > 0)
                {
                    int productoMasAlto = (int)Math.Ceiling(productos.Max(p => p.Precio));
                    if (productoMasAlto > 100000)
                        FiltroPrecio_RangeSlider.Maximum = productoMasAlto;
                    else
                        FiltroPrecio_RangeSlider.Maximum = 100000;
                }
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
        //                    Ordenar productos                   //
        // ------------------------------------------------------ //

        private void OrdenarProductos_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefrescarListaDeProductos();
        }

        private void OrdenarProductos_AscDesc_ToggleButton_CheckedUnchecked(object sender, RoutedEventArgs e)
        {
            RefrescarListaDeProductos();
        }

        private List<Producto> OrdenarProductos(List<Producto> productos)
        {
            if (OrdenarProductos_ComboBox.SelectedIndex == 0)
            {
                // Precio
                productos.OrderBy(p => p.Precio).Reverse();
            }
            else if (OrdenarProductos_ComboBox.SelectedIndex == 1)
            {
                // Alfabéticamente
                productos.Sort((x, y) => string.Compare(x.MostrarTipoProductoMarcaModelo, y.MostrarTipoProductoMarcaModelo));
            }
            else if (OrdenarProductos_ComboBox.SelectedIndex == 1)
            {
                // Fecha de creación
                productos.OrderBy(m => m.Id).ToList();
            }

            if (OrdenarProductos_AscDesc_ToggleButton.IsChecked.Value)
                productos.Reverse();

            return productos;
        }

        // ------------------------------------------------------ //
        //                    Filtrar presupuestos                //
        // ------------------------------------------------------ //

        private void FiltroPrecio_RangeSlider_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RefrescarListaDeProductos();
        }

        private List<Producto> FiltrarProductosPorPrecio(List<Producto> productos)
        {
            return productos.Where(p => p.Precio >= decimal.Parse(FiltroPrecio_RangeSlider.LowerValue.ToString())
            && p.Precio <= decimal.Parse(FiltroPrecio_RangeSlider.UpperValue.ToString())).ToList();
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
