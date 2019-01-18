using System;
using System.Collections.Generic;
using System.Text;

namespace Aiva.Models.Database {
    public class Chat {
        public int ChatId { get; set; }

        public DateTime Timestamp { get; set; }
        public string ChatMessage { get; set; }

        public int UsersId { get; set; }
        public virtual Users Users { get; set; }
    }
}
