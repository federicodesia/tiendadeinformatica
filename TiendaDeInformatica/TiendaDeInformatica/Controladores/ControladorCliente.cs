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
       public ControladorCliente()
        {
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
                //var clienteDb = context.Clientes.Find(cliente.Id);
                context.Attach(cliente);
                cliente.Nombre = nombre;
                cliente.Apellido = apellido;
                cliente.CUIT = cuit;
                cliente.Telefono = telefono;
                cliente.NombreDeLaEmpresa = nombredelaempresa;
                context.SaveChanges();
            }
            
        }

    }
}
