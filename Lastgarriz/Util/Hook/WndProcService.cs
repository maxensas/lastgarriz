using Run.Models.Serializable;
using Run.Util.Interop;
using Run.Views;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Threading;

namespace Run.Util.Hook
{
    /// <summary>
    /// Interaction logic for WndProcService.cs
    /// </summary>
    /// <remarks>This class can only be instancied once (singleton)</remarks>
    internal sealed class WndProcService //: IDisposable
    {
        private static WndProcService instance = null;
        private static readonly object Instancelock = new();

        private SpongeWindow Sponge { get; set; } = new();
        private MainWindow Main { get; set; } = (MainWindow)System.Windows.Application.Current.MainWindow;
        private TaskBarWindow TaskBar { get; set; } = new();
        private DispatcherTimer TimerRegister { get; set; }

        internal WndProcService()
        {
            Sponge.WndProcCalled += (s, e) => ProcessMessage(e);
            Global.HookHwnd = Sponge.Handle;

            TimerRegister = new(DispatcherPriority.Normal);
            TimerRegister.Interval = TimeSpan.FromMilliseconds(100);
            TimerRegister.Tick += new EventHandler(RegisterTimer_Tick);
                
            HotKey.InstallRegisterHotKey();
            TimerRegister.Start();

            TaskBar.Name = Strings.View.TASKBAR;
        }

