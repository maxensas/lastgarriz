using Lastgarriz.Models.Serializable;
using Lastgarriz.Util.Interop;
using Lastgarriz.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Lastgarriz.Util.Hook
{
    internal sealed class WndProcService //: IDisposable
    {
        private static WndProcService instance = null;
        private static readonly object Instancelock = new();

        private SpongeWindow Sponge { get; set; } = new();
        private MainWindow Main { get; set; } = (MainWindow)System.Windows.Application.Current.MainWindow;
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
        }

        internal static WndProcService GetInstance()
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

        private void RegisterTimer_Tick(object sender, EventArgs e) // WHEN USING SHORTCUTS : ENDLESS RUNNING PROCESS
        {
            
            if (Native.GetForegroundWindow().Equals(Native.FindWindow(Strings.HllClass, Strings.HllCaption))) // IF you have HLL game window in focus
            {
                if (!Global.IsHotKey) HotKey.InstallRegisterHotKey();
            }
            else
            {
                if (Global.IsHotKey) HotKey.RemoveRegisterHotKey(false);
            }
            
        }

        private void ProcessMessage(Message message)
        {
            // Here we process incoming messages
            if (!Global.HotkeyProcBlock && message.Msg == Native.WM_HOTKEY)
            {
                Global.HotkeyProcBlock = true;
                IntPtr findHwnd = Native.FindWindow(Strings.HllClass, Strings.HllCaption);
                bool hllLaunched = findHwnd.ToInt32() > 0;

                int keyIdx = message.WParam.ToInt32();
                ConfigShortcut shortcut = Global.DataJson.Config.Shortcuts[keyIdx - 10001];

                if (shortcut != null && shortcut.Value != null)
                {
                    //string value = shortcut.Value;
                    string fonctionLower = shortcut.Fonction.ToLowerInvariant();
                    try
                    {
                        if (Native.GetForegroundWindow().Equals(findHwnd) || DataManager.GetInstance().Config.Options.DevMode) // Only if HLL got the focus or in developer mode
                        {
                            if (fonctionLower is Strings.Feature.ARTILLERY_USGER or Strings.Feature.ARTILLERY_RU)
                            {
                                bool rusianMetrics = fonctionLower is Strings.Feature.ARTILLERY_RU;
                                IntPtr pHwnd = Native.FindWindow(null, Strings.View.ARTILLERY);
                                if (pHwnd.ToInt32() > 0)
                                {
                                    Native.SendMessage(pHwnd, Native.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                                }
                                else
                                {
                                    Main?.Close(); // close mainWindow
                                    ArtilleryWindow artiWin = new(rusianMetrics);
                                    artiWin.Name = Strings.View.ARTILLERY;
                                    artiWin.Show();
                                    artiWin.Visibility = Visibility.Visible;
                                    if (hllLaunched)
                                    {
                                        Native.BringWindowToTop(findHwnd);
                                    }
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
                            IntPtr pHwnd = Native.FindWindow(null, Strings.View.CONFIGURATION);
                            if (pHwnd.ToInt32() > 0)
                            {
                                Native.SendMessage(pHwnd, Native.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                            }
                            Main?.Close(); // close mainWindow
                            ConfigWindow configWin = new();
                            configWin.Name = Strings.View.CONFIGURATION;
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
