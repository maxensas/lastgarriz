using Lastgarriz.ViewModels;
using System;
using System.Windows;

namespace Lastgarriz.Views
{
    /// <summary>
    /// Interaction logic for RocketWindow.xaml
    /// </summary>
    public partial class RocketWindow : Window
    {
        public RocketViewModel ViewModel { get; private set; }

        public RocketWindow(bool isSchreck)
        {
            InitializeComponent();

            ViewModel = new(isSchreck);
            DataContext = ViewModel;
            
            Left = (SystemParameters.PrimaryScreenWidth - Width) / 2; 
            Top = (SystemParameters.PrimaryScreenHeight - Height) / 2;//1.8
        }

        protected override void OnClosed(EventArgs e)
        {
            this.Content = null;
            DataContext = null;
            this.ViewModel = null;
            
            base.OnClosed(e);
        }
    }
}
