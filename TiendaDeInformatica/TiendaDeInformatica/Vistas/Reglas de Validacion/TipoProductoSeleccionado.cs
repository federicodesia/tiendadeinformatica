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
            if (value != null && Enum.TryParse(value.ToString(), out TipoProducto tipoProducto))
                return new ValidationResult(true, null);
            return new ValidationResult(false, "Selecciona un tipo de producto");
        }
    }
}