using Aiva.Gui.Internal;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Aiva.Gui.ViewModels.Tabs {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class StreamGamesViewModel {
        public bool IsBankheistActive {
            get { return Core.Config.ConfigHandler.Config.StreamGames.Bankheist.General.Active; }
            set {
                Core.Config.ConfigHandler.Config.StreamGames.Bankheist.General.Active = value;
                if (value)
                    _bankheistHandler.StartGame();
                else
                    _bankheistHandler.StopGame();
            }
        }
        public ICommand ShowBankheistSettingsCommand { get; set; }

        private readonly Extensions.StreamGames.Bankheist _bankheistHandler;

        public StreamGamesViewModel() {
            _bankheistHandler = new Extensions.StreamGames.Bankheist();

            ShowBankheistSettingsCommand = new RelayCommand(
                bank => ShowBankheistSettings(), bank => _bankheistHandler != null);

            StartGames();
        }

        private void StartGames() {
            if(Core.Config.ConfigHandler.Config.StreamGames.Bankheist.General.Active) {
                _bankheistHandler.StartGame();
            }
        }

        private async void ShowBankheistSettings() {
            if (Application.Current.MainWindow != null) {
                var optionsChildWindow = new Views.ChildWindows.BankheistSettings();
                var currentStateActive = Core.Config.ConfigHandler.Config.StreamGames.Bankheist.General.Active;

                optionsChildWindow.ClosingFinished
                    += (s, args)
                    => { IsBankheistActive = false; IsBankheistActive = currentStateActive; };

                await ((MetroWindow)Application.Current.MainWindow).ShowChildWindowAsync(optionsChildWindow)
                    .ConfigureAwait(false);
            }
        }
    }
}
