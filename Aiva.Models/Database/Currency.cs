using System;
using System.Collections.Generic;
using System.Text;

namespace Aiva.Models.Database {
    public class Currency {
        public int CurrencyId { get; set; }

        public long Value { get; set; }

        public int UsersId { get; set; }
        public virtual Users Users { get; set; }
    }
}
