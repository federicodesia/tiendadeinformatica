using System;
using System.Globalization;
using System.Windows.Data;

namespace TiendaDeInformatica.Vistas.Ventanas.Bindings_Generales
{
    public class InvertedBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool _bool = (bool)value;
            return !_bool;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }
            return value;
        }
    }
}
