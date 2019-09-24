using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaDeInformatica.Modelos.Caracteristicas;

namespace TiendaDeInformatica.Modelos.Productos
{
    public class MemoriaRAM : Producto
    {
        public RAMFormato Formato { get; set; }
        public RAMTecnologia Tecnologia { get; set; }
        public int CantidadDeModulos { get; set; }
        public int CapacidadGB { get; set; }
        public int FrecuenciaMHz { get; set; }
        public bool PoseeDisipador { get; set; }
        public RGB TipoDeRGB { get; set; }

        public int ConsumoWatts { get; set; }
    }
}
