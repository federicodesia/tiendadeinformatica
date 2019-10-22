using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaDeInformatica.Datos;
using TiendaDeInformatica.Modelos;

namespace TiendaDeInformatica.Controladores
{
   public class ControladorValores
    {
        public List<Valor> Valores { get; set; }
        public static void AgregarValor(string nombre)
        {
            using (var context = new MyDbContext())
            {
                Valor valor = new Valor()
                {
                    Nombre = nombre
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
