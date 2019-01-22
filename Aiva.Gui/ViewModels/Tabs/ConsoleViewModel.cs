using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Aiva.Gui.ViewModels.Tabs {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class ConsoleViewModel {
        public ObservableCollection<string> Message { get; set; }
        public ObservableCollection<string> User { get; set; }

        public ConsoleViewModel() {
            Message = new ObservableCollection<string>();
            User = new ObservableCollection<string>();
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
