using Run.Models.Serializable;
using Run.Util.Interop;
using Run.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Run.Util.Hook
{
    /// <summary>
    /// Contain all methods needed to manage hotkey bindings.
    /// </summary>
    /// <remarks>This is a static class and can not be instancied.</remarks>
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
                        if (shortcut.Enable && shortcut.Value is not Strings.KEYLOG)
                        {
                            NativeWin.RegisterHotKey(Global.HookHwnd, 10001 + i, Convert.ToUInt32(shortcut.Modifier), (uint)Math.Abs(shortcut.Keycode));
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

            for (int i = 0; i < Global.DataJson.Config.Shortcuts.Length; i++)
            {
                ConfigShortcut shortcut = Global.DataJson.Config.Shortcuts[i];
                if (shortcut.Keycode > 0 && shortcut.Value?.Length > 0)
                {
                    if ((Strings.Feature.Unregisterable.Contains(shortcut.Fonction.ToLowerInvariant()) && !Global.DataJson.Config.Options.DevMode) || reInit)
                    {
                        if (shortcut.Enable && shortcut.Value is not Strings.KEYLOG)
                        {
                            NativeWin.UnregisterHotKey(Global.HookHwnd, 10001 + i);
                        }
                    }
                }
            }
        }

        internal static int GetModifier(string data)
        {
            int mod = MOD_NONE;
            if (data.Contains('+', StringComparison.Ordinal))
            {
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

        internal static void SetHotKey(HotkeyViewModel vm, EventArgs e, bool canUseModAsHotKey)
        {
            System.Windows.Forms.Keys actualKey;
            MouseEventArgs mouseArg = null;
            KeyEventArgs keyArg = null;
            bool keyDown = false;
            bool isKey = false, isMouse = false;
            if (e is MouseEventArgs)
            {
                mouseArg = (MouseEventArgs)e;
                if (mouseArg.LeftButton is MouseButtonState.Pressed)
                {
                    return;
                }
                mouseArg.Handled = isMouse = true;
            }
            if (e is KeyEventArgs)
            {
                keyArg = (KeyEventArgs)e;
                keyArg.Handled = isKey = true;
                keyDown = keyArg.IsDown;
            }

            if (isKey || isMouse)
            {
                actualKey = isMouse ? GetMouseKey(mouseArg) : GetVirtualKey(keyArg);

                var ignoreKeys = canUseModAsHotKey
                    ? new[] { System.Windows.Forms.Keys.LWin, System.Windows.Forms.Keys.RWin }
                    : Global.ModifierKeyList.ToArray();

                bool isModKey = Global.ModifierKeyList.Contains(actualKey);

                if ((keyDown || isMouse) && !ignoreKeys.Contains(actualKey))
                {
                    var modifiers = new List<ModifierKeys>();
                    if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && !isModKey)
                    {
                        modifiers.Add(ModifierKeys.Control);
                    }

                    if (Keyboard.Modifiers.HasFlag(ModifierKeys.Alt) && !isModKey)
                    {
                        modifiers.Add(ModifierKeys.Alt);
                    }

                    if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && !isModKey)
                    {
                        modifiers.Add(ModifierKeys.Shift);
                    }

                    string modifStr = System.ComponentModel.TypeDescriptor.GetConverter(typeof(ModifierKeys)).ConvertToString(Keyboard.Modifiers);
                    string hotKey = modifiers.Count == 0
                        ? string.Format("{0}", actualKey)
                        : string.Format("{0}+{1}", modifStr, actualKey);

                    if (VerifyHotKey(hotKey))
                    {
                        if (hotKey.Length == 2 && hotKey.StartsWith('D')) // D0 to D9
                        {
                            hotKey = hotKey.Replace("D", string.Empty);
                        }
                        vm.Hotkey = hotKey;
                    }
                }
                return;
            }
        }

        private static System.Windows.Forms.Keys GetMouseKey(MouseEventArgs m)
        {
            return m.RightButton is MouseButtonState.Pressed ? System.Windows.Forms.Keys.RButton :
                m.MiddleButton is MouseButtonState.Pressed ? System.Windows.Forms.Keys.MButton :
                m.XButton1 is MouseButtonState.Pressed ? System.Windows.Forms.Keys.XButton1 :
                m.XButton2 is MouseButtonState.Pressed ? System.Windows.Forms.Keys.XButton2 
                : System.Windows.Forms.Keys.LButton; //default
        }

        private static System.Windows.Forms.Keys GetVirtualKey(KeyEventArgs e)
        {
            var key = e.Key switch
            {
                Key.System => e.SystemKey,
                Key.ImeProcessed => e.ImeProcessedKey,
                Key.DeadCharProcessed => e.DeadCharProcessedKey,
                _ => e.Key,
            };
            return (System.Windows.Forms.Keys)KeyInterop.VirtualKeyFromKey(key);
        }

        private static bool VerifyHotKey(string hotKeyText)
        {
            if (hotKeyText.EndsWith('+')) // cannot set '+' as hotkey : ok for OemPlus & NumpadPlus
            {
                return false;
            }

            System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Windows.Forms.Keys)); // old System.Windows.Forms.Keys
            try
            {
                var returnKey = (int)converter.ConvertFromInvariantString(hotKeyText);
                return true;
            }
            catch // exception not used
            {
                return false;
            }
        }

        internal static List<System.Windows.Forms.Keys> GetFeatureKeys(string feature)
        {
            List<System.Windows.Forms.Keys> returnList = new();
            for (int i = 0; i < Global.DataJson.Config.Shortcuts.Length; i++)
            {
                ConfigShortcut shortcut = Global.DataJson.Config.Shortcuts[i];
                if (shortcut.Keycode > 0 && shortcut.Value?.Length > 0)
                {
                    if (feature == shortcut.Fonction.ToLowerInvariant())
                    {
                        returnList.Add((System.Windows.Forms.Keys)shortcut.Keycode);
                        break;
                    }
                }
            }

            if (returnList.Count == 0)
            {
                returnList.Add(System.Windows.Forms.Keys.None);
            }
            return returnList;
        }
    }
}
