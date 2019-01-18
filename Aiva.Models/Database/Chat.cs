using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Aiva.Models.Database {
    public class Chat {
        [Key]
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }
        public string ChatMessage { get; set; }
        public string MessageId { get; set; }

        public string UsersId { get; set; }
        public virtual Users Users { get; set; }
    }
}
