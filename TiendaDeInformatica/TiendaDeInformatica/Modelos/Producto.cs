﻿using System.Collections.Generic;
using System.ComponentModel;

namespace TiendaDeInformatica.Modelos
{
    public class Producto
    {
        public int Id { get; set; }
        public Marca Marca { get; set; }
        public int? MarcaId { get; set; }
        public string Modelo { get; set; }
        public decimal Precio { get; set; }
        public TipoProducto? Tipo { get; set; }
        public byte[] Imagen { get; set; }

        public List<ProductoValor> Valores { get; set; }

        public string MostrarTipoProductoMarcaModelo
        {
            get
            {
                return $"{Tipo.ToString().Replace("_", " ")} {Marca.Nombre} {Modelo}";
            }
        }
    }

    public enum TipoProducto
    {
        // Description: Tipo de Producto en plural
        // Value: Tipo de Producto en singular

        [Description("Motherboards")] Motherboard,
        [Description("Procesadores")] Procesador,
        [Description("Coolers")] Cooler,
        [Description("Memorias RAM")] Memoria_RAM,
        [Description("Placas de video")] Placa_de_video,
        [Description("Almacenamiento")] Almacenamiento,
        [Description("Gabinetes")] Gabinete,
        [Description("Fuentes")] Fuente
    }
}
