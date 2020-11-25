using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using TiendaDeInformatica.Controladores;
using TiendaDeInformatica.Modelos;
using TiendaDeInformatica.Vistas.Reglas_de_Validacion;

namespace TiendaDeInformatica.Vistas
{
    /// <summary>
    /// Lógica de interacción para ConfiguracionGuiada.xaml
    /// </summary>
    public partial class ConfiguracionGuiada : UserControl
    {
        private Principal _principal;
        public Presupuesto _presupuestoSeleccionado;

        private Producto motherboardSeleccionada;
        private Presupuesto presupuesto;

        private TipoProducto tipoProductoActual;
        private TipoProducto[] tipoProductos;

        public ConfiguracionGuiada(Principal principal, int presupuestoSeleccionadoId)
        {
            InitializeComponent();
            _principal = principal;

            try
            {
                if (presupuestoSeleccionadoId != -1)
                    _presupuestoSeleccionado = ControladorPresupuestos.ObtenerPresupuesto(presupuestoSeleccionadoId);
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }

            RefrescarListaDeProductos();
        }

        private void ConfiguracionGuiada_Vista_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Obtener el ScrollViewer del ListBox TipoProducto
                Border border = (Border)VisualTreeHelper.GetChild(TipoProducto_ListBox, 0);
                scrollViewerTipoProducto = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);

                // Cargar el enum TipoProducto en el ListBox
                tipoProductos = (TipoProducto[])Enum.GetValues(typeof(TipoProducto));
                foreach (TipoProducto tipoProducto in tipoProductos)
                {
                    TipoProductoProducto tipoProductoProducto = new TipoProductoProducto()
                    {
                        Tipo = tipoProducto,
                        Producto = null
                    };
                    TipoProducto_ListBox.Items.Add(tipoProductoProducto);
                }
                TipoProducto_ListBox.SelectedIndex = 0;

                // Cambiar el estado de la alerta si no hay un presupuesto seleccionado
                if (_presupuestoSeleccionado == null)
                    CambiarEstadoAlertaPresupuestoSeleccionado(true);
                else
                    ConfiguracionFinalizada_MoverAlPresupuesto_Button.IsEnabled = true;
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
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

