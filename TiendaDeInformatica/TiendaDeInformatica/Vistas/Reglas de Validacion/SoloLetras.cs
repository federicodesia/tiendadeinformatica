using System.Globalization;
using System.Windows.Controls;

namespace TiendaDeInformatica.Vistas.Reglas_de_Validacion
{
    public class SoloLetras : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string _string = value as string;

                if (_string == null || string.IsNullOrEmpty(_string))
                    return new ValidationResult(false, "Completa este campo");
                else if (System.Text.RegularExpressions.Regex.IsMatch(_string, "[^a-zA-Z áéíóúüñ'´]"))
                    return new ValidationResult(false, "Ingresa solo letras");
                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, "Oops! ocurrió un error inesperado");
            }
        }
    }
}
