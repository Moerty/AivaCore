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
    /// Interaktionslogik für BankSettingUserControl.xaml
    /// </summary>
    public partial class BankSettingUserControl : MetroContentControl {
        public BankSettingUserControl() {
            InitializeComponent();
        }



        public int MinUserForBank {
            get { return (int)GetValue(MinUserForBankProperty); }
            set { SetValue(MinUserForBankProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinUserForBank.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinUserForBankProperty =
            DependencyProperty.Register("MinUserForBank", typeof(int), typeof(BankSettingUserControl), new PropertyMetadata(0));



        public int SuccessRateForBank {
            get { return (int)GetValue(SuccessRateForBankProperty); }
            set { SetValue(SuccessRateForBankProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SuccessRateForBank.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SuccessRateForBankProperty =
            DependencyProperty.Register("SuccessRateForBank", typeof(int), typeof(BankSettingUserControl), new PropertyMetadata(0));



        public double WinningMultiplierForBank {
            get { return (double)GetValue(WinningMultiplierForBankProperty); }
            set { SetValue(WinningMultiplierForBankProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WinningMultiplierForBank.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WinningMultiplierForBankProperty =
            DependencyProperty.Register("WinningMultiplierForBank", typeof(double), typeof(BankSettingUserControl), new PropertyMetadata(0.0));



        public string Bank {
            get { return (string)GetValue(BankProperty); }
            set { SetValue(BankProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Bank.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BankProperty =
            DependencyProperty.Register("Bank", typeof(string), typeof(BankSettingUserControl), new PropertyMetadata(""));


    }
}
