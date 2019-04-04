using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Aiva.Gui.Views.Tabs.Giveaway {
    /// <summary>
    /// Interaktionslogik für Giveaway.xaml
    /// </summary>
    public partial class MainVM : MetroContentControl {
        public MainVM() {
            InitializeComponent();
            this.DataContext = new ViewModels.Tabs.GiveawayViewModel();
        }
    }
}
