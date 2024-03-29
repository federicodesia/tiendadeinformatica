﻿using MaterialDesignColors;
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
using TiendaDeInformatica.Controladores;
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

            try
            {
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
                    Productos_ListBox.Items.Add(tipoProducto.GetEnumDescription());

                ControladorProductos.EliminarProductoVacio();

                presupuestosUserControl = new Presupuestos(this);
                Contenido_Grid.Children.Add(presupuestosUserControl);
            }
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
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
            try
            {
                // Guardar en las preferencias del usuario
                Properties.Settings.Default.TemaOscuro = TemaOscuro_ToggleButton.IsChecked.Value;
                Properties.Settings.Default.ColorTema = ColorTema_ColorPicker.Color.ToString();
                Properties.Settings.Default.Save();

                TemaOscuro_Expander.IsExpanded = false;
                PersonalizarTema_Expander.IsExpanded = false;
                Colores_ToggleButton.IsChecked = false;
            }
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        //
        // Tema oscuro
        //

        private void TemaOscuro_ToggleButton_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                ApplyBase(TemaOscuro_ToggleButton.IsChecked.Value);
            }
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private void ApplyBase(bool isDark)
        {
            try
            {
                ModifyTheme(theme => theme.SetBaseTheme(isDark ? Theme.Dark : Theme.Light));
            }
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private void ModifyTheme(Action<ITheme> modificationAction)
        {
            try
            {
                PaletteHelper paletteHelper = new PaletteHelper();
                ITheme theme = paletteHelper.GetTheme();
                modificationAction?.Invoke(theme);
                paletteHelper.SetTheme(theme);
            }
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        //
        // Color de tema personalizado
        //

        private void ColorTema_ColorPicker_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Color color = ColorTema_ColorPicker.Color;
                ApplyColor(color);
                ColorHex_TextBox.Text = "#" + color.ToString().Substring(3, color.ToString().Length - 3);
            }
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private void ColorHex_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
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
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private readonly PaletteHelper paletteHelper = new PaletteHelper();

        private void ApplyColor(Color color)
        {
            try
            {
                ITheme theme = paletteHelper.GetTheme();
                theme.PrimaryLight = new ColorPair(color.Lighten(), theme.PrimaryLight.ForegroundColor);
                theme.PrimaryMid = new ColorPair(color, theme.PrimaryMid.ForegroundColor);
                theme.PrimaryDark = new ColorPair(color.Darken(), theme.PrimaryDark.ForegroundColor);
                paletteHelper.SetTheme(theme);
            }
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        //
        // Paleta de colores
        //

        private void CambiarColorTema(object sender, RoutedEventArgs e)
        {
            try
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
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private void Colores_ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckearRadioButtonsPaletaColores();
        }

        private void CheckearRadioButtonsPaletaColores()
        {
            try
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
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        // ------------------------------------------------------ //
        //                         Logo                           //
        // ------------------------------------------------------ //

        private void AgregarModificarLogo_Button_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private void EliminarLogo_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Logo_Image.Source = null;

                // Eliminar de las preferencias del usuario
                Properties.Settings.Default.Logo = null;
                Properties.Settings.Default.Save();
            }
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        // ------------------------------------------------------ //
        //                     Menú izquierdo                     //
        // ------------------------------------------------------ //

        private void MenuIzquierdo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Principal_Vista.IsLoaded && MenuIzquierdo.SelectedItems.Count == 1)
                {
                    int index = MenuIzquierdo.SelectedIndex;

                    PresupuestoSeleccionado_PopupBox.IsPopupOpen = false;
                    ProductosBackground_Grid.Visibility = Visibility.Hidden;
                    Productos_ListBox.UnselectAll();

                    PresupuestosBackground_Grid.Visibility = Visibility.Hidden;
                    presupuestosUserControl = null;
                    configuracionGuiadaUserControl = null;

                    Contenido_Grid.Children.Clear();
                    switch (index)
                    {
                        case 0:
                            presupuestosUserControl = new Presupuestos(this);
                            Contenido_Grid.Children.Add(presupuestosUserControl);
                            break;
                        case 1:
                            Contenido_Grid.Children.Add(new Clientes(this));
                            break;
                        case 2:
                            Contenido_Grid.Children.Add(new Marcas(this));
                            break;
                        case 4:
                            Contenido_Grid.Children.Add(new EquiposArmados(this));
                            break;
                        case 5:
                            configuracionGuiadaUserControl = new ConfiguracionGuiada(this, PresupuestoSeleccionadoId);
                            Contenido_Grid.Children.Add(configuracionGuiadaUserControl);
                            break;
                        case 6:
                            Contenido_Grid.Children.Add(new Productos(this, null));
                            break;
                        case 8:
                            Contenido_Grid.Children.Add(new Configuracion(this));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private void Productos_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
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
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private async void MenuProductos_Popup_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                await Task.Delay(50);
                if (!Productos_ListBoxItem.IsMouseOver)
                {
                    OscurecerContenido_DialogHost.IsOpen = false;
                    MenuProductos_Popup.IsOpen = false;
                }
            }
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private void Productos_ListBoxItem_MouseEnter(object sender, MouseEventArgs e)
        {
            OscurecerContenido_DialogHost.IsOpen = true;
            MenuProductos_Popup.IsOpen = true;
        }

        private async void Productos_ListBoxItem_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                await Task.Delay(50);
                if (!MenuProductos_Popup.IsMouseOver)
                {
                    OscurecerContenido_DialogHost.IsOpen = false;
                    MenuProductos_Popup.IsOpen = false;
                }
            }
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
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
        //                 Presupuesto seleccionado               //
        // ------------------------------------------------------ //

        private ConfiguracionGuiada configuracionGuiadaUserControl { get; set; }
        private Presupuestos presupuestosUserControl { get; set; }
        public ResumenPresupuesto reumenPresupuestosUserControl { get; set; }

        public static int PresupuestoSeleccionadoId = -1;

        public void SeleccionarPresupuesto(Presupuesto presupuesto)
        {
            PresupuestoSeleccionadoId = presupuesto.Id;
            PresupuestoSeleccionado_Icon.Kind = PackIconKind.FileDocumentBoxTick;
            PresupuestoSeleccionado_PopupBox.IsPopupOpen = true;
        }

        public void AgregarProductoAPresupuesto(Producto producto, bool abrirPopup)
        {
            try
            {
                if (PresupuestoSeleccionadoId != -1)
                {
                    Presupuesto presupuesto = ControladorPresupuestos.ObtenerPresupuesto(PresupuestoSeleccionadoId);
                    ControladorPresupuestos.AgregarPresupuestoProducto(presupuesto, producto);
                }
                if (abrirPopup) PresupuestoSeleccionado_PopupBox.IsPopupOpen = true;
            }
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private async void DeseleccionarPresupuesto_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PresupuestoSeleccionadoId = -1;
                PresupuestoSeleccionado_Icon.Kind = PackIconKind.FileDocumentBoxRemove;
                PresupuestoSeleccionado_PopupBox.IsPopupOpen = false;

                await Task.Delay(250);
                PresupuestoSelecciondo_StackPanel.DataContext = null;

                if (configuracionGuiadaUserControl != null)
                {
                    _ = configuracionGuiadaUserControl._presupuestoSeleccionado == null;
                    configuracionGuiadaUserControl.CambiarEstadoAlertaPresupuestoSeleccionado(true);
                }
            }
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private void EliminarPresupuestoProducto_TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                TextBlock textBlockSender = (TextBlock)sender;
                if (textBlockSender.DataContext is PresupuestoProducto)
                {
                    ControladorPresupuestos.EliminarPresupuestoProducto((PresupuestoProducto)textBlockSender.DataContext);
                    RefrescarListaPresupuestoProducto();
                }
            }
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private void RefrescarListaPresupuestoProducto()
        {
            try
            {
                if (PresupuestoSeleccionadoId != -1)
                {
                    Presupuesto presupuesto = ControladorPresupuestos.ObtenerPresupuesto(PresupuestoSeleccionadoId);
                    PresupuestoSelecciondo_StackPanel.DataContext = presupuesto;

                    PresupuestoSeleccionado_Productos_ListBox.Items.Clear();
                    foreach (PresupuestoProducto presupuestoProducto in presupuesto.Productos)
                        PresupuestoSeleccionado_Productos_ListBox.Items.Add(presupuestoProducto);

                    if (presupuestosUserControl != null)
                        presupuestosUserControl.RefrescarListaDePresupuestos();

                    if (reumenPresupuestosUserControl != null)
                        reumenPresupuestosUserControl.RefrescarListaPresupuestoProducto();
                }
            }
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        // Botón Agregar o Eliminar Cantidad. TextBox de Cantidad.

        private void AgregarCantidadPresupuestoProducto_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button buttonSender = (Button)sender;
                if (buttonSender.DataContext is PresupuestoProducto)
                {
                    PresupuestoProducto presupuestoProducto = (PresupuestoProducto)buttonSender.DataContext;
                    if (presupuestoProducto.Cantidad < 99)
                    {
                        ControladorPresupuestos.ModificarCantidadPresupuestoProducto(presupuestoProducto, presupuestoProducto.Cantidad += 1);
                        RefrescarListaPresupuestoProducto();
                    }
                }
            }
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private void EliminarCantidadPresupuestoProducto_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button buttonSender = (Button)sender;
                if (buttonSender.DataContext is PresupuestoProducto)
                {
                    PresupuestoProducto presupuestoProducto = (PresupuestoProducto)buttonSender.DataContext;
                    if (presupuestoProducto.Cantidad > 1)
                    {
                        ControladorPresupuestos.ModificarCantidadPresupuestoProducto(presupuestoProducto, presupuestoProducto.Cantidad -= 1);
                        RefrescarListaPresupuestoProducto();
                    }
                }
            }
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private async void CantidadPresupuestoProducto_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox textBoxSender = (TextBox)sender;
                string texto = textBoxSender.Text;

                await Task.Delay(100);
                if (texto == textBoxSender.Text)
                {
                    if (textBoxSender.DataContext is PresupuestoProducto)
                    {
                        bool esInt = int.TryParse(((TextBox)sender).Text, out int cantidad);
                        if (esInt && cantidad > 0 && cantidad <= 99)
                        {
                            ControladorPresupuestos.ModificarCantidadPresupuestoProducto((PresupuestoProducto)textBoxSender.DataContext, cantidad);
                            RefrescarListaPresupuestoProducto();
                        }
                    }
                }
            }
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }

        private void PresupuestoSeleccionado_PopupBox_Opened(object sender, RoutedEventArgs e)
        {
            RefrescarListaPresupuestoProducto();
        }

        private void Resumen_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PresupuestoSeleccionado_PopupBox.IsPopupOpen = false;
                Contenido_Grid.Children.Clear();
                ResumenPresupuesto resumenPresupuesto = new ResumenPresupuesto(this, PresupuestoSeleccionadoId);
                reumenPresupuestosUserControl = resumenPresupuesto;
                Contenido_Grid.Children.Add(resumenPresupuesto);
            }
            catch (Exception error)
            {
                _ = MostrarMensajeEnSnackbar("Oops! algo salió mal. Error: " + error);
            }
        }
    }
}
