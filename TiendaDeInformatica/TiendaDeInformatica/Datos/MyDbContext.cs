﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaDeInformatica.Controladores;
using TiendaDeInformatica.Modelos;

namespace TiendaDeInformatica.Datos
{
    public class MyDbContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=BaseDeDatos.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();

            modelBuilder.Entity<AtributoTipoProducto>()
                .HasKey(a => new { a.AtributoId, a.TipoProducto });

            modelBuilder
            .Entity<AtributoTipoProducto>()
            .Property(a => a.TipoProducto)
            .HasConversion(
                v => v.ToString(),
                v => (TipoProducto)Enum.Parse(typeof(TipoProducto), v));

            modelBuilder.Entity<ProductoValor>()
                .HasKey(pv => new { pv.ValorId, pv.ProductoId });
            modelBuilder.Entity<ProductoValor>()
                .HasOne(pv => pv.Producto)
                .WithMany(v => v.Valores)
                .HasForeignKey(pv => pv.ProductoId);
            modelBuilder.Entity<ProductoValor>()
                .HasOne(pv => pv.Valor)
                .WithMany(p => p.Productos)
                .HasForeignKey(v => v.ValorId);

            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Marca)
                .WithMany(m => m.Productos)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductoValor>()
                .HasOne(p => p.Producto)
                .WithMany(p => p.Valores)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Valor>()
               .HasOne(v => v.Atributo)
               .WithMany(a => a.Valores)
               .OnDelete(DeleteBehavior.Cascade);


        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Valor> Valores { get; set; }
        public DbSet<Presupuesto> Presupuestos { get; set; }
        public DbSet<Atributo> Atributos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<AtributoTipoProducto> AtributoTipoProductos { get; set; }
        public DbSet<ProductoValor> ValorProducto { get; set; }
    }
}
