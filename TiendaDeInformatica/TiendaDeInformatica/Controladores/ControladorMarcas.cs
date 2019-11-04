﻿using System;
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
    public class ControladorMarcas
    {
        public List<Marca> Marcas { get; set; }
        public ControladorMarcas()
        {
            Marcas = new List<Marca>();
        }
        
        public static void AgregarMarca(string nombre, string imagen)
        {
            using (var context = new MyDbContext())
            {
                Marca marca = new Marca()
                {
                    Nombre = nombre,
                    Productos = new List<Producto>()
                };
                if (imagen != null)
                {
                    marca.Imagen = ConvertirImagen.ConvertImageToByte(new FileInfo(imagen));
                }
                context.Marcas.Add(marca);
                context.SaveChanges();
            }
            
        }
        public static void ModificarMarca(Marca marca, string nombre, string imagen)
        {
            using (var context = new MyDbContext())
            {
                Marca marcaDb = context.Marcas.Find(marca.Id);
                marcaDb.Nombre = nombre;
                marcaDb.Imagen = ConvertirImagen.ConvertImageToByte(new FileInfo(imagen));
                context.SaveChanges();
            }
        }
        public static void EliminarMarca(Marca marca)
        {
            using (var context = new MyDbContext())
            {
                Marca marcaDb = context.Marcas.Find(marca.Id);
                context.Marcas.Remove(marcaDb);
                context.SaveChanges();
            }
        }
        public static List<Marca> ObtenerListaDeMarcas()
        {
            using (var context = new MyDbContext())
            {
                return context.Marcas.ToList();
            }
        }
    }
}
