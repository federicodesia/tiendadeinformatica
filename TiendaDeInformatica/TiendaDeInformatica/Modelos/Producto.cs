using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaDeInformatica.Modelos
{
    public class Producto
    {
        public int Id { get; set; }
        public Marca Marca { get; set; }
        public int MarcaId { get; set; }
        public string Modelo { get; set; }
        public decimal Precio { get; set; }
        public TipoProducto? Tipo { get; set; }
        public byte[] Imagen { get; set; }

        public List<ProductoValor> Valores { get; set; }
    }

    public enum TipoProducto
    {
        // Description: Tipo de Producto en plural
        // Value: Tipo de Producto en singular

        [Description("Procesadores")] Procesador,
        [Description("Motherboards")] Motherboard,
        [Description("Coolers")] Cooler,
        [Description("Memorias RAM")] Memoria_RAM,
        [Description("Placas de video")] Placa_de_video,
        [Description("Almacenamiento")] Almacenamiento,
        [Description("Gabinetes")] Gabinete,
        [Description("Fuentes")] Fuente
    }
}
