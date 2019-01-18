using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aiva.Models.Database {
    public class TimeWatched {
        [Key]
        public int Id { get; set; }

        public long Time { get; set; }

        public string UsersId { get; set; }
        public virtual Users Users { get; set; }
    }
}
