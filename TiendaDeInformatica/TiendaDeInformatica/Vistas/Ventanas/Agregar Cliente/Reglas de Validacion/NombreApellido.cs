using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace TiendaDeInformatica.Vistas.Ventanas.Agregar_Cliente.Reglas_de_Validacion
{
    public class NombreApellido : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string _string = value as string;

            if (_string == null || string.IsNullOrEmpty(_string))
            {
                return new ValidationResult(false, "Completa este campo");
            }
            else if(System.Text.RegularExpressions.Regex.IsMatch(_string, "[^a-zA-Z áéíóúüñ'´`]"))
            {
                return new ValidationResult(false, "Ingresa solo letras");
            }
            return new ValidationResult(true, null);
        }
    }
}