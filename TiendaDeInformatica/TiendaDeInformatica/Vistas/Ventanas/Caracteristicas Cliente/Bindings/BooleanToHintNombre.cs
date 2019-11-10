using System;
using System.Globalization;
using System.Windows.Data;

namespace TiendaDeInformatica.Vistas.Ventanas.Caracteristicas_Cliente.Bindings
{
    public class BooleanToHintNombre : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool _bool = (bool)value;
            if (_bool == true)
            {
                return "Nombre";
            }
            return "Nombre del representante";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
