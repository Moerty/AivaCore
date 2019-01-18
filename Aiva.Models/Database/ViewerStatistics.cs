using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aiva.Models.Database {
    public class ViewerStatistics {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public int Max { get; set; }
        public int Min { get; set; }
        public int Average { get; set; }

        public int AddedValue { get; set; }
        public int Count { get; set; }
    }
}
