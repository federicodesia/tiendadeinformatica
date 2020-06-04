namespace TiendaDeInformatica.Modelos
{
    public class PresupuestoProducto
    {
        public int Id { get; set; }
        public int PresupuestoId { get; set; }
        public Presupuesto Presupuesto { get; set; }
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }

        public decimal MostrarPrecioProducto
        {
            get
            {
                return Producto.Precio * Cantidad;
            }
        }
    }
}
