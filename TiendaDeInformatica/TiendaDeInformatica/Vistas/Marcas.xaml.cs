using System.Windows.Controls;

namespace TiendaDeInformatica.Vistas
{
    /// <summary>
    /// Lógica de interacción para Clientes.xaml
    /// </summary>
    public partial class Marcas : UserControl
    {
        private Principal _principal;

        public Marcas(Principal principal)
        {
            InitializeComponent();
            _principal = principal;
        }
    }
}
