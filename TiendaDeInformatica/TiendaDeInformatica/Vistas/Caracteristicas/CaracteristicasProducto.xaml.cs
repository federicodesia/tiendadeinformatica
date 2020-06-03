using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        private TipoProducto? _tipoProductoCrear { get; set; }

        public int TipoProducto_ComboBox_SelectedIndex { get; set; }
        public Marca Marca_ComboBox_SelectedItem { get; set; }
        public string Modelo_TextBox_Text { get; set; }
        public string Precio_TextBox_Text { get; set; }
        public byte[] ImagenSeleccionada { get; set; }

        public Atributo Atributos_ComboBox_SelectedItem { get; set; }

        public CaracteristicasProducto(Principal principal, Producto productoModificar, TipoProducto? tipoProductoCrear)
        {
            InitializeComponent();
            this.DataContext = this;

            _tipoProductoCrear = tipoProductoCrear;

            // Cargar el enum TipoProducto en el ComboBox
            TipoProducto[] tipoProductos = (TipoProducto[])Enum.GetValues(typeof(TipoProducto));
            List<string> tipoProductosConEspacios = new List<string>(tipoProductos.Select(v => v.ToString().Replace("_", " ")));
            TipoProducto_ComboBox.ItemsSource = tipoProductosConEspacios;

            // Cargar la lista de marcas en el ComboBox
            ActualizarListaDeMarcas();

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
                TipoProducto_ComboBox.SelectedIndex = (int)_productoModificar.Tipo;

                foreach (Marca marca in Marca_ComboBox.Items)
                {
                    if (marca.Id == _productoModificar.MarcaId)
                        Marca_ComboBox.SelectedItem = marca;
                }

                Modelo_TextBox.Text = _productoModificar.Modelo;
                Precio_TextBox.Text = _productoModificar.Precio.ToString();

                if (_productoModificar.Imagen != null)
                {
                    ImagenSeleccionada = _productoModificar.Imagen;
                    Imagen_Image.Source = ConvertirImagen.ConvertByteArrayToImage(ImagenSeleccionada);
                }
            }
            else
            {
                if (_tipoProductoCrear != null)
                    TipoProducto_ComboBox.SelectedIndex = (int)_tipoProductoCrear;
                else
                    TipoProducto_ComboBox.SelectedIndex = -1;
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
                    ControladorProductos.AgregarProducto((TipoProducto)TipoProducto_ComboBox.SelectedIndex, (Marca)Marca_ComboBox.SelectedItem, Modelo_TextBox.Text, decimal.Parse(Precio_TextBox.Text), ImagenSeleccionada);
                    _ = _principal.MostrarMensajeEnSnackbar("Producto agregado correctamente!");
                }
                else
                {
                    // Modificar producto
                    ControladorProductos.ModificarProducto(_productoModificar, (TipoProducto)TipoProducto_ComboBox.SelectedIndex, (Marca)Marca_ComboBox.SelectedItem, Modelo_TextBox.Text, decimal.Parse(Precio_TextBox.Text), ImagenSeleccionada);
                    _ = _principal.MostrarMensajeEnSnackbar("Producto modificado correctamente!");
                }
                CerrarVentana();
            }
            else
            {
                // Hay errores. Actualizar los mensajes de error
                TipoProducto_ComboBox.GetBindingExpression(ComboBox.SelectedIndexProperty).UpdateSource();
                Marca_ComboBox.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
                Modelo_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                Precio_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }

        private bool ObtenerResultadoReglasDeValidacion()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            if (TipoProducto_ComboBox.SelectedItem != null && (new TipoProductoSeleccionado().Validate(TipoProducto_ComboBox.SelectedIndex, currentCulture) == new ValidationResult(true, null))
                && new MarcaSeleccionada().Validate(Marca_ComboBox.SelectedItem, currentCulture) == new ValidationResult(true, null)
                && new CampoVacio().Validate(Modelo_TextBox.Text, currentCulture) == new ValidationResult(true, null)
                && new Precio().Validate(Precio_TextBox.Text, currentCulture) == new ValidationResult(true, null))
            {
                // No hay errores
                return true;
            }
            // Hay errores
            return false;
        }

        // ------------------------------------------------------ //
        //              Acceso directo Agregar Marca              //
        // ------------------------------------------------------ //

        private void AgregarMarca_Button_Click(object sender, RoutedEventArgs e)
        {
            CaracteristicasMarca caracteristicasMarca = new CaracteristicasMarca(_principal, null, false);
            caracteristicasMarca.Owner = Application.Current.MainWindow;

            caracteristicasMarca.ShowDialog();
            if(Marca_ComboBox.Items.Count != ControladorMarcas.ObtenerListaDeMarcas().Count)
                ActualizarListaDeMarcas();
        }

        // ------------------------------------------------------ //
        //              Actualizar la lista de marcas             //
        // ------------------------------------------------------ //

        private void ActualizarListaDeMarcas()
        {
            Marca_ComboBox.Items.Clear();
            foreach (Marca marca in ControladorMarcas.ObtenerListaDeMarcas())
                Marca_ComboBox.Items.Add(marca);
        }

        // ------------------------------------------------------ //
        //                     Cerrar ventana                     //
        // ------------------------------------------------------ //

        private void Cancelar_Button_Click(object sender, RoutedEventArgs e)
        {
            if (_productoModificar == null)
            {
                // Se iba a crear un producto
                if ((_tipoProductoCrear == null && TipoProducto_ComboBox.SelectedItem != null)
                    || (_tipoProductoCrear != null && (TipoProducto)TipoProducto_ComboBox.SelectedIndex != _tipoProductoCrear)
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
                Marca marcaSeleccionada = Marca_ComboBox.SelectedItem as Marca;
                if (TipoProducto_ComboBox.SelectedItem == null
                    || ((TipoProducto)TipoProducto_ComboBox.SelectedIndex != _productoModificar.Tipo)
                    || marcaSeleccionada == null
                    || marcaSeleccionada.Id != _productoModificar.MarcaId
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
        //                 Seleccionar una imagen                 //
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

        // ------------------------------------------------------ //
        //                   Pestaña Compatibilidad               //
        // ------------------------------------------------------ //

        private void TipoProducto_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TipoProducto_ComboBox.SelectedIndex != -1)
            {
                TipoProducto tipoProducto = (TipoProducto)TipoProducto_ComboBox.SelectedIndex;
                Atributos_ComboBox.Items.Clear();
                foreach (Atributo atributo in ControladorAtributos.ObtenerListaDeAtributosAsociadosATipoProducto(tipoProducto))
                    Atributos_ComboBox.Items.Add(atributo);
                Atributos_ComboBox.IsEnabled = true;
            }
            else
            {
                Atributos_ComboBox.IsEnabled = false;
            }
        }

        private void Atributos_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefrescarListBoxValores();
        }

        private void RefrescarListBoxValores()
        {
            Atributo atributo = Atributos_ComboBox.SelectedItem as Atributo;
            if (atributo != null)
            {
                // Falta realizar la verificación de Valores unicos o Valores multiples
                // para mostrar los RadioButtons o CheckBoxs en el ListBox.

                ValoresMultiples_ListBox.Items.Clear();
                foreach (Valor valor in ControladorAtributos.ObtenerAtributo(atributo.Id).Valores)
                    ValoresMultiples_ListBox.Items.Add(valor);
                Valores_Grid.Visibility = Visibility.Visible;
            }
            else
            {
                Valores_Grid.Visibility = Visibility.Collapsed;
            }
        }

        //
        // Agregar valor
        //

        private void AgregarValor_Button_Click(object sender, RoutedEventArgs e)
        {
            Atributo atributo = Atributos_ComboBox.SelectedItem as Atributo;
            if (atributo != null)
            {
                OscurecerFondo(true);
                CaracteristicasValor caracteristicasValor = new CaracteristicasValor(_principal, this, null, atributo, null);
                caracteristicasValor.Owner = this;

                caracteristicasValor.ShowDialog();
                RefrescarListBoxValores();
            }
        }

        public void OscurecerFondo(bool estado)
        {
            OscurecerFondo_DialogHost.IsOpen = estado;
        }

        private void ValoresMultiples_ListBox_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Deshabilitar el click derecho para seleccionar o deseleccionar un item del ListBox
            e.Handled = true;
        }
    }
}
