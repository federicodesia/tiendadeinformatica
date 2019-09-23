using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaDeInformatica.Modelos
{
    public abstract class Producto
    {
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
    }
}
