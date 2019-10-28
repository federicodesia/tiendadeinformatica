using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaDeInformatica.Datos;
using TiendaDeInformatica.Modelos;

namespace TiendaDeInformatica.Controladores
{
    public class ControladorMarcas
    {
        public List<Marca> Marcas { get; set; }
        public ControladorMarcas()
        {
            Marcas = new List<Marca>();
        }
        
        public static void AgregarMarca(string nombre)
        {
            using (var context = new MyDbContext())
            {
                Marca marca = new Marca()
                {
                    Nombre = nombre,
                    Productos = new List<Producto>()
                };
                context.Marcas.Add(marca);
                context.SaveChanges();
            }
            
        }
        public static void ModificarMarca(Marca marca, string nombre)
        {
            using (var context = new MyDbContext())
            {
                Marca marcaDb = context.Marcas.Find(marca.Id);
                marcaDb.Nombre = nombre;
                context.SaveChanges();
            }
        }
        public static void EliminarMarca(Marca marca)
        {
            using (var context = new MyDbContext())
            {
                Marca marcaDb = context.Marcas.Find(marca.Id);
                context.Marcas.Remove(marca);
                context.SaveChanges();
            }
        }
        public static List<Marca> ObtenerListaDeMarcas()
        {
            using (var context = new MyDbContext())
            {
                return context.Marcas.ToList();
            }
        }
    }
}
