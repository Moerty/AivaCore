using System;
using System.Collections.Generic;
using System.Text;

namespace Aiva.Models.Database {
    public class TimeWatched {
        public int TimeWatchedId { get; set; }

        public long Time { get; set; }

        public int UsersId { get; set; }
        public virtual Users Users { get; set; }
    }
}
