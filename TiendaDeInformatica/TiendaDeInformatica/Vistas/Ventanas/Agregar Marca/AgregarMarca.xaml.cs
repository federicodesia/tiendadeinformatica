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
using TiendaDeInformatica.Helpers;
using TiendaDeInformatica.Modelos;
using TiendaDeInformatica.Vistas.Ventanas.Agregar_Producto.Reglas_de_Validacion;
using TiendaDeInformatica.Vistas.Ventanas.Reglas_de_Validacion_Generales;

namespace TiendaDeInformatica.Vistas.Ventanas.Agregar_Marca
{
    /// <summary>
    /// Lógica de interacción para AgregarProducto.xaml
    /// </summary>
    public partial class AgregarMarca : MetroWindow
    {
        public string Nombre_TextBox_Text { get; set; }
        public string RutaDeLaImagen { get; set; }


        public AgregarMarca()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Cancelar_Button_Click(object sender, RoutedEventArgs e)
        {
            MostrarDialog();
        }

        private void Crear_Button_Click(object sender, RoutedEventArgs e)
        {
            if (new CampoVacio().Validate(Nombre_TextBox.Text, CultureInfo.CurrentCulture) == new ValidationResult(true, null))
            {
                ControladorMarcas.AgregarMarca(Nombre_TextBox.Text, RutaDeLaImagen);
                this.Close();
            }
            else
            {
                Nombre_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }

        private void MostrarDialog()
        {
            if(Nombre_TextBox.Text!="")
            {
                Dialog.IsOpen = true;
            }
            else
            {
                this.Close();
            }
        }

        private void SeleccionarImagen_Button_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "Todas las imágenes|*.png;*.jpg;*.jpeg";
            var result = ofd.ShowDialog();
            if (result == false) return;
            Imagen_Image.Source = new BitmapImage(new Uri(ofd.FileName));
            RutaDeLaImagen = ofd.FileName;
        }
    }
}
