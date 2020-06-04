using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TiendaDeInformatica.Controladores;
using TiendaDeInformatica.Helpers;
using TiendaDeInformatica.Modelos;
using TiendaDeInformatica.Vistas.Caracteristicas;

namespace TiendaDeInformatica.Vistas
{
    /// <summary>
    /// Lógica de interacción para Presupuestos.xaml
    /// </summary>
    public partial class Presupuestos : UserControl
    {
        private Principal _principal { get; set; }

        public Presupuestos(Principal principal)
        {
            InitializeComponent();
            _principal = principal;
        }

        private void Presupuestos_Vista_Loaded(object sender, RoutedEventArgs e)
        {
            // Colocar el Slider del filtro de precio al valor máximo
            List<Presupuesto> presupuestos = ControladorPresupuestos.ObtenerListaDePresupuestos().ToList();
            if (presupuestos.Count > 0)
            {
                double presupuestoMasAlto = double.Parse(presupuestos.Max(p => p.PrecioTotal.ToString()));
                if (presupuestoMasAlto > 100000)
                {
                    FiltroPrecio_RangeSlider.Maximum = presupuestoMasAlto;
                    FiltroPrecio_RangeSlider.UpperValue = presupuestoMasAlto;
                }
            }

            RefrescarListaDePresupuestos();
        }

        // ------------------------------------------------------ //
        //                  Agregar un presupuesto                //
        // ------------------------------------------------------ //

        private void AgregarPresupuesto_Button_Click(object sender, RoutedEventArgs e)
        {
            CaracteristicasPresupuesto caracteristicasPresupuesto = new CaracteristicasPresupuesto(_principal, null);
            caracteristicasPresupuesto.Owner = Application.Current.MainWindow;

            caracteristicasPresupuesto.ShowDialog();
            RefrescarListaDePresupuestos();
        }

        // ------------------------------------------------------ //
        //  Opciones al hacer click derecho sobre un presupuesto  //
        // ------------------------------------------------------ //

        private void SeleccionarPresupuesto(object sender, RoutedEventArgs e)
        {
            Presupuesto presupuesto = Presupuestos_ListBox.SelectedItem as Presupuesto;
            if (presupuesto != null)
            {
                _principal.SeleccionarPresupuesto(presupuesto);
            }
        }

        private void ResumenPresupuesto_Click(object sender, RoutedEventArgs e)
        {
            AbrirResumenPresupuesto();
        }

        private void ModificarPresupuesto(object sender, RoutedEventArgs e)
        {
            Presupuesto presupuesto = Presupuestos_ListBox.SelectedItem as Presupuesto;
            if (presupuesto != null)
            {
                CaracteristicasPresupuesto caracteristicasPresupuesto = new CaracteristicasPresupuesto(_principal, presupuesto);
                caracteristicasPresupuesto.Owner = Application.Current.MainWindow;

                caracteristicasPresupuesto.ShowDialog();
                RefrescarListaDePresupuestos();
            }
        }

        private void EliminarPresupuesto(object sender, RoutedEventArgs e)
        {
            _principal.OscurecerCompletamente(true);
            AlertaBorrarPresupuesto_DialogHost.IsOpen = true;
        }

        // ------------------------------------------------------ //
        //       Al hacer doble click sobre un presupuesto        //
        // ------------------------------------------------------ //

        private void Presupuestos_ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AbrirResumenPresupuesto();
        }

        private void AbrirResumenPresupuesto()
        {
            Presupuesto presupuesto = Presupuestos_ListBox.SelectedItem as Presupuesto;
            if (presupuesto != null)
            {
                _principal.Contenido_Grid.Children.Clear();
                ResumenPresupuesto resumenPresupuesto = new ResumenPresupuesto(_principal, presupuesto.Id);
                _principal.reumenPresupuestosUserControl = resumenPresupuesto;
                _principal.Contenido_Grid.Children.Add(resumenPresupuesto);
            }
        }

