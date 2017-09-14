using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Linq;

namespace MusicPlayer
{
    public sealed class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            try
            {
                string filePath = value as string;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();

                if (!string.IsNullOrEmpty(filePath))
                {
                    TagLib.File tagFile = TagLib.File.Create(filePath, TagLib.ReadStyle.Average);

                    if (tagFile.Tag.Pictures.Count() > 0)
                    {
                        bitmap.StreamSource = new MemoryStream(tagFile.Tag.Pictures[0].Data.Data);
                    }
                    else
                    {
                        bitmap.UriSource = new Uri("/MusicPlayer;component/Resources/Icons/NoImage.png", UriKind.Relative);
                    }
                }
                else
                {
                    bitmap.UriSource = new Uri("/MusicPlayer;component/Resources/Icons/NoImage.png", UriKind.Relative);
                }

                bitmap.EndInit();
                return bitmap;
            }
            catch
            {
                return new BitmapImage();
            }
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
