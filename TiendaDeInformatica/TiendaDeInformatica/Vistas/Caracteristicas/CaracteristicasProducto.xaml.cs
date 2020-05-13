using System;
using System.Windows;
using System.Windows.Media.Imaging;
using TiendaDeInformatica.Controladores;
using TiendaDeInformatica.Helpers;
using TiendaDeInformatica.Modelos;

namespace TiendaDeInformatica.Vistas.Caracteristicas
{
    /// <summary>
    /// Lógica de interacción para CaracteristicasProducto.xaml
    /// </summary>
    public partial class CaracteristicasProducto : Window
    {
        private Principal _principal { get; set; }
        private Producto _productoModificar { get; set; }

        public TipoProducto TipoProducto_ComboBox_SelectedItem { get; set; }
        public Marca Marca_ComboBox_SelectedItem { get; set; }
        public string Modelo_TextBox_Text { get; set; }
        public string Precio_TextBox_Text { get; set; }
        public byte[] ImagenSeleccionada { get; set; }

        public CaracteristicasProducto(Principal principal, Producto productoModificar)
        {
            InitializeComponent();
            this.DataContext = this;

            // Cargar el enum TipoProducto en el ComboBox
            TipoProducto[] tipoProductos = (TipoProducto[])Enum.GetValues(typeof(TipoProducto));
            TipoProducto_ComboBox.ItemsSource = tipoProductos;

            // Cargar la lista de marcas en el ComboBox
            Marca_ComboBox.ItemsSource = ControladorMarcas.ObtenerListaDeMarcas();

            _principal = principal;
            _productoModificar = productoModificar;
        }

        private void CaracteristicasProducto_Vista_Loaded(object sender, RoutedEventArgs e)
        {
            // Verificar si se va a crear o a modificar un producto
            if (_productoModificar != null)
            {
                // Cambiar el título y el botón
                Titulo_TextBlock.Text = "Modificar producto";
                AgregarModificar_Button.Content = "MODIFICAR";

                // Completar los datos del producto
            }

            _principal.OscurecerCompletamente(true);
            Contenido_DialogHost.IsOpen = true;
        }

        // ------------------------------------------------------ //
        //             Agregar o Modificar un producto            //
        // ------------------------------------------------------ //

        private void AgregarModificar_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        // ------------------------------------------------------ //
        //                     Cerrar ventana                     //
        // ------------------------------------------------------ //

        private void Cancelar_Button_Click(object sender, RoutedEventArgs e)
        {
            _principal.OscurecerCompletamente(false);
            this.Close();
        }

        // ------------------------------------------------------ //
        //        Alerta al cerrar sin guardar los cambios        //
        // ------------------------------------------------------ //

        private void CerrarIgual_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        // ------------------------------------------------------ //
        //                 Seleccionar una imagen                //
        // ------------------------------------------------------ //

        private void SeleccionarImagen_Button_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "PNG (*.png)|*.png";
            var result = openFileDialog.ShowDialog();

            if (result == false) return;

            Imagen_Image.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            ImagenSeleccionada = ConvertirImagen.ConvertImageToByteArray(openFileDialog.FileName);
        }
    }
}
