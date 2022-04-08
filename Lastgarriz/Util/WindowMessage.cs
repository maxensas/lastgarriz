using System;
using System.Windows;
using System.Windows.Threading;

namespace Lastgarriz.Util
{
    internal static class WindowMessage
    {
        internal static void SendForeground(string message, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            void DoWork()
            {
                MessageBox.Show(Application.Current.MainWindow, message, caption, button, icon);
            }
            //Application.Current.Dispatcher.Thread == Thread.CurrentThread
            if (Application.Current.Dispatcher.CheckAccess())
            {
                DoWork();
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                new Action(() => { DoWork(); }));
            }
        }
    }
}
