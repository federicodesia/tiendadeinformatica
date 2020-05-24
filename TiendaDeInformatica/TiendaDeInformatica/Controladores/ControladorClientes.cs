using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TiendaDeInformatica.Datos;
using TiendaDeInformatica.Modelos;

namespace TiendaDeInformatica.Controladores
{
    public class ControladorClientes
    {
        public List<Cliente> Clientes { get; set; }
        public ControladorClientes()
        {
            List<Cliente> Clientes = new List<Cliente>();
        }

        public static void AgregarCliente(string nombreDeLaEmpresa, string nombre, string apellido, string telefono, string cuit)
        {
            using (var context = new MyDbContext())
            {
                Cliente cliente = new Cliente()
                {
                    NombreDeLaEmpresa = nombreDeLaEmpresa,
                    Nombre = nombre,
                    Apellido = apellido,
                    Telefono = telefono,
                    CUIT = cuit
                };
                context.Clientes.Add(cliente);
                context.SaveChanges();
            }
        }
        public static void ModificarCliente(Cliente cliente, string nombreDeLaEmpresa, string nombre, string apellido, string telefono, string cuit)
        {
            using (var context = new MyDbContext())
            {
                Cliente clienteDb = context.Clientes.Find(cliente.Id);
                clienteDb.NombreDeLaEmpresa = nombreDeLaEmpresa;
                clienteDb.Nombre = nombre;
                clienteDb.Apellido = apellido;
                clienteDb.Telefono = telefono;
                clienteDb.CUIT = cuit;
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

        public static Cliente ObtenerCliente(int id)
        {
            using (var context = new MyDbContext())
            {
                return context.Clientes.Find(id);
            }
        }

        public static string ExportarListaDeClientes()
        {
            string texto = "";
            using (var context = new MyDbContext())
            {
                texto = texto + "Lista de personas:\r\n";
                List<Cliente> personas = context.Clientes.ToList().Where(c => c.Tipo == "Persona").ToList();
                if (personas.Count > 0)
                {
                    foreach (Cliente cliente in personas)
                    {
                        texto = texto + $"\r\n- {cliente.Nombre} {cliente.Apellido}";
                        if (cliente.Telefono != null)
                        {
                            texto = texto + $", Teléfono: {cliente.Telefono}";
                        }
                        if (cliente.CUIT != null)
                        {
                            texto = texto + $", CUIT: {cliente.CUIT}";
                        }
                    }
                }
                else
                {
                    texto = texto + "\r\nNo hay personas en la lista de clientes.";
                }

                texto = texto + "\r\n\r\nLista de empresas:\r\n";
                List<Cliente> empresas = context.Clientes.ToList().Where(c => c.Tipo == "Empresa").ToList();
                if (empresas.Count > 0)
                {
                    foreach (Cliente cliente in empresas)
                    {
                        texto = texto + $"\r\n- {cliente.NombreDeLaEmpresa} (Responsable: {cliente.Nombre} {cliente.Apellido})";
                        if (cliente.Telefono != null)
                        {
                            texto = texto + $", Teléfono: {cliente.Telefono}";
                        }
                        if (cliente.CUIT != null)
                        {
                            texto = texto + $", CUIT: {cliente.CUIT}";
                        }
                    }
                }
                else
                {
                    texto = texto + "\r\nNo hay empresas en la lista de clientes.";
                }
            }
            return texto;
        }
    }
}
