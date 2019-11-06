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

        public string Tipo
        {
            get
            {
                if (NombreDeLaEmpresa != null)
                {
                    return "Empresa";
                }
                return "Persona";
            }
        }

        public string MostrarNombre
        {
            get
            {
                if (Tipo=="Empresa")
                {
                    return NombreDeLaEmpresa;
                }
                else
                {
                    return $"{Nombre} {Apellido}";
                }
            }
        }

        public string SeguidoMostrarNombre
        {
            get
            {
                if (Tipo == "Empresa")
                {
                    return $"({Nombre} {Apellido})";
                }
                return "";
            }
        }

        public string NombreDelResponsable
        {
            get
            {
                if (Tipo == "Empresa")
                {
                    return $"{Nombre} {Apellido}";
                }
                return "";
            }
        }

        public string Descripcion
        {
            get
            {
                if (CUIT!="")
                {
                    return CUIT;
                }
                else if (Telefono != "")
                {
                    return $"Tel. {Telefono}";
                }
                return "";
            }
        }
    }
}
