using System;
using System.Globalization;
using System.Windows.Data;

namespace TiendaDeInformatica.Vistas.Converters
{
    public class PresupuestoBarraColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                DateTime fechaExpiracion = (DateTime)value;
                TimeSpan tiempoRestante = (fechaExpiracion.Date) - DateTime.Now.Date;

                if (tiempoRestante.Days < 0)
                {
                    // Naranja
                    return "#F9A825";
                }
                // Verde
                return "#4CAF50";
            }
            // Gris
            return "#707070";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.InvalidOperationException("Oops! ocurrió un error inesperado");
        }
    }
}
