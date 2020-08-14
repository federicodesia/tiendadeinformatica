namespace TiendaDeInformatica.Modelos
{
    public class TipoProductoProducto
    {
        public TipoProducto Tipo { get; set; }
        public Producto Producto { get; set; }

        public string MostrarTipoProducto
        {
            get
            {
                return Tipo.ToString().Replace("_", " ");
            }
        }

        public string MostrarMarcaModeloProducto
        {
            get
            {
                if (Producto != null)
                    return $"{Producto.Marca.Nombre} {Producto.Modelo}";
                return "";
            }
        }
    }
}