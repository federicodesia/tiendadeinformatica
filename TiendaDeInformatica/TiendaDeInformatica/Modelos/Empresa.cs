using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaDeInformatica.Modelos
{
   public class Empresa
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string NombreDelResponsable { get; set; }
        public string ApellidoDelResponsable { get; set; }
        public string CUIT { get; set; }
        public string Telefono { get; set; }
    }
}
