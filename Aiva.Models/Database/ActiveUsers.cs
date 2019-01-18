using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aiva.Models.Database {
    public class ActiveUsers {
        [Key]
        public int Id { get; set; }

        public DateTime JoinedTime { get; set; }

        public string UsersId { get; set; }
        public virtual Users Users { get; set; }
    }
}
