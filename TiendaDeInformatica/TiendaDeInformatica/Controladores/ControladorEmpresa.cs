using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaDeInformatica.Datos;
using TiendaDeInformatica.Modelos;

namespace TiendaDeInformatica.Controladores
{
    public class ControladorEmpresa
    {
        public ControladorEmpresa()
        {
            List<Empresa> Empresas = new List<Empresa>();
        }
        public static void AgregarEmpresa(string nombre, string nombredelresponsable, string apellidodelresponsable, string cuit, string telefono)
        {
            using (var context = new MyDbContext())
            {
                Empresa empresa = new Empresa()
                {
                    Nombre = nombre,
                    NombreDelResponsable = nombredelresponsable,
                    ApellidoDelResponsable = apellidodelresponsable,
                    CUIT = cuit,
                    Telefono = telefono
                };
                context.Empresas.Add(empresa);
                context.SaveChanges();
            }
        }

        
    }
}
