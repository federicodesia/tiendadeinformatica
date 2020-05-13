using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TiendaDeInformatica.Controladores;
using TiendaDeInformatica.Helpers;
using TiendaDeInformatica.Modelos;
using TiendaDeInformatica.Vistas.Reglas_de_Validacion;

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
            TipoProducto_ComboBox.SelectedIndex = -1;

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
            if (ObtenerResultadoReglasDeValidacion())
            {
                // No hay errores
                if (_productoModificar == null)
                {
                    // Crear producto
                    ControladorProductos.AgregarProducto((TipoProducto)TipoProducto_ComboBox.SelectedItem, (Marca)Marca_ComboBox.SelectedItem, Modelo_TextBox.Text, decimal.Parse(Precio_TextBox.Text), ImagenSeleccionada);
                    _ = _principal.MostrarMensajeEnSnackbar("Producto agregado correctamente!");
                }
                else
                {
                    // Modificar producto
                    ControladorProductos.ModificarProducto(_productoModificar, (TipoProducto)TipoProducto_ComboBox.SelectedItem, (Marca)Marca_ComboBox.SelectedItem, Modelo_TextBox.Text, decimal.Parse(Precio_TextBox.Text), ImagenSeleccionada);
                    _ = _principal.MostrarMensajeEnSnackbar("Producto modificado correctamente!");
                }
                CerrarVentana();
            }
            else
            {
                // Hay errores. Actualizar los mensajes de error
                TipoProducto_ComboBox.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
                Marca_ComboBox.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
                Modelo_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                Precio_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }

        private bool ObtenerResultadoReglasDeValidacion()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            if (new TipoProductoSeleccionado().Validate(TipoProducto_ComboBox.SelectedItem, currentCulture) == new ValidationResult(true, null)
                || new MarcaSeleccionada().Validate(Marca_ComboBox.SelectedItem, currentCulture) == new ValidationResult(true, null)
                || new CampoVacio().Validate(Modelo_TextBox.Text, currentCulture) == new ValidationResult(true, null)
                || new Precio().Validate(Precio_TextBox.Text, currentCulture) == new ValidationResult(true, null))
            {
                // No hay errores
                return true;
            }
            // Hay errores
            return false;
        }

        // ------------------------------------------------------ //
        //                     Cerrar ventana                     //
        // ------------------------------------------------------ //

        private void Cancelar_Button_Click(object sender, RoutedEventArgs e)
        {
            if (_productoModificar == null)
            {
                // Se iba a crear un producto
                if (TipoProducto_ComboBox.SelectedItem != null
                    || Marca_ComboBox.SelectedItem != null
                    || Modelo_TextBox.Text != "" || Precio_TextBox.Text != ""
                    || ImagenSeleccionada != null)
                {
                    // Se realizaron cambios
                    AlertaAlCerrar_Dialog.IsOpen = true;
                }
                else
                    CerrarVentana();
            }
            else
            {
                // Se iba a modificar un producto
                if(TipoProducto_ComboBox.SelectedItem == null
                    || ((TipoProducto)TipoProducto_ComboBox.SelectedItem != _productoModificar.Tipo)
                    || Marca_ComboBox.SelectedItem != _productoModificar.Marca
                    || Modelo_TextBox.Text != _productoModificar.Modelo
                    || Precio_TextBox.Text != _productoModificar.Precio.ToString()
                    || ImagenSeleccionada != _productoModificar.Imagen)
                {
                    // Se realizaron cambios
                    AlertaAlCerrar_Dialog.IsOpen = true;
                }
                else
                    // Falta verificar si se realizaron cambios en los valores.
                    CerrarVentana();
            }
        }

        private async void CerrarVentana()
        {
            _principal.OscurecerCompletamente(false);
            Contenido_DialogHost.IsOpen = false;
            await Task.Delay(300);
            this.Close();
        }

        // ------------------------------------------------------ //
        //        Alerta al cerrar sin guardar los cambios        //
        // ------------------------------------------------------ //

        private void CerrarIgual_Button_Click(object sender, RoutedEventArgs e)
        {
            AlertaAlCerrar_Dialog.IsOpen = false;
            CerrarVentana();
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
