using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client;

namespace Aiva.Core.Twitch.Functions {
    public class EventListener {
        private readonly Database.Functions.Users _databaseUsersHandler;
        private readonly Database.Functions.Chat _chatDatabaseHandler;
        private readonly Twitch.Commands.Currency _currencyCommandHandler;
        private readonly Commands.Mod.Currency _modCurrencyCommand;

        public EventListener() {
            _databaseUsersHandler = new Database.Functions.Users();
            _currencyCommandHandler = new Commands.Currency();
            _chatDatabaseHandler = new Database.Functions.Chat();
            _modCurrencyCommand = new Commands.Mod.Currency();
        }

        public void SetEvents() {
            // users
            AivaClient.TwitchClient.OnExistingUsersDetected += _databaseUsersHandler.AddUser.AddExistingUsers;
            AivaClient.TwitchClient.OnUserJoined += _databaseUsersHandler.AddUser.UserJoined;
            AivaClient.TwitchClient.OnUserLeft += _databaseUsersHandler.RemoveUser.UserLeft;

            // chat
            AivaClient.TwitchClient.OnMessageReceived += _chatDatabaseHandler.AddReceivedMessageToDatabase;

            // currency
            AivaClient.TwitchClient.OnChatCommandReceived += _currencyCommandHandler.OnChatCommandReceived;

            // mod
            AivaClient.TwitchClient.OnChatCommandReceived += _modCurrencyCommand.CommandReceived;
        }
    }
}
