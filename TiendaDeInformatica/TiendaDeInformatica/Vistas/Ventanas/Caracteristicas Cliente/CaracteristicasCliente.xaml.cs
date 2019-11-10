using MahApps.Metro.Controls;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using TiendaDeInformatica.Controladores;
using TiendaDeInformatica.Modelos;
using TiendaDeInformatica.Vistas.Ventanas.Caracteristicas_Cliente.Reglas_de_Validacion;

namespace TiendaDeInformatica.Vistas.Ventanas.Caracteristicas_Cliente
{
    /// <summary>
    /// Lógica de interacción para AgregarCliente.xaml
    /// </summary>
    public partial class CaracteristicasCliente : MetroWindow
    {
        public string NombreDeLaEmpresa_TextBox_Text { get; set; }
        public string Nombre_TextBox_Text { get; set; }
        public string Apellido_TextBox_Text { get; set; }
        public string Telefono_TextBox_Text { get; set; }
        public string CUIT_TextBox_Text { get; set; }

        public Cliente cliente { get; set; }
        public CaracteristicasCliente(Cliente clienteModificar)
        {
            InitializeComponent();
            this.DataContext = this;

            cliente = clienteModificar;
        }

        private void Crear_Button_Click(object sender, RoutedEventArgs e)
        {
            if (TabMenu_EmpresaButton.IsChecked == true && new NombreEmpresa().Validate(NombreDeLaEmpresa_TextBox.Text, CultureInfo.CurrentCulture) == new ValidationResult(true, null) && ObtenerResultadoReglasDeValidacion() == true)
            {
                if (cliente == null)
                {
                    ControladorClientes.AgregarCliente(NombreDeLaEmpresa_TextBox.Text, Nombre_TextBox.Text, Apellido_TextBox.Text, CUIT_TextBox.Text, Telefono_TextBox.Text);
                    this.Close();
                }
                else
                {
                    ControladorClientes.ModificarCliente(cliente, NombreDeLaEmpresa_TextBox.Text, Nombre_TextBox.Text, Apellido_TextBox.Text, CUIT_TextBox.Text, Telefono_TextBox.Text);
                    this.Close();
                }
                
            }
            else if (TabMenu_PersonaButton.IsChecked == true && ObtenerResultadoReglasDeValidacion() == true)
            {
                if (cliente == null)
                {
                    ControladorClientes.AgregarCliente(null, Nombre_TextBox.Text, Apellido_TextBox.Text, CUIT_TextBox.Text, Telefono_TextBox.Text);
                    this.Close();
                }
                else
                {
                    ControladorClientes.ModificarCliente(cliente, null, Nombre_TextBox.Text, Apellido_TextBox.Text, CUIT_TextBox.Text, Telefono_TextBox.Text);
                    this.Close();
                }
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
            if (cliente == null)
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
            else
            {
                if((NombreDeLaEmpresa_TextBox.Text != "" && cliente.NombreDeLaEmpresa == null) || (NombreDeLaEmpresa_TextBox.Text != cliente.NombreDeLaEmpresa && cliente.Tipo=="Empresa")||
                    (Nombre_TextBox.Text != "" && cliente.Nombre == null) || (Nombre_TextBox.Text != cliente.Nombre)||
                    (Apellido_TextBox.Text != "" && cliente.Apellido == null) || (Apellido_TextBox.Text != cliente.Apellido)||
                    (Telefono_TextBox.Text != "" && cliente.Telefono == null) || (Telefono_TextBox.Text != cliente.Telefono)||
                    (CUIT_TextBox.Text != "" && cliente.CUIT == null) || (CUIT_TextBox.Text != cliente.CUIT))
                {
                    Dialog.IsOpen = true;
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (cliente != null)
            {
                if (cliente.Tipo == "Empresa")
                {
                    TabMenu_EmpresaButton.IsChecked = true;
                    NombreDeLaEmpresa_TextBox.Text = cliente.NombreDeLaEmpresa;
                }
                Nombre_TextBox.Text = cliente.Nombre;
                Apellido_TextBox.Text = cliente.Apellido;
                Telefono_TextBox.Text = cliente.Telefono;
                CUIT_TextBox.Text = cliente.CUIT;

                VistaCaracteristicasCliente.Title = "Modificar cliente";
                Titulo_TextBlock.Text = "Modificar cliente";
                Crear_Button.Content = "MODIFICAR";
            }
        }
    }
}
