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
                    return "Empresa";
                return "Persona";
            }
        }

        public string MostrarNombre
        {
            get
            {
                if (Tipo == "Empresa")
                    return NombreDeLaEmpresa;
                return $"{Nombre} {Apellido}";
            }
        }

        public string NombreDelResponsable
        {
            get
            {
                if (Tipo == "Empresa")
                    return $"({Nombre} {Apellido})";
                return null;
            }
        }

        public string Descripcion
        {
            get
            {
                if (CUIT != "")
                    return CUIT;
                else if (Telefono != "")
                    return $"Tel. {Telefono}";
                return null;
            }
        }
    }
}
