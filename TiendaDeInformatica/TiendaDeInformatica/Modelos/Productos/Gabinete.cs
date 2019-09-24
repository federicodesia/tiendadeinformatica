using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaDeInformatica.Modelos.Caracteristicas;

namespace TiendaDeInformatica.Modelos.Productos
{
    public class Gabinete : Producto
    {
        public List<MotherTipo> TiposSoportados { get; set; }
        public int Bahias25pulgadas { get; set; }
        public int Bahias35pulgadas { get; set; }
        public int AlturaMaximaCooler { get; set; }
        public int VentiladoresFrontalesMM { get; set; }
        public int VentiladoresTraserosMM { get; set; }
        public int VentiladoresSuperioresMM { get; set; }
        public int VentiladoresInferioresMM { get; set; }
        public int LargoMaximoGPUSoportadoMM { get; set; }
        public int VentiladoresIncluidos { get; set; }
        public RGB TipoDeRGB { get; set; }
        public int LargoMaximoFuenteMM { get; set; }

    }
}
