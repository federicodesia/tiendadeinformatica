using System.Globalization;
using System.Windows.Controls;
using TiendaDeInformatica.Modelos;

namespace TiendaDeInformatica.Vistas.Ventanas.Agregar_Presupuesto.Reglas_de_Validacion
{
    public class ClienteSeleccionado : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is Cliente)
            {
                return new ValidationResult(true, null);
            }
            return new ValidationResult(false, "Selecciona un cliente");
        }
    }
}