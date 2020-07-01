using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaDeInformatica.Modelos;

namespace TiendaDeInformatica.Datos
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Atributo>().HasData(new Atributo { Id = 1, Nombre = "Socket" });
            modelBuilder.Entity<Valor>().HasData(new Valor { Id = 1, AtributoId = 1, Nombre = "1151" });
            modelBuilder.Entity<Valor>().HasData(new Valor { Id = 2, AtributoId = 1, Nombre = "AM4" });
            modelBuilder.Entity<AtributoTipoProducto>().HasData(new AtributoTipoProducto { AtributoId = 1, MultiplesValores = false, TipoProducto = TipoProducto.Procesador });
            modelBuilder.Entity<AtributoTipoProducto>().HasData(new AtributoTipoProducto { AtributoId = 1, MultiplesValores = true, TipoProducto = TipoProducto.Cooler });
            modelBuilder.Entity<AtributoTipoProducto>().HasData(new AtributoTipoProducto { AtributoId = 1, MultiplesValores = false, TipoProducto = TipoProducto.Motherboard });
        }
    }
}
