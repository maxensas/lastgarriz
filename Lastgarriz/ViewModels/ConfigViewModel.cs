using Lastgarriz.Models.Serializable;
using Lastgarriz.Util;
using Lastgarriz.Util.Hook;
using Lastgarriz.ViewModels.Command;
using System;
using System.Windows;

namespace Lastgarriz.ViewModels
{
    public sealed class ConfigViewModel : BaseViewModel
    {
        private ConfigData Config { get; set; }
        private string ConfigBackup { get; set; }

        private bool disableStartup;
        private bool devMode;
        private bool invertedMouse;
        private bool steadyAim;
        private bool convertIndicator;
        private bool showNoHoldValues;
        private FeaturesViewModel features = new();

        public bool DisableStartup { get => disableStartup; set => SetProperty(ref disableStartup, value); }
        public bool DevMode { get => devMode; set => SetProperty(ref devMode, value); }
        public bool InvertedMouse { get => invertedMouse; set => SetProperty(ref invertedMouse, value); }
        public bool SteadyAim { get => steadyAim; set => SetProperty(ref steadyAim, value); }
        public bool ConvertIndicator { get => convertIndicator; set => SetProperty(ref convertIndicator, value); }
        public bool ShowNoHoldValues { get => showNoHoldValues; set => SetProperty(ref showNoHoldValues, value); }
        public FeaturesViewModel Features { get => features; set => SetProperty(ref features, value); }

        public ConfigCommand Commands { get; private set; }

        public ConfigViewModel()
        {
            Commands = new(this);

            ConfigBackup = DataManager.Load_Config(Strings.File.CONFIG); //parentWindow
            Config = Json.Deserialize<ConfigData>(ConfigBackup);

            InitConfig();
        }

        internal static int VerifyKeycode(HotkeyViewModel vm, int keycode)
        {
            int returnKey;
            System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Windows.Forms.Keys));
            string modRet = string.Empty;
            try
            {
                string key;
                if (vm.Hotkey.Contains("+", StringComparison.Ordinal))
                {
                    modRet = vm.Hotkey.Substring(0, vm.Hotkey.LastIndexOf("+", StringComparison.Ordinal) + 1);
                    key = vm.Hotkey[(vm.Hotkey.LastIndexOf("+", StringComparison.Ordinal) + 1)..]; // box.Text.Substring(box.Text.LastIndexOf("+")+1);
                }
                else
                {
                    key = vm.Hotkey;
                }
                returnKey = (int)converter.ConvertFromString(key);
            }
            catch // exception not used
            {
                System.Windows.Forms.KeysConverter kc = new();
                returnKey = keycode;
                vm.Hotkey = modRet + kc.ConvertToString(keycode);
            }

