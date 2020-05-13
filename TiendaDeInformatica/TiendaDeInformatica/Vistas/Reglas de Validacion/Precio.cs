using System;
using System.Globalization;
using System.Windows.Controls;

namespace TiendaDeInformatica.Vistas.Reglas_de_Validacion
{
    public class Precio : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string _string = value as string;

            if (_string == null || string.IsNullOrEmpty(_string))
                return new ValidationResult(false, "Completa este campo");

            if (Decimal.TryParse(_string, out decimal _decimal))
                return new ValidationResult(true, null);
            return new ValidationResult(false, "Ingresa solo números");
        }
    }
}
