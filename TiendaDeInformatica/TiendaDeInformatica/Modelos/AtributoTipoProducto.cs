namespace TiendaDeInformatica.Modelos
{
    public class AtributoTipoProducto
    {
        public int AtributoId { get; set; }
        public Atributo Atributo { get; set; }
        public bool MultiplesValores { get; set; }
        public TipoProducto TipoProducto { get; set; }
    }
}
