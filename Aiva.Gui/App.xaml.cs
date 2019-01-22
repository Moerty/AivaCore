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

        private void StartApp(object sender, EventArgs e) {
            if (!File.Exists(Core.Config.ConfigHandler.GetConfigPath())) {
                setupWindow = new Views.Windows.SetupWindow();
                setupWindow.ConfirmValues += OnSetupValuesConfirmed;

                setupWindow.Show();
            } else {
                Core.Boot.Main();

                var mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }

        private async void OnSetupValuesConfirmed(object sender, ConfirmSetupModel e) {
            var auth = new Core.Twitch.Authentication();
            auth.SendRequestToBrowser(e.ClientId);

            var result = await auth.GetAuthenticationValuesAsync().ConfigureAwait(false);
            var isValid = await Core.Twitch.AivaClient.CheckBotuser(e.ClientId, result.Token).ConfigureAwait(false);
            var channelDetails = await Core.Twitch.AivaClient.GetChannelDetails(result.Token, e.Channel).ConfigureAwait(false);

            if (isValid) {
                new Core.Config.ConfigHandler(e.ClientId, result.Token, e.Botname, e.Channel, channelDetails.Id);

                Dispatcher.Invoke(new Action(() => StartApp(this, EventArgs.Empty)));
                Dispatcher.Invoke(new Action(() => setupWindow.Close()));
            } else {
                throw new Exception("Failed to check twitch credentials");
            }
        }

        private void ExitApp(object sender, EventArgs e) {
            // when setup is closed without saving the config file,
            // aivaclient cant save the config, cause the file doesnt exists
            if (File.Exists("Config\\config.json")) {
                Core.Twitch.AivaClient.TwitchClient.Disconnect();
                Core.Config.ConfigHandler.SaveConfig();
            }
            //CefSharp.Cef.Shutdown();
            Environment.Exit(0);
        }

    }
}
