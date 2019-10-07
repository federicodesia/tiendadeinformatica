using System;
using System.Globalization;
using System.Windows.Data;

namespace TiendaDeInformatica.Vistas.Ventanas.Agregar_Presupuesto.Bindings
{
    public class KindConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string Tipo = (string)value;
            if (Tipo=="Empresa")
            {
                return "OfficeBuilding";
            }
            return "Person";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
