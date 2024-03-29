﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TiendaDeInformatica.Datos.Migraciones
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Atributos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atributos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(nullable: true),
                    Apellido = table.Column<string>(nullable: true),
                    CUIT = table.Column<string>(nullable: true),
                    Telefono = table.Column<string>(nullable: true),
                    NombreDeLaEmpresa = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Marcas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(nullable: true),
                    Imagen = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marcas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AtributoTipoProductos",
                columns: table => new
                {
                    AtributoId = table.Column<int>(nullable: false),
                    TipoProducto = table.Column<string>(nullable: false),
                    MultiplesValores = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtributoTipoProductos", x => new { x.AtributoId, x.TipoProducto });
                    table.ForeignKey(
                        name: "FK_AtributoTipoProductos_Atributos_AtributoId",
                        column: x => x.AtributoId,
                        principalTable: "Atributos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Valores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(nullable: true),
                    AtributoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Valores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Valores_Atributos_AtributoId",
                        column: x => x.AtributoId,
                        principalTable: "Atributos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Presupuestos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClienteId = table.Column<int>(nullable: false),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    FechaDeExpiracion = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presupuestos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Presupuestos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MarcaId = table.Column<int>(nullable: true),
                    Modelo = table.Column<string>(nullable: true),
                    Precio = table.Column<decimal>(nullable: false),
                    Tipo = table.Column<int>(nullable: true),
                    Imagen = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Productos_Marcas_MarcaId",
                        column: x => x.MarcaId,
                        principalTable: "Marcas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PresupuestoProducto",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PresupuestoId = table.Column<int>(nullable: false),
                    ProductoId = table.Column<int>(nullable: false),
                    Cantidad = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresupuestoProducto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PresupuestoProducto_Presupuestos_PresupuestoId",
                        column: x => x.PresupuestoId,
                        principalTable: "Presupuestos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PresupuestoProducto_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ValorProducto",
                columns: table => new
                {
                    ProductoId = table.Column<int>(nullable: false),
                    ValorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValorProducto", x => new { x.ValorId, x.ProductoId });
                    table.ForeignKey(
                        name: "FK_ValorProducto_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ValorProducto_Valores_ValorId",
                        column: x => x.ValorId,
                        principalTable: "Valores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Atributos",
                columns: new[] { "Id", "Nombre" },
                values: new object[] { 1, "Socket" });

            migrationBuilder.InsertData(
                table: "AtributoTipoProductos",
                columns: new[] { "AtributoId", "TipoProducto", "MultiplesValores" },
                values: new object[] { 1, "Procesador", false });

            migrationBuilder.InsertData(
                table: "AtributoTipoProductos",
                columns: new[] { "AtributoId", "TipoProducto", "MultiplesValores" },
                values: new object[] { 1, "Cooler", true });

            migrationBuilder.InsertData(
                table: "AtributoTipoProductos",
                columns: new[] { "AtributoId", "TipoProducto", "MultiplesValores" },
                values: new object[] { 1, "Motherboard", false });

            migrationBuilder.InsertData(
                table: "Valores",
                columns: new[] { "Id", "AtributoId", "Nombre" },
                values: new object[] { 1, 1, "1151" });

            migrationBuilder.InsertData(
                table: "Valores",
                columns: new[] { "Id", "AtributoId", "Nombre" },
                values: new object[] { 2, 1, "AM4" });

            migrationBuilder.CreateIndex(
                name: "IX_PresupuestoProducto_PresupuestoId",
                table: "PresupuestoProducto",
                column: "PresupuestoId");

            migrationBuilder.CreateIndex(
                name: "IX_PresupuestoProducto_ProductoId",
                table: "PresupuestoProducto",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Presupuestos_ClienteId",
                table: "Presupuestos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_MarcaId",
                table: "Productos",
                column: "MarcaId");

            migrationBuilder.CreateIndex(
                name: "IX_Valores_AtributoId",
                table: "Valores",
                column: "AtributoId");

            migrationBuilder.CreateIndex(
                name: "IX_ValorProducto_ProductoId",
                table: "ValorProducto",
                column: "ProductoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AtributoTipoProductos");

            migrationBuilder.DropTable(
                name: "PresupuestoProducto");

            migrationBuilder.DropTable(
                name: "ValorProducto");

            migrationBuilder.DropTable(
                name: "Presupuestos");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Valores");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Marcas");

            migrationBuilder.DropTable(
                name: "Atributos");
        }
    }
}
