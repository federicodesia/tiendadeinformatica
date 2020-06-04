using System.Globalization;
using System.Windows.Controls;
using TiendaDeInformatica.Helpers;
using TiendaDeInformatica.Modelos;
using TiendaDeInformatica.Vistas.Caracteristicas;

namespace TiendaDeInformatica.Vistas.Reglas_de_Validacion
{
    public class ValorValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string _string = value as string;

            if (string.IsNullOrEmpty(_string))
                return new ValidationResult(false, "Completa este campo");

            Atributo _atributo = CaracteristicasValor._atributo;
            Valor _valorModificar = CaracteristicasValor._valorModificar;
            string nombre = TextHelper.QuitarTildes(_string).ToUpper();

            foreach (Valor valor in _atributo.Valores)
            {
                if (((_valorModificar != null) && (_valorModificar.Id != valor.Id) && (TextHelper.QuitarTildes(valor.Nombre).ToUpper() == nombre))
                    || (_valorModificar == null && (TextHelper.QuitarTildes(valor.Nombre).ToUpper() == nombre)))
                {
                    return new ValidationResult(false, $"El valor {nombre} ya existe");
                }
            }

            return new ValidationResult(true, null);
        }
    }
}
