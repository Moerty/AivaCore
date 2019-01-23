﻿using Aiva.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aiva.Models.StreamGames.Roulette {
    public class User {
        public string Name { get; set; }
        public string UserId { get; set; }
        public int BetValue { get; set; }
        public BetTypes BetType { get; set; }

        public bool IsWon { get; set; }
        public int WonSum { get; set; }
    }
}
