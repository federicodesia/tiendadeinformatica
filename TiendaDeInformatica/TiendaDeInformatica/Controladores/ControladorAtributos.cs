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
        
        public static void AgregarAtributo(string nombre)
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
        public static void AgregarValor(Atributo atributo, string nombre)
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
                context.Valores.Remove(valor);
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
        public static void EliminarValorDeAtributo(Valor valor)
        {
            using (var context = new MyDbContext())
            {
                Valor valorDb = context.Valores.Find(valor.Id);
                context.Valores.Remove(valor);
                context.SaveChanges();
            }
        }

    }
}
