using System.Globalization;
using System.Windows.Controls;
using TiendaDeInformatica.Modelos;

namespace TiendaDeInformatica.Vistas.Reglas_de_Validacion
{
    public class ClienteSeleccionado : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (value is Cliente)
                    return new ValidationResult(true, null);
                return new ValidationResult(false, "Selecciona un cliente");
            }
            catch
            {
                return new ValidationResult(false, "Oops! ocurrió un error inesperado");
            }
        }
    }
}
