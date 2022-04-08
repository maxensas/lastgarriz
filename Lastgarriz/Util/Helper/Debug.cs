using System.Windows;

namespace Lastgarriz.Util.Helper
{
    internal static class Debug
    {
        private static System.Diagnostics.Stopwatch CodeWatchUi { get; set; }
        private static System.Diagnostics.Stopwatch CodeWatch { get; set; }

        internal static void StartTimer()
        {
#if DEBUG
            if (Application.Current.Dispatcher.CheckAccess())
            {
                if (Global.DEBUG_TIMERS)
                {
                    if (CodeWatchUi == null)
                    {
                        CodeWatchUi = System.Diagnostics.Stopwatch.StartNew();
                    }
                    else
                    {
                        CodeWatchUi.Restart();
                    }
                }
            }
            else
            {
                if (Global.DEBUG_TIMERS)
                {
                    if (CodeWatch == null)
                    {
                        CodeWatch = System.Diagnostics.Stopwatch.StartNew();
                    }
                    else
                    {
                        CodeWatch.Restart();
                    }
                }
            }

#endif
        }

        internal static void StopTimer(string message)
        {
#if DEBUG
            if (Application.Current.Dispatcher.CheckAccess())
            {
                if (Global.DEBUG_TIMERS)
                {
                    if (CodeWatchUi != null)
                    {
                        if (CodeWatchUi.IsRunning)
                        {
                            CodeWatchUi.Stop();
                            /*
                            if (Global.PriceWatch.IsRunning)
                            {
                                Trace("Total elapsed : " + Global.PriceWatch.ElapsedMilliseconds + "ms | " + message + " : " + CodeWatchUi.ElapsedMilliseconds + "ms");
                            }
                            else
                            {
                                Trace(message + " : " + CodeWatchUi.ElapsedMilliseconds + "ms");
                            }
                            */
                        }
                    }
                }
            }
            else
            {
                if (Global.DEBUG_TIMERS)
                {
                    if (CodeWatch != null)
                    {
                        if (CodeWatch.IsRunning)
                        {
                            CodeWatch.Stop();
                            /*
                            if (Global.PriceWatch.IsRunning)
                            {
                                Trace("Total elapsed : " + Global.PriceWatch.ElapsedMilliseconds + "ms | " + message + " : " + CodeWatch.ElapsedMilliseconds + "ms");
                            }
                            else
                            {
                                Trace(message + " : " + CodeWatch.ElapsedMilliseconds + "ms");
                            }
                            */
                        }
                    }
                }
            }

#endif
        }

        internal static void Trace(string message)
        {
#if DEBUG
            System.Diagnostics.Trace.WriteLine(message);
#endif
        }
    }
}
