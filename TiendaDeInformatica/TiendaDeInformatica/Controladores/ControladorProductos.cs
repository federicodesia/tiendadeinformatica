using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public static void AgregarProducto(Marca marca, string modelo, decimal precio, TipoProducto tipo, string imagen)
        {
            using (var context = new MyDbContext())
            {
                Producto producto = new Producto()
                {
                    MarcaId = marca.Id,
                    Modelo = modelo,
                    Precio = precio,
                    Tipo = tipo,
                    Imagen = ConvertirImagen.ConvertImageToByteArray(imagen),
                    Valores=new List<ProductoValor>()
                };
                context.Productos.Add(producto);
                context.SaveChanges();
            }
        }
        public static void ModificarProducto(Producto producto, Marca marca, string modelo, decimal precio, TipoProducto tipo, string imagen)
        {
            using (var context = new MyDbContext())
            {
                Producto productoDb = context.Productos.Find(producto.Id);
                productoDb.Marca = marca;
                productoDb.Modelo = modelo;
                productoDb.Precio = precio;
                productoDb.Tipo = tipo;
                productoDb.Imagen = ConvertirImagen.ConvertImageToByteArray(imagen);
                context.SaveChanges();
            }
        }
        public static void EliminarProducto(Producto producto)
        {
            using (var context = new MyDbContext())
            {
                Producto productoDb = context.Productos.Find(producto.Id);
                context.Productos.Remove(productoDb);
                context.SaveChanges();
            }
        }
        public static List<Producto> ObtenerListaDeProductos()
        {
            using (var context = new MyDbContext())
            {
                return context.Productos.ToList();
            }
        }

        public static void AgregarAtributoATipoProducto(Atributo atributo, TipoProducto tipoProducto)
        {
            using (var context = new MyDbContext())
            {
                AtributoTipoProducto atributoTipoProducto = new AtributoTipoProducto()
                {
                    TipoProducto = tipoProducto,
                    AtributoId = atributo.Id
                };
                context.Add(atributoTipoProducto);
                context.SaveChanges();
            }
        }
        public static void BorrarAtributoDeTipoProducto(Atributo atributo, TipoProducto tipoProducto, AtributoTipoProducto atributoTipoProducto)
        {
            using (var context = new MyDbContext())
            {
                AtributoTipoProducto atributoTipoProductoDb = context.Set<AtributoTipoProducto>().Find(atributo.Id);
                context.Remove(atributoTipoProductoDb);
                context.SaveChanges();

            }
        }
        
    }
}
