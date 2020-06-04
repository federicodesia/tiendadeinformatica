using System;
using System.Globalization;
using System.Windows.Data;

namespace TiendaDeInformatica.Vistas.Converters
{
    public class PresupuestoBarraToolTipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                DateTime fechaExpiracion = (DateTime)value;
                TimeSpan tiempoRestante = fechaExpiracion.Date - DateTime.Now.Date;

                if (tiempoRestante.Days < 0)
                {
                    if (tiempoRestante.Days >= -30)
                        return $"Expiró hace {tiempoRestante.Days * -1} días";
                    return $"Expiró el {fechaExpiracion.Date.ToString("dd/MM/yyyy")}";
                }
                else if (tiempoRestante.Days == 0)
                    return "Expira hoy";
                else if (tiempoRestante.Days == 1)
                    return "Expira en 1 día";
                else if (tiempoRestante.Days < 30)
                    return $"Expira en {tiempoRestante.Days} días";
                return $"Expira el {fechaExpiracion.Date.ToString("dd/MM/yyyy")}";
            }
            return "Sin fecha de expiración";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
