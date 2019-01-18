using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client;

namespace Aiva.Core.Twitch.Functions {
    public class EventListener {
        private readonly Database.Functions.Users _databaseUsersHandler;
        private readonly Database.Functions.Chat _chatDatabaseHandler;

        public EventListener() {
            _databaseUsersHandler = new Database.Functions.Users();
        }

        public void SetEvents() {
            // users
            AivaClient.TwitchClient.OnExistingUsersDetected += _databaseUsersHandler.AddUser.AddExistingUsers;
            AivaClient.TwitchClient.OnUserJoined += _databaseUsersHandler.AddUser.UserJoined;
            AivaClient.TwitchClient.OnUserLeft += _databaseUsersHandler.RemoveUser.UserLeft;

            // chat
            AivaClient.TwitchClient.OnMessageReceived += _chatDatabaseHandler.AddReceivedMessageToDatabase;
        }
    }
}
