using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using TiendaDeInformatica.Controladores;
using TiendaDeInformatica.Modelos;

namespace TiendaDeInformatica.Vistas
{
    /// <summary>
    /// Lógica de interacción para ConfiguracionGuiada.xaml
    /// </summary>
    public partial class ConfiguracionGuiada : UserControl
    {
        private Principal _principal;
        public Presupuesto _presupuestoSeleccionado;
        private TipoProducto[] tipoProductos;

        private List<Producto> ProductosSeleccionados = new List<Producto>();

        public ConfiguracionGuiada(Principal principal, int presupuestoSeleccionadoId)
        {
            InitializeComponent();
            _principal = principal;
            RefrescarProductos();

            if (presupuestoSeleccionadoId != -1)
                _presupuestoSeleccionado = ControladorPresupuestos.ObtenerPresupuesto(presupuestoSeleccionadoId);
        }

        private void ConfiguracionGuiada_Vista_Loaded(object sender, RoutedEventArgs e)
        {
            // Obtener el ScrollViewer del ListBox TipoProducto
            Border border = (Border)VisualTreeHelper.GetChild(TipoProducto_ListBox, 0);
            scrollViewerTipoProducto = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);

            // Cargar el enum TipoProducto en el ListBox
            tipoProductos = (TipoProducto[])Enum.GetValues(typeof(TipoProducto));
            List<string> tipoProductosConEspacios = new List<string>(tipoProductos.Select(v => v.ToString().Replace("_", " ")));
            TipoProducto_ListBox.ItemsSource = tipoProductosConEspacios;
            TipoProducto_ListBox.SelectedIndex = 0;

            // Cambiar el estado de la alerta si no hay un presupuesto seleccionado
            if (_presupuestoSeleccionado == null)
                CambiarEstadoAlertaPresupuestoSeleccionado(true);
            else
                ConfiguracionFinalizada_MoverAlPresupuesto_Button.IsEnabled = true;
        }

        private void Alerta_BotonEntendido_Click(object sender, RoutedEventArgs e)
        {
            // Cerrar alerta
            CambiarEstadoAlertaPresupuestoSeleccionado(false);
        }

        public void CambiarEstadoAlertaPresupuestoSeleccionado(bool estado)
        {
            Alerta_Snackbar.IsActive = estado;
            Espacio_Snackbar.IsActive = estado;
            ConfiguracionFinalizada_MoverAlPresupuesto_Button.IsEnabled = !estado;
        }

        // ------------------------------------------------------ //
        //                    ListBox TipoProducto                //
        // ------------------------------------------------------ //

        private TipoProducto tipoProductoActual;

