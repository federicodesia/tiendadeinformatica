using System;
using System.Collections.Generic;
using TiendaDeInformatica.Controladores;

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

        public decimal PrecioTotal
        {
            get
            {
                decimal precioTotal = 0;
                foreach(PresupuestoProducto presupuestoProducto in Productos)
                    precioTotal += ControladorProductos.ObtenerProducto(presupuestoProducto.ProductoId).Precio;
                return precioTotal;
            }
        }

        public List<PresupuestoProducto> Productos { get; set; }

        public string ClienteTipo
        {
            get
            {
                return ControladorClientes.ObtenerCliente(ClienteId).Tipo;
            }
        }

        public string ClienteMostrarNombre
        {
            get
            {
                return ControladorClientes.ObtenerCliente(ClienteId).MostrarNombre;
            }
        }

        public string ClienteNombreDelResponsable
        {
            get
            {
                return ControladorClientes.ObtenerCliente(ClienteId).NombreDelResponsable;
            }
        }

        public string ClienteDescripcion
        {
            get
            {
                return ControladorClientes.ObtenerCliente(ClienteId).Descripcion;
            }
        }

        public int CantidadProductos
        {
            get
            {
                return Productos.Count;
            }
        }
    }
}
