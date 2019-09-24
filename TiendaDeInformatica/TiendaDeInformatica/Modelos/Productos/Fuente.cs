using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaDeInformatica.Modelos.Caracteristicas;

namespace TiendaDeInformatica.Modelos.Productos
{
   public class Fuente : Producto
    {
        public FuenteTipoCertificacion TipoCertificacion { get; set; }
        public FuenteTipo TipoDeFuente { get; set; }
        public int LargoMM { get; set; }
        public int Watts { get; set; }
        public FuenteTipoCable TipoDeCable { get; set; }
        public RGB TipoDeRGB { get; set; }
    }
}
