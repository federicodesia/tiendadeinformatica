using System;
using System.Globalization;
using System.Windows.Controls;
using TiendaDeInformatica.Vistas.Caracteristicas;

namespace TiendaDeInformatica.Vistas.Reglas_de_Validacion
{
    public class FechaDeExpiracion : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (CaracteristicasPresupuesto.ObtenerEstadoFechaExpiracion() == true)
            {
                DateTime time;
                if (DateTime.TryParse((value ?? "").ToString(), CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces, out time))
                {
                    return new ValidationResult(true, null);
                }
                return new ValidationResult(false, "Ingresa una fecha de expiración");
            }
            return new ValidationResult(true, null);
        }
    }
}
