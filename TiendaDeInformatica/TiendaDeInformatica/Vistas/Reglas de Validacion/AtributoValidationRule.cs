using System.Globalization;
using System.Windows.Controls;
using TiendaDeInformatica.Controladores;
using TiendaDeInformatica.Helpers;
using TiendaDeInformatica.Modelos;
using TiendaDeInformatica.Vistas.Caracteristicas;

namespace TiendaDeInformatica.Vistas.Reglas_de_Validacion
{
    public class AtributoValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string _string = value as string;

            if (string.IsNullOrEmpty(_string))
                return new ValidationResult(false, "Completa este campo");

            Atributo _atributoModificar = CaracteristicasAtributo._atributoModificar;
            string nombre = TextHelper.QuitarTildes(_string).ToUpper();

            foreach (Atributo atributo in ControladorAtributos.ObtenerListaDeAtributos())
            {
                if (((_atributoModificar != null) && (atributo.Id != _atributoModificar.Id) && (TextHelper.QuitarTildes(atributo.Nombre).ToUpper() == nombre))
                    || (_atributoModificar == null && (TextHelper.QuitarTildes(atributo.Nombre).ToUpper() == nombre)))
                {
                    return new ValidationResult(false, $"El atributo {_string} ya existe");
                }
            }

            return new ValidationResult(true, null);
        }
    }
}
