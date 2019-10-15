using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaDeInformatica.Datos;
using TiendaDeInformatica.Helpers;
using TiendaDeInformatica.Modelos;

namespace TiendaDeInformatica.Controladores
{
    public class ControladorProductos
    {
        public List<Producto> Productos { get; set; }
        public ControladorProductos()
        {
            Productos = new List<Producto>();
        }
        public static void AgregarProducto(Marca marca, string modelo,int cantidad,decimal precio, TipoProducto tipo, string imagen)
        {
            using (var context = new MyDbContext())
            {
                Producto producto = new Producto()
                {
                    MarcaId = marca.Id,
                    Modelo = modelo,
                    Cantidad = cantidad,
                    Precio = precio,
                    Tipo = tipo,
                    Imagen = ConvertirImagen.ConvertImageToByte(new FileInfo(imagen))
                };
            }
        }
    }
}
