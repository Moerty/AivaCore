using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aiva.Models.Database {
    public class Currency {
        [Key]
        public int Id { get; set; }

        public long Value { get; set; }

        public string UsersId { get; set; }
        public virtual Users Users { get; set; }
    }
}
