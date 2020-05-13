﻿using System.Globalization;
using System.Windows.Controls;
using TiendaDeInformatica.Modelos;

namespace TiendaDeInformatica.Vistas.Reglas_de_Validacion
{
    public class MarcaSeleccionada : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is Marca)
                return new ValidationResult(true, null);
            return new ValidationResult(false, "Selecciona una marca");
        }
    }
}
