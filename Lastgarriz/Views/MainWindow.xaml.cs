using Run.Util;
using Run.Util.Interop;
using Run.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace Run
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new();
            DataContext = ViewModel;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (NativeWin.GetForegroundWindow().Equals(Global.MainHwnd))
            {
                try
                {
                    this.DragMove();//myWindow
                }
                catch (Exception ex)
                {
                    Util.Helper.Debug.Trace("Exception with Window.DragMove : " + ex.Message);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Global.MainHwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            if (Global.DataJson.Config.Options.DisableStartupMessage)
            {
                Close();
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !Global.Terminate;

            Keyboard.ClearFocus();
            this.Visibility = Visibility.Hidden;

            if (Global.Terminate)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }
    }
}
