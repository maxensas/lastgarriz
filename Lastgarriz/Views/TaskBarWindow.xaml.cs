using Lastgarriz.Util.Interop;
using Lastgarriz.ViewModels;
using System;
using System.Windows;
using System.Windows.Interop;

namespace Lastgarriz.Views
{
    /// <summary>
    /// Interaction logic for TaskBarWindow.xaml
    /// </summary>
    public partial class TaskBarWindow : Window
    {
        public TaskBarViewModel ViewModel { get; private set; } = new();

        public TaskBarWindow()
        {
            InitializeComponent();

            DataContext = ViewModel;
            //Top = SystemParameters.PrimaryScreenHeight - 60;
            //Left = SystemParameters.PrimaryScreenWidth - 60;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var hwnd = new WindowInteropHelper(this).Handle;
            NativeWin.SetWindowExTransparent(hwnd);
        }
    }
}
