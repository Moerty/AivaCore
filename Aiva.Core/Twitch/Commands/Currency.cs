using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client.Events;

namespace Aiva.Core.Twitch.Commands {
    internal class Currency {
        private readonly Database.Functions.Currency _currencyDatabaseHandler;

        public Currency() {
            _currencyDatabaseHandler = new Database.Functions.Currency();
        }

        internal async void OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e) {
            if(e.Command.CommandText == Config.ConfigHandler.Config.Currency.CurrencyCommands.GetCurrency && e.Command.ArgumentsAsList?.Count == 0) {
                var currencyForUser = await _currencyDatabaseHandler.GetCurrency(e.Command.ChatMessage.UserId)
                    .ConfigureAwait(false);

                if(currencyForUser.HasValue) {
                    AivaClient.TwitchClient.SendMessage(
                        channel: Config.ConfigHandler.Config.General.Channel,
                        message: $"@{e.Command.ChatMessage.DisplayName} : You have {currencyForUser.Value} currency!",
                        dryRun: AivaClient.DryRun);
                }
            }
        }
    }
}
