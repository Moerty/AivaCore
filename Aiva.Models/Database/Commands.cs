using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aiva.Models.Database {
    public class Commands {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Text { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public string CreatedFrom { get; set; }
        public System.DateTime? ModifiedAt { get; set; }
        public string ModifiedFrom { get; set; }
        public long? Stack { get; set; }
        public System.DateTime? LastExecution { get; set; }
    }
}
