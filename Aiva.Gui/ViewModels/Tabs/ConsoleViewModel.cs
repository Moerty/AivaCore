using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Aiva.Gui.Internal;

namespace Aiva.Gui.ViewModels.Tabs {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class ConsoleViewModel {
        public AsyncObservableCollection<string> Message { get; set; }
        public AsyncObservableCollection<string> User { get; set; }

        public ConsoleViewModel() {
            Message = new AsyncObservableCollection<string>();
            User = new AsyncObservableCollection<string>();
            Core.Twitch.AivaClient.TwitchClient.OnMessageReceived
                += (s, e)
                => {
                    Message.Add(e.ChatMessage.Message);
                };

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
    }
}
