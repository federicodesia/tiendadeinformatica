using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TiendaDeInformatica.Helpers
{
    public class TextHelper
    {
        public static string QuitarTildes(string texto)
        {
            return new String(texto.Normalize(NormalizationForm.FormD).Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray()).Normalize(NormalizationForm.FormC);
        }
    }
}