        private void TipoProducto_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = TipoProducto_ListBox.SelectedIndex;
            if (index != -1)
            {
                tipoProductoActual = (TipoProducto)index;
                ActualizarBotones(false);
            }
        }

        private void TipoProducto_UniformGrid_Loaded(object sender, RoutedEventArgs e)
        {
            // Ajustar la cantidad de columnas del ListBox a la cantidad de TipoProductos.
            (sender as UniformGrid).Columns = tipoProductos.Count();
        }

        // ---------------------------------- //
        //   Mover el ListBox con el mouse    //
        // ---------------------------------- //

        private ScrollViewer scrollViewerTipoProducto;
        private Point scrollMousePoint = new Point();
        private double horizontalOffset = 1;

        private void TipoProducto_ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Guardar la posicion horizontal del ListBox, la posicion del mouse y capturar el ScrollViewer con el mouse.
            scrollMousePoint = e.GetPosition(scrollViewerTipoProducto);
            horizontalOffset = scrollViewerTipoProducto.HorizontalOffset;
            scrollViewerTipoProducto.CaptureMouse();
        }

        private void TipoProducto_ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            // Mover la posicio horizontal del ListBox.
            if (scrollViewerTipoProducto.IsMouseCaptured)
                scrollViewerTipoProducto.ScrollToHorizontalOffset(horizontalOffset + (scrollMousePoint.X - e.GetPosition(scrollViewerTipoProducto).X));
        }

        private void TipoProducto_ListBox_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Remover la captura del mouse en el SrollViewer.
            scrollViewerTipoProducto.ReleaseMouseCapture();

            // Determinar a partir de la posicion inicial si fue un click sin movimiento.
            if (scrollMousePoint == Mouse.GetPosition(scrollViewerTipoProducto))
            {
                // Obtener el Item del ListBox que fue clickeado y seleccionarlo.
                ListBox listBox = sender as ListBox;
                if (listBox != null)
                {
                    var element = VisualTreeHelper.HitTest(listBox, scrollMousePoint).VisualHit;
                    if (element.GetType() != typeof(ScrollViewer))
                    {
                        while (element.GetType() != typeof(ListBoxItem))
                            element = VisualTreeHelper.GetParent(element);

                        (element as ListBoxItem).IsSelected = true;
                        RefrescarProductos();
                    }
                }
            }
        }

        // ------------------------------------------------------ //
        //         Botones Anterior, Siguiente y Finalizar        //
        // ------------------------------------------------------ //

        private void ActualizarBotones(bool productoSeleccionado)
        {
            if (tipoProductoActual == tipoProductos.First())
                // Primer TipoProducto, no se puede retroceder.
                Anterior_Button.IsEnabled = false;
            else
            {
                // Se puede retroceder.
                Anterior_Button.IsEnabled = true;
                if (tipoProductoActual == tipoProductos.Last())
                {
                    // Ultimo TipoProducto, se puede finalizar.
                    Siguiente_PackIcon.Visibility = Visibility.Collapsed;
                    Siguiente_TextBlock.Text = "FINALIZAR";
                }
                else
                {
                    // Se puede avanzar.
                    Siguiente_PackIcon.Visibility = Visibility.Visible;
                    Siguiente_TextBlock.Text = "SIGUIENTE";
                }
            }

            // Cambiar el estado del botón para avanzar o finalizar dependiendo si hay un producto seleccionado.
            SiguienteFinalizar_Button.IsEnabled = productoSeleccionado;
        }

        private void Anterior_Button_Click(object sender, RoutedEventArgs e)
        {
            // Eliminar todos los productos del TipoProducto actual antes de retroceder.
            ProductosSeleccionados.RemoveAll(p => p.Tipo == tipoProductoActual);

            TipoProducto_ListBox.SelectedIndex -= 1;
            RefrescarProductos();
        }

        private void SiguienteFinalizar_Button_Click(object sender, RoutedEventArgs e)
        {
            PasarAlSiguientePasoOFinalizar();
        }

        // ------------------------------------------------------ //
        //                     ListBox Productos                  //
        // ------------------------------------------------------ //

        private void Productos_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Cambiar el estado del botón dependiendo si hay un producto seleccionado o no.
            Producto producto = Productos_ListBox.SelectedItem as Producto;
            if (producto != null)
                SiguienteFinalizar_Button.IsEnabled = true;
            else
                SiguienteFinalizar_Button.IsEnabled = false;
        }

        private void Productos_ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PasarAlSiguientePasoOFinalizar();
        }

        private void PasarAlSiguientePasoOFinalizar()
        {
            Producto productoSeleccionado = Productos_ListBox.SelectedItem as Producto;
            if (productoSeleccionado != null)
            {
                // Eliminar todos los productos del TipoProducto actual antes de avanzar.
                ProductosSeleccionados.RemoveAll(p => p.Tipo == tipoProductoActual);

                ProductosSeleccionados.Add(productoSeleccionado);
                if (tipoProductoActual == tipoProductos.Last())
                {
                    // Cambiar al resumen de la configuración.
                    SeleccionComponentes_Grid.Visibility = Visibility.Collapsed;
                    ConfiguracionFinalizada_Grid.Visibility = Visibility.Visible;

                    ConfiguracionFinalizada_Productos_ListBox.Items.Clear();
                    foreach (Producto producto in ProductosSeleccionados)
                        ConfiguracionFinalizada_Productos_ListBox.Items.Add(producto);

                    TituloResumen_StackPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    // Avanzar al siguiente TipoProducto.
                    TipoProducto_ListBox.SelectedIndex += 1;
                    RefrescarProductos();
                }
            }
        }

        private void TituloConfiguracionGuiada_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (ConfiguracionFinalizada_Grid.Visibility == Visibility.Visible)
            {
                TituloResumen_StackPanel.Visibility = Visibility.Collapsed;
                ConfiguracionFinalizada_Grid.Visibility = Visibility.Collapsed;
                SeleccionComponentes_Grid.Visibility = Visibility.Visible;
            }
        }

        // --------------------------------- //
        //    Ajustar las filas y columnas   //
        // --------------------------------- //

        private UniformGrid productosUniformGrid;

        private void Productos_UniformGrid_Loaded(object sender, RoutedEventArgs e)
        {
            productosUniformGrid = sender as UniformGrid;
            AjustarFilasColumnas();
        }

        private void ConfiguracionGuiada_Vista_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AjustarFilasColumnas();
        }

        private void AjustarFilasColumnas()
        {
            if (productosUniformGrid != null)
            {
                // Cantidad de columnas a partir del ancho
                productosUniformGrid.Columns = (int)(Contenido_Grid.ActualWidth / 184);

                if (productosUniformGrid.Columns > 0)
                    // Calcular la cantidad de filas dependiendo de la cantidad de columnas y productos
                    productosUniformGrid.Rows = (int)Math.Ceiling((decimal)Productos_ListBox.Items.Count / (decimal)productosUniformGrid.Columns);
                else
                    // Hay una sola columna. Filas iguales a la cantidad de productos
                    productosUniformGrid.Rows = Productos_ListBox.Items.Count;

                // Alto de las filas para ajustar el ScrollBar vertical
                productosUniformGrid.Height = productosUniformGrid.Rows * 298;
            }
        }


        // --------------------------------- //
        //          prueba                   //
        // --------------------------------- //
        private void RefrescarProductos()
        {
            Productos_ListBox.Items.Clear();
            foreach (Producto producto in ControladorProductos.ObtenerListaDeProductos().Where(p => p.Tipo == tipoProductoActual))
            {
                Productos_ListBox.Items.Add(producto);
            }
        }
    }
}
