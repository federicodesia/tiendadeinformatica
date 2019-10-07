using System.Globalization;
using System.Windows.Controls;

namespace TiendaDeInformatica.Vistas.Ventanas.Agregar_Cliente.Reglas_de_Validacion
{
    public class NombreEmpresa : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string _string = value as string;

            if (_string == null || string.IsNullOrEmpty(_string))
            {
                return new ValidationResult(false, "Completa este campo");
            }
            return new ValidationResult(true, null);
        }
    }
}