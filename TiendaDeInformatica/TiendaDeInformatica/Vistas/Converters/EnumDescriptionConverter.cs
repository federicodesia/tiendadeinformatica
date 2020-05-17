using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using TiendaDeInformatica.Modelos;

namespace TiendaDeInformatica.Vistas.Converters
{
    public class EnumDescriptionConverter : IValueConverter
    {
        private string GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Enum.TryParse(value.ToString(), out TipoProducto tipoProducto))
            {
                return GetEnumDescription(tipoProducto);
            }
            return string.Empty;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
