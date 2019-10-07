using System.Globalization;
using System.Windows.Controls;

namespace TiendaDeInformatica.Vistas.Ventanas.Agregar_Cliente.Reglas_de_Validacion
{
    public class Telefono : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string _string = value as string;

            if (_string != null && System.Text.RegularExpressions.Regex.IsMatch(_string, "[^-0-9+()# *]"))
            {
                return new ValidationResult(false, "Teléfono incorrecto");
            }
            return new ValidationResult(true, null);
        }
    }
}