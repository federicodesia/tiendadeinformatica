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
    public class ControladorCliente
    {
        public List<Cliente> Clientes { get; set; }
        public ControladorCliente()
        {
            //GetClientes();
            List<Cliente> Clientes = new List<Cliente>();
        }

       public static void AgregarCliente(string nombre, string apellido, string cuit, string telefono, string nombredelaempresa)
        {
            using (var context = new MyDbContext())
            {
                Cliente cliente = new Cliente()
                {
                    Nombre = nombre,
                    Apellido = apellido,
                    CUIT = cuit,
                    Telefono = telefono,
                    NombreDeLaEmpresa = nombredelaempresa
                };
                context.Clientes.Add(cliente);
                context.SaveChanges();
            }
        }
        public static void ModificarCliente(Cliente cliente, string nombre, string apellido, string cuit, string telefono, string nombredelaempresa)
        {
            using (var context = new MyDbContext())
            {
                Cliente clienteDb = context.Clientes.Find(cliente.Id);
                //context.Attach(cliente);
                clienteDb.Nombre = nombre;
                clienteDb.Apellido = apellido;
                clienteDb.CUIT = cuit;
                clienteDb.Telefono = telefono;
                clienteDb.NombreDeLaEmpresa = nombredelaempresa;
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
        public static void GetClientes(Cliente cliente) //Método para obtener la lista de clientes desde la base de datos.
        {
            using (var context = new MyDbContext())
            {
                if (context.Clientes.Find(cliente.Id) != null)
                { 
                List<Cliente> Clientes = context.Clientes.ToList();
                context.SaveChanges();
                }

            }
        }

    }
}
