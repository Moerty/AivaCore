﻿using Aiva.Models.Extensions;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aiva.Models.StreamGames.Bankheist {
    [AddINotifyPropertyChangedInterface]
    public class Properties {
        private string _Command;
        public string Command {
            get {
                return _Command;
            }
            set {
                _Command = value.ModCommand();
            }
        }

        public int BankheistDuration { get; set; }
        public int BankheistCooldown { get; set; }

        public int MinUserBank1 { get; set; }
        public int SuccessRateBank1 { get; set; }
        public double WinningMultiplierBank1 { get; set; }

        public int MinUserBank2 { get; set; }
        public int SuccessRateBank2 { get; set; }
        public double WinningMultiplierBank2 { get; set; }

        public int MinUserBank3 { get; set; }
        public int SuccessRateBank3 { get; set; }
        public double WinningMultiplierBank3 { get; set; }

        public int MinUserBank4 { get; set; }
        public int SuccessRateBank4 { get; set; }
        public double WinningMultiplierBank4 { get; set; }

        public int MinUserBank5 { get; set; }
        public int SuccessRateBank5 { get; set; }
        public double WinningMultiplierBank5 { get; set; }
        public object Config { get; }
    }
}
