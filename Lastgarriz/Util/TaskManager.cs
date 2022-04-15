using Lastgarriz.Util.Hook;
using Lastgarriz.ViewModels;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Lastgarriz.Util
{
    internal static class TaskManager // light
    {
        internal static Task UpdateCheckerTask { get; private set; } // only last task launched
        internal static Task KeystrokeCatcherTask { get; private set; } // only last task launched
        internal static CancellationTokenSource TokenSourceCatcher { get; private set; }

        internal static void StartCatcherTask(ArtilleryViewModel vm)
        {
            StopCatcherTask();
            TokenSourceCatcher = new();

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
                    } while (!TokenSourceCatcher.Token.IsCancellationRequested);
                }
                catch (Exception ex)
                {
                    WindowMessage.SendForeground(String.Format("{0} Error:  {1}\r\n\r\n{2}\r\n\r\n", ex.Source, ex.Message, ex.StackTrace), "Keystroke catcher task encountered an error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            , TokenSourceCatcher.Token);
        }

        internal static void StopCatcherTask()
        {
            TokenSourceCatcher?.Cancel();
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
