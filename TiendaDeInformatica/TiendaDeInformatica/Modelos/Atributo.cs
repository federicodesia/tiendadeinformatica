using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TiendaDeInformatica.Modelos.Producto;

namespace TiendaDeInformatica.Modelos
{
    public class Atributo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public List<AtributoTipoProducto> TiposProductos { get; set; }
        public List<Valor> Valores { get; set; }
    }
}
