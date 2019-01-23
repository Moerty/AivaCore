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

namespace Aiva.Gui.Views.UserControls {
    /// <summary>
    /// Interaktionslogik für StreamGameUserControl.xaml
    /// </summary>
    public partial class StreamGameUserControl : MetroContentControl {
        public StreamGameUserControl() {
            InitializeComponent();
        }



        public bool IsActive {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsActive.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(StreamGameUserControl), new PropertyMetadata(false));



        public ICommand ShowSettingsCommand {
            get { return (ICommand)GetValue(ShowSettingsCommandProperty); }
            set { SetValue(ShowSettingsCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowSettingsCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowSettingsCommandProperty =
            DependencyProperty.Register("ShowSettingsCommand", typeof(ICommand), typeof(StreamGameUserControl), new PropertyMetadata(default(ICommand)));


    }
}
