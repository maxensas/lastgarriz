using Lastgarriz.ViewModels;
using System;
using System.Windows;

namespace Lastgarriz.Views
{
    /// <summary>
    /// Logique d'interaction pour RocketWindow.xaml
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
        /*
        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
           IntPtr findHwnd = Native.FindWindow(Strings.HllClass, Strings.HllCaption);
           bool hllLaunched = findHwnd.ToInt32() > 0;
           if (hllLaunched)
           {
               Native.BringWindowToTop(findHwnd);
           }
        }*/
        protected override void OnClosed(EventArgs e)
        {
            //cross.Children.Clear();

            this.Content = null;
            DataContext = null;
            this.ViewModel = null;
            

            base.OnClosed(e);

            //Native.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
            /*
            if (chkCleanup.IsChecked == true)
            {
                BindingOperations.ClearBinding(leak, TextBlock.TextProperty);
            }
            this.ClearValue(Canvas.prop);*/
        }
    }
}
