using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaDeInformatica.Modelos.Caracteristicas;

namespace TiendaDeInformatica.Modelos.Productos
{
    public class PlacaDeVideo : Producto
    {
        public GPUInterfaz Interfaz { get; set; }
        public GPUChipset Chipset { get; set; }
        public GPUTecnologiaVRAM TecnologiaDeLaMemoria { get; set; }
        public int MemoriaGB { get; set; }

        public int CantidadDeCoolers { get; set; }
        public int CantidadDeSlotsOcupados { get; set; }
        public int LargoMm { get; set; }
        public bool SoportaSLIoCroosfire { get; set; }
        public GPURayTracing TipoDeRayTracing { get; set; }
        public RGB TipoDeRGB { get; set; }

        public int ConsumoWatts { get; set; }
        public int LargoMM { get; set; }
    }
}
