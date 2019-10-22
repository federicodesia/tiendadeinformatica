using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaDeInformatica.Modelos
{
    public class Valor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public Atributo Atributo { get; set; }
        public int AtributoId { get; set; }

        public List<ProductoValor> Productos { get; set; }
    }
}
