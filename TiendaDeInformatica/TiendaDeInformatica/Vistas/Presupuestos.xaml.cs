using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TiendaDeInformatica.Controladores;
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
            RefrescarListaDePresupuestos();
        }

        //
        // Agregar presupuesto
        //

        private void AgregarPresupuesto_Button_Click(object sender, RoutedEventArgs e)
        {
            int cantidadDePresupuestosAntes = ControladorPresupuestos.ObtenerListaDePresupuestos().Count();

            CaracteristicasPresupuesto caracteristicasPresupuesto = new CaracteristicasPresupuesto(_principal, null);
            caracteristicasPresupuesto.Owner = Application.Current.MainWindow;

            caracteristicasPresupuesto.ShowDialog();
            RefrescarListaDePresupuestos();
        }

        //
        // Opciones al hacer click derecho sobre un presupuesto
        //

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

        private void SeleccionarPresupuesto(object sender, RoutedEventArgs e)
        {
            Presupuesto presupuesto = Presupuestos_ListBox.SelectedItem as Presupuesto;
            if (presupuesto != null)
            {
                _principal.SeleccionarPresupuesto(presupuesto);
            }
        }

        //
        // Alerta
        //

        private void Cancelar_Button_Click(object sender, RoutedEventArgs e)
        {
            _principal.OscurecerCompletamente(false);
            AlertaBorrarPresupuesto_DialogHost.IsOpen = false;
        }

        private void EliminarPresupuesto_Button_Click(object sender, RoutedEventArgs e)
        {
            Presupuesto presupuesto = Presupuestos_ListBox.SelectedItem as Presupuesto;
            if (presupuesto != null)
            {
                ControladorPresupuestos.EliminarPresupuesto(Presupuestos_ListBox.SelectedItem as Presupuesto);
                RefrescarListaDePresupuestos();

                AlertaBorrarPresupuesto_DialogHost.IsOpen = false;
                _principal.OscurecerCompletamente(false);
                _ = _principal.MostrarMensajeEnSnackbar("Presupuesto eliminado correctamente!");
            }
        }

        //
        // Buscar presupuesto por cliente
        //

        private void BuscarPresupuestoPorCliente_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefrescarListaDePresupuestos();
        }


        //
        // Filtros
        //

        private void ActualizarFiltros(object sender, RoutedEventArgs e)
        {
            RefrescarListaDePresupuestos();
        }

        private void FiltroPrecio_RangeSlider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            RefrescarListaDePresupuestos();
        }

        private void OrdenarPresupuestos_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefrescarListaDePresupuestos();
        }

        private void OrdenarPresupuestos_AscDesc_ToggleButton_CheckedUnchecked(object sender, RoutedEventArgs e)
        {
            RefrescarListaDePresupuestos();
        }

        private void RefrescarListaDePresupuestos()
        {
            if (Presupuestos_Vista.IsLoaded)
            {
                Presupuestos_ListBox.Items.Clear();
                List<Presupuesto> presupuestos = ControladorPresupuestos.ObtenerListaDePresupuestos().ToList();
                List<Presupuesto> resultados = OrdenarPresupuestos(FiltrarPresupuestosPorPrecio(FiltrarPresupuestosPorEstado(FiltrarPresupuestosPorTipoDeCliente(BuscarPresupuestoPorCliente(presupuestos, BuscarPresupuestoPorCliente_TextBox.Text)))));

                foreach (Presupuesto presupuesto in resultados)
                {
                    Presupuestos_ListBox.Items.Add(presupuesto);
                }
                CantidadDeResultados_TextBlock.Text = Presupuestos_ListBox.Items.Count.ToString();
            }
        }

        private List<Presupuesto> BuscarPresupuestoPorCliente(List<Presupuesto> presupuestos, string busqueda)
        {
            if (busqueda != "")
            {
                busqueda = busqueda.ToUpper();
                List<Presupuesto> resultado = new List<Presupuesto>();
                foreach (Presupuesto presupuesto in presupuestos)
                {
                    Cliente cliente = ControladorClientes.ObtenerCliente(presupuesto.ClienteId);
                    if ((cliente.NombreDeLaEmpresa != null && cliente.NombreDeLaEmpresa.ToUpper().StartsWith(busqueda))
                        || (cliente.NombreDelResponsable != null && cliente.NombreDelResponsable.ToUpper().StartsWith(busqueda))
                        || (cliente.CUIT != null && cliente.CUIT.StartsWith(busqueda))
                        || (cliente.Telefono != null && cliente.Telefono.StartsWith(busqueda)
                        || cliente.MostrarNombre.ToUpper().StartsWith(busqueda)
                        || cliente.Apellido.ToUpper().StartsWith(busqueda)))
                    {
                        resultado.Add(presupuesto);
                    }
                }
                return resultado;
            }
            return presupuestos;
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
                presupuestos.OrderBy(p => p.PrecioTotal);
            }

            if (OrdenarPresupuestos_AscDesc_ToggleButton.IsChecked.Value)
            {
                presupuestos.Reverse();
            }
            return presupuestos;
        }

        private List<Presupuesto> FiltrarPresupuestosPorTipoDeCliente(List<Presupuesto> presupuestos)
        {
            if (FiltroCliente_Persona_CheckBox.IsChecked.Value == true && FiltroCliente_Empresa_CheckBox.IsChecked.Value == false)
            {
                return presupuestos.Where(p => p.ClienteTipo == "Persona").ToList();
            }
            else if (FiltroCliente_Persona_CheckBox.IsChecked.Value == false && FiltroCliente_Empresa_CheckBox.IsChecked.Value == true)
            {
                return presupuestos.Where(p => p.ClienteTipo == "Empresa").ToList();
            }
            else if (FiltroCliente_Persona_CheckBox.IsChecked.Value == true && FiltroCliente_Empresa_CheckBox.IsChecked.Value == true)
            {
                return presupuestos;
            }
            return new List<Presupuesto>();
        }

        private List<Presupuesto> FiltrarPresupuestosPorEstado(List<Presupuesto> presupuestos)
        {
            if (FiltroEstado_Vigentes_CheckBox.IsChecked.Value == true && FiltroEstado_Expirados_CheckBox.IsChecked.Value == false)
            {
                return presupuestos.Where(p => p.FechaDeExpiracion == null || (p.FechaDeExpiracion.GetValueOrDefault().Date - DateTime.Now.Date).Days >= 0).ToList();
            }
            else if (FiltroEstado_Vigentes_CheckBox.IsChecked.Value == false && FiltroEstado_Expirados_CheckBox.IsChecked.Value == true)
            {
                return presupuestos.Where(p => p.FechaDeExpiracion != null && ((p.FechaDeExpiracion.GetValueOrDefault().Date - DateTime.Now.Date).Days < 0)).ToList();
            }
            else if (FiltroEstado_Vigentes_CheckBox.IsChecked.Value == true && FiltroEstado_Expirados_CheckBox.IsChecked.Value == true)
            {
                return presupuestos;
            }
            return new List<Presupuesto>();
        }

        private List<Presupuesto> FiltrarPresupuestosPorPrecio(List<Presupuesto> presupuestos)
        {
            return presupuestos.Where(p => p.PrecioTotal >= decimal.Parse(FiltroPrecio_RangeSlider.LowerValue.ToString())
            && p.PrecioTotal <= decimal.Parse(FiltroPrecio_RangeSlider.UpperValue.ToString())).ToList();
        }
    }
}
