using System.Windows.Controls;
using TiendaDeInformatica.Vistas;

namespace TiendaDeInformatica.Vistas
{
    /// <summary>
    /// Lógica de interacción para Clientes.xaml
    /// </summary>
    public partial class Productos : UserControl
    {
        private Principal _principal;

        public Productos(Principal principal)
        {
            InitializeComponent();
            _principal = principal;
        }
    }
}
