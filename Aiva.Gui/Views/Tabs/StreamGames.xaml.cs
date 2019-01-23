using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
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

namespace Aiva.Gui.Views.Tabs {
    public partial class StreamGames : MetroContentControl {
        private readonly ViewModels.Tabs.StreamGamesViewModel _viewModel;

        public StreamGames() {
            InitializeComponent();
            _viewModel = new ViewModels.Tabs.StreamGamesViewModel();
            this.DataContext = _viewModel;
        }
    }
}
