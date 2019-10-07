using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace TiendaDeInformatica.Converters
{

    public class ByteToBitmapImageConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] imageBytes = (byte[])value;
            return ConvertByteToImage(imageBytes).Result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Convert byte[] to image
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <returns></returns>
        private async Task<BitmapImage> ConvertByteToImage(byte[] imageBytes)
        {
            BitmapImage image = new BitmapImage();
            using (InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream())
            {
                using (DataWriter writer = new DataWriter(randomAccessStream.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(imageBytes);
                    await writer.StoreAsync();
                    image.SetSourceAsync(randomAccessStream);
                }

            }
            return image;
        }

    }
}
