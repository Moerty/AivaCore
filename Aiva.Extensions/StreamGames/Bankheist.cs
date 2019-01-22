using Aiva.Core.Twitch;
using Aiva.Models.Enums;
using Aiva.Models.StreamGames.Bankheist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchLib.Client.Events;

namespace Aiva.Extensions.StreamGames {
    public class Bankheist : Models.Interfaces.IStreamGames {
        private DateTime _nextStartAfterCooldown;
        private List<Models.StreamGames.Bankheist.UserBetModel> _userList;
        private readonly Core.Database.Functions.Currency _databaseCurrencyHandler;
        private System.Timers.Timer _bankheistEndTimer;

        public Bankheist() {
            _databaseCurrencyHandler = new Core.Database.Functions.Currency();
        }

        private async void ChatCommandReceived(object sender, OnChatCommandReceivedArgs e) {
            if (string.Equals(e.Command.CommandText, Core.Config.ConfigHandler.Config.StreamGames.Bankheist.General.Command, StringComparison.OrdinalIgnoreCase)) {
                // continue only if CooldownDatetime is null (never running) or cooldown is lower than Datetime.now
                if (_nextStartAfterCooldown == default(DateTime) || _nextStartAfterCooldown.TimeOfDay <= DateTime.Now.TimeOfDay) {
                    // check if chat argument is an integer
                    if (int.TryParse(e.Command.ArgumentsAsString, out int bankheistBet)) {
                        // check if user has enough currency
                        if (await _databaseCurrencyHandler.HasUserEnoughCurrency(e.Command.ChatMessage.UserId, bankheistBet)) {
                            // first startup? then create userlist and writes in chat that Bankheist is started
                            if (_userList == null) {
                                _userList = new List<Models.StreamGames.Bankheist.UserBetModel>();
                                WriteBankheistStartupInChat();
                                AddUserToList(e.Command.ChatMessage.UserId, bankheistBet, e.Command.ChatMessage.DisplayName);
                                StartBankheistEndTimer();
                            } else {
                                // check if user is already in list
                                if (!_userList.Any(u => u.TwitchID == e.Command.ChatMessage.UserId)) {
                                    AddUserToList(e.Command.ChatMessage.UserId, bankheistBet, e.Command.ChatMessage.DisplayName);
                                }
                            }

                            await _databaseCurrencyHandler.Remove.Remove(e.Command.ChatMessage.UserId, bankheistBet)
                                .ConfigureAwait(false);
                        }
                    }
                } else {
                    AivaClient.TwitchClient.SendMessage(
                        Core.Config.ConfigHandler.Config.General.Channel,
                        "Bankheist is on cooldown!",
                        AivaClient.DryRun);
                }
            }
        }

        private void StartBankheistEndTimer() {
            _bankheistEndTimer = new System.Timers.Timer {
                AutoReset = false,
                Interval = TimeSpan.FromMinutes(Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Cooldowns.BankheistDuration).TotalMilliseconds
            };
            _bankheistEndTimer.Elapsed += (sender, ElapsedEventArgs) => StopBankheist();
            _bankheistEndTimer.Start();
        }

        private async void StopBankheist() {
            _nextStartAfterCooldown = DateTime.Now.AddMinutes(Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Cooldowns.BankheistCooldown);
            var bank = IdentifyBank();
            var winningDetailsForBank = GetWinningDetailsForBank(bank);

            var Winners = new List<UserBetModel>();

            foreach (var user in _userList) {
                if (IsUserWinner(winningDetailsForBank.Item1)) {
                    user.Bet = (int)(user.Bet * winningDetailsForBank.Item2);
                    Winners.Add(user);
                }
            }

            if(Winners.Count > 0) {
                foreach(var winner in Winners) {
                    await _databaseCurrencyHandler.Add.Add(winner.TwitchID, winner.Bet);
                }

                WriteWinnersInChat(Winners);
            }
        }

        private void WriteWinnersInChat(List<UserBetModel> winners) {
            var sb = new StringBuilder();
            sb.Append("Winners: ");
            winners.ForEach(w => sb.Append(w.Name).Append("|"));
            AivaClient.TwitchClient.SendMessage(
                channel: Core.Config.ConfigHandler.Config.General.Channel,
                message: sb.ToString().TrimEnd('|'),
                dryRun: AivaClient.DryRun);
        }

        private bool IsUserWinner(long item1) {
            return new Random().Next(100) <= item1;
        }

        private Tuple<long, double> GetWinningDetailsForBank(Banks bank) {
            switch (bank) {
                case Banks.Bank5:
                    return new Tuple<long, double>(
                        Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank5.SuccessRate,
                        Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank5.WinningMultiplier);
                case Banks.Bank4:
                    return new Tuple<long, double>(
                        Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank4.SuccessRate,
                        Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank4.WinningMultiplier);
                case Banks.Bank3:
                    return new Tuple<long, double>(
                        Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank3.SuccessRate,
                        Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank3.WinningMultiplier);
                case Banks.Bank2:
                    return new Tuple<long, double>(
                        Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank2.SuccessRate,
                        Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank2.WinningMultiplier);
                default: // bank 1
                    return new Tuple<long, double>(
                        Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank1.SuccessRate,
                        Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank1.WinningMultiplier);
            }
        }

        private Banks IdentifyBank() {
            var count = _userList.Count;

            if (count >= Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank5.MinimumPlayers) {
                return Banks.Bank5;
            } else if (count >= Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank4.MinimumPlayers) {
                return Banks.Bank4;
            } else if (count >= Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank3.MinimumPlayers) {
                return Banks.Bank3;
            } else if (count >= Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank2.MinimumPlayers) {
                return Banks.Bank2;
            } else {
                return Banks.Bank1;
            }
        }

        private void AddUserToList(string userId, int bankheistBet, string displayName) {
            _userList.Add(
                new Models.StreamGames.Bankheist.UserBetModel {
                    Name = displayName,
                    TwitchID = userId,
                    Bet = bankheistBet
                });
        }

        private void WriteBankheistStartupInChat() {
            AivaClient.TwitchClient.SendMessage(
                channel: Core.Config.ConfigHandler.Config.General.Channel,
                message: $"New Bankheist started! Write !{Core.Config.ConfigHandler.Config.StreamGames.Bankheist.General.Command} in the Chat to rob the bank!",
                dryRun: AivaClient.DryRun);
        }

        public void StartGame() {
            _bankheistEndTimer = null;
            _userList = null;
            _nextStartAfterCooldown = default(DateTime);
            AivaClient.TwitchClient.OnChatCommandReceived += ChatCommandReceived;
        }

        public void StopGame() {
            _bankheistEndTimer = null;
            _userList = null;
            _nextStartAfterCooldown = default(DateTime);
            AivaClient.TwitchClient.OnChatCommandReceived -= ChatCommandReceived;
        }
    }
}
