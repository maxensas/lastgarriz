using Lastgarriz.Models.Serializable;
using Lastgarriz.Util.Interop;
using System;
using System.Windows.Input;

namespace Lastgarriz.Util.Hook
{
    internal static class HotKey
    {
        // constants
        private static readonly int MOD_NONE = 0x0; // No modifier
        private static readonly int MOD_ALT = 0x1;     // If bit 0 is set, Alt is pressed
        private static readonly int MOD_CONTROL = 0x2; // If bit 1 is set, Ctrl is pressed
        private static readonly int MOD_SHIFT = 0x4;   // If bit 2 is set, Shift is pressed 
        //private static readonly int MOD_WIN = 0x8;     // If bit 3 is set, Win is pressed

        internal static void InstallRegisterHotKey()
        {
            Global.IsHotKey = true;

            for (int i = 0; i < Global.DataJson.Config.Shortcuts.Length; i++)
            {
                ConfigShortcut shortcut = Global.DataJson.Config.Shortcuts[i];
                if (shortcut.Keycode > 0 && shortcut.Value?.Length > 0)
                {
                    if ((Strings.Feature.Unregisterable.Contains(shortcut.Fonction.ToLowerInvariant()) && !Global.DataJson.Config.Options.DevMode) || Global.FirstRegisterHK)
                    {
                        if (shortcut.Enable)
                        {
                            Native.RegisterHotKey(Global.HookHwnd, 10001 + i, Convert.ToUInt32(shortcut.Modifier), (uint)Math.Abs(shortcut.Keycode));
                        }
                    }
                }
            }
            Global.FirstRegisterHK = false;
        }

        internal static void RemoveRegisterHotKey(bool reInit)
        {
            Global.IsHotKey = false;
            if (reInit)
            {
                Global.FirstRegisterHK = true;
            }
            //bool checkStat = true;
            /*
            bool checkStat = false;
            if (GetStatus().Contains("Patron, you can use extra features for") && GetStatus().Contains("Thanks again for your support !"))
            {
                checkStat = true;
            }
            */

            for (int i = 0; i < Global.DataJson.Config.Shortcuts.Length; i++)
            {
                ConfigShortcut shortcut = Global.DataJson.Config.Shortcuts[i];
                if (shortcut.Keycode > 0 && shortcut.Value?.Length > 0)
                {
                    if ((Strings.Feature.Unregisterable.Contains(shortcut.Fonction.ToLowerInvariant()) && !Global.DataJson.Config.Options.DevMode) || reInit)
                    {
                        if (shortcut.Enable)
                        {
                            Native.UnregisterHotKey(Global.HookHwnd, 10001 + i);
                        }
                    }
                }
            }
        }

        internal static int GetModifier(string data)
        {
            int mod = MOD_NONE;
            if (data.ToLowerInvariant().Contains(System.ComponentModel.TypeDescriptor.GetConverter(typeof(ModifierKeys)).ConvertToString(ModifierKeys.Control).ToLowerInvariant(), StringComparison.Ordinal))
            {
                mod |= MOD_CONTROL;
            }
            if (data.ToLowerInvariant().Contains(System.ComponentModel.TypeDescriptor.GetConverter(typeof(ModifierKeys)).ConvertToString(ModifierKeys.Alt).ToLowerInvariant(), StringComparison.Ordinal))
            {
                mod |= MOD_ALT;
            }
            if (data.ToLowerInvariant().Contains(System.ComponentModel.TypeDescriptor.GetConverter(typeof(ModifierKeys)).ConvertToString(ModifierKeys.Shift).ToLowerInvariant(), StringComparison.Ordinal))
            {
                mod |= MOD_SHIFT;
            }
            return mod;
        }

        internal static string GetModString(int modifier)
        {
            string returnVal = string.Empty;
            //ModifierKeysConverter c = new();
            //var modifiers = (ModifierKeys) c.ConvertFrom(Convert.ToUInt32(modifier));
            ModifierKeys modifiers = (ModifierKeys)Enum.Parse(typeof(ModifierKeys), modifier.ToString());

            if (modifiers.HasFlag(ModifierKeys.Control) || modifiers.HasFlag(ModifierKeys.Alt) || modifiers.HasFlag(ModifierKeys.Shift))
            {
                returnVal += System.ComponentModel.TypeDescriptor.GetConverter(typeof(ModifierKeys)).ConvertToString(modifiers) + "+";
            }

            return returnVal;
        }
    }
}
