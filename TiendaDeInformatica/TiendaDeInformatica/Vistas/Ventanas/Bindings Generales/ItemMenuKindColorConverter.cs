using System;
using System.Globalization;
using System.Windows.Data;

namespace TiendaDeInformatica.Vistas.Ventanas.Bindings_Generales
{
    public class ItemMenuKindColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool _bool = (bool)value;
            if (_bool)
            {
                return "#FFFFFF";
            }
            return "#909090";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
