using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Api;
using TwitchLib.Client;

namespace Aiva.Core.Twitch {
    public class AivaClient {
        public bool DryRun = false;

        public static TwitchClient TwitchClient { get; private set; }
        public static TwitchAPI TwitchApi { get; private set; }

        public AivaClient() {
            TwitchApi = new TwitchAPI();
            TwitchApi.Settings.ClientId = Config.ConfigHandler.Config.Credentials.TwitchClientID;
            TwitchApi.Settings.AccessToken = Config.ConfigHandler.Config.Credentials.TwitchOAuth;
        }
    }
}
