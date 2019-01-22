using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Aiva.Gui.Internal;
using TwitchLib.Client.Events;
using System.Linq;

namespace Aiva.Gui.ViewModels.Tabs {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class ConsoleViewModel {
        public AsyncObservableCollection<Models.Gui.Tabs.Chat.MessageModel> Message { get; set; }
        public AsyncObservableCollection<string> User { get; set; }

        public ConsoleViewModel() {
            Message = new AsyncObservableCollection<Models.Gui.Tabs.Chat.MessageModel>();
            User = new AsyncObservableCollection<string>();
            AddListener();
        }

        private void AddListener() {
            Core.Twitch.AivaClient.TwitchClient.OnMessageReceived
                            += (s, e)
                            => AddMessage(e);

            Core.Twitch.AivaClient.TwitchClient.OnExistingUsersDetected
                += (s, e)
                => {
                    e.Users.ForEach(u => User.Add(u));
                };

            Core.Twitch.AivaClient.TwitchClient.OnUserJoined
                += (s, e)
                => User.Add(e.Username);

            Core.Twitch.AivaClient.TwitchClient.OnUserLeft
                += (s, e)
                => User.Remove(e.Username);
        }

        private void AddMessage(OnMessageReceivedArgs e) {
            Message.Add(
                new Models.Gui.Tabs.Chat.MessageModel {
                    Username = e.ChatMessage.Username,
                    Message = e.ChatMessage.Message,
                    Badges = e.ChatMessage.Badges.Select(s => s.Key)
                });
        }
    }
}