        // ------------------------------------------------------ //
        //            Alerta al eliminar un presupuesto           //
        // ------------------------------------------------------ //

        private void EliminarPresupuesto_Button_Click(object sender, RoutedEventArgs e)
        {
            Presupuesto presupuesto = Presupuestos_ListBox.SelectedItem as Presupuesto;
            if (presupuesto != null)
            {
                ControladorPresupuestos.EliminarPresupuesto(presupuesto);
                RefrescarListaDePresupuestos();

                AlertaBorrarPresupuesto_DialogHost.IsOpen = false;
                _principal.OscurecerCompletamente(false);
                _ = _principal.MostrarMensajeEnSnackbar("Presupuesto eliminado correctamente!");
            }
        }

        private void CancelarEliminarPresupuesto_Button_Click(object sender, RoutedEventArgs e)
        {
            _principal.OscurecerCompletamente(false);
            AlertaBorrarPresupuesto_DialogHost.IsOpen = false;
        }

        // ------------------------------------------------------ //
        //             Buscar presupuesto por el cliente          //
        // ------------------------------------------------------ //

        private void BuscarPresupuestoPorCliente_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefrescarListaDePresupuestos();
        }

        private List<Presupuesto> BuscarPresupuestoPorCliente(List<Presupuesto> presupuestos, string busqueda)
        {
            if (busqueda != "")
            {
                busqueda = TextHelper.QuitarTildes(busqueda).ToUpper();
                List<Presupuesto> resultado = new List<Presupuesto>();
                foreach (Presupuesto presupuesto in presupuestos)
                {
                    Cliente cliente = presupuesto.Cliente;
                    if ((cliente.NombreDelResponsable != null && TextHelper.QuitarTildes(cliente.NombreDelResponsable).ToUpper().StartsWith(busqueda))
                        || (cliente.CUIT != null && cliente.CUIT.StartsWith(busqueda))
                        || (cliente.Telefono != null && cliente.Telefono.StartsWith(busqueda)
                        || TextHelper.QuitarTildes(cliente.MostrarNombre).ToUpper().StartsWith(busqueda)
                        || TextHelper.QuitarTildes(cliente.Apellido).ToUpper().StartsWith(busqueda)))
                    {
                        resultado.Add(presupuesto);
                    }
                }
                return resultado;
            }
            return presupuestos;
        }

        // ------------------------------------------------------ //
        //            Refrescar la lista de presupuestos          //
        // ------------------------------------------------------ //

        public void RefrescarListaDePresupuestos()
        {
            if (Presupuestos_Vista.IsLoaded)
            {
                Presupuestos_ListBox.Items.Clear();
                List<Presupuesto> presupuestos = ControladorPresupuestos.ObtenerListaDePresupuestos().ToList();
                List<Presupuesto> resultados = OrdenarPresupuestos(FiltrarPresupuestosPorPrecio(FiltrarPresupuestosPorEstado(FiltrarPresupuestosPorTipoDeCliente(BuscarPresupuestoPorCliente(presupuestos, BuscarPresupuestoPorCliente_TextBox.Text)))));

                foreach (Presupuesto presupuesto in resultados)
                    Presupuestos_ListBox.Items.Add(presupuesto);

                CantidadDeResultados_TextBlock.Text = Presupuestos_ListBox.Items.Count.ToString();

                // Cambiar el valor máximo del filtro de precio
                if (presupuestos.Count > 0)
                {
                    int presupuestoMasAlto = (int)Math.Ceiling(presupuestos.Max(p => p.PrecioTotal));
                    if (presupuestoMasAlto > 100000)
                        FiltroPrecio_RangeSlider.Maximum = presupuestoMasAlto;
                    else
                        FiltroPrecio_RangeSlider.Maximum = 100000;
                }
            }
        }

        // ------------------------------------------------------ //
        //                    Ordenar presupuestos                //
        // ------------------------------------------------------ //

        private void OrdenarPresupuestos_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefrescarListaDePresupuestos();
        }

        private void OrdenarPresupuestos_AscDesc_ToggleButton_CheckedUnchecked(object sender, RoutedEventArgs e)
        {
            RefrescarListaDePresupuestos();
        }

        private List<Presupuesto> OrdenarPresupuestos(List<Presupuesto> presupuestos)
        {
            if (OrdenarPresupuestos_ComboBox.SelectedIndex == 0)
            {
                // Fecha de creación
                presupuestos.OrderBy(p => p.FechaCreacion);
            }
            else if (OrdenarPresupuestos_ComboBox.SelectedIndex == 1)
            {
                // Fecha de modificación
                presupuestos.OrderBy(p => p.FechaModificacion);
            }
            else if (OrdenarPresupuestos_ComboBox.SelectedIndex == 2)
            {
                // Fecha de expiración
                List<Presupuesto> sinFechaDeExpiracion = presupuestos.Where(p => p.FechaDeExpiracion == null).ToList();
                List<Presupuesto> conFechaDeExpiracion = presupuestos.Where(p => p.FechaDeExpiracion != null).ToList();

                conFechaDeExpiracion.OrderBy(p => p.FechaDeExpiracion);

                presupuestos = sinFechaDeExpiracion;
                presupuestos.AddRange(conFechaDeExpiracion);
            }
            else if (OrdenarPresupuestos_ComboBox.SelectedIndex == 3)
            {
                // Precio
                presupuestos.Sort((p1, p2) => p1.PrecioTotal.CompareTo(p2.PrecioTotal));
                presupuestos.Reverse();
            }

            if (OrdenarPresupuestos_AscDesc_ToggleButton.IsChecked.Value)
                presupuestos.Reverse();

            return presupuestos;
        }

        // ------------------------------------------------------ //
        //                    Filtrar presupuestos                //
        // ------------------------------------------------------ //

        private void ActualizarFiltros(object sender, RoutedEventArgs e)
        {
            RefrescarListaDePresupuestos();
        }

        private void FiltroPrecio_RangeSlider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            RefrescarListaDePresupuestos();
        }

        private List<Presupuesto> FiltrarPresupuestosPorTipoDeCliente(List<Presupuesto> presupuestos)
        {
            if (FiltroCliente_Persona_CheckBox.IsChecked.Value != FiltroCliente_Empresa_CheckBox.IsChecked.Value)
            {
                if(FiltroCliente_Persona_CheckBox.IsChecked.Value == true)
                    return presupuestos.Where(p => p.Cliente.Tipo == "Persona").ToList();
                return presupuestos.Where(p => p.Cliente.Tipo == "Empresa").ToList();
            }
            return presupuestos;
        }

        private List<Presupuesto> FiltrarPresupuestosPorEstado(List<Presupuesto> presupuestos)
        {
            if (FiltroEstado_Vigentes_CheckBox.IsChecked.Value != FiltroEstado_Expirados_CheckBox.IsChecked.Value)
            {
                if (FiltroEstado_Vigentes_CheckBox.IsChecked.Value == true)
                    return presupuestos.Where(p => p.FechaDeExpiracion == null || (p.FechaDeExpiracion.GetValueOrDefault().Date - DateTime.Now.Date).Days >= 0).ToList();
                return presupuestos.Where(p => p.FechaDeExpiracion != null && ((p.FechaDeExpiracion.GetValueOrDefault().Date - DateTime.Now.Date).Days < 0)).ToList();
            }
            return presupuestos;
        }

        private List<Presupuesto> FiltrarPresupuestosPorPrecio(List<Presupuesto> presupuestos)
        {
            return presupuestos.Where(p => p.PrecioTotal >= decimal.Parse(FiltroPrecio_RangeSlider.LowerValue.ToString())
            && p.PrecioTotal <= decimal.Parse(FiltroPrecio_RangeSlider.UpperValue.ToString())).ToList();
        }
    }
}
