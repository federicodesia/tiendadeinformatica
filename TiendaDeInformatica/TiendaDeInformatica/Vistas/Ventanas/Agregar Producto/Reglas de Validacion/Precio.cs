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
    public class Precio : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string _string = value as string;
            if (_string == null || string.IsNullOrEmpty(_string))
            {
                return new ValidationResult(false, "Ingresa un precio");
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(_string, "[^0-9.]"))
            {
                return new ValidationResult(false, "Ingresa solo números y puntos");
            }
            return new ValidationResult(true, null);
        }
    }
}
