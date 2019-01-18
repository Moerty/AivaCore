using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aiva.Models.Database {
    public class BlacklistedWords {
        [Key]
        public int Id { get; set; }

        public string Word { get; set; }
    }
}
