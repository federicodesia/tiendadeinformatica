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
        public List<MotherTipo> TiposDePlacaMadreSoportados { get; set; }
        public int Bahias25Pulgadas { get; set; }
        public int Bahias35Pulgadas { get; set; }
        public int VentiladoresFrontalesMm { get; set; }
        public int VentiladoresTraserosMm { get; set; }
        public int VentiladoresSuperioresMm { get; set; }
        public int VentiladoresInferioresMm { get; set; }
        public int AlturaMaximaCoolerSoportadoMm { get; set; }
        public int LargoMaximoGPUSoportadaMm { get; set; }
        public int LargoMaximoFuenteSoportadaMM { get; set; }
        public int VentiladoresIncluidos { get; set; }
        public RGB TipoDeRGB { get; set; }
    }
}
