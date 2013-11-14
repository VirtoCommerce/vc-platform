using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Converters
{
    public class BinaryImageConverter:IValueConverter
    {

        private static readonly ThreadLocal<BinaryImageConverter> _instance = new ThreadLocal<BinaryImageConverter>(() => CreateInstance());
        public static BinaryImageConverter Current
        {
            get
            {
                return _instance.Value;
            }
        }

        private static BinaryImageConverter CreateInstance()
        {
            return new BinaryImageConverter();
        }


        #region IValueConverter members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is byte[])
            {
                byte[] bytes = value as byte[];

                MemoryStream ms = new MemoryStream(bytes);

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = ms;
                image.EndInit();

                return image;

            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        
    }
}
