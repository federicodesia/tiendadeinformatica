using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
        
        public static Atributo AgregarAtributo(string nombre)
        {
            using (var context = new MyDbContext())
            {
                Atributo atributo = new Atributo()
                {
                    Nombre = nombre,
                    Valores = new List<Valor>()
                };
                context.Atributos.Add(atributo);
                context.SaveChanges();
                return atributo;
            }
        }
        public static void ModificarAtributo(Atributo atributo, string nombre)
        {
            using (var context = new MyDbContext())
            {
                Atributo atributoDb = context.Atributos.Find(atributo.Id);
                atributoDb.Nombre = nombre;
                context.SaveChanges();
            }
        }
        public static void EliminarAtributo(Atributo atributo)
        {
            using (var context = new MyDbContext())
            {
                Atributo atributoDb = context.Atributos.Find(atributo.Id);
                context.Atributos.Remove(atributoDb);
                context.SaveChanges();
            }
        }
        public static List<Atributo> ObtenerListaDeAtributos() //Obtiene la lista de los atributos.
        {
            using (var context = new MyDbContext())
            {
                return context.Atributos.Include(a => a.TiposProductos).ToList();
            }
        }
        public static List<Atributo> ObtenerListaDeValoresAsociadosAAtributo() //Obtiene una lista de todos los valores asociados a un atributo.
        {
            using (var context = new MyDbContext())
            {
                return context.Atributos.Include(v => v.Valores).ToList();
            }
        }
        public static Valor AgregarValor(Atributo atributo, string nombre)
        {
            using (var context = new MyDbContext())
            {
                Valor valor = new Valor()
                {
                    Nombre = nombre,
                    AtributoId = atributo.Id
                };
                context.Valores.Add(valor);
                context.SaveChanges();
                return valor;
            }
        }
        public static void ModificarValor(Valor valor, string nombre)
        {
            using (var context = new MyDbContext())
            {
                Valor valorDb = context.Valores.Find(valor.Id);
                valorDb.Nombre = nombre;
                context.SaveChanges();
            }
        }
        public static void EliminarValor(Valor valor)
        {
            using (var context = new MyDbContext())
            {
                Valor valorDb = context.Valores.Find(valor.Id);
                context.Valores.Remove(valorDb);
                context.SaveChanges();
            }

        }
        public static List<Valor> ObtenerListaDeValores() //Obtiene la lista de valores.
        {
            using (var context = new MyDbContext())
            {
                return context.Valores.ToList();
            }
        }
        public static List<Valor> ObtenerListaDeProductosAsociadosAValor() //Obtiene una lista de todos los productos asociados a un valor.
        {
            using (var context = new MyDbContext())
            {
                return context.Valores.Include(p => p.Productos).ToList();
            }
        }
        public static void EliminarValorDeAtributo(Valor valor)
        {
            using (var context = new MyDbContext())
            {
                Valor valorDb = context.Valores.Find(valor.Id);
                context.Valores.Remove(valorDb);
                context.SaveChanges();
            }
        }
        public static void AgregarAtributoTipoProducto(Atributo atributo, TipoProducto tipoProducto)
        {
            using (var context = new MyDbContext())
            {
                AtributoTipoProducto atributoTipoProducto = new AtributoTipoProducto()
                {
                    AtributoId = atributo.Id,
                    TipoProducto = tipoProducto
                };
                context.AtributoTipoProductos.Add(atributoTipoProducto);
                context.SaveChanges();
            }
        }
        public static void EliminarAtributoTipoProducto(AtributoTipoProducto atributoTipoProducto)
        {
            using (var context = new MyDbContext())
            {
                {
                    AtributoTipoProducto atributoTipoProductoDb = context.AtributoTipoProductos.Find(atributoTipoProducto.AtributoId);
                    context.AtributoTipoProductos.Remove(atributoTipoProductoDb);
                    context.SaveChanges();
                }
            }
        }
    }
}
