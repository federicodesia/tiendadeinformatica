using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaDeInformatica.Modelos.Caracteristicas;

namespace TiendaDeInformatica.Modelos.Productos
{
    public class Procesador : Producto
    {
        public CPUSocket Socket { get; set; }
        public CPULinea Linea { get; set; }
        public int Nucleos { get; set; }
        public int FrecuenciaGHz { get; set; }
        public int FrecuenciaMemoriaRAMSoportadaMHz { get; set; }

        public bool PoseeVideoIntegrado { get; set; }
        public bool PoseeCooler { get; set; }

        public int ConsumoWatts { get; set; }
    }
}
