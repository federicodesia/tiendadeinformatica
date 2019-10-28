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
using TiendaDeInformatica.Controladores;
using TiendaDeInformatica.Modelos;
using TiendaDeInformatica.Vistas.Ventanas.Agregar_Producto.Reglas_de_Validacion;

namespace TiendaDeInformatica.Vistas.Ventanas.Agregar_Producto
{
    /// <summary>
    /// Lógica de interacción para AgregarProducto.xaml
    /// </summary>
    public partial class AgregarProducto : MetroWindow
    {
        public TipoProducto BuscarTipoProducto_ComboBox_SelectedItem { get; set; }
        public Marca BuscarMarca_ComboBox_SelectedItem { get; set; }
        public string Modelo_TextBox_Text { get; set; }
        public string Precio_TextBox_Text { get; set; }


        public AgregarProducto()
        {
            InitializeComponent();

            BuscarTipoProducto_ComboBox.ItemsSource = Enum.GetValues(typeof(TipoProducto));
            ActualizarComboBoxMarcas();
            this.DataContext = this;
        }

        private void Cancelar_Button_Click(object sender, RoutedEventArgs e)
        {
            MostrarDialog();
        }

        private void Crear_Button_Click(object sender, RoutedEventArgs e)
        {
            TipoProducto? tipoProducto = BuscarTipoProducto_ComboBox.SelectedItem as TipoProducto?;
            Marca marca = BuscarMarca_ComboBox.SelectedItem as Marca;

            if (new TipoProductoSeleccionado().Validate(tipoProducto, CultureInfo.CurrentCulture) == new ValidationResult(true, null)
                && new MarcaSeleccionada().Validate(marca, CultureInfo.CurrentCulture) == new ValidationResult(true, null)
                && new Modelo().Validate(Modelo_TextBox.Text, CultureInfo.CurrentCulture) == new ValidationResult(true, null)
                && new Precio().Validate(Precio_TextBox.Text, CultureInfo.CurrentCulture) == new ValidationResult(true, null))
            {
                // Llamar al controlador de productos y agregarlo.
                this.Close();
            }
            else
            {
                BuscarTipoProducto_ComboBox.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
                BuscarMarca_ComboBox.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
                Modelo_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                Precio_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }

        private async Task MostrarPopupAsync()
        {
            await Task.Delay(500);
            MarcaCreada_Snackbar.IsActive = true;
            await Task.Delay(5000);
            MarcaCreada_Snackbar.IsActive = false;
        }

        private void MostrarDialog()
        {
            if ( BuscarTipoProducto_ComboBox.SelectedItem!=null || BuscarMarca_ComboBox.SelectedItem != null || Modelo_TextBox.Text!="" || Precio_TextBox.Text != "")
            {
                Dialog.IsOpen = true;
            }
            else
            {
                this.Close();
            }
        }

        private void AgregarMarca_Button_ActionClick(object sender, RoutedEventArgs e)
        {
            MarcaCreada_Snackbar.IsActive = false;
        }

        private void ActualizarComboBoxMarcas()
        {
            BuscarMarca_ComboBox.Items.Clear();
            foreach (Marca marca in ControladorMarcas.ObtenerListaDeMarcas())
            {
                BuscarMarca_ComboBox.Items.Add(marca);
            }
        }

        private void AgregarMarca_Button_Click(object sender, RoutedEventArgs e)
        {
            int CantidadDeMarcas = ControladorMarcas.ObtenerListaDeMarcas().Count;

            //Falta crear la vista para agregar marcas
            //AgregarMarca agregarMarca = new AgregarMarca();
            //agregarMarca.ShowDialog();

            ActualizarComboBoxMarcas();
            if (ControladorMarcas.ObtenerListaDeMarcas().Count > CantidadDeMarcas)
            {
                _ = MostrarPopupAsync();
            }
        }
    }
}
