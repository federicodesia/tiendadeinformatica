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
    public partial class ResumenPresupuesto : UserControl
    {
        private Principal _principal;
        private Presupuesto _presupuesto;

        public ResumenPresupuesto(Principal principal, int presupestoId)
        {
            InitializeComponent();
            _principal = principal;
            _presupuesto = ControladorPresupuestos.ObtenerPresupuesto(presupestoId);
            this.DataContext = _presupuesto;

            TituloPresupuesto_TextBlock.Text = $"Presupuesto #{_presupuesto.Id.ToString()}";
        }

        private void ResumenPresupuesto_Vista_Loaded(object sender, RoutedEventArgs e)
        {
            RefrescarListaDeProductos();
        }

        private void Presupuestos_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            _principal.MenuIzquierdo.SelectedIndex = 0;
        }

        // ------------------------------------------------------ //
        //           Refrescar la lista de los productos          //
        // ------------------------------------------------------ //

        private void RefrescarListaDeProductos()
        {
            Productos_ListBox.Items.Clear();
            foreach (PresupuestoProducto presupuestoProducto in _presupuesto.Productos)
                Productos_ListBox.Items.Add(presupuestoProducto);
        }
    }
}
