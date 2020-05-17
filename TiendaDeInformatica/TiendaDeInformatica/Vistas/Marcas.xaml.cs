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
    public partial class Marcas : UserControl
    {
        private Principal _principal;
        private bool modificandoListBoxTipoProducto = false;

        public Marcas(Principal principal)
        {
            InitializeComponent();
            _principal = principal;
        }

        private void Marcas_Vista_Loaded(object sender, RoutedEventArgs e)
        {
            // Cargar los Tipos de Productos de las Marcas en el ListBox
            modificandoListBoxTipoProducto = true;
            foreach (Producto producto in ControladorProductos.ObtenerListaDeProductos())
            {
                if (!TipoProducto_ListBox.Items.Contains(producto.Tipo))
                   TipoProducto_ListBox.Items.Add(producto.Tipo);
            }
            TipoProducto_ListBox.SelectAll();
            modificandoListBoxTipoProducto = false;

            RefrescarListaDeMarcas();
        }

        private void AgregarMarca_Button_Click(object sender, RoutedEventArgs e)
        {
            CaracteristicasMarca caracteristicasMarca = new CaracteristicasMarca(_principal, null, true);
            caracteristicasMarca.Owner = Application.Current.MainWindow;

            caracteristicasMarca.ShowDialog();
            RefrescarListaDeMarcas();
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

        private void Marcas_Vista_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AjustarFilasColumnas();
        }

        private void AjustarFilasColumnas()
        {
            if (itemsGrid != null)
            {
                // Cantidad de columnas a partir del ancho
                itemsGrid.Columns = (int)(Contenido_Grid.ActualWidth / 200);

                if (itemsGrid.Columns > 0)
                    // Calcular la cantidad de filas dependiendo de la cantidad de columnas y marcas
                    itemsGrid.Rows = (int)Math.Ceiling((decimal)Marcas_ListBox.Items.Count / (decimal)itemsGrid.Columns);
                else
                    // Hay una sola columna. Filas iguales a la cantidad de marcas
                    itemsGrid.Rows = Marcas_ListBox.Items.Count;

                // Alto de las filas para ajustar el ScrollBar vertical
                itemsGrid.Height = itemsGrid.Rows * 80;
            }
        }

        // ------------------------------------------------------ //
        //     Opciones al hacer click derecho sobre una marca    //
        // ------------------------------------------------------ //

        private void ModificarMarca_Click(object sender, RoutedEventArgs e)
        {
            Marca marca = Marcas_ListBox.SelectedItem as Marca;
            if (marca != null)
            {
                CaracteristicasMarca caracteristicasMarca = new CaracteristicasMarca(_principal, marca, true);
                caracteristicasMarca.Owner = Application.Current.MainWindow;

                caracteristicasMarca.ShowDialog();
                RefrescarListaDeMarcas();
            }
        }

        private void EliminarMarca_Click(object sender, RoutedEventArgs e)
        {
            _principal.OscurecerCompletamente(true);
            AlertaEliminarMarca_DialogHost.IsOpen = true;
        }

        //
        // Alerta al eliminar una marca
        //

        private void CancelarEliminarMarca_Button_Click(object sender, RoutedEventArgs e)
        {
            _principal.OscurecerCompletamente(false);
            AlertaEliminarMarca_DialogHost.IsOpen = false;
        }

        private void EliminarMarca_Button_Click(object sender, RoutedEventArgs e)
        {
            Marca marca = Marcas_ListBox.SelectedItem as Marca;
            if (marca != null)
            {
                ControladorMarcas.EliminarMarca(marca);
                RefrescarListaDeMarcas();

                AlertaEliminarMarca_DialogHost.IsOpen = false;
                _principal.OscurecerCompletamente(false);
                _ = _principal.MostrarMensajeEnSnackbar("Marca eliminada correctamente!");

                modificandoListBoxTipoProducto = true;
                // Tipos de productos seleccionados anteriormente
                List<TipoProducto?> tiposProductosAnteriores = new List<TipoProducto?>();
                foreach (TipoProducto tipoProductoSeleccionado in TipoProducto_ListBox.SelectedItems)
                    tiposProductosAnteriores.Add(tipoProductoSeleccionado);

                // Nuevos tipos de productos
                List<TipoProducto?> tiposProductosNuevos = new List<TipoProducto?>();
                foreach (Producto producto in ControladorProductos.ObtenerListaDeProductos())
                {
                    if (!tiposProductosNuevos.Contains(producto.Tipo))
                        tiposProductosNuevos.Add(producto.Tipo);
                }

                // Mostrar los nuevos tipos de productos
                TipoProducto_ListBox.Items.Clear();
                foreach (TipoProducto tipoProducto in tiposProductosNuevos)
                {
                    TipoProducto_ListBox.Items.Add(tipoProducto);
                    // Verificar si antes estaban seleccionados y seleccionarlos nuevamente
                    if (tiposProductosAnteriores.Contains(tipoProducto))
                        TipoProducto_ListBox.SelectedItems.Add(tipoProducto);
                }
                modificandoListBoxTipoProducto = false;
            }
        }

        // ------------------------------------------------------ //
        //              Refrescar la lista de marcas              //
        // ------------------------------------------------------ //

        private void RefrescarListaDeMarcas()
        {
            if (Marcas_Vista.IsLoaded)
            {
                Marcas_ListBox.Items.Clear();

                List<Marca> marcas = ControladorMarcas.ObtenerListaDeMarcas();
                List<Marca> resultados = OrdenarMarcas(FiltrarMarcasPorTipoProducto(BuscarMarca(marcas, BuscarMarca_TextBox.Text)));

                foreach (Marca marca in resultados)
                    Marcas_ListBox.Items.Add(marca);

                CantidadDeResultados_TextBlock.Text = Marcas_ListBox.Items.Count.ToString();
                AjustarFilasColumnas();
            }
        }

        // ------------------------------------------------------ //
        //                      Buscar marca                      //
        // ------------------------------------------------------ //

        private void BuscarMarca_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefrescarListaDeMarcas();
        }

        private List<Marca> BuscarMarca(List<Marca> marcas, string busqueda)
        {
            if (busqueda != "")
            {
                busqueda = TextHelper.QuitarTildes(busqueda).ToUpper();
                List<Marca> resultado = new List<Marca>();
                foreach (Marca marca in marcas)
                {
                    if (TextHelper.QuitarTildes(marca.Nombre).ToUpper().StartsWith(busqueda))
                        resultado.Add(marca);
                }
                return resultado;
            }
            return marcas;
        }

        // ------------------------------------------------------ //
        //                     Ordenar marcas                     //
        // ------------------------------------------------------ //

        private void OrdenarMarcas_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefrescarListaDeMarcas();
        }

        private void OrdenarMarcas_AscDesc_ToggleButton_CheckedUnchecked(object sender, RoutedEventArgs e)
        {
            RefrescarListaDeMarcas();
        }

        private List<Marca> OrdenarMarcas(List<Marca> marcas)
        {
            if (OrdenarMarcas_ComboBox.SelectedIndex == 0)
            {
                // Alfabéticamente
                marcas.Sort((x, y) => string.Compare(x.Nombre, y.Nombre));
            }
            else if (OrdenarMarcas_ComboBox.SelectedIndex == 1)
            {
                // Fecha de creación
                marcas.OrderBy(m => m.Id).ToList();
            }
            else if (OrdenarMarcas_ComboBox.SelectedIndex == 2)
            {
                // Cantidad de productos
                marcas.OrderBy(m => m.Productos.Count()).ToList();
            }

            if (OrdenarMarcas_AscDesc_ToggleButton.IsChecked.Value)
                marcas.Reverse();
            
            return marcas;
        }

        // ------------------------------------------------------ //
        //                     Filtrar marcas                     //
        // ------------------------------------------------------ //

        private List<Marca> FiltrarMarcasPorTipoProducto(List<Marca> marcas)
        {
            List<Marca> resultado = new List<Marca>();

            foreach(Marca marca in marcas)
            {
                if (marca.Productos.Count > 0)
                {
                    foreach (Producto producto in marca.Productos)
                    {
                        if (TipoProducto_ListBox.SelectedItems.Contains(producto.Tipo))
                        {
                            resultado.Add(marca);
                            break;
                        }
                    }
                }
                else if (SinProductos_CheckBox.IsChecked == true)
                    resultado.Add(marca);
            }
            return resultado;
        }

        private void TipoProducto_ListBox_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Deshabilitar el click derecho para seleccionar o deseleccionar un item del ListBox
            e.Handled = true;
        }

        private void TipoProducto_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(!modificandoListBoxTipoProducto)
                RefrescarListaDeMarcas();
        }

        private void SinProductos_CheckBox_CheckedUnchecked(object sender, RoutedEventArgs e)
        {
            if(!modificandoListBoxTipoProducto)
                RefrescarListaDeMarcas();
        }
    }
}
