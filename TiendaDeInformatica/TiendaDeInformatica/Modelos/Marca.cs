using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaDeInformatica.Modelos
{
    public class Marca
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public byte[] Imagen { get; set; }
        public List<Producto> Productos { get; set; }
    }
}
