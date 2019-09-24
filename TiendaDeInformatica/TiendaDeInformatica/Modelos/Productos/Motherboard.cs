using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaDeInformatica.Modelos.Caracteristicas;

namespace TiendaDeInformatica.Modelos.Productos
{
    public class Motherboard : Producto
    {
        public MotherTipo Tipo { get; set; }
        public MotherChipset Chipset { get; set; }
        public int ZocalosRAM { get; set; }
        public RAMFormato FormatoRAM { get; set; }
        public RAMTecnologia TecnologiaRAM { get; set; }
        public int ConexionesSATA { get; set;}
        public int PCIExpressX16 { get; set;}
        public CPUSocket Socket { get; set; }
        public RGB TipoDeRGB { get; set; }
        public int ConexionesM2 { get; set; }
    }
}
