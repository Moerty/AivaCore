using Aiva.Core.Twitch;
using Aiva.Models.Enums;
using Aiva.Models.StreamGames.Roulette;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;

namespace Aiva.Extensions.StreamGames {
    public class Roulette : Models.Interfaces.IStreamGames {
        private readonly Core.Database.Functions.Currency _databaseCurrencyHandler;
        private readonly Random _randomGenerator;
        private readonly TableLayout _tableLayout;
        private List<User> registeredUsers;
        private int _winningNumber;
        private bool _isActive;

        private const int minNumber = 0;
        private const int maxNumber = 36;
        private const int timeActiveRoulette = 300000;
        private const int timeToWaitForWinner = 30000;
        private const int timeToWaitForNewRoulette = 300000;

        public Roulette() {
            _databaseCurrencyHandler = new Core.Database.Functions.Currency();
            _randomGenerator = new Random();
            _tableLayout = new TableLayout();
        }

        public void StartGame() {
            _isActive = true;
            AivaClient.TwitchClient.OnChatCommandReceived += ChatMessageReceived;
            StartRoulette();
        }

        public void StopGame() {
            _isActive = false;
            AivaClient.TwitchClient.OnChatCommandReceived -= ChatMessageReceived;
        }

        private async void StartRoulette() {
            while (_isActive) {
                AivaClient.TwitchClient
                    .SendMessage(
                        Core.ConfigHandler.Config.General.Channel,
                        "Roulette started",
                        AivaClient.DryRun);

                registeredUsers = new List<User>();

                await Task.Delay(timeActiveRoulette)
                    .ConfigureAwait(false);

                AivaClient.TwitchClient
                    .SendMessage(
                        Core.ConfigHandler.Config.General.Channel,
                        "Roulette closed",
                        AivaClient.DryRun);

                await Task.Delay(timeToWaitForWinner)
                    .ConfigureAwait(false);

                SpinTheWheel();

                var winners = registeredUsers
                    .Where(w => w.IsWon);

                var stringBuilder = new StringBuilder();

                stringBuilder.Append($"Winning Number is {_winningNumber}.");
                if (winners.Any()) {
                    foreach (var winner in winners) {
                        stringBuilder.Append($"{winner.Name} - {winner.WonSum} ");
                    }

                    stringBuilder.Append(" Winner is: ");
                } else {
                    stringBuilder.Append("No winners");
                }

                if (stringBuilder.ToString().Any()) {
                    AivaClient.TwitchClient.SendMessage(
                        Core.ConfigHandler.Config.General.Channel,
                        stringBuilder.ToString(),
                        AivaClient.DryRun);
                }

                await Task.Delay(timeToWaitForNewRoulette)
                    .ConfigureAwait(false);
            }
        }

        private async void SpinTheWheel() {
            _winningNumber = _randomGenerator.Next(minNumber, maxNumber);
            var wonItem = _tableLayout.AllNumbers.FirstOrDefault(w => w.Number == _winningNumber);

            foreach (var user in registeredUsers) {
                if (user.BetType == BetTypes.Number) {
                    if (_winningNumber == user.BetValue) {
                        user.IsWon = true;
                        user.WonSum = user.BetValue * 36;
                    }
                } else if (user.BetType == BetTypes.Black && !wonItem.IsRed) {
                    user.IsWon = true;
                    user.WonSum = user.BetValue * 2;
                } else if (user.BetType == BetTypes.Red && wonItem.IsRed) {
                    user.IsWon = true;
                    user.WonSum = user.BetValue * 2;
                } else if (user.BetType == BetTypes.Even && wonItem.IsEven) {
                    user.IsWon = true;
                    user.WonSum = user.BetValue * 2;
                } else if (user.BetType == BetTypes.Odd && !wonItem.IsEven) {
                    user.IsWon = true;
                    user.WonSum = user.BetValue * 2;
                }

                if (user.IsWon) {
                    await _databaseCurrencyHandler.Add.Add(user.UserId, user.WonSum);
                }
            }
        }

        private async void ChatMessageReceived(object sender, OnChatCommandReceivedArgs e) {
            // check if correct command
            if (!string.Equals(e.Command.CommandText, "roulette", StringComparison.InvariantCultureIgnoreCase)) return;

            // check if arguments are valid
            if (e.Command.ArgumentsAsList.Count < 2) {
                return;
            }

            if (int.TryParse(e.Command.ArgumentsAsList[0], out int rouletteBet)) {
                var userId = Convert.ToInt32(e.Command.ChatMessage.UserId);

                // check if already registered
                if (registeredUsers.Any(u => u.UserId == e.Command.ChatMessage.UserId)) {
                    return;
                }

                // check if user has enough currency
                var enoughCurrencyCheckResult = await _databaseCurrencyHandler.HasUserEnoughCurrency(e.Command.ChatMessage.UserId, rouletteBet);
                if (!enoughCurrencyCheckResult) {
                    return;
                } else {
                    await _databaseCurrencyHandler.Remove.Remove(e.Command.ChatMessage.UserId, rouletteBet);
                }

                var betType = GetBetType(e.Command.ArgumentsAsList[1]);
                if (betType == BetTypes.Unknown) return;

                var user = new User {
                    Name = e.Command.ChatMessage.DisplayName,
                    UserId = e.Command.ChatMessage.UserId,
                    BetValue = rouletteBet,
                    BetType = betType
                };

                registeredUsers.Add(user);
            }
        }

        private BetTypes GetBetType(string type) {
            if (int.TryParse(type, out int number)) {
                return BetTypes.Number;
            }

            switch (type.ToLower()) {
                case "odd":
                    return BetTypes.Odd;
                case "even":
                    return BetTypes.Even;
                case "red":
                    return BetTypes.Red;
                case "black":
                    return BetTypes.Black;
                default:
                    return BetTypes.Unknown;
            }
        }
    }
}
