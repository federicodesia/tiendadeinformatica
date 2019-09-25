using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TiendaDeInformatica.Modelos.Menu;
using TiendaDeInformatica.Vistas.Controles_de_Usuario;

namespace TiendaDeInformatica
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var PresupuestosSubItems = new List<SubItem>();
            var Presupuestos = new Item("Presupuestos", PresupuestosSubItems, "ItemPresupuestos.png");
            Menu.Children.Add(new ItemUserControl(Presupuestos));

            var ArmadoDePCSubItems = new List<SubItem>();
            ArmadoDePCSubItems.Add(new SubItem("Procesadores"));
            ArmadoDePCSubItems.Add(new SubItem("Placas Madre"));
            ArmadoDePCSubItems.Add(new SubItem("Memorias RAM"));
            ArmadoDePCSubItems.Add(new SubItem("Placas de Video"));
            ArmadoDePCSubItems.Add(new SubItem("Almacenamiento"));
            ArmadoDePCSubItems.Add(new SubItem("Fuentes"));
            ArmadoDePCSubItems.Add(new SubItem("Gabinetes"));
            var ArmadoDePC = new Item("Armado de PC", ArmadoDePCSubItems, "ItemArmadoDePC.png");
            Menu.Children.Add(new ItemUserControl(ArmadoDePC));

            var PerifericosSubItems = new List<SubItem>();
            PerifericosSubItems.Add(new SubItem("Teclados"));
            PerifericosSubItems.Add(new SubItem("Mouses"));
            PerifericosSubItems.Add(new SubItem("Mousepads"));
            PerifericosSubItems.Add(new SubItem("Monitores"));
            PerifericosSubItems.Add(new SubItem("Parlantes"));
            var Productos = new Item("Periféricos", PerifericosSubItems, "ItemPerifericos.png");
            Menu.Children.Add(new ItemUserControl(Productos));

            var OtrosProductosSubItems = new List<SubItem>();
            OtrosProductosSubItems.Add(new SubItem("Pendrives"));
            OtrosProductosSubItems.Add(new SubItem("Discos externos"));
            var OtrosProductos = new Item("Otros productos", OtrosProductosSubItems, "ItemOtrosProductos.png");
            Menu.Children.Add(new ItemUserControl(OtrosProductos));

            for(int i=0; i < 4; i++)
            {
                Presupuesto presupuesto = new Presupuesto();
                Contenido.Children.Add(presupuesto);
            }
        }
    }
}
