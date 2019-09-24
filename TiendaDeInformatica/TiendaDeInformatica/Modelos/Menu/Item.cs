using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TiendaDeInformatica.Modelos.Menu
{
    public class Item
    {
        public Item(string header, List<SubItem> subItems, string icon)
        {
            Header = header;
            SubItems = subItems;
            Icon = "/MyProject;component/Vistas/Imagenes/Menu/"+icon;
        }

        public Item(string header, UserControl screen, string icon)
        {
            Header = header;
            Screen = screen;
            Icon = "/MyProject;component/Vistas/Imagenes/Menu/"+icon;
        }

        public string Header { get; private set; }
        public string Icon { get; private set; }
        public List<SubItem> SubItems { get; private set; }
        public UserControl Screen { get; private set; }
    }
}
