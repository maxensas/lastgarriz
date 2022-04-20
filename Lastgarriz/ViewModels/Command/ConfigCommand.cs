using Lastgarriz.Util;
using Lastgarriz.Util.Hook;
using System.Windows.Input;

namespace Lastgarriz.ViewModels.Command
{
    public sealed class ConfigCommand
    {
        private static ConfigViewModel Vm { get; set; }

        private readonly DelegateCommand closeWindow;
        private readonly DelegateCommand loadDefaultConfig;
        private readonly DelegateCommand saveConfig;
        private readonly DelegateCommand setHotKey;

        public ICommand CloseWindow => closeWindow;
        public ICommand LoadDefaultConfig => loadDefaultConfig;
        public ICommand SaveConfig => saveConfig;
        public ICommand SetHotKey => setHotKey;

        public ConfigCommand(ConfigViewModel vm)
        {
            Vm = vm;
            closeWindow = new(OnCloseWindow, CanCloseWindow);
            loadDefaultConfig = new(OnLoadDefaultConfig, CanLoadDefaultConfig);
            saveConfig = new(OnSaveConfig, CanSaveConfig);
            setHotKey = new(OnSetHotKey, CanSetHotKey);
        }

        private static bool CanCloseWindow(object commandParameter)
        {
            return true;
        }

        private static void OnCloseWindow(object commandParameter)
        {
            Common.CloseWindow(Strings.View.CONFIGURATION);
        }

        private static bool CanLoadDefaultConfig(object commandParameter)
        {
            return true;
        }

        private static void OnLoadDefaultConfig(object commandParameter)
        {
            Vm.LoadDefaultConfig();
        }

        private static bool CanSaveConfig(object commandParameter)
        {
            return true;
        }

        private static void OnSaveConfig(object commandParameter)
        {
            Vm.SaveConfig();
            Common.CloseWindow(Strings.View.CONFIGURATION);
        }

        private static bool CanSetHotKey(object commandParameter)
        {
            return true;
        }

        private static void OnSetHotKey(object commandParameter)
        {
            if (commandParameter is CompositeCommandParameter)
            {
                var param = (CompositeCommandParameter)commandParameter;

                //var arg = (KeyEventArgs)param.EventArgs;
                var vm = (HotkeyViewModel)param.Parameter;
                bool canUseModAsHotKey = 
                    vm == Vm.Features.Artillery_validate || 
                    vm == Vm.Features.Rocket_start;

                HotKey.SetHotKey(vm, param.EventArgs, canUseModAsHotKey);
            }
        }
    }
}
