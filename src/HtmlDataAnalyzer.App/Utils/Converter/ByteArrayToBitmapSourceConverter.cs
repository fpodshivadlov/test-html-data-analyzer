using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HtmlDataAnalyzer.App.Utils.Converter
{
    public class ByteArrayToBitmapSourceConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] byteArray)
            {
                return this.ConvertByteArrayToBitMapImage(byteArray);
            }

            return null;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public BitmapSource? ConvertByteArrayToBitMapImage(byte[] imageByteArray)
        {
            var imageSourceConverter = new ImageSourceConverter();
            var result = imageSourceConverter.ConvertFrom(imageByteArray);
            var bitmap = result as BitmapSource;
            return bitmap;
        }
    }
}
