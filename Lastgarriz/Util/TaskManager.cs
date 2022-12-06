using Lastgarriz.Util.Hook;
using Lastgarriz.Util.Interop;
using Lastgarriz.ViewModels;
using Lastgarriz.Views;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace Lastgarriz.Util
{
    internal static class TaskManager // more elegant please
    {
        internal static Task UpdateCheckerTask { get; private set; } // only last task launched

        internal static Task KeystrokeCatcherTask { get; private set; } // only last task launched
        internal static CancellationTokenSource TokenSourceKeyCatcher { get; private set; }

        internal static Task RocketCrossairTask { get; private set; } // only last task launched
        internal static Task CloseRocketCrossairTask { get; private set; } // only last task launched
        internal static CancellationTokenSource TokenSourceRocketCrossair { get; private set; }

        internal static Task MapHistoryTask { get; private set; } // only last task launched
        internal static CancellationTokenSource TokenSourceMapHistory { get; private set; }

        internal static Task ExtraQueueTask { get; private set; } // only last task launched
        internal static CancellationTokenSource TokenSourceExtraQueue { get; private set; }

        // NOT USED ANYMORE
        internal static Task MouseCatcherTask { get; private set; } // only last task launched
        internal static Task CloseMouseCatcherTask { get; private set; } // only last task launched
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

        internal static void StopKeyCatcherTask()
        {
            TokenSourceKeyCatcher?.Cancel();
        }

        internal static void StartMouseCatcherTask(RocketViewModel vm) // mouse Y Axis -- // NOT USED ANYMORE
        {
            StopMouseCatcherTask();
            TokenSourceMouseCatcher = new();

            MouseCatcherTask = Task.Run(() =>
            {
                try
                {
                    Thread.Sleep(200);
                    System.Timers.Timer MouseCatcherTimer = new(Global.INDICATOR_TIMER);
                    //MouseCatcherTimer.Elapsed += (sender, e) => OnMouseCatcherTimedEvent(sender, e, vm);
                    MouseCatcherTimer.AutoReset = false;
                    bool reInit = false;
                    int heightMultiplier = 0, widthMultiplier = 0;
                    int maxHeightPixels = (int)SystemParameters.PrimaryScreenHeight - 1;
                    int maxWidthPixels = (int)SystemParameters.PrimaryScreenWidth - 1;
                    bool heightMin, heightMax, widthMin, widthMax;
                    NativeWin.POINT pos;

                    do
                    {
                        if (Common.IsMiddleButtonMousePushedOnce() || reInit)
                        {
                            reInit = false;
                            NativeWin.SetCursorPos(Global.HalfScreenWidth, Global.HalfScreenHeight);

                            MouseCatcherTimer.Start();
                            vm.ShowDisclaimer = false;
                            vm.ShowWindow = true;
                            //vm.RocketZook = Common.GetRocketKind();

                            heightMultiplier = 0;
                            widthMultiplier = 0;
                            
                            do // around 360 iterations with 1ms sleep and 1 100 000 without.
                            {
                                Thread.Sleep(1); // Use 3 % processor with 1ms sleep and 26% without
                                NativeWin.GetCursorPos(out pos);
                                //Trace.WriteLine($"[Mouse position] X: {pos.X}  Y: {pos.Y}");

                                heightMin = pos.Y == 0;
                                heightMax = pos.Y == maxHeightPixels;
                                widthMin = pos.X == 0;
                                widthMax = pos.X == maxWidthPixels;

                                if (heightMin || heightMax || widthMin || widthMax)
                                {
                                    if (heightMin) heightMultiplier--;
                                    if (heightMax) heightMultiplier++;
                                    if (widthMin) widthMultiplier--;
                                    if (widthMax) widthMultiplier++;

                                    NativeWin.SetCursorPos((widthMin || widthMax) ? Global.HalfScreenWidth : pos.X, 
                                        (heightMin || heightMax) ? Global.HalfScreenHeight : pos.Y);
                                    NativeWin.GetCursorPos(out pos);
                                }
                                
                                vm.Indicator = Common.ConvertHeightCursorPosition(pos.Y, heightMultiplier);
                                vm.Kind = Common.GetRocketIndicatorKind();
                                
                                //vm.Xhair = Common.UpdateCrosshairAbscissa(pos.X, widthMultiplier); // memory leak : canvas line WPF object (stack issue)
                                //vm.CrosshairColor = (vm.Xhair < -8 || vm.Xhair > 8) ? Brushes.Red : Brushes.Lime;

                                Global.CrossUpdates++;

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

        internal static void EndMouseCatcherTask(RocketViewModel vm) // NOT USED ANYMORE
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
                        vm.DisclaimerText = vm.RocketZook ? Strings.Aim.DISCLAIMER_PANZERSCHRECK_OFF : Strings.Aim.DISCLAIMER_BAZOOKA_OFF;
                        vm.DisclaimerColor = Brushes.Red;
                        vm.ShowWindow = true;
                        Global.CrossUpdates = 0;

                        Thread.Sleep(2000);

                        IntPtr pHwnd = NativeWin.FindWindow(null, Strings.View.ROCKET);
                        if (pHwnd.ToInt32() > 0)
                        {
                            NativeWin.SendMessage(pHwnd, NativeWin.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                        }
                        Thread.Sleep(1000);
                        
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();
                        /*
                        IntPtr pHandle = Native.GetCurrentProcess();
                        Native.SetProcessWorkingSetSize(pHwnd, -1, -1);*/
                    }
                    catch (Exception ex)
                    {
                        WindowMessage.SendForeground(String.Format("{0} Error:  {1}\r\n\r\n{2}\r\n\r\n", ex.Source, ex.Message, ex.StackTrace), "Error encountered in CloseWindowAfterDelay method", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            );
            }
        }

        internal static void StopMouseCatcherTask()
        {
            TokenSourceMouseCatcher?.Cancel();
        }

        internal static void StartRocketCrossairTask(RocketViewModel vm)
        {
            StopRocketCrossairTask();
            TokenSourceRocketCrossair = new();

            RocketCrossairTask = Task.Run(() =>
            {
                try
                {
                    Thread.Sleep(200);
                    System.Timers.Timer RocketCrossairTimer = new(Global.INDICATOR_TIMER)
                    {
                        AutoReset = false
                    };
                    bool reInit = false;

                    do
                    {
                        var key = HotKey.GetFeatureKeys(Strings.Feature.ROCKETINDICATOR_START);
                        if (key.Count > 0)
                        {
                            if (Common.IsKeyPushedDown(key[0]) || reInit)
                            {
                                reInit = false;
                                // Native.SetCursorPos(Global.HalfScreenWidth, Global.HalfScreenHeight);

                                RocketCrossairTimer.Start();
                                vm.ShowDisclaimer = false;
                                vm.ShowWindow = true;
                                //vm.RocketZook = Common.GetRocketKind();

                                do
                                {
                                    Thread.Sleep(1);

                                    if (Common.IsKeyPushedDown(key[0]))
                                    {
                                        reInit = true;
                                    }
                                    
                                    if (Common.IsLeftButtonMousePushedOnce())
                                    {
                                        RocketCrossairTimer.Stop();
                                    }

                                } while (RocketCrossairTimer.Enabled && !TokenSourceRocketCrossair.Token.IsCancellationRequested && !reInit);

                                if (!TokenSourceRocketCrossair.Token.IsCancellationRequested && !RocketCrossairTimer.Enabled)
                                {
                                    vm.ShowWindow = false;
                                }
                                RocketCrossairTimer.Stop();
                            }
                        }
                        Thread.Sleep(100);
                    } while (!TokenSourceRocketCrossair.Token.IsCancellationRequested);
                }
                catch (Exception ex)
                {
                    WindowMessage.SendForeground(String.Format("{0} Error:  {1}\r\n\r\n{2}\r\n\r\n", ex.Source, ex.Message, ex.StackTrace), "Keystroke catcher task encountered an error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            , TokenSourceRocketCrossair.Token);

            // HideDisclaimer
            _ = Task.Run(() =>
            {
                try
                {
                    if (vm.ShowDisclaimer)
                    {
                        Thread.Sleep(2000);
                        bool closeCalled = false;
                        if (CloseRocketCrossairTask?.Status is TaskStatus.Running)
                        {
                            closeCalled = true;
                        }
                        if (vm.ShowDisclaimer && !TokenSourceRocketCrossair.IsCancellationRequested && !closeCalled)
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

        internal static void EndRocketCrossairTask(RocketViewModel vm)
        {
            if (TokenSourceRocketCrossair is null || RocketCrossairTask is null)
            {
                return;
            }

            bool alreadyRunning = false;
            if (CloseRocketCrossairTask?.Status is TaskStatus.Running)
            {
                alreadyRunning = true;
            }

            if (!TokenSourceRocketCrossair.IsCancellationRequested && !alreadyRunning)
            {
                StopRocketCrossairTask();
                CloseRocketCrossairTask = Task.Run(async () =>
                {
                    try
                    {
                        await Task.WhenAll(RocketCrossairTask);

                        vm.ShowDisclaimer = true;
                        vm.DisclaimerText = vm.RocketZook ? Strings.Aim.DISCLAIMER_PANZERSCHRECK_OFF : Strings.Aim.DISCLAIMER_BAZOOKA_OFF;
                        vm.DisclaimerColor = Brushes.Red;
                        vm.ShowWindow = true;

                        Thread.Sleep(2000);

                        IntPtr pHwnd = NativeWin.FindWindow(null, Strings.View.ROCKET);
                        if (pHwnd.ToInt32() > 0)
                        {
                            NativeWin.SendMessage(pHwnd, NativeWin.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                        }
                        Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {
                        WindowMessage.SendForeground(String.Format("{0} Error:  {1}\r\n\r\n{2}\r\n\r\n", ex.Source, ex.Message, ex.StackTrace), "Error encountered in CloseWindowAfterDelay method", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            );
            }
        }

        internal static void StopRocketCrossairTask()
        {
            TokenSourceRocketCrossair?.Cancel();
        }

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

        internal static void StopMapHistoryTask()
        {
            TokenSourceMapHistory?.Cancel();
        }

        internal static void StartMapHistoryTask(string mapFolderName)
        {
            StopMapHistoryTask();
            TokenSourceMapHistory = new();

            MapHistoryTask = Task.Run(() =>
            {
                try
                {
                    int count = 0;
                    Stopwatch watch = new();
                    Global.LastMapSaveTime = 0;
                    var key = HotKey.GetFeatureKeys(Strings.Feature.MAP_OPEN)[0];
                    watch.Start();
                    do
                    {
                        if (Common.IsKeyPushedDown(key))
                        {
                            Thread.Sleep(150);
                            if (Common.IsHllLaunchedAndFocused() && (watch.ElapsedMilliseconds >= (Global.LastMapSaveTime + Global.LIMIT_MAP_TIMER)) || count == 0)
                            {
                                count++;
                                Common.GenerateMapHistory(mapFolderName, watch);
                            }
                        }
                    } while (!TokenSourceMapHistory.Token.IsCancellationRequested);
                    watch.Stop();
                }
                catch (Exception ex)
                {
                    WindowMessage.SendForeground(String.Format("{0} Error:  {1}\r\n\r\n{2}\r\n\r\n", ex.Source, ex.Message, ex.StackTrace), "Map history feature encountered an error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            , TokenSourceMapHistory.Token);
        }

        internal static void StopExtraQueueTask()
        {
            IntPtr pHwnd = NativeWin.FindWindow(null, Strings.View.TASKBAR);
            if (pHwnd.ToInt32() > 0)
            {
                HwndSource hwndSource = HwndSource.FromHwnd(pHwnd);
                if (hwndSource.RootVisual is TaskBarWindow taskWin)
                {
                    taskWin.ViewModel.Queue = false;
                }
            }
            TokenSourceExtraQueue?.Cancel();
        }

        internal static void StartExtraQueueTask()
        {
            StopExtraQueueTask();
            TokenSourceExtraQueue = new();

            ExtraQueueTask = Task.Run(() =>
            {
                try
                {
                    int watchdog = 0;
                    do
                    {
                        Thread.Sleep(1);
                        if (Common.IsHllLaunchedAndFocused())
                        {
                            if (!Common.JoinQueue())
                            {
                                watchdog++;
                                if (Common.ByPassFullQueue())
                                {
                                    watchdog = 0;
                                }
                            }
                            else
                            {
                                watchdog = 0;
                            }

                            if (watchdog >= 15)
                            {
                                StopExtraQueueTask();
                            }
                        }
                    } while (!TokenSourceExtraQueue.Token.IsCancellationRequested);
                }
                catch (Exception ex)
                {
                    WindowMessage.SendForeground(String.Format("{0} Error:  {1}\r\n\r\n{2}\r\n\r\n", ex.Source, ex.Message, ex.StackTrace), "Auto queue feature encountered an error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            , TokenSourceExtraQueue.Token);
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
