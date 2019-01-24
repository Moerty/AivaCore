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

namespace Aiva.Gui.Views.ChildWindows {
    /// <summary>
    /// Interaktionslogik für BankheistSettings.xaml
    /// </summary>
    public partial class BankheistSettings : MahApps.Metro.SimpleChildWindow.ChildWindow {
        private readonly ViewModels.ChildWindows.BankheistSettingsViewModel _viewModel;
        public BankheistSettings() {
            InitializeComponent();
            _viewModel = new ViewModels.ChildWindows.BankheistSettingsViewModel(Core.ConfigHandler.Config.StreamGames.Bankheist.General.Active);
            this.DataContext = _viewModel;
        }

        private void ChildWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            _viewModel.SafePropertiesToConfig();
        }
    }
}
