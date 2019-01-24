using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Aiva.Models.Gui.Windows;

namespace Aiva.Gui {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        private Views.Windows.SetupWindow setupWindow;

        public void StartApp(object sender, EventArgs e) {
            if (!File.Exists(Core.ConfigHandler.GetConfigPath())) {
                setupWindow = new Views.Windows.SetupWindow();

                setupWindow.Show();
            } else {
                Core.Boot.Main();

                var mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }

        private void ExitApp(object sender, EventArgs e) {
            // when setup is closed without saving the config file,
            // aivaclient cant save the config, cause the file doesnt exists
            if (File.Exists("Config\\config.json")) {
                Core.Twitch.AivaClient.TwitchClient.Disconnect();
                Core.ConfigHandler.SaveConfig();
            }
            //CefSharp.Cef.Shutdown();
            Environment.Exit(0);
        }

    }
}
