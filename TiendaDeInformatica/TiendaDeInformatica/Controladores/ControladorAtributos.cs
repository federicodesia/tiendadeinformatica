using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        //
        // Atributos
        //
        
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

        public static List<Atributo> ObtenerListaDeAtributos()
        {
            using (var context = new MyDbContext())
            {
                return context.Atributos.Include(a => a.TiposProductos).Include(a => a.Valores).ToList();
            }
        }

        public static Atributo ObtenerAtributo(int id)
        {
            using (var context = new MyDbContext())
            {
                return ObtenerListaDeAtributos().Find(a => a.Id == id);
            }
        }

        public static List<Atributo> ObtenerListaDeAtributosAsociadosATipoProducto(TipoProducto tipoProducto)
        {
            using (var context = new MyDbContext())
            {
                List<Atributo> atributos = new List<Atributo>();
                foreach (AtributoTipoProducto atributoTipoProducto in context.AtributoTipoProductos.Include(atp => atp.Atributo.Valores))
                {
                    if (atributoTipoProducto.TipoProducto == tipoProducto)
                        atributos.Add(atributoTipoProducto.Atributo);
                }
                return atributos;
            }
        }

        //
        // Valores
        //

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

        public static List<Valor> ObtenerListaDeValores()
        {
            using (var context = new MyDbContext())
            {
                return context.Valores.ToList();
            }
        }

        public static List<Valor> ObtenerListaDeValoresConProductos()
        {
            using (var context = new MyDbContext())
            {
                return context.Valores.Include(p => p.Productos).ToList();
            }
        }

        public static void AgregarAtributoTipoProducto(Atributo atributo, TipoProducto tipoProducto)
        {
            using (var context = new MyDbContext())
            {
                AtributoTipoProducto atributoTipoProducto = new AtributoTipoProducto()
                {
                    AtributoId = atributo.Id,
                    TipoProducto = tipoProducto,
                    MultiplesValores = false
                };
                context.AtributoTipoProductos.Add(atributoTipoProducto);
                context.SaveChanges();
            }
        }

        public static void ModificarAtributoTipoProducto(Atributo atributo, TipoProducto tipoProducto, bool multiplesValores)
        {
            using (var context = new MyDbContext())
            {
                AtributoTipoProducto atributoTipoProductoDb = context.AtributoTipoProductos.ToList().Find(a => a.AtributoId == atributo.Id && a.TipoProducto == tipoProducto);
                atributoTipoProductoDb.MultiplesValores = multiplesValores;
                context.SaveChanges();
            }
        }

        public static void EliminarAtributoTipoProducto(Atributo atributo, TipoProducto tipoProducto)
        {
            using (var context = new MyDbContext())
            {
                AtributoTipoProducto atributoTipoProductoDb = context.AtributoTipoProductos.ToList().Find(a => a.AtributoId == atributo.Id && a.TipoProducto == tipoProducto);
                context.AtributoTipoProductos.Remove(atributoTipoProductoDb);
                context.SaveChanges();
            }
        }
    }
}
