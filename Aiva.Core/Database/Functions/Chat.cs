using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client.Events;

namespace Aiva.Core.Database.Functions {
    internal class Chat {
        private readonly Database.Functions.Users.Add _databaseAddHandler;

        public Chat() {
            _databaseAddHandler = new Users.Add();
        }

        internal async void AddReceivedMessageToDatabase(object sender, OnMessageReceivedArgs e) {
            // add user to database if not exist
            // cause irc "join" message needs time
            await _databaseAddHandler.AddUserToDatabaseAsync(e.ChatMessage);

            using(var context = new Context()) {
                await context.Chat.AddAsync(
                    new Models.Database.Chat {
                        UsersId = e.ChatMessage.UserId,
                        ChatMessage = e.ChatMessage.Message,
                        Timestamp = DateTime.Now,
                        MessageId = e.ChatMessage.Id
                    })
                    .ConfigureAwait(false);

                await context.SaveChangesAsync()
                    .ConfigureAwait(false);
            }
        }
    }
}