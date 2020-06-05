﻿using System;
using System.Collections;
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
    public partial class Configuracion : UserControl
    {
        private Principal _principal;

        public Configuracion(Principal principal)
        {
            InitializeComponent();
            _principal = principal;
        }

        private void Configuracion_Vista_Loaded(object sender, RoutedEventArgs e)
        {
            TipoProducto[] tipoProductos = (TipoProducto[])Enum.GetValues(typeof(TipoProducto));
            foreach (TipoProducto tipoProducto in tipoProductos)
                TipoProducto_ListBox.Items.Add(tipoProducto);

            RefrescarListaDeAtributos();
        }

        private void Alerta_BotonEntendido_Click(object sender, RoutedEventArgs e)
        {
            Alerta_Snackbar.IsActive = false;
            Espacio_Snackbar.IsActive = false;
        }

        // ------------------------------------------------------ //
        //                    Agregar un atributo                 //
        // ------------------------------------------------------ //

        private void AgregarAtributo_Button_Click(object sender, RoutedEventArgs e)
        {
            CaracteristicasAtributo caracteristicasAtributo = new CaracteristicasAtributo(_principal, this, null);
            AbrirCaracteristicasAtributoCentrado(caracteristicasAtributo);

            RefrescarListaDeAtributos();
        }

        private void AbrirCaracteristicasAtributoCentrado(CaracteristicasAtributo caracteristicasAtributo)
        {
            caracteristicasAtributo.Owner = Application.Current.MainWindow;

            caracteristicasAtributo.WindowStartupLocation = WindowStartupLocation.Manual;
            Point point = Atributos_Card.PointToScreen(new Point(Atributos_Card.ActualWidth / 2, Atributos_Card.ActualHeight / 2));
            caracteristicasAtributo.Left = point.X - (caracteristicasAtributo.Width / 2);
            caracteristicasAtributo.Top = point.Y - (caracteristicasAtributo.Height / 2);

            caracteristicasAtributo.ShowDialog();
        }

        // ------------------------------------------------------ //
        //              Refrescar la lista de atributos           //
        // ------------------------------------------------------ //

        private void RefrescarListaDeAtributos()
        {
            if (Configuracion_Vista.IsLoaded)
            {
                Atributos_ListBox.Items.Clear();
                foreach (Atributo atributo in ControladorAtributos.ObtenerListaDeAtributos())
                    Atributos_ListBox.Items.Add(atributo);
            }
        }

        // ------------------------------------------------------ //
        //   Opciones al hacer click derecho sobre un atributo    //
        // ------------------------------------------------------ //

        private void ModificarAtributo_Click(object sender, RoutedEventArgs e)
        {
            Atributo atributo = Atributos_ListBox.SelectedItem as Atributo;
            if (atributo != null)
            {
                CaracteristicasAtributo caracteristicasAtributo = new CaracteristicasAtributo(_principal, this, atributo);
                AbrirCaracteristicasAtributoCentrado(caracteristicasAtributo);

                RefrescarListaDeAtributos();
            }
        }

        private void EliminarAtributo_Click(object sender, RoutedEventArgs e)
        {
            _principal.OscurecerCompletamente(true);
            AlertaEliminarAtributo_DialogHost.IsOpen = true;
        }

        //
        // Alerta al eliminar un atributo
        //

        private void CancelarEliminarAtributo_Button_Click(object sender, RoutedEventArgs e)
        {
            _principal.OscurecerCompletamente(false);
            AlertaEliminarAtributo_DialogHost.IsOpen = false;
        }

        private void EliminarAtributo_Button_Click(object sender, RoutedEventArgs e)
        {
            Atributo atributo = Atributos_ListBox.SelectedItem as Atributo;
            if (atributo != null)
            {
                ControladorAtributos.EliminarAtributo(atributo);
                RefrescarListaDeAtributos();

                _principal.OscurecerCompletamente(false);
                AlertaEliminarAtributo_DialogHost.IsOpen = false;
                _ = _principal.MostrarMensajeEnSnackbar("Atributo eliminado correctamente!");
            }
        }

        // ------------------------------------------------------ //
        //                  Cambios de selección                  //
        // ------------------------------------------------------ //

        private bool editandoListBoxTipoProducto = false;

        private void Atributos_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            editandoListBoxTipoProducto = true;
            Atributo atributo = Atributos_ListBox.SelectedItem as Atributo;
            if (atributo != null)
            {
                Atributo atributoActualizado = ControladorAtributos.ObtenerAtributo(atributo.Id);

                // Seleccionar los TipoProducto del atributo seleccionado
                TipoProducto_ListBox.UnselectAll();
                foreach (AtributoTipoProducto atributoTipoProducto in atributoActualizado.TiposProductos)
                    TipoProducto_ListBox.SelectedItems.Add(atributoTipoProducto.TipoProducto);

                RefrescarListBoxValores();

                TipoProducto_ListBox.IsEnabled = true;
                Valores_Card.IsEnabled = true;
            }
            else
            {
                TipoProducto_ListBox.UnselectAll();
                Valores_ListBox.Items.Clear();

                TipoProducto_ListBox.IsEnabled = false;
                Valores_Card.IsEnabled = false;
            }
            editandoListBoxTipoProducto = false;
        }

        private void TipoProducto_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!editandoListBoxTipoProducto)
            {
                Atributo atributo = Atributos_ListBox.SelectedItem as Atributo;

                IList addedItems = e.AddedItems;
                if (addedItems.Count > 0)
                    ControladorAtributos.AgregarAtributoTipoProducto(atributo, (TipoProducto)addedItems[0]);
                else
                {
                    IList removedItems = e.RemovedItems;
                    if (removedItems.Count > 0)
                        ControladorAtributos.EliminarAtributoTipoProducto(atributo, (TipoProducto)removedItems[0]);
                }
            }
        }

        // ------------------------------------------------------ //
        //                      Agregar un valor                  //
        // ------------------------------------------------------ //

        private void AgregarValor_Button_Click(object sender, RoutedEventArgs e)
        {
            Atributo atributo = Atributos_ListBox.SelectedItem as Atributo;
            if (atributo != null)
            {
                CaracteristicasValor caracteristicasValor = new CaracteristicasValor(_principal, null, this, atributo, null);
                AbrirCaracteristicasValorCentrado(caracteristicasValor);

                RefrescarListBoxValores();
            }
        }

        private void AbrirCaracteristicasValorCentrado(CaracteristicasValor caracteristicasValor)
        {
            caracteristicasValor.Owner = Application.Current.MainWindow;

            caracteristicasValor.WindowStartupLocation = WindowStartupLocation.Manual;
            Point point = Valores_Card.PointToScreen(new Point(Valores_Card.ActualWidth / 2, Valores_Card.ActualHeight / 2));
            caracteristicasValor.Left = point.X - (caracteristicasValor.Width / 2);
            caracteristicasValor.Top = point.Y - (caracteristicasValor.Height / 2);

            caracteristicasValor.ShowDialog();
        }

        private void RefrescarListBoxValores()
        {
            Atributo atributo = Atributos_ListBox.SelectedItem as Atributo;
            if (atributo != null)
            {
                Atributo atributoActualizado = ControladorAtributos.ObtenerAtributo(atributo.Id);
                Valores_ListBox.Items.Clear();
                foreach (Valor valor in atributoActualizado.Valores)
                    Valores_ListBox.Items.Add(valor);
            }
        }

        // ------------------------------------------------------ //
        //     Opciones al hacer click derecho sobre un valor     //
        // ------------------------------------------------------ //

        private void ModificarValor_Click(object sender, RoutedEventArgs e)
        {
            Atributo atributo = Atributos_ListBox.SelectedItem as Atributo;
            Valor valor = Valores_ListBox.SelectedItem as Valor;
            if (atributo != null && valor != null)
            {
                CaracteristicasValor caracteristicasValor = new CaracteristicasValor(_principal, null, this, atributo, valor);
                AbrirCaracteristicasValorCentrado(caracteristicasValor);

                RefrescarListBoxValores();
            }
        }

        private void EliminarValor_Click(object sender, RoutedEventArgs e)
        {
            _principal.OscurecerCompletamente(true);
            AlertaEliminarValor_DialogHost.IsOpen = true;
        }

        //
        // Alerta al eliminar un valor
        //

        private void CancelarEliminarValor_Button_Click(object sender, RoutedEventArgs e)
        {
            _principal.OscurecerCompletamente(false);
            AlertaEliminarValor_DialogHost.IsOpen = false;
        }

        private void EliminarValor_Button_Click(object sender, RoutedEventArgs e)
        {
            Valor valor = Valores_ListBox.SelectedItem as Valor;
            if (valor != null)
            {
                ControladorAtributos.EliminarValor(valor);
                RefrescarListBoxValores();

                _principal.OscurecerCompletamente(false);
                AlertaEliminarValor_DialogHost.IsOpen = false;
                _ = _principal.MostrarMensajeEnSnackbar("Valor eliminado correctamente!");
            }
        }

        // ------------------------------------------------------ //
        //                       Oscurecer                        //
        // ------------------------------------------------------ //

        public void OscurecerFondoAtributos(bool estado)
        {
            OscurecerAtributos_DialogHost.IsOpen = estado;
        }

        public void OscurecerFondoValores(bool estado)
        {
            OscurecerValores_DialogHost.IsOpen = estado;
        }
    }
}