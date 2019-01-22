using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Aiva.Gui.Converters {
    public class BadgeConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var badge = value as string;

            switch(badge) {
                case "moderator":
                    return Image.FromFile(@"Images\Badges\chatmod.png");
                default: return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
