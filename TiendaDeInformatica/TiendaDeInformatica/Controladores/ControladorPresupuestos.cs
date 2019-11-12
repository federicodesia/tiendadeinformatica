﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    PrecioTotal = 0,
                    Productos = new List<PresupuestoProducto>()
                };
                context.Presupuestos.Add(presupuesto);
                context.SaveChanges();
            }
        }
        public static void ModificarPresupuesto(Cliente cliente, DateTime? fechaDeExpiracion, Presupuesto presupuesto)
        {
            using (var context = new MyDbContext())
            {
                Presupuesto presupuestoDb = context.Presupuestos.Find(presupuesto.Id);
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
                return context.Presupuestos.ToList();
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
        
        public static void EliminarProductoDelPresupuesto(Presupuesto presupuesto, PresupuestoProducto presupuestoProducto, Producto producto)
        {
            using (var context = new MyDbContext())
            {
                PresupuestoProducto presupuestoProductoDb = context.Set<PresupuestoProducto>().Find(producto.Id);
                context.Remove(presupuestoProductoDb);
                context.SaveChanges();
            }
        }
    }
}
