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

       public static void AgregarCliente(string nombre, string apellido, string cuit, string telefono)
        {
            using (var context = new MyDbContext())
            {
                Cliente cliente = new Cliente()
                {
                    Nombre = nombre,
                    Apellido = apellido,
                    CUIT = cuit,
                    Telefono = telefono
                };
                context.Clientes.Add(cliente);
                context.SaveChanges();
            }
        }
    }
}
