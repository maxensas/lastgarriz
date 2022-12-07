using Lastgarriz.Util.Hook;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace Lastgarriz.Util
{
    /// <summary>
    /// Centralize globals necessary for the application.
    /// </summary>
    /// <remarks>This is a static class and can not be instancied.</remarks>
    internal static class Global
    {
        internal static readonly bool DEBUG_TIMERS = false;
        /*
         * old
        internal static readonly double RATIO_BAZOOKA = 0.358;
        internal static readonly double RATIO_PANZERSCHRECK = 0.3428;
        internal static readonly double RATIO_WITHOUT_STEADY = 0.777;
        */
        /* 
            -INGAME READINGS- 

            PANZERSCHRECK : -new-
            960m - 3585 (0.2378) -0.0422
            840m - 3000 (0.28)   -0.0080
            720m - 2500 (0.288)  -0.0046
            600m - 2050 (0.2926) -0.0018
            480m - 1630 (0.2944) -0.0056
            360m - 1200 (0.3)    -0 (probably bad value)
            240m - 800  (0.3)    -0.0287
            120m - 365  (0.3287) init

            BAZOOKA : -old-
            720m - 2080 (0.34615) -0.00067
            600m - 1730 (0.34682) -0.01138
            480m - 1340 (0.35820) -0.00698
            360m - 1025 (0.35122) -0.00699
            240m - 670  (0.35821) -0.01679
            120m - 320  (0.375)   init
        */

        // C:\Users\'user'\AppData\Local\HLL\Saved\Config\WindowsNoEditor\GameUserSettings.ini
        // to get inv mouse : InvertMouseFirstPerson=True/False
        /* Feature : Rocket distance indicator
         The displayed value depends on 3 variables:
            - In-game mouse sensitivity with ADS (ADSMouseSensitivity under GameUserSettings.ini)
            - Mouse cursor speed under windows (between 1 to 20, default:10)
            - Mouse DPI resolution (device driver, ex with logitech G502: 1500 DPI)

        How to align CORRECTLY ping with marker : 
            ![ping-ig](https://user-images.githubusercontent.com/62154281/182184856-a7c5c26f-fb45-4d90-be55-be3fa41c9fa9.png)
            ![ping-map](https://user-images.githubusercontent.com/62154281/182184874-1e2234f0-a805-4aa0-b136-32a52085dc96.png)
        */
        internal static readonly double RATIO_BAZOOKA = 0.3012; // WIP
        internal static readonly double RATIO_PANZERSCHRECK = 0.2782; // WIP
        internal static readonly double RATIO_WITHOUT_STEADY = 1.24; // WIP
        internal static readonly int SCALING_VALUE = 1000; // GOOD - DO NOT CHANGE

        internal static readonly int INDICATOR_TIMER = 8000; // in milliseconds
        internal static readonly int LIMIT_MAP_TIMER = 10000; // in milliseconds
        internal static bool Terminate { get; set; }
        internal static bool IsHotKey { get; set; }
        internal static bool TaskBarActive { get; set; }
        internal static bool IsSelectFocused { get; set; }
        internal static bool HotkeyProcBlock { get; set; }
        internal static bool FirstRegisterHK { get; set; }
        internal static bool FirstCheckUpdate { get; set; }

        internal static int CrossUpdates { get; set; } = 0;

        internal static long LastMapSaveTime { get; set; }

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

        internal static Dictionary<KeyValuePair<int,int>, Rectangle> MapSizeList { get; private set; } = new()
        {
            { new KeyValuePair<int,int>(1920,1080), new Rectangle(520,70,880,902) }, // 1K
            { new KeyValuePair<int,int>(2560,1440), new Rectangle(693,94,1173,1201) }, // 2K
            { new KeyValuePair<int,int>(3840,2160), new Rectangle(1038,141,1762,1802) }, // 4K
            { new KeyValuePair<int,int>(3325,1871), new Rectangle(900,121,1524,1562) },
            { new KeyValuePair<int,int>(2880,1620), new Rectangle(779,105,1321,1352) },
            { new KeyValuePair<int,int>(2715,1527), new Rectangle(735,99,1244,1274) },
            { new KeyValuePair<int,int>(2351,1323), new Rectangle(636,86,1078,1104) },
            { new KeyValuePair<int,int>(2103,1183), new Rectangle(569,77,964,987) },
            { new KeyValuePair<int,int>(1440,900), new Rectangle(365,70,709,726) },
            { new KeyValuePair<int,int>(1280,1024), new Rectangle(226,70,826,847) },
            { new KeyValuePair<int,int>(1024,768), new Rectangle(219,70,585,598) }
        };

        internal static List<Keys> ModifierKeyList { get; private set; } = new()
        {
            Keys.LShiftKey, Keys.RShiftKey, Keys.LControlKey, Keys.RControlKey, Keys.LWin, Keys.RWin, Keys.LMenu, Keys.RMenu // menu = alt
        };

        internal static List<string> BoxOk { get; private set; } = new()
        {
            "ok",
            "vale",
            "we" // chinese ZH & TW
        };

        internal static List<string> BoxCancel { get; private set; } = new()
        {
            "cancel",
            "annuler",
            "abbrechen",
            "cancelar",
            "anuluj",
            "otmeha" // ru to test
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
