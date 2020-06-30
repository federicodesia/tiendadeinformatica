﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using TiendaDeInformatica.Modelos;

namespace TiendaDeInformatica.Vistas
{
    /// <summary>
    /// Lógica de interacción para ConfiguracionGuiada.xaml
    /// </summary>
    public partial class ConfiguracionGuiada : UserControl
    {
        private Principal _principal;
        private TipoProducto[] tipoProductos;

        public ConfiguracionGuiada(Principal principal)
        {
            InitializeComponent();
            _principal = principal;
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
        }

        // ------------------------------------------------------ //
        //                    ListBox TipoProducto                //
        // ------------------------------------------------------ //

        private TipoProducto tipoProductoActual;

        private void TipoProducto_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = TipoProducto_ListBox.SelectedIndex;
            if (index != -1)
                tipoProductoActual = (TipoProducto)index;
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
                    }
                }
            }
        }

        // ------------------------------------------------------ //
        //                     ListBox Productos                  //
        // ------------------------------------------------------ //

        private void Productos_ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TipoProducto_ListBox.SelectedIndex += 1;
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
    }
}
