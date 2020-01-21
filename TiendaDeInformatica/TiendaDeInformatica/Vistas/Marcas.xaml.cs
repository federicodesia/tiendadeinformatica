using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TiendaDeInformatica.Controladores;
using TiendaDeInformatica.Modelos;
using TiendaDeInformatica.Vistas;
using TiendaDeInformatica.Vistas.Caracteristicas;

namespace TiendaDeInformatica.Vistas
{
    /// <summary>
    /// Lógica de interacción para Clientes.xaml
    /// </summary>
    public partial class Marcas : UserControl
    {
        private Principal _principal;

        public Marcas(Principal principal)
        {
            InitializeComponent();
            _principal = principal;

            RefrescarListaDeMarcas(true);
        }

        private void AgregarMarca_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CaracteristicasMarca caracteristicasMarca = new CaracteristicasMarca(_principal, null);
            caracteristicasMarca.Owner = System.Windows.Application.Current.MainWindow;

            caracteristicasMarca.ShowDialog();
            RefrescarListaDeMarcas(false);
        }

        //
        // Refrescar la lista de marcas
        //

        private void RefrescarListaDeMarcas(bool saltearVerificacion)
        {
            if (Marcas_Vista.IsLoaded || saltearVerificacion)
            {
                Marcas_ListBox.Items.Clear();

                foreach (Marca marca in ControladorMarcas.ObtenerListaDeMarcas())
                {
                    Marcas_ListBox.Items.Add(marca);
                }
                CantidadDeResultados_TextBlock.Text = Marcas_ListBox.Items.Count.ToString();
            }
        }

        //
        // Ajustar las columnas y filas de la lista de marcas
        //

        private UniformGrid itemsGrid;

        private void Items_UniformGrid_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            itemsGrid = sender as UniformGrid;
            AjustarFilasColumnas();
        }

        private void Marcas_Vista_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            AjustarFilasColumnas();
        }

        private void AjustarFilasColumnas()
        {
            if (itemsGrid != null)
            {
                itemsGrid.Columns = (int)(Contenido_Grid.ActualWidth / 200);
                if (itemsGrid.Columns > 0)
                    itemsGrid.Rows = (int)Math.Ceiling((decimal)Marcas_ListBox.Items.Count / (decimal)itemsGrid.Columns);
                else
                    itemsGrid.Rows = Marcas_ListBox.Items.Count;
                itemsGrid.Height = itemsGrid.Rows * 80;
            }
        }
    }
}
