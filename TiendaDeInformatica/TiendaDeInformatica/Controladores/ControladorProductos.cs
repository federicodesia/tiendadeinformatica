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
                    Valores = new List<ProductoValor>()
                };

                context.Productos.Add(producto);
                context.SaveChanges();
            }
        }

        public static Producto AgregarProductoVacio()
        {
            using (var context = new MyDbContext())
            {
                Producto producto = new Producto()
                {
                    MarcaId = null,
                    Modelo = null,
                    Precio = 0,
                    Tipo = null,
                    Imagen = null,
                    Valores = new List<ProductoValor>()
                };

                context.Productos.Add(producto);
                context.SaveChanges();

                return ObtenerProducto(producto.Id);
            }
        }

        public static Producto ModificarProductoVacio(Producto producto, TipoProducto tipo)
        {
            using (var context = new MyDbContext())
            {
                Producto productoDb = context.Productos.Find(producto.Id);
                productoDb.Tipo = tipo;
                context.SaveChanges();

                return ObtenerProducto(productoDb.Id);
            }
        }

        public static Producto ObtenerProducto(int id)
        {
            using (var context = new MyDbContext())
            {
                return context.Productos.Include(p => p.Valores).ThenInclude(v => v.Valor).ThenInclude(v => v.Atributo).ThenInclude(a => a.TiposProductos).ToList().Find(p => p.Id == id);
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
                EliminarProductoVacio();
                return context.Productos.Include(p => p.Marca).Include(p => p.Valores).ThenInclude(v => v.Valor).ToList();
            }
        }

        public static void EliminarProductoVacio()
        {
            using (var context = new MyDbContext())
            {
                if (context.Productos.Count() > 0 && context.Productos.Last().MarcaId == null)
                {
                    context.Remove(context.Productos.Last());
                    context.SaveChanges();
                }

            }
        }

        public static void AgregarAtributoATipoProducto(Atributo atributo, TipoProducto tipoProducto)
        {
            using (var context = new MyDbContext())
            {
                AtributoTipoProducto atributoTipoProducto = new AtributoTipoProducto()
                {
                    Atributo = atributo,
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

        public static Producto AgregarValorAProducto(Valor valor, Producto producto)
        {
            using (var context = new MyDbContext())
            {
                AtributoTipoProducto atributoTipoProducto = context.AtributoTipoProductos.ToList().Find(atp => atp.AtributoId == valor.AtributoId && atp.TipoProducto == producto.Tipo);
                if (atributoTipoProducto.MultiplesValores == false)
                {
                    foreach(ProductoValor contextProductoValor in context.ValorProducto.Include(pv => pv.Valor).ToList().Where(pv => pv.Valor.AtributoId == valor.AtributoId && pv.ProductoId == producto.Id))
                    {
                        context.Remove(contextProductoValor);
                        context.SaveChanges();
                    }
                }

                ProductoValor productoValor = new ProductoValor()
                {
                    ProductoId = producto.Id,
                    ValorId = valor.Id
                };
                context.Add(productoValor);
                context.SaveChanges();

                return ObtenerProducto(producto.Id);
            }
        }

        public static Producto EliminarValorAProducto(Valor valor, Producto producto)
        {
            using (var context = new MyDbContext())
            {
                ProductoValor valorProductoDb = context.ValorProducto.ToList().Find(v => v.ValorId == valor.Id && v.ProductoId == producto.Id);
                context.Remove(valorProductoDb);
                context.SaveChanges();

                return ObtenerProducto(producto.Id);
            }
        }
    }
}
