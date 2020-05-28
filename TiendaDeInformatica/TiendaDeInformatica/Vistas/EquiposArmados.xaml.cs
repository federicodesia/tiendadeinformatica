using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TiendaDeInformatica.Controladores;
using TiendaDeInformatica.Helpers;
using TiendaDeInformatica.Modelos;
using TiendaDeInformatica.Vistas.Caracteristicas;

namespace TiendaDeInformatica.Vistas
{
    /// <summary>
    /// Lógica de interacción para Clientes.xaml
    /// </summary>
    public partial class EquiposArmados : UserControl
    {
        private Principal _principal;

        public EquiposArmados(Principal principal)
        {
            InitializeComponent();
            _principal = principal;
        }

        private void EquiposArmados_Vista_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
