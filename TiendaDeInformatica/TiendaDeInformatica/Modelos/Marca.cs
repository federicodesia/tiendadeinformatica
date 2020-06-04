using System.Collections.Generic;

namespace TiendaDeInformatica.Modelos
{
    public class Marca
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public byte[] Imagen { get; set; }
        public List<Producto> Productos { get; set; }

        public string CantidadProductos
        {
            get
            {
                if (Productos != null)
                {
                    if (Productos.Count == 1)
                        return "1 producto";
                    return $"{Productos.Count.ToString()} productos";
                }
                return "0 productos";
            }
        }
    }
}
