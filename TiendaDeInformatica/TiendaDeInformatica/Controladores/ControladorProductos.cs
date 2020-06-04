using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TiendaDeInformatica.Datos;
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

        public static void AgregarProducto(TipoProducto tipo, Marca marca, string modelo, decimal precio, byte[] imagen)
        {
            using (var context = new MyDbContext())
            {
                Producto producto = new Producto()
                {
                    MarcaId = marca.Id,
                    Modelo = modelo,
                    Precio = precio,
                    Tipo = tipo,
                    Imagen = imagen,
                    Valores=new List<ProductoValor>()
                };

                context.Productos.Add(producto);
                context.SaveChanges();
            }
        }

        public static void ModificarProducto(Producto producto, TipoProducto tipo, Marca marca, string modelo, decimal precio, byte[] imagen)
        {
            using (var context = new MyDbContext())
            {
                Producto productoDb = context.Productos.Find(producto.Id);
                productoDb.MarcaId = marca.Id;
                productoDb.Modelo = modelo;
                productoDb.Precio = precio;
                productoDb.Tipo = tipo;
                productoDb.Imagen = imagen;
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
                return context.Productos.Include(p => p.Marca).ToList();
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
