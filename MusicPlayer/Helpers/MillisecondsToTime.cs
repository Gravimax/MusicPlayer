using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MusicPlayer
{
    public sealed class MillisecondsToTime : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    long time = 0;
                    long.TryParse(value.ToString(), out time);

                    if (time > 0)
                    {
                        TimeSpan timeSpan = new TimeSpan(time);
                        if (timeSpan.Hours > 0)
                        {
                            return new TimeSpan(time).ToString(@"hh\:mm\:ss");
                        }
                        else
                        {
                            return new TimeSpan(time).ToString(@"mm\:ss");
                        }
                    }
                }

                return "0:0";
            }
            catch
            {
                return "0:0";
            }
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
