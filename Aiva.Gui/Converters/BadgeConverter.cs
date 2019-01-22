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
    public class BadgeConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var badge = (value as string).ToLower();

            switch(badge) {
                case "moderator":
                    return new BitmapImage(new Uri(Core.Twitch.AivaClient.Badges?.Mod?.Image ?? null, UriKind.RelativeOrAbsolute));
                case "subscriber":
                    return new BitmapImage(new Uri(Core.Twitch.AivaClient.Badges?.Subscriber?.Image ?? null, UriKind.RelativeOrAbsolute));
                case "globalmod":
                    return new BitmapImage(new Uri(Core.Twitch.AivaClient.Badges?.GlobalMod?.Image ?? null, UriKind.RelativeOrAbsolute));
                case "admin":
                    return new BitmapImage(new Uri(Core.Twitch.AivaClient.Badges?.Admin?.Image ?? null, UriKind.RelativeOrAbsolute));
                case "broadcaster":
                    return new BitmapImage(new Uri(Core.Twitch.AivaClient.Badges?.Broadcaster?.Image ?? null, UriKind.RelativeOrAbsolute));
                case "turbo":
                    return new BitmapImage(new Uri(Core.Twitch.AivaClient.Badges?.Turbo?.Image ?? null, UriKind.RelativeOrAbsolute));
                case "staff":
                    return new BitmapImage(new Uri(Core.Twitch.AivaClient.Badges?.Staff?.Image ?? null, UriKind.RelativeOrAbsolute));
                case "premium":
                    return new BitmapImage(new Uri(@"/Images/Badges/prime.png", UriKind.Relative));
                default: return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
