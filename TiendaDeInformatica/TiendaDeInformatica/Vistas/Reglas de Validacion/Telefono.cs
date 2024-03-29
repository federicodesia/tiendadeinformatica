﻿using System.Globalization;
using System.Windows.Controls;

namespace TiendaDeInformatica.Vistas.Reglas_de_Validacion
{
    public class Telefono : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string _string = value as string;

                if (_string != null && System.Text.RegularExpressions.Regex.IsMatch(_string, "[^-0-9+()# *]"))
                    return new ValidationResult(false, "Teléfono incorrecto");
                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, "Oops! ocurrió un error inesperado");
            }
        }
    }
}
