using System;
using System.Globalization;
using System.Windows.Controls;
using TiendaDeInformatica.Modelos;

namespace TiendaDeInformatica.Vistas.Reglas_de_Validacion
{
    public class TipoProductoSeleccionado : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (value != null)
                {
                    int index = int.Parse(value.ToString());
                    if (Enum.IsDefined(typeof(TipoProducto), index))
                        return new ValidationResult(true, null);
                }
                return new ValidationResult(false, "Selecciona un tipo de producto");
            }
            catch
            {
                return new ValidationResult(false, "Oops! ocurrió un error inesperado");
            }
        }
    }
}