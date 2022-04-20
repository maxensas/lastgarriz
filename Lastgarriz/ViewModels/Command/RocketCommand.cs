using Lastgarriz.Util;
using Lastgarriz.Util.Interop;
using System;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media;

namespace Lastgarriz.ViewModels.Command
{
    public sealed class RocketCommand
    {
        private static RocketViewModel Vm { get; set; }

        private readonly DelegateCommand closeRocketWindow;
        private readonly DelegateCommand loadRocketWindow;

        public ICommand CloseRocketWindow => closeRocketWindow;
        public ICommand LoadRocketWindow => loadRocketWindow;

        public RocketCommand(RocketViewModel vm)
        {
            Vm = vm;
            closeRocketWindow = new(OnCloseRocketWindow, CanCloseRocketWindow);
            loadRocketWindow = new(OnLoadRocketWindow, CanLoadRocketWindow);
        }

        private static bool CanCloseRocketWindow(object commandParameter)
        {
            return true;
        }

        private static void OnCloseRocketWindow(object commandParameter)
        {
            
        }

        private static bool CanLoadRocketWindow(object commandParameter)
        {
            return true;
        }

        private static void OnLoadRocketWindow(object commandParameter)
        {
            
        }
    }
}
