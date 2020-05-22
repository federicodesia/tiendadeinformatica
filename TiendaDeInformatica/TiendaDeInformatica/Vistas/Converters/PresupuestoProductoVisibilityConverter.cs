using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TiendaDeInformatica.Vistas.Converters
{
    public class PresupuestoProductoVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Principal.PresupuestoSeleccionadoId != -1)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
