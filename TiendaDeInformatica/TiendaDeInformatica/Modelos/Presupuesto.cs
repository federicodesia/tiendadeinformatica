using System;
using System.Collections.Generic;

namespace TiendaDeInformatica.Modelos
{
    public class Presupuesto
    {
        public int Id { get; set; }
        public Cliente Cliente { get; set; }
        public int ClienteId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public DateTime? FechaDeExpiracion { get; set; }

        public List<PresupuestoProducto> Productos { get; set; }

        public decimal PrecioTotal
        {
            get
            {
                decimal precioTotal = 0;
                foreach(PresupuestoProducto presupuestoProducto in Productos)
                    precioTotal += presupuestoProducto.MostrarPrecioProducto;
                return precioTotal;
            }
        }

        public int CantidadProductos
        {
            get
            {
                int cantidad = 0;
                foreach (PresupuestoProducto presupuestoProducto in Productos)
                    cantidad += presupuestoProducto.Cantidad;
                return cantidad;
            }
        }
    }
}
