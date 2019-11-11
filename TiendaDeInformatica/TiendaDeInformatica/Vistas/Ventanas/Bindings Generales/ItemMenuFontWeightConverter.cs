using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TiendaDeInformatica.Vistas.Ventanas.Bindings_Generales
{
    public class ItemMenuFontWeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool _bool = (bool)value;
            if (_bool)
            {
                return FontWeights.SemiBold;
            }
            return FontWeights.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
