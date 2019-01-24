using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Aiva.Gui.Converters {
    public class SliderMinutesToMinHoursConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var time = System.Convert.ToInt16(value);

            return $"Active time: {new DateTime(TimeSpan.FromMinutes(time).Ticks).ToString("HH'h' mm'm'")}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
