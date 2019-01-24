using System;
using System.Collections.Generic;
using System.Text;
using PropertyChanged;

namespace Aiva.Gui.ViewModels.Tabs {
    [AddINotifyPropertyChangedInterface]
    public class GiveawayViewModel {
        public bool PermitStuff { get; set; } = true;
        public bool PermitAdmin { get; set; } = true;
        public bool PermitMods { get; set; } = true;
        public bool PermitViewers { get; set; } = true;

        public bool All { get; set; } = true;
        public bool WithActiveTime { get; set; }
        public bool WithKeyword { get; set; }

        public int Currency { get; set; }
        public int SubscribedTime { get; set; }
        public int SubscriberLuck { get; set; }

        public bool UncheckWinner { get; set; } = true;
        public bool AnnounceWinner { get; set; } = true;

        public bool IsCaseSensitive { get; set; }
        public bool IsKeywordAntispamActive { get; set; }
        
        public string ForbiddenWords { get; set; }

        public string Keyword { get; set; }

        public GiveawayViewModel() {

        }
    }
}
