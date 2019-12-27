using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TiendaDeInformatica.Helpers
{
    public class ConvertirImagen
    {
        public static byte[] ConvertImageToByteArray(string rutaImagen)
        {
            FileInfo file = new FileInfo(rutaImagen);
            return File.ReadAllBytes(file.FullName);
        }

        public static ImageSource ConvertByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream memoryStream = new MemoryStream(byteArrayIn);
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.EndInit();

            return bitmapImage;
        }
    }
}
