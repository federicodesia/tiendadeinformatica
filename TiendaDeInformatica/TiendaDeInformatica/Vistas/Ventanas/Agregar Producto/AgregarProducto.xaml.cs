using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;
using TiendaDeInformatica.Modelos;
using TiendaDeInformatica.Vistas.Ventanas.Agregar_Producto.Reglas_de_Validación;

namespace TiendaDeInformatica.Vistas.Ventanas.Agregar_Producto
{
    /// <summary>
    /// Lógica de interacción para AgregarProducto.xaml
    /// </summary>
    public partial class AgregarProducto : MetroWindow
    {
        public TipoProducto BuscarTipoProducto_ComboBox_SelectedItem { get; set; }
        public Marca BuscarMarca_ComboBox_SelectedItem { get; set; }


        public AgregarProducto()
        {
            InitializeComponent();

            BuscarTipoProducto_ComboBox.ItemsSource = Enum.GetValues(typeof(TipoProducto));
        }

        private void Cancelar_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void Crear_Button_Click(object sender, RoutedEventArgs e)
        {
            TipoProducto? tipoProducto = BuscarTipoProducto_ComboBox.SelectedItem as TipoProducto?;
            Marca marca = BuscarMarca_ComboBox.SelectedItem as Marca;

            if (new TipoProductoSeleccionado().Validate(tipoProducto, CultureInfo.CurrentCulture) == new ValidationResult(true, null)
                && new MarcaSeleccionada().Validate(marca, CultureInfo.CurrentCulture) == new ValidationResult(true, null))
            {
                // Llamar al controlador de productos y agregarlo.
                this.Close();
            }
            else
            {
                BuscarTipoProducto_ComboBox.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
                BuscarMarca_ComboBox.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
            }
        }

        private void AgregarMarca_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
