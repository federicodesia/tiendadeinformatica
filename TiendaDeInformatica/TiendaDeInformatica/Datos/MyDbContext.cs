using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            modelBuilder.Entity<AtributoTipoProducto>()
                .HasKey(a => new { a.AtributoId, a.TipoProducto });

            modelBuilder
            .Entity<AtributoTipoProducto>()
            .Property(a => a.TipoProducto)
            .HasConversion(
                v => v.ToString(),
                v => (TipoProducto)Enum.Parse(typeof(TipoProducto), v));
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Valor> Valores { get; set; }
        public DbSet<Presupuesto> Presupuestos { get; set; }
        public DbSet<Atributo> Atributos { get; set; }
        public DbSet<Producto> Productos { get; set; }
    }
}
