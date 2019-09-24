using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaDeInformatica.Modelos
{
    public abstract class Producto
    {
        public int Id { get; set; }
        public Marca Marca { get; set; }
        public int MarcaId { get; set; }
        public string Modelo { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }

        // Falta agregar la imagen del producto.
    }
}
