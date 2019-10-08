using MahApps.Metro.Controls;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using TiendaDeInformatica.Controladores;
using TiendaDeInformatica.Vistas.Ventanas.Agregar_Cliente.Reglas_de_Validacion;

namespace TiendaDeInformatica.Vistas.Ventanas.Agregar_Cliente
{
    /// <summary>
    /// Lógica de interacción para AgregarCliente.xaml
    /// </summary>
    public partial class AgregarCliente : MetroWindow
    {
        public string NombreDeLaEmpresa_TextBox_Text { get; set; }
        public string Nombre_TextBox_Text { get; set; }
        public string Apellido_TextBox_Text { get; set; }
        public string Telefono_TextBox_Text { get; set; }
        public string CUIT_TextBox_Text { get; set; }


        public AgregarCliente()
        {
            InitializeComponent();
            ClienteID_TextBlock.Text = "Nuevo cliente #" + (ControladorClientes.ObtenerListaDeClientes().Count + 1).ToString();
            this.DataContext = this;
        }

        private void Crear_Button_Click(object sender, RoutedEventArgs e)
        {
            if (TabMenu_EmpresaButton.IsChecked == true && new NombreEmpresa().Validate(NombreDeLaEmpresa_TextBox.Text, CultureInfo.CurrentCulture) == new ValidationResult(true, null) && ObtenerResultadoReglasDeValidacion() == true)
            {
                ControladorClientes.AgregarCliente(NombreDeLaEmpresa_TextBox.Text, Nombre_TextBox.Text, Apellido_TextBox.Text, CUIT_TextBox.Text, Telefono_TextBox.Text);
                this.Close();
            }
            else if (TabMenu_PersonaButton.IsChecked == true && ObtenerResultadoReglasDeValidacion() == true)
            {
                ControladorClientes.AgregarCliente(null, Nombre_TextBox.Text, Apellido_TextBox.Text, CUIT_TextBox.Text, Telefono_TextBox.Text);
                this.Close();
            }
            else
            {
                NombreDeLaEmpresa_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                Nombre_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                Apellido_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                Telefono_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                CUIT_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }

        private bool ObtenerResultadoReglasDeValidacion()
        {
            if (new NombreApellido().Validate(Nombre_TextBox.Text, CultureInfo.CurrentCulture) == new ValidationResult(true, null)
                && new NombreApellido().Validate(Apellido_TextBox.Text, CultureInfo.CurrentCulture) == new ValidationResult(true, null)
                && new Telefono().Validate(Telefono_TextBox.Text, CultureInfo.CurrentCulture) == new ValidationResult(true, null)
                && new CUIT().Validate(CUIT_TextBox.Text, CultureInfo.CurrentCulture) == new ValidationResult(true, null))
            {
                return true;
            }
            return false;
        }

        private void Cancelar_Button_Click(object sender, RoutedEventArgs e)
        {
            MostrarDialog();
        }

        private void MostrarDialog()
        {
            if ((TabMenu_EmpresaButton.IsChecked == true && NombreDeLaEmpresa_TextBox.Text != "") || Nombre_TextBox.Text != "" || Apellido_TextBox.Text != "" || Telefono_TextBox.Text != "" || CUIT_TextBox.Text != "")
            {
                Dialog.IsOpen = true;
            }
            else
            {
                this.Close();
            }
        }
    }
}
