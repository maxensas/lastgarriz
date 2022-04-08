using Lastgarriz.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Lastgarriz.Views
{
    /// <summary>
    /// Logique d'interaction pour ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        public ConfigViewModel ViewModel { get; private set; } = new ConfigViewModel();

        public ConfigWindow()
        {
            InitializeComponent();

            DataContext = ViewModel;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            IsEnabled = true;
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        // DO MvvM for hotkey management
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if ((sender as System.Windows.Controls.TextBox) != null)
            {
                if (((System.Windows.Controls.TextBox)sender).Text.EndsWith('+'))
                {
                    ((System.Windows.Controls.TextBox)sender).Clear();
                }
            }
        }

        private static Key RealKey(KeyEventArgs e)
        {
            return e.Key switch
            {
                Key.System => e.SystemKey,
                Key.ImeProcessed => e.ImeProcessedKey,
                Key.DeadCharProcessed => e.DeadCharProcessedKey,
                _ => e.Key,
            };
        }

        private void SetHotKey(object sender, KeyEventArgs e)
        {
            var nonShortcuttableKeys = new[] { Key.LeftAlt, Key.RightAlt, Key.LeftCtrl, Key.RightCtrl, Key.LeftShift, Key.RightShift, Key.LWin, Key.RWin };
            var actualKey = RealKey(e);

            if (e.IsDown && !nonShortcuttableKeys.Contains(actualKey))
            {
                var tb = sender as System.Windows.Controls.TextBox;

                var modifiers = new List<ModifierKeys>();
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                {
                    modifiers.Add(ModifierKeys.Control);
                }

                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Alt))
                {
                    modifiers.Add(ModifierKeys.Alt);
                }

                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                {
                    modifiers.Add(ModifierKeys.Shift);
                }

                string modifStr = System.ComponentModel.TypeDescriptor.GetConverter(typeof(ModifierKeys)).ConvertToString(Keyboard.Modifiers);
                tb.Text = modifiers.Count == 0
                    ? string.Format("{0}", actualKey)
                    : string.Format("{0}+{1}", modifStr, actualKey);

                VerifyBoxKey(tb);
            }

            e.Handled = true;
        }

        private static bool VerifyBoxKey(System.Windows.Controls.TextBox box)
        {
            System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Windows.Forms.Keys));

            try
            {
                int returnKey = (int)converter.ConvertFromString(box.Text.ToString());
                box.Foreground = System.Windows.Media.Brushes.White;
                return true;
            }
            catch // exception not used
            {
                box.Text = "Undefined";
                box.Foreground = System.Windows.Media.Brushes.OrangeRed;
                return false;
            }
        }
    }
}
