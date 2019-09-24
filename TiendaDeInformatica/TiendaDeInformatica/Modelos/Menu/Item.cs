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
        public Item(string header, List<SubItem> subItems, Image icon)
        {
            Header = header;
            SubItems = subItems;
            Icon = icon;
        }

        public Item(string header, UserControl screen, Image icon)
        {
            Header = header;
            Screen = screen;
            Icon = icon;
        }

        public string Header { get; private set; }
        public Image Icon { get; private set; }
        public List<SubItem> SubItems { get; private set; }
        public UserControl Screen { get; private set; }
    }
}
