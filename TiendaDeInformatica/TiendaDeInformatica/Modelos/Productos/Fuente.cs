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
        public FuenteTipo TipoDeFuente { get; set; }
        public FuenteTipoCertificacion TipoDeCertificacion { get; set; }
        public FuenteTipoCableado TipoDeCableado { get; set; }
        public int LargoMm { get; set; }
        public int Watts { get; set; }
        public RGB TipoDeRGB { get; set; }
    }
}
