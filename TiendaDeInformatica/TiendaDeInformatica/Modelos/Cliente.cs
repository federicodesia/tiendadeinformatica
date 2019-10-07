using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaDeInformatica.Modelos
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string CUIT { get; set; }
        public string Telefono { get; set; }
        public string NombreDeLaEmpresa { get; set; }
        public byte[] Imagen { get; set; }
    }
}
