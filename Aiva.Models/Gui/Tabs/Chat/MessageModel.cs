using System;
using System.Collections.Generic;
using System.Text;

namespace Aiva.Models.Gui.Tabs.Chat {
    public class MessageModel {
        public string Username { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Badges { get; set; }
    }
}
