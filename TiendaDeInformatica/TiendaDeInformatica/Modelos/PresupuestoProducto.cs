using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaDeInformatica.Controladores;
using TiendaDeInformatica.Modelos;

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

        public string MostrarTipoProductoMarcaModelo
        {
            get
            {
                return ControladorProductos.ObtenerProducto(ProductoId).MostrarTipoProductoMarcaModelo;
            }
        }

        public byte[] MostrarImagenProducto
        {
            get
            {
                return ControladorProductos.ObtenerProducto(ProductoId).Imagen;
            }
        }

        public decimal MostrarPrecioProducto
        {
            get
            {
                return ControladorProductos.ObtenerProducto(ProductoId).Precio;
            }
        }
    }
}
