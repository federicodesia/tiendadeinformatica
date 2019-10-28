using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TiendaDeInformatica.Modelos;

namespace TiendaDeInformatica.Vistas.Ventanas.Agregar_Producto.Reglas_de_Validacion
{
    public class TipoProductoSeleccionado : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is TipoProducto)
            {
                return new ValidationResult(true, null);
            }
            return new ValidationResult(false, "Selecciona un tipo de producto");
        }
    }
}