            return returnKey;
        }

        internal void LoadDefaultConfig()
        {
            ConfigBackup = DataManager.Load_Config(Strings.File.DEFAULT_CONFIG);
            Config = Json.Deserialize<ConfigData>(ConfigBackup);
            InitConfig();
        }

        private void InitConfig()
        {
            //Config.Options.Language = 0;
            //Config.Options.Opacity
            //Config.Options.DisableStartupMessage = false;
            //Config.Options.CheckUpdates = true;

            System.Windows.Forms.KeysConverter kc = new();

            DisableStartup = Config.Options.DisableStartupMessage;
            DevMode = Config.Options.DevMode;
            ShowNoHoldValues = Config.Options.ShowNoHoldValues;

            foreach (var item in Config.Shortcuts)
            {
                if (item.Keycode > 0 && item.Value?.Length > 0)
                {
                    switch (item.Fonction)
                    {
                        case Strings.Feature.ARTILLERY_USGER:
                            Features.Artillery_usger.Hotkey = HotKey.GetModString(item.Modifier) + kc.ConvertToString(item.Keycode);
                            Features.Artillery_usger.IsEnable = item.Enable;
                            break;
                        case Strings.Feature.ARTILLERY_RU:
                            Features.Artillery_ru.Hotkey = HotKey.GetModString(item.Modifier) + kc.ConvertToString(item.Keycode);
                            Features.Artillery_ru.IsEnable = item.Enable;
                            break;
                        case Strings.Feature.ARTILLERY_VALIDATE:
                            Features.Artillery_validate.Hotkey = HotKey.GetModString(item.Modifier) + kc.ConvertToString(item.Keycode);
                            Features.Artillery_validate.IsEnable = item.Enable;
                            break;
                        case Strings.Feature.ROCKETINDICATOR_START:
                            Features.Rocket_start.Hotkey = HotKey.GetModString(item.Modifier) + kc.ConvertToString(item.Keycode);
                            Features.Rocket_start.IsEnable = item.Enable;
                            break;
                        case Strings.Feature.ROCKETINDICATOR_US:
                            Features.Rocket_us.Hotkey = HotKey.GetModString(item.Modifier) + kc.ConvertToString(item.Keycode);
                            Features.Rocket_us.IsEnable = item.Enable;
                            break;
                        case Strings.Feature.ROCKETINDICATOR_GER:
                            Features.Rocket_ger.Hotkey = HotKey.GetModString(item.Modifier) + kc.ConvertToString(item.Keycode);
                            Features.Rocket_ger.IsEnable = item.Enable;
                            break;
                        case Strings.Feature.MAP_OPEN:
                            Features.Map_open.Hotkey = HotKey.GetModString(item.Modifier) + kc.ConvertToString(item.Keycode);
                            Features.Map_open.IsEnable = item.Enable;
                            break;
                        case Strings.Feature.MAP_RECORD:
                            Features.Map_record.Hotkey = HotKey.GetModString(item.Modifier) + kc.ConvertToString(item.Keycode);
                            Features.Map_record.IsEnable = item.Enable;
                            break;
                        case Strings.Feature.AUTOQUEUE:
                            Features.Autoqueue.Hotkey = HotKey.GetModString(item.Modifier) + kc.ConvertToString(item.Keycode);
                            Features.Autoqueue.IsEnable = item.Enable;
                            break;
                        case Strings.Feature.CONFIG:
                            Features.Configuration.Hotkey = HotKey.GetModString(item.Modifier) + kc.ConvertToString(item.Keycode);
                            Features.Configuration.IsEnable = item.Enable;
                            break;
                        case Strings.Feature.BROWSER:
                            Features.Browser.Hotkey = HotKey.GetModString(item.Modifier) + kc.ConvertToString(item.Keycode);
                            Features.Browser.IsEnable = item.Enable;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        internal void SaveConfig()
        {
            //Config.Options.Language = 0;
            //Config.Options.Opacity
            Config.Options.DisableStartupMessage = DisableStartup;
            Config.Options.DevMode = DevMode;
            Config.Options.ShowNoHoldValues = ShowNoHoldValues;
            //Config.Options.CheckUpdates = true;

            //Thread.CurrentThread.CurrentUICulture = new CultureInfo(Strings.Culture[Config.Options.Language]);

            foreach (var item in Config.Shortcuts)
            {
                if (item.Keycode > 0 && item.Value?.Length > 0)
                {
                    switch (item.Fonction)
                    {
                        case Strings.Feature.ARTILLERY_USGER:
                            item.Modifier = HotKey.GetModifier(Features.Artillery_usger.Hotkey);
                            item.Keycode = VerifyKeycode(Features.Artillery_usger, item.Keycode);
                            item.Enable = Features.Artillery_usger.IsEnable;
                            break;
                        case Strings.Feature.ARTILLERY_RU:
                            item.Modifier = HotKey.GetModifier(Features.Artillery_ru.Hotkey);
                            item.Keycode = VerifyKeycode(Features.Artillery_ru, item.Keycode);
                            item.Enable = Features.Artillery_ru.IsEnable;
                            break;
                        case Strings.Feature.ARTILLERY_VALIDATE:
                            item.Modifier = HotKey.GetModifier(Features.Artillery_validate.Hotkey);
                            item.Keycode = VerifyKeycode(Features.Artillery_validate, item.Keycode);
                            item.Enable = Features.Artillery_validate.IsEnable;
                            break;
                        case Strings.Feature.ROCKETINDICATOR_START:
                            item.Modifier = HotKey.GetModifier(Features.Rocket_start.Hotkey);
                            item.Keycode = VerifyKeycode(Features.Rocket_start, item.Keycode);
                            item.Enable = Features.Rocket_start.IsEnable;
                            break;
                        case Strings.Feature.ROCKETINDICATOR_US:
                            item.Modifier = HotKey.GetModifier(Features.Rocket_us.Hotkey);
                            item.Keycode = VerifyKeycode(Features.Rocket_us, item.Keycode);
                            item.Enable = Features.Rocket_us.IsEnable;
                            break;
                        case Strings.Feature.ROCKETINDICATOR_GER:
                            item.Modifier = HotKey.GetModifier(Features.Rocket_ger.Hotkey);
                            item.Keycode = VerifyKeycode(Features.Rocket_ger, item.Keycode);
                            item.Enable = Features.Rocket_ger.IsEnable;
                            break;
                        case Strings.Feature.MAP_OPEN:
                            item.Modifier = HotKey.GetModifier(Features.Map_open.Hotkey);
                            item.Keycode = VerifyKeycode(Features.Map_open, item.Keycode);
                            item.Enable = Features.Map_open.IsEnable;
                            break;
                        case Strings.Feature.MAP_RECORD:
                            item.Modifier = HotKey.GetModifier(Features.Map_record.Hotkey);
                            item.Keycode = VerifyKeycode(Features.Map_record, item.Keycode);
                            item.Enable = Features.Map_record.IsEnable;
                            break;
                        case Strings.Feature.AUTOQUEUE:
                            item.Modifier = HotKey.GetModifier(Features.Autoqueue.Hotkey);
                            item.Keycode = VerifyKeycode(Features.Autoqueue, item.Keycode);
                            item.Enable = Features.Autoqueue.IsEnable;
                            break;
                        case Strings.Feature.CONFIG:
                            item.Modifier = HotKey.GetModifier(Features.Configuration.Hotkey);
                            item.Keycode = VerifyKeycode(Features.Configuration, item.Keycode);
                            item.Enable = Features.Configuration.IsEnable;
                            break;
                        case Strings.Feature.BROWSER:
                            item.Modifier = HotKey.GetModifier(Features.Browser.Hotkey);
                            item.Keycode = VerifyKeycode(Features.Browser, item.Keycode);
                            item.Enable = Features.Browser.IsEnable;
                            break;
                        default:
                            break;
                    }
                }
            }

            string configToSave = Json.Serialize<ConfigData>(Config);

            HotKey.RemoveRegisterHotKey(true);
            DataManager.Instance.Save_Config(configToSave, "cfg");

            if (!DataManager.Instance.InitSettings())
            {
                MessageBox.Show("Closing application...", "Fatal error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Application.Current.Shutdown();
                //Application.Current.MainWindow.Close();
                return;
            }

            HotKey.InstallRegisterHotKey();
        }
    }
}
