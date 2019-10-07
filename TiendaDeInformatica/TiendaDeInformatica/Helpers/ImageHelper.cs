using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaDeInformatica.Helpers
{
    public class ImageHelper
    {
        public static byte[] ConvertImageToByte(FileInfo file)
        {            
            return File.ReadAllBytes(file.FullName);
        }
    }
}
