using Lastgarriz.Util;
using Lastgarriz.Util.Hook;
using Lastgarriz.Util.Interop;
using Lastgarriz.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Lastgarriz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; private set; } = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = ViewModel;

            Global.InitGlobals(/*ViewModel*/);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Native.GetForegroundWindow().Equals(Global.MainHwnd))
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

        private void Window_Closed(object sender, EventArgs e)
        {
            if (Global.DataJson.Config != null && Global.Terminate)
            {
                if (Global.IsHotKey) HotKey.RemoveRegisterHotKey(false);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !Global.Terminate;

            Keyboard.ClearFocus();
            this.Visibility = Visibility.Hidden;
        }

        private void Tray_Mouseclick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ContextMenu menu = (ContextMenu)this.FindResource("NotifierContextMenu");

            if (menu.IsOpen)
            {
                menu.IsOpen = false;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right || e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                menu.IsOpen = true;
            }
        }
    }
}
