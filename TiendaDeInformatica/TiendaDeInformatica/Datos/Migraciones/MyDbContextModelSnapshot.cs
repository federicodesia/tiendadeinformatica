﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TiendaDeInformatica.Datos;

namespace TiendaDeInformatica.Datos.Migraciones
{
    [DbContext(typeof(MyDbContext))]
    partial class MyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("TiendaDeInformatica.Modelos.Atributo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Nombre");

                    b.HasKey("Id");

                    b.ToTable("Atributos");
                });

            modelBuilder.Entity("TiendaDeInformatica.Modelos.AtributoTipoProducto", b =>
                {
                    b.Property<int>("AtributoId");

                    b.Property<string>("TipoProducto");

                    b.HasKey("AtributoId", "TipoProducto");

                    b.ToTable("AtributoTipoProducto");
                });

            modelBuilder.Entity("TiendaDeInformatica.Modelos.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Apellido");

                    b.Property<string>("CUIT");

                    b.Property<string>("Nombre");

                    b.Property<string>("NombreDeLaEmpresa");

                    b.Property<string>("Telefono");

                    b.HasKey("Id");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("TiendaDeInformatica.Modelos.Marca", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Imagen");

                    b.Property<string>("Nombre");

                    b.HasKey("Id");

                    b.ToTable("Marcas");
                });

            modelBuilder.Entity("TiendaDeInformatica.Modelos.Presupuesto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ClienteId");

                    b.Property<DateTime>("FechaCreacion");

                    b.Property<DateTime?>("FechaDeExpiracion");

                    b.Property<DateTime>("FechaModificacion");

                    b.Property<decimal>("PrecioTotal");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.ToTable("Presupuestos");
                });

            modelBuilder.Entity("TiendaDeInformatica.Modelos.Producto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Cantidad");

                    b.Property<byte[]>("Imagen");

                    b.Property<int>("MarcaId");

                    b.Property<string>("Modelo");

                    b.Property<decimal>("Precio");

                    b.Property<int?>("Tipo");

                    b.HasKey("Id");

                    b.HasIndex("MarcaId");

                    b.ToTable("Productos");
                });

            modelBuilder.Entity("TiendaDeInformatica.Modelos.ProductoValor", b =>
                {
                    b.Property<int>("ValorId");

                    b.Property<int>("ProductoId");

                    b.HasKey("ValorId", "ProductoId");

                    b.HasIndex("ProductoId");

                    b.ToTable("ProductoValor");
                });

            modelBuilder.Entity("TiendaDeInformatica.Modelos.Valor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AtributoId");

                    b.Property<string>("Nombre");

                    b.HasKey("Id");

                    b.HasIndex("AtributoId");

                    b.ToTable("Valores");
                });

            modelBuilder.Entity("TiendaDeInformatica.Modelos.AtributoTipoProducto", b =>
                {
                    b.HasOne("TiendaDeInformatica.Modelos.Atributo", "Atributo")
                        .WithMany("TiposProductos")
                        .HasForeignKey("AtributoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TiendaDeInformatica.Modelos.Presupuesto", b =>
                {
                    b.HasOne("TiendaDeInformatica.Modelos.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TiendaDeInformatica.Modelos.Producto", b =>
                {
                    b.HasOne("TiendaDeInformatica.Modelos.Marca", "Marca")
                        .WithMany("Productos")
                        .HasForeignKey("MarcaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TiendaDeInformatica.Modelos.ProductoValor", b =>
                {
                    b.HasOne("TiendaDeInformatica.Modelos.Producto", "Producto")
                        .WithMany("Valores")
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TiendaDeInformatica.Modelos.Valor", "Valor")
                        .WithMany("Productos")
                        .HasForeignKey("ValorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TiendaDeInformatica.Modelos.Valor", b =>
                {
                    b.HasOne("TiendaDeInformatica.Modelos.Atributo", "Atributo")
                        .WithMany("Valores")
                        .HasForeignKey("AtributoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
