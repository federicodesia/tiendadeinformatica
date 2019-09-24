using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaDeInformatica.Modelos.Caracteristicas;

namespace TiendaDeInformatica.Modelos.Productos
{
    public class Almacenamiento : Producto
    {
        public AlmacenamientoTipo Tipo { get; set; }
        public AlmacenamientoTecnologia Tecnologia { get; set; }
        public AlmacenamientoConexion Conexion { get; set; }
        public int CapacidadGB { get; set; }
        public int VelocidadDeEscrituraMBps { get; set; }
        public int VelocidadDeLecturaMBps { get; set; }
        public int RPM { get; set; }
        public RGB TipoDeRGB { get; set; }
        public int ConsumoWatts { get; set; }
    }
}
