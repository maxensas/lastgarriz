using Run.Util;
using Run.Util.Hook;
using Run.Util.Interop;
using Run.Views;
using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Run.ViewModels.Command
{
    public sealed class MainCommand
    {
        private static MainViewModel Vm { get; set; }

        private readonly DelegateCommand closeWindow;
        private readonly DelegateCommand closedWindow;
        private readonly DelegateCommand openAbout;
        private readonly DelegateCommand checkVersion;
        private readonly DelegateCommand openSettings;

        public ICommand CloseWindow => closeWindow;
        public ICommand ClosedWindow => closedWindow;
        public ICommand OpenAbout => openAbout;
        public ICommand CheckVersion => checkVersion;
        public ICommand OpenSettings => openSettings;

        public MainCommand(MainViewModel vm)
        {
            Vm = vm;
            closeWindow = new(OnCloseWindow, CanCloseWindow);
            closedWindow = new(OnClosedWindow, CanClosedWindow);
            openAbout = new(OnOpenAbout, CanOpenAbout);
            checkVersion = new(OnCheckVersion, CanCheckVersion);
            openSettings = new(OnOpenSettings, CanOpenSettings);
        }

        private static bool CanCloseWindow(object commandParameter)
        {
            return true;
        }

        private static void OnCloseWindow(object commandParameter)
        {
            static void DoWork()
            {
                if (System.Windows.Application.Current.MainWindow is not null)
                {
                    System.Windows.Application.Current.MainWindow.IsEnabled = false;
                    System.Windows.Application.Current.MainWindow.Close();
                    GC.Collect(); // find finalizable objects
                    GC.WaitForPendingFinalizers(); // wait until finalizers executed
                    GC.Collect(); // collect finalized objects
                }
            }

            if (commandParameter is string)
            {
                if ((commandParameter as string) == "terminate")
                {
                    Global.Terminate = true;
                }
            }

            if (System.Windows.Application.Current.Dispatcher.CheckAccess())
            {
                DoWork();
            }
            else
            {
                System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { DoWork(); }));
            }
        }

        private static bool CanClosedWindow(object commandParameter)
        {
            return true;
        }

        private static void OnClosedWindow(object commandParameter)
        {
            if (Global.DataJson.Config != null && Global.Terminate)
            {
                if (Global.IsHotKey) HotKey.RemoveRegisterHotKey(false);
            }
        }

        private static bool CanOpenAbout(object commandParameter)
        {
            return true;
        }

        private static void OnOpenAbout(object commandParameter)
        {
            StringBuilder message = new("Version: ");
            message.Append(Common.GetFileVersion()).AppendLine().AppendLine()
                .Append("Lastgarriz is a free tool that add some quality of life when playing HELL LET LOOSE.").AppendLine().AppendLine()
                .Append("Some features work as overlay, please consider reading 'Black Matter' Term of uses first before using this software.").AppendLine().AppendLine()
                .Append("This software is open source and developed for free.").AppendLine().AppendLine()
                .Append("If you like the program and want more features, you can contribute with pull requests or new issues describing your fresh ideas.");
            MessageBox.Show(System.Windows.Application.Current.MainWindow, message.ToString(), "Lastgarriz by maxensas");
        }

        private static bool CanCheckVersion(object commandParameter)
        {
            return true;
        }

        private static void OnCheckVersion(object commandParameter)
        {
            TaskManager.CheckUpdateTask();

            //TODO
            if (!Global.FirstCheckUpdate)
            {
                //AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
                Global.FirstCheckUpdate = true;
            }
            //AutoUpdater.Start(Strings.VersionUrl);
        }

        private static bool CanOpenSettings(object commandParameter)
        {
            return true;
        }

        private static void OnOpenSettings(object commandParameter)
        {
            IntPtr pHwnd = NativeWin.FindWindow(null, Strings.View.CONFIGURATION);
            if (pHwnd.ToInt32() > 0)
            {
                NativeWin.SendMessage(pHwnd, NativeWin.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
            System.Windows.Application.Current.MainWindow.IsEnabled = false;
            System.Windows.Application.Current.MainWindow.Close();
            ConfigWindow configWin = new();
            configWin.Name = Strings.View.CONFIGURATION;
            configWin.Show();
            configWin.Visibility = Visibility.Visible;
        }
    }
}
