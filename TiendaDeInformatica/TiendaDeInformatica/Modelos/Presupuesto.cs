using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaDeInformatica.Modelos
{
    public class Presupuesto
    {
        public int Id { get; set; }
        public Cliente Cliente { get; set; }
        public int ClienteId { get; set; }
        public Empresa Empresa { get; set; }
        public int EmpresaId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public DateTime FechaDeExpiracion { get; set; }
        public decimal PrecioTotal { get; set; }
        

        // Falta agregar la lista de productos (Cantidad y Producto).
    }
}
