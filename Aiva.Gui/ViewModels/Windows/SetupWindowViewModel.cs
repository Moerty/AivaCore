using Aiva.Core.Twitch;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Aiva.Gui.ViewModels.Windows {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class SetupWindowViewModel {
        public string ClientId { get; set; }
        public string BotName { get; set; }
        public string Channel { get; set; }

        public string StreamerName { get; set; }
        public bool IsStreamerNameEnabled { get; set; }

        public string BotOAuthToken { get; set; }
        public string StreamerOAuthToken { get; set; }

        private string _channelId;

        public ICommand BotUserOAuthRequest { get; set; }
        public ICommand StreamerAccountOAuthRequest { get; set; }

        public EventHandler<string> ShowMessageBox;
        public EventHandler CloseSetupPage;

        public SetupWindowViewModel() {
            BotUserOAuthRequest = new Internal.RelayCommand(
                setup => BotUserRequest());

            StreamerAccountOAuthRequest = new Internal.RelayCommand(
                setup => StreamerUserRequest());
        }

        private async void StreamerUserRequest() {
            var auth = new Core.Twitch.Authentication();
            auth.SendRequestToBrowser(ClientId);

            var result = await auth.GetAuthenticationValuesAsync().ConfigureAwait(false);
            var isValid = await Core.Twitch.AivaClient.CheckBotuser(ClientId, result.Token).ConfigureAwait(false);

            if(isValid) {
                var api = new TwitchLib.Api.TwitchAPI();
                api.Settings.ClientId = ClientId;
                api.Settings.AccessToken = result.Token;

                var channelDetails = await Core.Twitch.AivaClient.GetChannelDetails(result.Token, Channel).ConfigureAwait(false);

                new Core.ConfigHandler(ClientId, BotOAuthToken, BotName, Channel, channelDetails.Id, result.Token);

                Application.Current.Dispatcher.Invoke(() => ((App)Application.Current).StartApp(this, EventArgs.Empty));
            }
        }

        private async void BotUserRequest() {
            var auth = new Core.Twitch.Authentication();
            auth.SendRequestToBrowser(ClientId);

            var result = await auth.GetAuthenticationValuesAsync().ConfigureAwait(false);
            var isValid = await Core.Twitch.AivaClient.CheckBotuser(ClientId, result.Token).ConfigureAwait(false);

            if (isValid) {
                StreamerName = Channel;
                IsStreamerNameEnabled = true;
                BotOAuthToken = result.Token;

                ShowMessageBox?.Invoke(this, "Please login to your Streameraccount!");

                //new Core.ConfigHandler(e.ClientId, result.Token, e.Botname, e.Channel, channelDetails.Id);

                //Dispatcher.Invoke(new Action(() => StartApp(this, EventArgs.Empty)));
                //Dispatcher.Invoke(new Action(() => setupWindow.Close()));
            }
            else {
                throw new Exception("Failed to check twitch credentials");
            }
        }
    }
}
