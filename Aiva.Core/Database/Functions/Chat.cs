using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client.Events;

namespace Aiva.Core.Database.Functions {
    internal class Chat {
        internal async void AddReceivedMessageToDatabase(object sender, OnMessageReceivedArgs e) {
            using(var context = new Context()) {
                await context.Chat.AddAsync(
                    new Models.Database.Chat {
                        UsersId = Convert.ToInt32(e.ChatMessage.UserId),
                        ChatMessage = e.ChatMessage.Message,
                        Timestamp = DateTime.Now
                    })
                    .ConfigureAwait(false);

                await context.SaveChangesAsync()
                    .ConfigureAwait(false);
            }
        }
    }
}
