using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace Aiva.Core.Twitch {
    public class AivaClient {
        public static bool DryRun = false;

        public static TwitchClient TwitchClient { get; private set; }
        public static TwitchAPI TwitchApi { get; private set; }

        public static TwitchLib.Api.V5.Models.Chat.ChannelBadges Badges { get; private set; }

        public AivaClient() {
            TwitchApi = new TwitchAPI();
            TwitchApi.Settings.ClientId = Config.ConfigHandler.Config.Credentials.TwitchClientID;
            TwitchApi.Settings.AccessToken = Config.ConfigHandler.Config.Credentials.TwitchOAuth;

            var credentials = new ConnectionCredentials(
                Config.ConfigHandler.Config.General.BotName,
                Config.ConfigHandler.Config.Credentials.TwitchOAuth);

            TwitchClient = new TwitchClient();
            TwitchClient.Initialize(
                credentials: credentials,
                channel: Config.ConfigHandler.Config.General.Channel,
                chatCommandIdentifier: Convert.ToChar(Config.ConfigHandler.Config.General.CommandIdentifier),
                whisperCommandIdentifier: '@',
                autoReListenOnExceptions: true);

            TwitchClient.OnJoinedChannel += OnJoinedChannel;

            TwitchClient.Connect();
        }

        private void OnJoinedChannel(object sender, OnJoinedChannelArgs e) {
            TwitchClient.SendMessage(
                Config.ConfigHandler.Config.General.Channel,
                "Aiva started, hi at all!",
                DryRun);

            GetChannelBadges();
        }

        private async void GetChannelBadges() {
            Badges = await TwitchApi.V5.Chat.GetChatBadgesByChannelAsync(Core.Config.ConfigHandler.Config.General.ChannelID);
        }

        public static async Task<TwitchLib.Api.V5.Models.Users.User> GetChannelDetails(string oAuthToken, string channel) {
            var apiClient = new TwitchAPI();
            apiClient.Settings.AccessToken = oAuthToken;

            var users = await apiClient.V5.Users.GetUserByNameAsync(channel);

            return users?.Total > 0
                ? users.Matches[0]
                : throw new Exception("User not found!");
        }

        public async static Task<bool> CheckBotuser(string clientId, string oauthToken) {
            var apiClient = new TwitchAPI();
            apiClient.Settings.AccessToken = oauthToken;

            var cred = await apiClient.V5.Root.CheckCredentialsAsync();
            return cred?.Result ?? false;
        }
    }
}
