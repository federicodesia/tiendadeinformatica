using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaDeInformatica.Modelos.Caracteristicas;

namespace TiendaDeInformatica.Modelos.Productos
{
    public class Cooler : Producto
    {
        public List<CPUSocket> Sockets { get; set; }
        public int ConsumoWatts { get; set; }
        public RGB TipoDeRGB { get; set; }
        public int CantidadDeVentiladores { get; set; }
        public CoolerTipo Tipo { get; set; }
        public int TamanioVentiladoresMM { get; set; }

    }
}
