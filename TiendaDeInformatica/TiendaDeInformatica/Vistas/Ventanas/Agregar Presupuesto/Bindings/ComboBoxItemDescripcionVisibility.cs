using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TiendaDeInformatica.Vistas.Ventanas.Agregar_Presupuesto.Bindings
{
    public class ComboBoxItemDescripcionVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string _string = (string)value;
            if (_string == "")
            {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
