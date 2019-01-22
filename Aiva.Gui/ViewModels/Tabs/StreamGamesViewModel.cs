using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Aiva.Gui.ViewModels.Tabs {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class StreamGamesViewModel {
        public bool IsBankheistActive {
            get { return Core.Config.ConfigHandler.Config.StreamGames.Bankheist.General.Active; }
            set { Core.Config.ConfigHandler.Config.StreamGames.Bankheist.General.Active = value;
                if (value)
                    _bankheistHandler.StartGame();
                else
                    _bankheistHandler.StopGame();
            }
        }
        private readonly Extensions.StreamGames.Bankheist _bankheistHandler;

        public StreamGamesViewModel() {
            _bankheistHandler = new Extensions.StreamGames.Bankheist();

            StartGames();
        }

        private void StartGames() {
            if(Core.Config.ConfigHandler.Config.StreamGames.Bankheist.General.Active) {
                _bankheistHandler.StartGame();
            }
        }
    }
}