        internal static WndProcService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (Instancelock)
                    {
                        if (instance == null)
                        {
                            instance = new WndProcService();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// This method is called by a DispatcherTimer each 100ms throughout application execution.
        /// </summary>
        private void RegisterTimer_Tick(object sender, EventArgs e) // WHEN USING SHORTCUTS : ENDLESS RUNNING PROCESS
        {
            IntPtr hllHwnd = NativeWin.FindWindow(Strings.HllClass, Strings.HllCaption);
            if (NativeWin.GetForegroundWindow().Equals(hllHwnd)) // IF you have HLL game window in focus
            {
                if (!Global.IsHotKey) HotKey.InstallRegisterHotKey();

                if (!Global.TaskBarActive)
                {
                    TaskBar.Show();
                    Global.TaskBarActive = true;
                }
                return;
            }
            if (Global.TaskBarActive)
            {
                TaskBar.Hide();
                Global.TaskBarActive = false;
            }

            if (Global.IsHotKey) HotKey.RemoveRegisterHotKey(false);

            if (hllHwnd.ToInt32() > 0) // If HLL is launched
            {
                IntPtr pHwndCfg = NativeWin.FindWindow(null, Strings.View.CONFIGURATION);
                if (pHwndCfg.ToInt32() > 0)
                {
                    HotKey.RemoveRegisterHotKey(true);
                    return;
                }
            }

            // TODO : TaskBar icons should only be managed in this method

            //TaskBar.ViewModel.Map = TaskManager.MapHistoryTask?.Status is TaskStatus.Running;
            //TaskBar.ViewModel.Artillery = TaskManager.KeystrokeCatcherTask?.Status is TaskStatus.Running;
        }

        /// <summary>
        /// This method is called each time a registered hotkey is pressed.
        /// </summary>
        private void ProcessMessage(Message message)
        {
            // Here we process incoming messages
            if (!Global.HotkeyProcBlock && message.Msg == NativeWin.WM_HOTKEY)
            {
                Global.HotkeyProcBlock = true;
                IntPtr findHwnd = NativeWin.FindWindow(Strings.HllClass, Strings.HllCaption);
                bool hllLaunched = findHwnd.ToInt32() > 0;

                int keyIdx = message.WParam.ToInt32();
                ConfigShortcut shortcut = Global.DataJson.Config.Shortcuts[keyIdx - 10001];

                if (shortcut != null && shortcut.Value != null)
                {
                    //string value = shortcut.Value;
                    string fonctionLower = shortcut.Fonction.ToLowerInvariant();
                    try
                    {
                        if (NativeWin.GetForegroundWindow().Equals(findHwnd) || DataManager.Instance.Config.Options.DevMode) // Only if HLL got the focus or in developer mode
                        {
                            if (fonctionLower is Strings.Feature.ARTILLERY_USGER or Strings.Feature.ARTILLERY_RU)
                            {
                                bool rusianMetrics = fonctionLower is Strings.Feature.ARTILLERY_RU;
                                IntPtr pHwnd = NativeWin.FindWindow(null, Strings.View.ARTILLERY);
                                if (pHwnd.ToInt32() > 0)
                                {
                                    NativeWin.SendMessage(pHwnd, NativeWin.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                                    TaskBar.ViewModel.ArtilleryUssr = TaskBar.ViewModel.ArtilleryUsGer = false;
                                }
                                else
                                {
                                    Main?.Close(); // close mainWindow
                                    ArtilleryWindow artiWin = new(rusianMetrics)
                                    {
                                        Name = Strings.View.ARTILLERY
                                    };
                                    artiWin.Show();
                                    artiWin.Visibility = Visibility.Visible;
                                    TaskBar.ViewModel.ArtilleryUssr = rusianMetrics;
                                    TaskBar.ViewModel.ArtilleryUsGer = !rusianMetrics;
                                    if (hllLaunched)
                                    {
                                        NativeWin.BringWindowToTop(findHwnd);
                                    }
                                }
                            }

                            if (fonctionLower is Strings.Feature.ROCKETINDICATOR_GER or Strings.Feature.ROCKETINDICATOR_US)
                            {
                                IntPtr pHwnd = NativeWin.FindWindow(null, Strings.View.ROCKET);
                                if (pHwnd.ToInt32() > 0)
                                {
                                    HwndSource hwndSource = HwndSource.FromHwnd(pHwnd);
                                    if (hwndSource.RootVisual is RocketWindow rocketWin)
                                    {
                                        //TaskManager.EndMouseCatcherTask(rocketWin.ViewModel);
                                        TaskManager.EndRocketCrossairTask(rocketWin.ViewModel);
                                        TaskBar.ViewModel.Panzerschreck = TaskBar.ViewModel.Bazooka = false;
                                    }
                                }
                                else
                                {
                                    Main?.Close(); // close mainWindow
                                    bool isSchreck = fonctionLower is Strings.Feature.ROCKETINDICATOR_GER;
                                    RocketWindow rocketWin = new(isSchreck)
                                    {
                                        Name = Strings.View.ROCKET
                                    };
                                    rocketWin.Show();
                                    rocketWin.Visibility = Visibility.Visible;
                                    rocketWin.ViewModel.ShowWindow = true;
                                    //TaskManager.StartMouseCatcherTask(rocketWin.ViewModel);
                                    TaskManager.StartRocketCrossairTask(rocketWin.ViewModel);
                                    TaskBar.ViewModel.Panzerschreck = isSchreck;
                                    TaskBar.ViewModel.Bazooka = !isSchreck;
                                    if (hllLaunched)
                                    {
                                        NativeWin.BringWindowToTop(findHwnd);
                                    }
                                }
                            }

                            if (fonctionLower is Strings.Feature.MAP_RECORD)
                            {
                                if(TaskManager.MapHistoryTask?.Status is TaskStatus.Running)
                                {
                                    TaskManager.StopMapHistoryTask();
                                    TaskBar.ViewModel.Map = false;
                                }
                                else
                                {
                                    string mapFolderName = DateTime.Now.ToString("yyMMdd-HH-mm");
                                    TaskManager.StartMapHistoryTask(mapFolderName);
                                    TaskBar.ViewModel.Map = true;
                                }
                            }

                            if (fonctionLower is Strings.Feature.AUTOQUEUE)
                            {
                                if (TaskManager.ExtraQueueTask?.Status is TaskStatus.Running)
                                {
                                    TaskManager.StopExtraQueueTask();
                                    TaskBar.ViewModel.Queue = false;
                                }
                                else
                                {
                                    TaskManager.StartExtraQueueTask();
                                    TaskBar.ViewModel.Queue = true;
                                }
                            }
                        }

                        // In all case
                        /*
                        if (fonctionLower is Strings.Feature.CLOSE)
                        {
                            IntPtr pHwnd = Native.FindWindow(null, Strings.View.CONFIGURATION);

                            if (Main?.Visibility == Visibility.Hidden && pHwnd.ToInt32() == 0)
                            {
                                Native.SendMessage(findHwnd, Native.WM_KEYUP, new IntPtr(shortcut.Keycode), IntPtr.Zero);
                            }
                            else
                            {
                                if (pHwnd.ToInt32() != 0)
                                    Native.SendMessage(pHwnd, Native.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

                                if (Main?.Visibility == Visibility.Visible)
                                    Main?.Close();
                            }
                        }
                        */
                        if (fonctionLower is Strings.Feature.CONFIG)
                        {
                            IntPtr pHwnd = NativeWin.FindWindow(null, Strings.View.CONFIGURATION);
                            if (pHwnd.ToInt32() > 0)
                            {
                                NativeWin.SendMessage(pHwnd, NativeWin.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                            }
                            Main?.Close(); // close mainWindow
                            ConfigWindow configWin = new()
                            {
                                Name = Strings.View.CONFIGURATION
                            };
                            configWin.Show();
                            configWin.Visibility = Visibility.Visible;
                        }
                    }
                    catch (ExternalException ex)
                    {
                        WindowMessage.SendForeground(String.Format("{0} Error:  {1}\r\n\r\n{2}\r\n\r\n", ex.Source, ex.Message, ex.StackTrace), "Clipboard access error in WndProc", MessageBoxButton.OK, MessageBoxImage.Error);
                        //ForegroundMessage("Invalid shortcut command.", "Shortcut Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    {
                        WindowMessage.SendForeground(String.Format("{0} Error:  {1}\r\n\r\n{2}\r\n\r\n", ex.Source, ex.Message, ex.StackTrace), "Main commands error", MessageBoxButton.OK, MessageBoxImage.Error);
                        //ForegroundMessage("Invalid shortcut command.", "Shortcut Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                Global.HotkeyProcBlock = false;
            }
        }

        /*
        public void Dispose()
        {
            // to implement
            
            //_sponge.Dispose();
        }
        */
    }
}
