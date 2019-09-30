using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TiendaDeInformatica.Modelos.Producto;

namespace TiendaDeInformatica.Modelos
{
    public class AtributoTipoProducto
    {
        public int AtributoId { get; set; }
        public Atributo Atributo { get; set; }
        public TipoProducto TipoProducto { get; set; }
    }
}
