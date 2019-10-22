using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaDeInformatica.Modelos
{
    public class ProductoValor
    {
        public Producto Producto { get; set; }
        public int ProductoId { get; set; }
        public Valor Valor { get; set; }
        public int ValorId { get; set; }
    }
}
