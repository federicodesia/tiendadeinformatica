using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TiendaDeInformatica.Modelos;

namespace TiendaDeInformatica.Vistas.Ventanas.Reglas_de_Validacion_Generales
{
    public class CampoVacio : ValidationRule
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
