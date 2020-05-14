using MaterialDesignColors;
using MaterialDesignColors.ColorManipulation;
using MaterialDesignThemes.Wpf;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TiendaDeInformatica.Helpers;
using TiendaDeInformatica.Modelos;

namespace TiendaDeInformatica.Vistas
{
    /// <summary>
    /// Lógica de interacción para Principal.xaml
    /// </summary>
    public partial class Principal : Window
    {
        public Principal()
        {
            InitializeComponent();

            // Cargar el tema de las preferencias del usuario
            bool temaOscuro = Properties.Settings.Default.TemaOscuro;
            ApplyBase(temaOscuro);
            TemaOscuro_ToggleButton.IsChecked = temaOscuro;

            // Cargar el color de las preferencias del usuario
            Color colorTema = (Color)ColorConverter.ConvertFromString(Properties.Settings.Default.ColorTema);
            ApplyColor(colorTema);
            ColorTema_ColorPicker.Color = colorTema;
            ColorHex_TextBox.Text = "#" + colorTema.ToString().Substring(3, colorTema.ToString().Length - 3);

            CheckearRadioButtonsPaletaColores();

            // Cargar el logo de las preferencias del usuario
            byte[] logo = Properties.Settings.Default.Logo;
            if (logo != null) Logo_Image.Source = ConvertirImagen.ConvertByteArrayToImage(logo);

            
            // Cargar el enum TipoProducto en el ListBox con los valores en plural
            TipoProducto[] tipoProductos = (TipoProducto[])Enum.GetValues(typeof(TipoProducto));
            foreach (TipoProducto tipoProducto in tipoProductos)
                Productos_ListBox.Items.Add(tipoProducto.ToDescription());

            Contenido_Grid.Children.Add(new Presupuestos(this));
        }

        

        // ------------------------------------------------------ //
        //                       Preferencias                     //
        // ------------------------------------------------------ //

        private void TemaOscuro_Expander_Expanded(object sender, RoutedEventArgs e)
        {
            PersonalizarTema_Expander.IsExpanded = false;
        }

        private void PersonalizarTema_Expander_Expanded(object sender, RoutedEventArgs e)
        {
            TemaOscuro_Expander.IsExpanded = false;
        }

        private void Preferencias_PopupBox_Closed(object sender, RoutedEventArgs e)
        {
            // Guardar en las preferencias del usuario
            Properties.Settings.Default.TemaOscuro = TemaOscuro_ToggleButton.IsChecked.Value;
            Properties.Settings.Default.ColorTema = ColorTema_ColorPicker.Color.ToString();
            Properties.Settings.Default.Save();

            TemaOscuro_Expander.IsExpanded = false;
            PersonalizarTema_Expander.IsExpanded = false;
            Colores_ToggleButton.IsChecked = false;
        }

        //
        // Tema oscuro
        //

        private void TemaOscuro_ToggleButton_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ApplyBase(TemaOscuro_ToggleButton.IsChecked.Value);
        }

        private static void ApplyBase(bool isDark)
        {
            ModifyTheme(theme => theme.SetBaseTheme(isDark ? Theme.Dark : Theme.Light));
        }

        private static void ModifyTheme(Action<ITheme> modificationAction)
        {
            PaletteHelper paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            modificationAction?.Invoke(theme);
            paletteHelper.SetTheme(theme);
        }

        //
        // Color de tema personalizado
        //

