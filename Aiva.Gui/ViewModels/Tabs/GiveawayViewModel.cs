using System;
using System.Collections.Generic;
using System.Text;
using PropertyChanged;
using Aiva.Models.Enums;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Aiva.Gui.ViewModels.Tabs {
    [AddINotifyPropertyChangedInterface]
    public class GiveawayViewModel {
        public ObservableCollection<TabItem> Tabs { get; set; }
            = new ObservableCollection<TabItem>();

        public TabItem SelectedTab { get; set; }


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
            SetTabsControl();
        }

        private void SetTabsControl() {
            foreach(var type in Enum.GetValues(typeof(GiveawayTypes)).Cast<GiveawayTypes>()) {
                Tabs.Add(new TabItem {
                    Header = type.ToString(),
                    Content = GetContentTabsControl(type)
                });
            }
        }

        private object GetContentTabsControl(GiveawayTypes type) {
            switch(type) {
                case Models.Enums.GiveawayTypes.Active:
                    return null;
                case Models.Enums.GiveawayTypes.All:
                    return new Views.Tabs.Giveaway.ActiveUserTabControll();
                case Models.Enums.GiveawayTypes.Keyword:
                    return null;
                case Models.Enums.GiveawayTypes.Random_Number:
                    return null;
            }

            return null;
        }
    }
}
