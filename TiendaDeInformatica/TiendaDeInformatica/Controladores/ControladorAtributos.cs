using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaDeInformatica.Datos;
using TiendaDeInformatica.Modelos;

namespace TiendaDeInformatica.Controladores
{
    public class ControladorAtributos
    {
        public List<Atributo> Atributos { get; set; }
        public ControladorAtributos()
        {
            Atributos = new List<Atributo>();
        }
        
        public static void AgregarAtributo(string nombre, List<AtributoTipoProducto> atributoTipoProducto)
        {
            using (var context = new MyDbContext())
            {
                Atributo atributo = new Atributo()
                {
                    Nombre = nombre,
                    TiposProductos = atributoTipoProducto,
                    Valores = new List<Valor>()
                };
                context.Atributos.Add(atributo);
                context.SaveChanges();
            }
        }
        public static void ModificarAtributo(Atributo atributo, string nombre, List<AtributoTipoProducto> atributoTipoProducto)
        {
            using (var context = new MyDbContext())
            {
                Atributo atributoDb = context.Atributos.Find(atributo.Id);
                atributoDb.Nombre = nombre;
                atributoDb.TiposProductos = atributoTipoProducto;
                context.SaveChanges();
            }
        }
        public static void EliminarAtributo(Atributo atributo)
        {
            using (var context = new MyDbContext())
            {
                Atributo atributoDb = context.Atributos.Find(atributo.Id);
                context.Atributos.Remove(atributo);
                context.SaveChanges();
            }
        }
        public static List<Atributo> ObtenerListaDeAtributos()
        {
            using (var context = new MyDbContext())
            {
                return context.Atributos.ToList();
            }
        }

    }
}
