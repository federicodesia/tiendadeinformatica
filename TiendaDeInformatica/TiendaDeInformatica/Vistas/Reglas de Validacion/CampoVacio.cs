using System.Globalization;
using System.Windows.Controls;

namespace TiendaDeInformatica.Vistas.Reglas_de_Validacion
{
    public class CampoVacio : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string _string = value as string;

                if (_string == null || string.IsNullOrEmpty(_string))
                    return new ValidationResult(false, "Completa este campo");
                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, "Oops! ocurrió un error inesperado");
            }
        }
    }
}
