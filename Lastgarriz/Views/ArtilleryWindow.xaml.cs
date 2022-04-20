using Lastgarriz.Util;
using Lastgarriz.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace Lastgarriz.Views
{
    /// <summary>
    /// Logique d'interaction pour ArtilleryWindow.xaml
    /// </summary>
    public partial class ArtilleryWindow : Window
    {
        public ArtilleryViewModel ViewModel { get; private set; }

        public ArtilleryWindow(bool rusianMetrics)
        {
            InitializeComponent();

            ViewModel = new(rusianMetrics);
            DataContext = ViewModel;
            /*
            Left = SystemParameters.PrimaryScreenWidth / 1.215; // 1920p : 1580
            Top = SystemParameters.PrimaryScreenHeight / 1.195; // 1080p : 904
            */
            /*
            Left = (SystemParameters.PrimaryScreenWidth + Width) / 1.2975; // 1920p : 1579.96
            Top = (SystemParameters.PrimaryScreenHeight + Height) / 1.261; // 1080p : 904.044
            */

            Left = (SystemParameters.PrimaryScreenWidth - Width) / 1.133; // 1920p : 1579.87
            Top = (SystemParameters.PrimaryScreenHeight - Height) / 1.128; // 1080p : 904.255
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            IsEnabled = true;
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            /*
            var top = Top;
            var left = Left;
            */
        }
    }
}
