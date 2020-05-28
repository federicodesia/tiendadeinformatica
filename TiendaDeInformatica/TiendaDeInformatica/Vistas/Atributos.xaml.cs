using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class Atributos : UserControl
    {
        private Principal _principal;

        public Atributos(Principal principal)
        {
            InitializeComponent();
            _principal = principal;
        }

        private void Atributos_Vista_Loaded(object sender, RoutedEventArgs e)
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
            CaracteristicasAtributo caracteristicasAtributo = new CaracteristicasAtributo(_principal, null);
            caracteristicasAtributo.Owner = Application.Current.MainWindow;

            caracteristicasAtributo.ShowDialog();
            RefrescarListaDeAtributos();
        }

        // ------------------------------------------------------ //
        //              Refrescar la lista de atributos           //
        // ------------------------------------------------------ //

        private void RefrescarListaDeAtributos()
        {
            if (Atributos_Vista.IsLoaded)
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
                CaracteristicasAtributo caracteristicasAtributo = new CaracteristicasAtributo(_principal, atributo);
                caracteristicasAtributo.Owner = Application.Current.MainWindow;

                caracteristicasAtributo.ShowDialog();
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

                AlertaEliminarAtributo_DialogHost.IsOpen = false;
                _principal.OscurecerCompletamente(false);
                _ = _principal.MostrarMensajeEnSnackbar("Atributo eliminado correctamente!");
            }
        }

        // ------------------------------------------------------ //
        //                  Cambios de selección                  //
        // ------------------------------------------------------ //

        private void Atributos_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Atributo atributo = Atributos_ListBox.SelectedItem as Atributo;
            if (atributo != null)
            {
                // Seleccionar los TipoProducto del atributo seleccionado
                foreach (AtributoTipoProducto atributoTipoProducto in atributo.TiposProductos)
                    TipoProducto_ListBox.SelectedItems.Add(atributoTipoProducto.TipoProducto);
                TipoProducto_ListBox.IsEnabled = true;
            }
            else
            {
                TipoProducto_ListBox.UnselectAll();
                TipoProducto_ListBox.IsEnabled = false;
            }
        }

        private void TipoProducto_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Atributo atributo = Atributos_ListBox.SelectedItem as Atributo;
            ControladorAtributos.ModificarAtributoTipoProducto(atributo);
        }
    }
}
