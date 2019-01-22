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
        public SetupWindow() {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e) {
            if(!string.IsNullOrEmpty(ClientId.Text) && !string.IsNullOrEmpty(Channel.Text) && !string.IsNullOrEmpty(Botname.Text)) {
                ConfirmValues?.Invoke(this, new Models.Gui.Windows.ConfirmSetupModel { ClientId = ClientId.Text, Channel = Channel.Text, Botname = Botname.Text });
            }
        }

        public EventHandler<Models.Gui.Windows.ConfirmSetupModel> ConfirmValues;
    }
}
