﻿using Microsoft.EntityFrameworkCore;
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
                return context.Presupuestos.Include(p => p.Productos).ThenInclude(p => p.Producto).Include(p => p.Cliente).ToList();
            }
        }

        public static Presupuesto ObtenerPresupuesto(int id)
        {
            using (var context = new MyDbContext())
            {
                return context.Presupuestos.Include(p => p.Productos).ThenInclude(p => p.Producto).ThenInclude(p => p.Marca).Include(p => p.Cliente).ToList().Find(p => p.Id == id);
            }
        }

        public static void AgregarPresupuestoProducto(Presupuesto presupuesto, Producto producto)
        {
            using (var context = new MyDbContext())
            {
                presupuesto.FechaModificacion = DateTime.Now;

                bool existiaAntes = false;
                foreach(PresupuestoProducto pp in presupuesto.Productos)
                {
                    if(pp.ProductoId == producto.Id)
                    {
                        existiaAntes = true;
                        PresupuestoProducto presupuestoProductoDb = context.Set<PresupuestoProducto>().Find(pp.Id);
                        presupuestoProductoDb.Cantidad += 1;
                        context.SaveChanges();
                    }
                }

                if (!existiaAntes)
                {
                    PresupuestoProducto presupuestoProducto = new PresupuestoProducto()
                    {
                        ProductoId = producto.Id,
                        PresupuestoId = presupuesto.Id,
                        Cantidad = 1
                    };
                    context.Add(presupuestoProducto);
                    context.SaveChanges();
                }
            }
        }
        
        public static void EliminarPresupuestoProducto(PresupuestoProducto presupuestoProducto)
        {
            using (var context = new MyDbContext())
            {
                PresupuestoProducto presupuestoProductoDb = context.Set<PresupuestoProducto>().Find(presupuestoProducto.Id);
                context.Remove(presupuestoProductoDb);
                context.SaveChanges();
            }
        }

        public static void ModificarCantidadPresupuestoProducto(PresupuestoProducto presupuestoProducto, int cantidad)
        {
            using (var context = new MyDbContext())
            {
                PresupuestoProducto presupuestoProductoDb = context.Set<PresupuestoProducto>().Find(presupuestoProducto.Id);
                presupuestoProductoDb.Cantidad = cantidad;
                context.SaveChanges();
            }
        }
    }
}
