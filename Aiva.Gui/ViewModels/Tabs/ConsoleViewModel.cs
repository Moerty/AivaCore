using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Aiva.Gui.Internal;
using TwitchLib.Client.Events;
using System.Linq;
using System.Threading.Tasks;

namespace Aiva.Gui.ViewModels.Tabs {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class ConsoleViewModel {
        public AsyncObservableCollection<Models.Gui.Tabs.Chat.MessageModel> Message { get; set; }
        public AsyncObservableCollection<Models.Gui.Tabs.Chat.UserModel> User { get; set; }

        public ConsoleViewModel() {
            Message = new AsyncObservableCollection<Models.Gui.Tabs.Chat.MessageModel>();
            User = new AsyncObservableCollection<Models.Gui.Tabs.Chat.UserModel>();
            AddListener();
        }

        private void AddListener() {
            Core.Twitch.AivaClient.TwitchClient.OnMessageReceived
                            += (s, e)
                            => AddMessage(e);

            Core.Twitch.AivaClient.TwitchClient.OnExistingUsersDetected
                += (s, e)
                => AddUsers(e);

            Core.Twitch.AivaClient.TwitchClient.OnUserJoined
                += (s, e)
                => UserJoined(e);

            Core.Twitch.AivaClient.TwitchClient.OnUserLeft
                += (s, e)
                => RemoveUser(e);
        }

        private async void AddUsers(OnExistingUsersDetectedArgs e) {
            foreach(var user in e.Users) {
                var twitchUser = await Core.Twitch.AivaClient.TwitchApi.V5.Users.GetUserByNameAsync(user)
                    .ConfigureAwait(false);

                if (twitchUser?.Total > 0) {
                    User.Add(
                        new Models.Gui.Tabs.Chat.UserModel {
                            UserId = twitchUser.Matches[0].Id,
                            Username = twitchUser.Matches[0].DisplayName,
                            UserType = "Viewer"
                        });
                }
            }
        }

        private void RemoveUser(OnUserLeftArgs e) {
            if(User.Any(u => u.Username == e.Username)) {
                var user = User.FirstOrDefault(u => u.Username == e.Username);

                User.Remove(user);
            }
        }

        private async void UserJoined(OnUserJoinedArgs e) {
            var twitchUser = await Core.Twitch.AivaClient.TwitchApi.V5.Users.GetUserByNameAsync(e.Username)
                .ConfigureAwait(false);

            if(twitchUser?.Total > 0) {
                User.Add(
                    new Models.Gui.Tabs.Chat.UserModel {
                        UserId = twitchUser.Matches[0].Id,
                        Username = twitchUser.Matches[0].DisplayName,
                        UserType = "Viewer"
                    });
            }
        }

        private async Task UserJoined(string user) {
            var twitchUser = await Core.Twitch.AivaClient.TwitchApi.V5.Users.GetUserByNameAsync(user)
                .ConfigureAwait(false);

            if (twitchUser?.Total > 0) {
                User.Add(
                    new Models.Gui.Tabs.Chat.UserModel {
                        UserId = twitchUser.Matches[0].Id,
                        Username = twitchUser.Matches[0].DisplayName,
                        UserType = "Viewer"
                    });
            }
        }

        private void AddMessage(OnMessageReceivedArgs e) {
            Message.Add(
                new Models.Gui.Tabs.Chat.MessageModel {
                    Username = e.ChatMessage.Username,
                    Message = e.ChatMessage.Message,
                    Badges = e.ChatMessage.Badges.Select(s => s.Key)
                });

            CheckUserRights(e);
        }

        private void CheckUserRights(OnMessageReceivedArgs e) {
            if(User.Any(u => u.Username == e.ChatMessage.Username)) {
                var user = User.FirstOrDefault(u => u.Username == e.ChatMessage.Username);

                if(e.ChatMessage.UserType.ToString() != user.UserType) {
                    user.UserType = e.ChatMessage.UserType.ToString();
                }
            } else {
                User.Add(
                    new Models.Gui.Tabs.Chat.UserModel {
                        UserType = e.ChatMessage.UserType.ToString(),
                        UserId = e.ChatMessage.UserId,
                        Username = e.ChatMessage.Username
                    });
            }
        }
    }
}
