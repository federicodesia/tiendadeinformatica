using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaDeInformatica.Modelos;
using Microsoft.EntityFrameworkCore;
using TiendaDeInformatica.Datos;
using Microsoft.Data.Sqlite;

namespace TiendaDeInformatica.Controladores
{
    public class ControladorClientes
    {
        public List<Cliente> Clientes { get; set; }
        public ControladorClientes()
        {
            List<Cliente> Clientes = new List<Cliente>();
        }

       public static void AgregarCliente(string nombreDeLaEmpresa, string nombre, string apellido, string cuit, string telefono)
        {
            using (var context = new MyDbContext())
            {
                Cliente cliente = new Cliente()
                {
                    NombreDeLaEmpresa = nombreDeLaEmpresa,
                    Nombre = nombre,
                    Apellido = apellido,
                    CUIT = cuit,
                    Telefono = telefono
                };
                context.Clientes.Add(cliente);
                context.SaveChanges();
            }
        }

        public static void ModificarCliente(Cliente cliente, string nombreDeLaEmpresa, string nombre, string apellido, string cuit, string telefono)
        {
            using (var context = new MyDbContext())
            {
                Cliente clienteDb = context.Clientes.Find(cliente.Id);
                clienteDb.NombreDeLaEmpresa = nombreDeLaEmpresa;
                clienteDb.Nombre = nombre;
                clienteDb.Apellido = apellido;
                clienteDb.CUIT = cuit;
                clienteDb.Telefono = telefono;
                context.SaveChanges();
            }
            
        }
        public static void EliminarCliente(Cliente cliente)
        {
            using (var context = new MyDbContext())
            {
                Cliente clienteDb = context.Clientes.Find(cliente.Id);
                context.Clientes.Remove(clienteDb);
                context.SaveChanges();
            }
        }
        public static List<Cliente> ObtenerListaDeClientes()
        {
            using (var context = new MyDbContext())
            {
                return context.Clientes.ToList();
            }
        }

    }
}
