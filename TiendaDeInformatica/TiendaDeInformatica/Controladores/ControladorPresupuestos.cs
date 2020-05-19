using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TiendaDeInformatica.Datos;
using TiendaDeInformatica.Modelos;

namespace TiendaDeInformatica.Controladores
{
    public class ControladorPresupuestos
    {
        public List<Presupuesto> Presupuestos { get; set; }

        public ControladorPresupuestos()
        {
            Presupuestos = new List<Presupuesto>();
        }

        public static void AgregarPresupuesto(Cliente cliente, DateTime? fechaDeExpiracion)
        {
            using (var context = new MyDbContext())
            {
                Presupuesto presupuesto = new Presupuesto()
                {
                    ClienteId = cliente.Id,
                    FechaCreacion = DateTime.Now,
                    FechaModificacion = DateTime.Now,
                    FechaDeExpiracion = fechaDeExpiracion,
                    Productos = new List<PresupuestoProducto>()
                };
                context.Presupuestos.Add(presupuesto);
                context.SaveChanges();
            }
        }

        public static void ModificarPresupuesto(Presupuesto presupuesto, Cliente cliente, DateTime? fechaDeExpiracion)
        {
            using (var context = new MyDbContext())
            {
                Presupuesto presupuestoDb = context.Presupuestos.Find(presupuesto.Id);
                presupuestoDb.ClienteId = cliente.Id;
                presupuestoDb.FechaModificacion = DateTime.Now;
                presupuestoDb.FechaDeExpiracion = fechaDeExpiracion;
                context.SaveChanges();
            }
        }

        public static void EliminarPresupuesto(Presupuesto presupuesto)
        {
            using (var context = new MyDbContext())
            {
                Presupuesto presupuestoDb = context.Presupuestos.Find(presupuesto.Id);
                context.Presupuestos.Remove(presupuestoDb);
                context.SaveChanges();
            }
        }

        public static List<Presupuesto> ObtenerListaDePresupuestos()
        {
            using (var context = new MyDbContext())
            {
                return context.Presupuestos.Include(p => p.Productos).ToList();
            }
        }

        public static void AgregarProductoAPresupuesto(Presupuesto presupuesto, Producto producto, int cantidad)
        {
            using (var context = new MyDbContext())
            {
                PresupuestoProducto presupuestoProducto = new PresupuestoProducto()
                {
                    ProductoId = producto.Id,
                    Cantidad = cantidad,
                    PresupuestoId = presupuesto.Id
                };
                context.Add(presupuestoProducto);
                context.SaveChanges();
            }
        }
        
        public static void EliminarProductoDelPresupuesto(Presupuesto presupuesto, PresupuestoProducto presupuestoProducto)
        {
            using (var context = new MyDbContext())
            {
                PresupuestoProducto presupuestoProductoDb = context.Set<PresupuestoProducto>().Find(presupuestoProducto.Id);
                context.Remove(presupuestoProductoDb);
                context.SaveChanges();
            }
        }

        public static Presupuesto ObtenerPresupuesto(int id)
        {
            using (var context = new MyDbContext())
            {
                return ObtenerListaDePresupuestos().Find(p => p.Id == id);
            }
        }
    }
}
