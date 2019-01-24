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

namespace Aiva.Gui.Views.Windows {
    /// <summary>
    /// Interaktionslogik für SetupWindow.xaml
    /// </summary>
    public partial class SetupWindow : MetroWindow {
        private readonly ViewModels.Windows.SetupWindowViewModel _vm;
        public SetupWindow() {
            InitializeComponent();

            _vm = new ViewModels.Windows.SetupWindowViewModel();
            _vm.ShowMessageBox
                += (s, e)
                => MessageBox.Show(e);

            _vm.CloseSetupPage
                += (s, e)
                => this.Close();

            this.DataContext = _vm;

            MessageBox.Show("Login in Twitch with your Botaccount");
        }
    }
}
