using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaDeInformatica.Modelos;
using Microsoft.EntityFrameworkCore;
using TiendaDeInformatica.Datos;
using Microsoft.Data.Sqlite;
using System.IO;

namespace TiendaDeInformatica.Controladores
{
    public class ControladorCliente
    {
        public List<Cliente> Clientes { get; set; }
        public ControladorCliente()
        {
            List<Cliente> Clientes = new List<Cliente>();
        }

       public static void AgregarCliente(string nombre, string apellido, string cuit, string telefono, string nombredelaempresa, string imagen)
        {
            using (var context = new MyDbContext())
            {
                Cliente cliente = new Cliente()
                {
                    Nombre = nombre,
                    Apellido = apellido,
                    CUIT = cuit,
                    Telefono = telefono,
                    NombreDeLaEmpresa = nombredelaempresa,
                    Imagen = ConvertImageToByte(new FileInfo(imagen))
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
        public static List<Cliente> GetClientes() //Método para obtener lista de clientes.
        {
            using (var context = new MyDbContext())
            {
                return context.Clientes.ToList();
            }
        }
        public static byte[] ConvertImageToByte(FileInfo file)
        {
            return File.ReadAllBytes(file.FullName);
        }

    }
}
