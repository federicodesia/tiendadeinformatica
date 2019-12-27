using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TiendaDeInformatica.Vistas.Converters
{
    public class PresupuestoCirculoColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                DateTime fechaExpiracion = (DateTime)value;
                TimeSpan tiempoRestante = (fechaExpiracion.Date) - DateTime.Now.Date;

                if (tiempoRestante.Days < 3 && tiempoRestante.Days >= 0)
                {
                    // Naranja
                    return "#FF6D00";
                }
                else if (tiempoRestante.Days < 0)
                {
                    // Rojo
                    return "#D50000";
                }
                // Verde
                return "#64DD17";
            }
            // Gris
            return "#707070";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
