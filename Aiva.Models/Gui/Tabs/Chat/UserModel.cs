using System;
using System.Collections.Generic;
using System.Text;

namespace Aiva.Models.Gui.Tabs.Chat {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class UserModel {
        public string Username { get; set; }
        public string UserType { get; set; }
        public string UserId { get; set; }
    }
}
