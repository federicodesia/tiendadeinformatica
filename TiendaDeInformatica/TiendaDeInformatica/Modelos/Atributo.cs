using System.Collections.Generic;

namespace TiendaDeInformatica.Modelos
{
    public class Atributo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public List<AtributoTipoProducto> TiposProductos { get; set; }
        public List<Valor> Valores { get; set; }
    }
}
