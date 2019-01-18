﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Aiva.Models.Database {
    public class Timers {
        public int TimersId { get; set; }

        public string Name { get; set; }
        public string Text { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public System.DateTime? ModifiedAt { get; set; }
        public int Interval { get; set; }
        public System.DateTime? NextExecution { get; set; }
    }
}
