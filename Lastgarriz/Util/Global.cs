using Lastgarriz.Util.Hook;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;

namespace Lastgarriz.Util
{
    internal static class Global
    {
        internal static readonly bool DEBUG_TIMERS = false;
        internal static readonly int SCALING_VALUE = 1000; // GOOD - DO NOT CHANGE
        internal static readonly double RATIO_BAZOOKA = 0.358; // TO CHECK & MODIFY
        internal static readonly double RATIO_PANZERSCHRECK = 0.3428; // TO CHECK & MODIFY
        internal static readonly double RATIO_WITHOUT_STEADY = 0.777; // TO CHECK & MODIFY

        /* 
            -INGAME READINGS- - not precise enough : TO REDO-

            PANZERSCHRECK :
            960m - 3000 (0.32)
            840m - 2525 (0.33267)
            720m - 2115 (0.3404)
            600m - 1750 (0.342857)
            480m - 1420 (0.3380)
            360m - 1050 (0.3428)
            240m - 700  (0.3428)
            120m - 350  (0.3428)

            BAZOOKA :
            720m - 2080 (0.34615)
            600m - 1730 (0.34682)
            480m - 1340 (0.35820)
            360m - 1025 (0.35122)
            240m - 670  (0.35821)
            120m - 320  (0.375)
        */

        internal static bool Terminate { get; set; }
        internal static bool IsHotKey { get; set; }
        internal static bool IsSelectFocused { get; set; }
        internal static bool HotkeyProcBlock { get; set; }
        internal static bool FirstRegisterHK { get; set; }
        internal static bool FirstCheckUpdate { get; set; }

        internal static IntPtr MainHwnd { get; set; }
        internal static IntPtr HookHwnd { get; set; }

        internal static string ErrorMsg { get; set; }

        internal static int HalfScreenHeight { get; private set; } = Convert.ToInt32(SystemParameters.PrimaryScreenHeight / 2) - 1;
        internal static int HalfScreenWidth { get; private set; } = Convert.ToInt32(SystemParameters.PrimaryScreenWidth / 2) - 1;

        //internal static MainViewModel Vm { get; set; }
        internal static DataManager DataJson { get; set; }
        internal static WndProcService WndService { get; private set; }

        internal static Dictionary<Keys, int> NumericKeyList { get; private set; } = new()
        {
            { Keys.D0, 0 }, { Keys.D1, 1 }, { Keys.D2, 2 }, { Keys.D3, 3 }, { Keys.D4, 4 }, 
            { Keys.D5, 5 }, { Keys.D6, 6 }, { Keys.D7, 7 }, { Keys.D8, 8 }, { Keys.D9, 9 },
            { Keys.NumPad0, 0 }, { Keys.NumPad1, 1 }, { Keys.NumPad2, 2 }, { Keys.NumPad3, 3 }, { Keys.NumPad4, 4 },
            { Keys.NumPad5, 5 }, { Keys.NumPad6, 6 }, { Keys.NumPad7, 7 }, { Keys.NumPad8, 8 }, { Keys.NumPad9, 9 }
        };

        internal static List<Keys> ModifierKeyList { get; private set; } = new()
        {
            Keys.LShiftKey, Keys.RShiftKey, Keys.LControlKey, Keys.RControlKey, Keys.LWin, Keys.RWin, Keys.LMenu, Keys.RMenu // menu = alt
        };

        internal static void InitGlobals()
        {
            Terminate = false;
            IsHotKey = false;
            HotkeyProcBlock = false;
            FirstRegisterHK = true;

            DataJson = DataManager.Instance;

            if (!DataJson.InitSettings())
            {
                System.Windows.MessageBox.Show("Closing application...", "Fatal error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Exclamation);
                System.Windows.Application.Current.Shutdown();
                return;
            }

            WndService = WndProcService.Instance;
        }
    }
}
