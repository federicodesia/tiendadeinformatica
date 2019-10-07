using System;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace TiendaDeInformatica.Vistas.Ventanas.Agregar_Cliente.Reglas_de_Validacion
{
    public class CUIT : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string cuit = value as string;
            if (cuit == null || string.IsNullOrEmpty(cuit))
            {
                return new ValidationResult(true, null);
            }
            else
            {
                cuit = cuit.Replace("-", null);
                if (System.Text.RegularExpressions.Regex.IsMatch(cuit, "[^0-9-]"))
                {
                    return new ValidationResult(false, "Ingresa solo números y guiones");
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(cuit) || cuit.Length != 11 || !cuit.All(Char.IsNumber))
                    {
                        return new ValidationResult(false, "CUIT incorrecto");
                    }
                    var factores = new int[] { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
                    var acumulado = 0;

                    for (int i = 0; i < factores.Length; i++)
                    {
                        acumulado += int.Parse(cuit[i].ToString()) * factores[i];
                    }

                    acumulado = 11 - (acumulado % 11);
                    if (acumulado == 11)
                    {
                        acumulado = 0;
                    }

                    if (int.Parse(cuit[10].ToString()) != acumulado)
                    {
                        return new ValidationResult(false, "CUIT incorrecto");
                    }
                    return new ValidationResult(true, null);
                }
            }
        }
    }
}