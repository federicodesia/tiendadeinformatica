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
    public partial class ConfiguracionGuiada : UserControl
    {
        private Principal _principal;

        public ConfiguracionGuiada(Principal principal)
        {
            InitializeComponent();
            _principal = principal;
        }

        private void ConfiguracionGuiada_Vista_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
