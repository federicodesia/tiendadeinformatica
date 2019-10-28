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
    public class Modelo : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string _string = value as string;
            if (_string == null || string.IsNullOrEmpty(_string))
            {
                return new ValidationResult(false, "Ingresa un modelo");
            }
            return new ValidationResult(true, null);
        }
    }
}
