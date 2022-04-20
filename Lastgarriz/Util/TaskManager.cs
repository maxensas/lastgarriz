using Lastgarriz.Util.Hook;
using Lastgarriz.Util.Interop;
using Lastgarriz.ViewModels;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Lastgarriz.Util
{
    internal static class TaskManager // light, to redo
    {
        internal static Task UpdateCheckerTask { get; private set; } // only last task launched
        internal static Task KeystrokeCatcherTask { get; private set; } // only last task launched
        internal static Task MouseCatcherTask { get; private set; } // only last task launched
        internal static Task CloseMouseCatcherTask { get; private set; } // only last task launched
        internal static CancellationTokenSource TokenSourceKeyCatcher { get; private set; }
        internal static CancellationTokenSource TokenSourceMouseCatcher { get; private set; }

        internal static void StartKeystrokeCatcherTask(ArtilleryViewModel vm)
        {
            StopKeyCatcherTask();
            TokenSourceKeyCatcher = new();

            KeystrokeCatcherTask = Task.Run(() =>
            {
                try
                {
                    Thread.Sleep(200);
                    bool blockNum = false, blockVal = false;
                    do
                    {
                        Thread.Sleep(40);
                        bool doBreak = false;

                        foreach (var key in HotKey.GetFeatureKeys(Strings.Feature.ARTILLERY_VALIDATE))
                        {
                            if (Common.IsKeyPushedDown(key))
                            {
                                if (!blockVal)
                                {
                                    if (vm.Meter < vm.Metrics.MeterMinValue && vm.Meter != 0)
                                    {
                                        vm.Meter = vm.Metrics.MeterMinValue;
                                    }

                                    if (vm.Milliradian == 0 && vm.Meter > 0)
                                    {
                                        vm.Milliradian = Convert.ToInt32(vm.Metrics.MilliradianBase - (vm.Meter * vm.Metrics.MeterRatio));
                                    }
                                }
                                doBreak = true;
                                break;
                            }
                        }
                        blockVal = doBreak;

                        doBreak = false;
                        foreach (var key in Global.NumericKeyList)
                        {
                            if (Common.IsKeyPushedDown(key.Key))
                            {
                                if (!blockNum)
                                {
                                    if (vm.Milliradian > 0)
                                    {
                                        vm.Meter = vm.Milliradian = 0;
                                    }

                                    if (vm.Meter == 0)
                                    {
                                        vm.Meter = key.Value;
                                    }
                                    else if (vm.Meter < vm.Metrics.MeterMaxValue)
                                    {
                                        vm.Meter = (vm.Meter * 10) + key.Value;
                                    }

                                    if (vm.Meter > vm.Metrics.MeterMaxValue)
                                    {
                                        vm.Meter = vm.Metrics.MeterMaxValue;
                                    }
                                }
                                doBreak = true;
                            }
                            if (doBreak) break;
                        }
                        blockNum = doBreak;
                    } while (!TokenSourceKeyCatcher.Token.IsCancellationRequested);
                }
                catch (Exception ex)
                {
                    WindowMessage.SendForeground(String.Format("{0} Error:  {1}\r\n\r\n{2}\r\n\r\n", ex.Source, ex.Message, ex.StackTrace), "Keystroke catcher task encountered an error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            , TokenSourceKeyCatcher.Token);
        }

        internal static void StartMouseCatcherTask(RocketViewModel vm) // mouse Y Axis
        {
            StopMouseCatcherTask();
            TokenSourceMouseCatcher = new();

            MouseCatcherTask = Task.Run(() =>
            {
                try
                {
                    Thread.Sleep(200);
                    int interval = 5000; // 5s, add to globals
                    System.Timers.Timer MouseCatcherTimer = new(interval);
                    //MouseCatcherTimer.Elapsed += (sender, e) => OnMouseCatcherTimedEvent(sender, e, vm);
                    MouseCatcherTimer.AutoReset = false;
                    bool reInit = false;
                    do
                    {
                        if (Common.IsMiddleButtonMousePushedOnce() || reInit)
                        {
                            reInit = false;
                            Native.SetCursorPos(Global.HalfScreenWidth, Global.HalfScreenHeight);

                            MouseCatcherTimer.Start();
                            vm.ShowDisclaimer = false;
                            vm.ShowWindow = true;

                            int multiplier = 0;
                            int maxPixels = (int)SystemParameters.PrimaryScreenHeight - 1;
                            do
                            {
                                Native.GetCursorPos(out Native.POINT pos);
                                //Trace.WriteLine($"[Mouse position] X: {pos.X}  Y: {pos.Y}");

                                bool min = pos.Y == 0;
                                bool max = pos.Y == maxPixels;
                                if (min || max)
                                {
                                    if (min)
                                    {
                                        multiplier--;
                                    }
                                    if (max)
                                    {
                                        multiplier++;
                                    }
                                    Native.SetCursorPos(Global.HalfScreenWidth, Global.HalfScreenHeight);
                                    Native.GetCursorPos(out pos);
                                }

                                vm.Indicator = Common.ConvertCursorPositionToRocketIndicator(pos.Y, multiplier);

                                //Thread.Sleep(1);
                                if (Common.IsMiddleButtonMousePushedOnce())
                                {
                                    reInit = true;
                                }
                            } while (MouseCatcherTimer.Enabled && !TokenSourceMouseCatcher.Token.IsCancellationRequested && !reInit);
                            
                            if (!TokenSourceMouseCatcher.Token.IsCancellationRequested && !MouseCatcherTimer.Enabled)
                            {
                                vm.ShowWindow = false;
                            }
                            MouseCatcherTimer.Stop();
                        }
                        Thread.Sleep(100);
                    } while (!TokenSourceMouseCatcher.Token.IsCancellationRequested);
                }
                catch (Exception ex)
                {
                    WindowMessage.SendForeground(String.Format("{0} Error:  {1}\r\n\r\n{2}\r\n\r\n", ex.Source, ex.Message, ex.StackTrace), "Keystroke catcher task encountered an error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            , TokenSourceMouseCatcher.Token);

            // HideDisclaimerMouseCatcherTask
            _ = Task.Run(() =>
            {
                try
                {
                    if (vm.ShowDisclaimer)
                    {
                        Thread.Sleep(2000);
                        bool closeCalled = false;
                        if (CloseMouseCatcherTask?.Status is TaskStatus.Running)
                        {
                            closeCalled = true;
                        }
                        if (vm.ShowDisclaimer && !TokenSourceMouseCatcher.IsCancellationRequested && !closeCalled)
                        {
                            vm.ShowWindow = false;
                            vm.ShowDisclaimer = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    WindowMessage.SendForeground(String.Format("{0} Error:  {1}\r\n\r\n{2}\r\n\r\n", ex.Source, ex.Message, ex.StackTrace), "Error encountered in HideRocketDisclaimer method", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            );
        }

        internal static void EndMouseCatcherTask(RocketViewModel vm)
        {
            if (TokenSourceMouseCatcher is null || MouseCatcherTask is null)
            {
                return;
            }

            bool alreadyRunning = false;
            if (CloseMouseCatcherTask?.Status is TaskStatus.Running)
            {
                alreadyRunning = true;
            }

            if (!TokenSourceMouseCatcher.IsCancellationRequested && !alreadyRunning)
            {
                StopMouseCatcherTask();
                CloseMouseCatcherTask = Task.Run(async () =>
                {
                    try
                    {
                        await Task.WhenAll(MouseCatcherTask);

                        vm.ShowDisclaimer = true;
                        vm.DisclaimerText = "Rocket indicator desactivated";
                        vm.DisclaimerColor = Brushes.Red;
                        vm.ShowWindow = true;

                        Thread.Sleep(2000);

                        IntPtr pHwnd = Native.FindWindow(null, Strings.View.ROCKET);
                        if (pHwnd.ToInt32() > 0)
                        {
                            Native.SendMessage(pHwnd, Native.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                        }
                    }
                    catch (Exception ex)
                    {
                        WindowMessage.SendForeground(String.Format("{0} Error:  {1}\r\n\r\n{2}\r\n\r\n", ex.Source, ex.Message, ex.StackTrace), "Error encountered in CloseWindowAfterDelay method", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            );
            }
        }

        internal static void StopKeyCatcherTask()
        {
            TokenSourceKeyCatcher?.Cancel();
        }

        internal static void StopMouseCatcherTask()
        {
            TokenSourceMouseCatcher?.Cancel();
        }
        /*
        private static void OnMouseCatcherTimedEvent(object source, System.Timers.ElapsedEventArgs e, RocketViewModel vm)
        {

            Trace.WriteLine("The Elapsed event was raised at {0}", e.SignalTime.ToString());
        }
        */

        internal static void CheckUpdateTask()
        {
            UpdateCheckerTask = Task.Run(async () =>
            {
                try
                {
                    string webVersion = await Net.SendHTTP(null, Strings.VersionUrl, Client.Default);
                    webVersion = webVersion?.Replace("\n", string.Empty);

                    if (webVersion?.Length > 0)
                    {
                        Version version = new(Common.GetFileVersion());
                        bool isUpdates = version.CompareTo(new Version(webVersion)) < 0;

                        if (isUpdates)
                        {
                            static void DoWork()
                            {
                                MessageBoxResult result = MessageBox.Show(Application.Current.MainWindow,
                                    "A new version of Lastgarriz has been released, update now ?", "Lastgarriz by maxensas", MessageBoxButton.YesNo, MessageBoxImage.Question);

                                if (result == MessageBoxResult.Yes)
                                {
                                    string url = "https://github.com/maxensas/lastgarriz";
                                    Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
                                }
                            }

                            if (Application.Current.Dispatcher.CheckAccess())
                            {
                                DoWork();
                            }
                            else
                            {
                                await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                                new Action(() => { DoWork(); }));
                            }
                        }
                        else
                        {
                            WindowMessage.SendForeground("Application is up to date.", "Lastgarriz by maxensas", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    WindowMessage.SendForeground(String.Format("{0} Error:  {1}\r\n\r\n{2}\r\n\r\n", ex.Source, ex.Message, ex.StackTrace), "Update checker task encountered an error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            );
        }

        /*
        internal static void CheckPreviousTask(Task task)
        {
            bool runTask = task is null;
            if (!runTask)
            {
                runTask = task.IsCanceled;

                if (!runTask)
                {
                    throw new TaskCanceledException("A task cant be canceled.");
                }
            }
        }
        */
    }
}
