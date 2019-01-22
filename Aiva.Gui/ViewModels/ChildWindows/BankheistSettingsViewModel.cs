using System;
using System.Collections.Generic;
using System.Text;

namespace Aiva.Gui.ViewModels.ChildWindows {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class BankheistSettingsViewModel {
        public Models.StreamGames.Bankheist.Properties Properties { get; set; }

        private readonly bool _bankheistActive;

        public BankheistSettingsViewModel(bool bankheistActive) {
            _bankheistActive = bankheistActive;
            LoadPropertiesFromConfig();
        }

        private void LoadPropertiesFromConfig() {
            Properties = new Models.StreamGames.Bankheist.Properties {
                Command = Core.Config.ConfigHandler.Config.StreamGames.Bankheist.General.Command,
                BankheistCooldown = (int)Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Cooldowns.BankheistCooldown,
                BankheistDuration = (int)Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Cooldowns.BankheistDuration,

                MinUserBank1 = Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank1.MinimumPlayers,
                SuccessRateBank1 = Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank1.SuccessRate,
                WinningMultiplierBank1 = Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank1.WinningMultiplier,

                MinUserBank2 = Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank2.MinimumPlayers,
                SuccessRateBank2 = Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank2.SuccessRate,
                WinningMultiplierBank2 = Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank2.WinningMultiplier,

                MinUserBank3 = Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank3.MinimumPlayers,
                SuccessRateBank3 = Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank3.SuccessRate,
                WinningMultiplierBank3 = Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank3.WinningMultiplier,

                MinUserBank4 = Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank4.MinimumPlayers,
                SuccessRateBank4 = Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank4.SuccessRate,
                WinningMultiplierBank4 = Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank4.WinningMultiplier,

                MinUserBank5 = Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank5.MinimumPlayers,
                SuccessRateBank5 = Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank5.SuccessRate,
                WinningMultiplierBank5 = Core.Config.ConfigHandler.Config.StreamGames.Bankheist.Settings.Bank5.WinningMultiplier,
            };
        }

        public void SafePropertiesToConfig() {
            Core.Config.ConfigHandler.Config.StreamGames.Bankheist = new Models.Config.Model.Bankheist {
                General = new Models.Config.Model.PurpleGeneral {
                    Active = _bankheistActive,
                    Command = Properties.Command.TrimEnd('!')
                },
                Cooldowns = new Models.Config.Model.Cooldowns {
                    BankheistCooldown = Properties.BankheistCooldown,
                    BankheistDuration = Properties.BankheistDuration
                },
                Settings = new Models.Config.Model.Settings {
                    Bank1 = new Models.Config.Model.Bank {
                        MinimumPlayers = Properties.MinUserBank1,
                        SuccessRate = Properties.SuccessRateBank1,
                        WinningMultiplier = Properties.WinningMultiplierBank1
                    },
                    Bank2 = new Models.Config.Model.Bank {
                        MinimumPlayers = Properties.MinUserBank2,
                        SuccessRate = Properties.SuccessRateBank2,
                        WinningMultiplier = Properties.WinningMultiplierBank2
                    },
                    Bank3 = new Models.Config.Model.Bank {
                        MinimumPlayers = Properties.MinUserBank3,
                        SuccessRate = Properties.SuccessRateBank3,
                        WinningMultiplier = Properties.WinningMultiplierBank3
                    },
                    Bank4 = new Models.Config.Model.Bank {
                        MinimumPlayers = Properties.MinUserBank4,
                        SuccessRate = Properties.SuccessRateBank4,
                        WinningMultiplier = Properties.WinningMultiplierBank4
                    },
                    Bank5 = new Models.Config.Model.Bank {
                        MinimumPlayers = Properties.MinUserBank5,
                        SuccessRate = Properties.SuccessRateBank5,
                        WinningMultiplier = Properties.WinningMultiplierBank5
                    }
                }
            };
        }
    }
}
