using System;
using System.Collections.Generic;
using System.Text;

namespace Aiva.Models.Database {
    public class ActiveUsers {
        public int TwitchUserId { get; set; }

        public DateTime JoinedTime { get; set; }

        public int UsersId { get; set; }
        public virtual Users Users { get; set; }
    }
}