        private void ColorTema_ColorPicker_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Color color = ColorTema_ColorPicker.Color;
            ApplyColor(color);
            ColorHex_TextBox.Text = "#" + color.ToString().Substring(3, color.ToString().Length - 3);
        }

        private void ColorHex_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string hex = ColorHex_TextBox.Text;
            hex = hex.Replace("#", "");
            if ($"#FF{hex}" != ColorTema_ColorPicker.Color.ToString())
            {
                if (hex.Length == 6 && !Regex.IsMatch(hex, "[^a-fA-F0-9]"))
                {
                    ColorHex_TextBox.Text = $"#{hex.ToUpper()}";
                    Color color = (Color)ColorConverter.ConvertFromString($"#FF{hex}");
                    ColorTema_ColorPicker.Color = color;
                    ApplyColor(color);
                    CheckearRadioButtonsPaletaColores();
                    ColorHex_TextBox.Select(7, 0);
                }
            }
        }

        private readonly PaletteHelper paletteHelper = new PaletteHelper();

        private void ApplyColor(Color color)
        {
            ITheme theme = paletteHelper.GetTheme();
            theme.PrimaryLight = new ColorPair(color.Lighten(), theme.PrimaryLight.ForegroundColor);
            theme.PrimaryMid = new ColorPair(color, theme.PrimaryMid.ForegroundColor);
            theme.PrimaryDark = new ColorPair(color.Darken(), theme.PrimaryDark.ForegroundColor);
            paletteHelper.SetTheme(theme);
        }

        //
        // Paleta de colores
        //

        private void CambiarColorTema(object sender, RoutedEventArgs e)
        {
            foreach (RadioButton radioButton in PaletaColores_WrapPanel.Children)
            {
                if (radioButton.IsChecked.Value)
                {
                    Brush newColor = radioButton.Background;
                    SolidColorBrush newBrush = (SolidColorBrush)newColor;
                    Color myColorFromBrush = newBrush.Color;

                    Color colorTema = myColorFromBrush;
                    ApplyColor(colorTema);
                    ColorTema_ColorPicker.Color = colorTema;
                    ColorHex_TextBox.Text = "#" + colorTema.ToString().Substring(3, colorTema.ToString().Length - 3);
                    break;
                }
            }
        }

        private void Colores_ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckearRadioButtonsPaletaColores();
        }

        private void CheckearRadioButtonsPaletaColores()
        {
            Color colorTema = ColorTema_ColorPicker.Color;
            var converter = new BrushConverter();
            foreach (RadioButton radioButton in PaletaColores_WrapPanel.Children)
            {
                var brush = (Brush)converter.ConvertFromString(colorTema.ToString());
                if (radioButton.Background.ToString() == brush.ToString())
                {
                    radioButton.IsChecked = true;
                    break;
                }
                else
                    radioButton.IsChecked = false;
            }
        }

        // ------------------------------------------------------ //
        //                         Logo                           //
        // ------------------------------------------------------ //

        private void AgregarModificarLogo_Button_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "PNG (*.png)|*.png";
            var result = openFileDialog.ShowDialog();
            if (result == false) return;
            Logo_Image.Source = new BitmapImage(new Uri(openFileDialog.FileName));

            // Guardar en las preferencias del usuario
            Properties.Settings.Default.Logo = ConvertirImagen.ConvertImageToByteArray(openFileDialog.FileName);
            Properties.Settings.Default.Save();
        }

        private void EliminarLogo_Button_Click(object sender, RoutedEventArgs e)
        {
            Logo_Image.Source = null;

            // Eliminar de las preferencias del usuario
            Properties.Settings.Default.Logo = null;
            Properties.Settings.Default.Save();
        }

        // ------------------------------------------------------ //
        //                     Menú izquierdo                     //
        // ------------------------------------------------------ //

        private void MenuIzquierdo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Principal_Vista.IsLoaded && MenuIzquierdo.SelectedItems.Count == 1)
            {
                int index = MenuIzquierdo.SelectedIndex;
                ProductosBackground_Grid.Visibility = Visibility.Hidden;
                Productos_ListBox.UnselectAll();
                Contenido_Grid.Children.Clear();
                switch (index)
                {
                    case 0:
                        Contenido_Grid.Children.Add(new Presupuestos(this));
                        break;
                    case 1:
                        Contenido_Grid.Children.Add(new Clientes(this));
                        break;
                    case 2:
                        Contenido_Grid.Children.Add(new Marcas(this));
                        break;
                    case 6:
                        Contenido_Grid.Children.Add(new Productos(this, null));
                        break;
                    default:
                        break;
                }
            }
        }

        private void Productos_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Productos_ListBox.SelectedItems.Count == 1)
            {
                MenuProductos_Popup.IsOpen = false;
                ProductosBackground_Grid.Visibility = Visibility.Visible;
                MenuIzquierdo.UnselectAll();

                Contenido_Grid.Children.Clear();
                Contenido_Grid.Children.Add(new Productos(this, (TipoProducto)Productos_ListBox.SelectedIndex));
            }
        }

        private async void MenuProductos_Popup_MouseLeave(object sender, MouseEventArgs e)
        {
            await Task.Delay(50);
            if (!Productos_ListBoxItem.IsMouseOver)
            {
                OscurecerContenido_DialogHost.IsOpen = false;
                MenuProductos_Popup.IsOpen = false;
            }
        }

        private void Productos_ListBoxItem_MouseEnter(object sender, MouseEventArgs e)
        {
            OscurecerContenido_DialogHost.IsOpen = true;
            MenuProductos_Popup.IsOpen = true;
        }

        private async void Productos_ListBoxItem_MouseLeave(object sender, MouseEventArgs e)
        {
            await Task.Delay(50);
            if (!MenuProductos_Popup.IsMouseOver)
            {
                OscurecerContenido_DialogHost.IsOpen = false;
                MenuProductos_Popup.IsOpen = false;
            }
        }

        // ------------------------------------------------------ //
        //                 Oscurecer completamente                //
        // ------------------------------------------------------ //

        public void OscurecerCompletamente(bool estado)
        {
            OscurecerPantalla_DialogHost.IsOpen = estado;
        }

        // ------------------------------------------------------ //
        //             Mensajes arriba a la izquierda             //
        // ------------------------------------------------------ //

        public async Task MostrarMensajeEnSnackbar(string mensaje)
        {
            Mensaje_SnackbarMessage.Content = mensaje;
            await Task.Delay(500);
            Mensaje_Snackbar.IsActive = true;
            await Task.Delay(5000);
            Mensaje_Snackbar.IsActive = false;
        }

        private void Mensaje_SnackbarMessage_ActionClick(object sender, RoutedEventArgs e)
        {
            Mensaje_Snackbar.IsActive = false;
        }

        // ------------------------------------------------------ //
        //       Presupuesto seleccionado (falta completar)       //
        // ------------------------------------------------------ //


        public Presupuesto PresupuestoSeleccionado { get; set; }
        public void SeleccionarPresupuesto(Presupuesto presupuesto)
        {
            // Seleccionar el presupuesto y cambiar el icono
            PresupuestoSeleccionado = presupuesto;
            PresupuestoSeleccionado_Icon.Kind = PackIconKind.FileDocumentBoxTick;

            // Agregar el presupuesto, y cambiar visibilidades
            NoHayPresupuestoSeleccionado_StackPanel.Visibility = Visibility.Collapsed;
            PresupuestoSeleccionado_ListBox.Items.Clear();
            PresupuestoSeleccionado_ListBox.Items.Add(presupuesto);
            PresupuestoSeleccionado_ListBox.Visibility = Visibility.Visible;

            PresupuestoSeleccionado_PopupBox.IsPopupOpen = true;
        }

        private async void DeseleccionarPresupuesto_Button_Click(object sender, RoutedEventArgs e)
        {
            PresupuestoSeleccionado_PopupBox.IsPopupOpen = false;

            // Deseleccionar el presupuesto y cambiar el icono
            PresupuestoSeleccionado = null;
            PresupuestoSeleccionado_Icon.Kind = PackIconKind.FileDocumentBoxRemove;

            // Esperar para cambiar visibilidades
            await Task.Delay(250);
            NoHayPresupuestoSeleccionado_StackPanel.Visibility = Visibility.Visible;
            PresupuestoSeleccionado_ListBox.Visibility = Visibility.Collapsed;
        }
    }
}
