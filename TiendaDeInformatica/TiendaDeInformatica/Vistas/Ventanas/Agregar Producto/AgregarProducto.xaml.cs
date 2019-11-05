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
using TiendaDeInformatica.Vistas.Ventanas.Agregar_Marca;
using TiendaDeInformatica.Vistas.Ventanas.Agregar_Producto.Reglas_de_Validacion;
using TiendaDeInformatica.Vistas.Ventanas.Reglas_de_Validacion_Generales;

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
        public string RutaDeLaImagen { get; set; }


        public AgregarProducto()
        {
            InitializeComponent();

            TipoProducto[] tipoProductos = (TipoProducto[])Enum.GetValues(typeof(TipoProducto));
            IEnumerable<TipoProducto> tipoProductosOrdenados = tipoProductos.OrderBy(v => v.ToString());
            BuscarTipoProducto_ComboBox.ItemsSource = tipoProductosOrdenados;

            ActualizarComboBoxMarcas();
            this.DataContext = this;


            // Prueba
            var atributo=ControladorAtributos.AgregarAtributo("Socket");
            Atributo_ComboBox.Items.Add(atributo);

            var valor1 = ControladorAtributos.AgregarValor(atributo, "AM4");
            var valor2= ControladorAtributos.AgregarValor(atributo, "1151");
            Prueba_ListBox.Items.Add(valor1);
            Prueba_ListBox.Items.Add(valor2);
        }

        private void Cancelar_Button_Click(object sender, RoutedEventArgs e)
        {
            MostrarDialog();
        }

        private void Crear_Button_Click(object sender, RoutedEventArgs e)
        {
            TipoProducto? tipoProductoNullable = BuscarTipoProducto_ComboBox.SelectedItem as TipoProducto?;
            Marca marca = BuscarMarca_ComboBox.SelectedItem as Marca;

            if (new TipoProductoSeleccionado().Validate(tipoProductoNullable, CultureInfo.CurrentCulture) == new ValidationResult(true, null)
                && new MarcaSeleccionada().Validate(marca, CultureInfo.CurrentCulture) == new ValidationResult(true, null)
                && new CampoVacio().Validate(Modelo_TextBox.Text, CultureInfo.CurrentCulture) == new ValidationResult(true, null)
                && new Precio().Validate(Precio_TextBox.Text, CultureInfo.CurrentCulture) == new ValidationResult(true, null))
            {
                TipoProducto tipoProducto = (TipoProducto)BuscarTipoProducto_ComboBox.SelectedItem;
                ControladorProductos.AgregarProducto(marca, Modelo_TextBox.Text, decimal.Parse(Precio_TextBox.Text), tipoProducto, RutaDeLaImagen);
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
            if (BuscarTipoProducto_ComboBox.SelectedItem!=null || BuscarMarca_ComboBox.SelectedItem != null || Modelo_TextBox.Text!="" || Precio_TextBox.Text != "" || RutaDeLaImagen!=null)
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
            foreach (Marca marca in ControladorMarcas.ObtenerListaDeMarcas().OrderBy(v => v.Nombre).ToList())
            {
                BuscarMarca_ComboBox.Items.Add(marca);
            }
        }

        private void AgregarMarca_Button_Click(object sender, RoutedEventArgs e)
        {
            int CantidadDeMarcas = ControladorMarcas.ObtenerListaDeMarcas().Count;
            AgregarMarca agregarMarca = new AgregarMarca();
            agregarMarca.ShowDialog();
            ActualizarComboBoxMarcas();
            if (ControladorMarcas.ObtenerListaDeMarcas().Count > CantidadDeMarcas)
            {
                _ = MostrarPopupAsync();
            }
        }

        private void BuscarTipoProducto_ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            BuscarTipoProducto_ComboBox.SelectedIndex = -1;
        }

        private void SeleccionarImagen_Button_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "Todas las imágenes|*.png;*.jpg;*.jpeg";
            var result = ofd.ShowDialog();
            if (result == false) return;
            Imagen_Image.Source = new BitmapImage(new Uri(ofd.FileName));
            RutaDeLaImagen = ofd.FileName;
            EliminarImagen_Button.Visibility = Visibility.Visible;
        }

        private void EliminarImagen_Button_Click(object sender, RoutedEventArgs e)
        {
            RutaDeLaImagen = null;
            Imagen_Image.Source = null;
            EliminarImagen_Button.Visibility = Visibility.Hidden;
        }
    }
}
