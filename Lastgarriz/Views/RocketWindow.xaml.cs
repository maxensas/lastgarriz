using Lastgarriz.Util;
using Lastgarriz.Util.Interop;
using Lastgarriz.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lastgarriz.Views
{
    /// <summary>
    /// Logique d'interaction pour RocketWindow.xaml
    /// </summary>
    public partial class RocketWindow : Window
    {
        public RocketViewModel ViewModel { get; private set; } = new();

        public RocketWindow()
        {
            InitializeComponent();

            DataContext = ViewModel;
            
            Left = (SystemParameters.PrimaryScreenWidth - Width) / 2; 
            Top = (SystemParameters.PrimaryScreenHeight - Height) / 2;//1.8
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            //IsEnabled = true;
            /*
            IntPtr findHwnd = Native.FindWindow(Strings.HllClass, Strings.HllCaption);
            bool hllLaunched = findHwnd.ToInt32() > 0;
            if (hllLaunched)
            {
                Native.BringWindowToTop(findHwnd);
            }*/
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
    }
}