        private void TipoProducto_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int index = TipoProducto_ListBox.SelectedIndex;
                if (index != -1)
                {
                    TipoProducto_ListBox.ScrollIntoView(TipoProducto_ListBox.Items[index]);

                    tipoProductoActual = (TipoProducto)index;
                    ActualizarBotones(false);
                    RefrescarListaDeProductos();
                }
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
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
            try
            {
                // Guardar la posicion horizontal del ListBox, la posicion del mouse y capturar el ScrollViewer con el mouse.
                scrollMousePoint = e.GetPosition(scrollViewerTipoProducto);
                horizontalOffset = scrollViewerTipoProducto.HorizontalOffset;
                scrollViewerTipoProducto.CaptureMouse();
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private void TipoProducto_ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                // Mover la posicion horizontal del ListBox.
                if (scrollViewerTipoProducto.IsMouseCaptured)
                    scrollViewerTipoProducto.ScrollToHorizontalOffset(horizontalOffset + (scrollMousePoint.X - e.GetPosition(scrollViewerTipoProducto).X));
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private void TipoProducto_ListBox_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
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
                            ModificarTipoProductoProducto(null);
                        }
                    }
                }
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        // ------------------------------------------------------ //
        //         Botones Anterior, Siguiente y Finalizar        //
        // ------------------------------------------------------ //

        private void ActualizarBotones(bool productoSeleccionado)
        {
            try
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
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private void Anterior_Button_Click(object sender, RoutedEventArgs e)
        {
            TipoProducto_ListBox.SelectedIndex -= 1;
            ModificarTipoProductoProducto(null);
        }

        private void SiguienteFinalizar_Button_Click(object sender, RoutedEventArgs e)
        {
            SeleccionarProducto();
        }

        // ------------------------------------------------------ //
        //                     ListBox Productos                  //
        // ------------------------------------------------------ //

        private void Productos_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Cambiar el estado del botón dependiendo si hay un producto seleccionado o no.
                Producto producto = Productos_ListBox.SelectedItem as Producto;
                if (producto != null)
                    SiguienteFinalizar_Button.IsEnabled = true;
                else
                    SiguienteFinalizar_Button.IsEnabled = false;
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private void Productos_ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SeleccionarProducto();
        }

        private void SeleccionarProducto()
        {
            try
            {
                Producto productoSeleccionado = Productos_ListBox.SelectedItem as Producto;
                if (productoSeleccionado != null)
                {
                    if (tipoProductoActual == tipoProductos.First())
                    {
                        motherboardSeleccionada = Productos_ListBox.SelectedItem as Producto;
                    }
                    ModificarTipoProductoProducto(productoSeleccionado);
                    PasarAlSiguientePasoOFinalizar();
                }
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private void ModificarTipoProductoProducto(Producto producto)
        {
            try
            {
                int index = (int)tipoProductoActual;

                // Eliminar todos los Productos de los siguientes TipoProductoProducto.
                foreach (TipoProducto tipoProducto in tipoProductos.Where(tp => (int)tp > index))
                {
                    int tipoProductoIndex = (int)tipoProducto;
                    TipoProductoProducto tipoProductoProducto1 = TipoProducto_ListBox.Items[tipoProductoIndex] as TipoProductoProducto;
                    tipoProductoProducto1.Producto = null;
                    TipoProducto_ListBox.Items[tipoProductoIndex] = tipoProductoProducto1;
                }

                // Agregar el Producto al TipoProductoProducto correspondiente.
                TipoProductoProducto tipoProductoProducto = TipoProducto_ListBox.Items[index] as TipoProductoProducto;
                tipoProductoProducto.Producto = producto;
                TipoProducto_ListBox.Items[index] = tipoProductoProducto;
                TipoProducto_ListBox.SelectedIndex = index;
                TipoProducto_ListBox.Items.Refresh();
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private void PasarAlSiguientePasoOFinalizar()
        {
            try
            {
                if (tipoProductoActual == tipoProductos.Last())
                {
                    // Cambiar al resumen de la configuración.
                    SeleccionComponentes_Grid.Visibility = Visibility.Collapsed;
                    ConfiguracionFinalizada_Grid.Visibility = Visibility.Visible;
                    TituloResumen_StackPanel.Visibility = Visibility.Visible;

                    // Mostrar los productos seleccionados en el ListBox.
                    presupuesto = new Presupuesto()
                    {
                        Productos = new List<PresupuestoProducto>()
                    };

                    ConfiguracionFinalizada_Productos_ListBox.Items.Clear();
                    foreach (TipoProductoProducto tipoProductoProducto in TipoProducto_ListBox.Items)
                    {
                        if (tipoProductoProducto.Producto != null)
                        {
                            PresupuestoProducto presupuestoProducto = new PresupuestoProducto()
                            {
                                Presupuesto = presupuesto,
                                Producto = tipoProductoProducto.Producto,
                                Cantidad = 1
                            };
                            presupuesto.Productos.Add(presupuestoProducto);
                            ConfiguracionFinalizada_Productos_ListBox.Items.Add(presupuestoProducto.Producto);
                        }
                    }
                    ConfiguracionFinalizada_Presupuesto.DataContext = presupuesto;
                }
                else
                {
                    // Avanzar al siguiente TipoProducto.
                    TipoProducto_ListBox.SelectedIndex += 1;
                }
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private void ConfiguracionFinalizada_MoverAlPresupuesto_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (PresupuestoProducto presupuestoProducto in presupuesto.Productos)
                {
                    if (presupuestoProducto == presupuesto.Productos.Last())
                        _principal.AgregarProductoAPresupuesto(presupuestoProducto.Producto, true);
                    else
                        _principal.AgregarProductoAPresupuesto(presupuestoProducto.Producto, false);
                }
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private void TituloConfiguracionGuiada_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (ConfiguracionFinalizada_Grid.Visibility == Visibility.Visible)
            {
                TituloResumen_StackPanel.Visibility = Visibility.Collapsed;
                ConfiguracionFinalizada_Grid.Visibility = Visibility.Collapsed;
                SeleccionComponentes_Grid.Visibility = Visibility.Visible;
                ModificarTipoProductoProducto(null);
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
            try
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
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        // ------------------------------------------------------ //
        //      Refrescar la lista de productos compatibles       //
        // ------------------------------------------------------ //

        private void RefrescarListaDeProductos()
        {
            try
            {
                Productos_ListBox.Items.Clear();
                foreach (Producto producto in ObtenerProductosCompatibles())
                    Productos_ListBox.Items.Add(producto);
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private List<Producto> Compatibilidad()
        {
            // Función no utilizada.
            try
            {
                List<Producto> productos = new List<Producto>();
                if ((int)tipoProductoActual == 0)
                {
                    return ControladorProductos.ObtenerListaDeProductos().Where(p => p.Tipo == tipoProductoActual).ToList();
                }
                else
                {
                    foreach (Producto producto in ControladorProductos.ObtenerListaDeProductos().Where(p => p.Tipo == tipoProductoActual))
                    {
                        foreach (ProductoValor productoValor in producto.Valores)
                        {
                            if (producto.Valores.Count() > 0)
                            {
                                if (motherboardSeleccionada.Valores.Any(p => p.Valor.Nombre == productoValor.Valor.Nombre))
                                    if (productos.Contains(producto) == false)
                                        productos.Add(producto);
                            }
                            else
                            {
                                productos.Add(producto);
                            }
                        }
                    }
                    return productos;
                }
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
                return new List<Producto>();
            }
        }

        private List<Producto> ObtenerProductosCompatibles()
        {
            try
            {
                if ((int)tipoProductoActual != 0)
                {
                    List<Producto> productosCompatibles = new List<Producto>();
                    if (motherboardSeleccionada != null)
                    {
                        List<int> atributosMotherboard = new List<int>();
                        foreach (Atributo atributoMotherboard in ControladorAtributos.ObtenerListaDeAtributosAsociadosATipoProducto(0))
                            atributosMotherboard.Add(atributoMotherboard.Id);

                        List<int> atributosTipoProductoActual = new List<int>();
                        foreach (Atributo atributoTipoProductoActual in ControladorAtributos.ObtenerListaDeAtributosAsociadosATipoProducto(tipoProductoActual))
                            atributosTipoProductoActual.Add(atributoTipoProductoActual.Id);

                        List<int> atributosEnComun = atributosMotherboard.Intersect(atributosTipoProductoActual).ToList();

                        List<Producto> productosTipoProductoActual = ControladorProductos.ObtenerListaDeProductosPorTipoProducto(tipoProductoActual);
                        foreach (Producto producto in productosTipoProductoActual)
                        {
                            bool compatible = true;
                            foreach (int atributoId in atributosEnComun)
                            {
                                List<int> valoresMotherboardId = new List<int>();
                                foreach (ProductoValor productoValorMotherboard in motherboardSeleccionada.Valores.Where(v => v.Valor.AtributoId == atributoId))
                                    valoresMotherboardId.Add(productoValorMotherboard.ValorId);

                                List<int> valoresProductoId = new List<int>();
                                foreach (ProductoValor productoValor in producto.Valores.Where(v => v.Valor.AtributoId == atributoId))
                                    valoresProductoId.Add(productoValor.ValorId);

                                List<int> valoresEnComun = valoresMotherboardId.Intersect(valoresProductoId).ToList();
                                if (valoresEnComun.Count == 0)
                                    compatible = false;
                            }

                            if (compatible)
                                productosCompatibles.Add(producto);
                        }
                    }
                    return productosCompatibles;
                }
                return ControladorProductos.ObtenerListaDeProductosPorTipoProducto(tipoProductoActual);
            }
            catch (Exception error)
            {
                _ = _principal.MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
                return new List<Producto>();
            }
        }
    }
}