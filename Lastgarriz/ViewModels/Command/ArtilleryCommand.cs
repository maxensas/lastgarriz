using Lastgarriz.Util;
using System.Windows.Input;

namespace Lastgarriz.ViewModels.Command
{
    public sealed class ArtilleryCommand
    {
        private static ArtilleryViewModel Vm { get; set; }

        private readonly DelegateCommand displayFeature;
        private readonly DelegateCommand closeArtilleryWindow;

        public ICommand DisplayFeature => displayFeature;
        public ICommand CloseArtilleryWindow => closeArtilleryWindow;

        public ArtilleryCommand(ArtilleryViewModel vm)
        {
            Vm = vm;
            displayFeature = new(OnDisplayFeature, CanDisplayFeature);
            closeArtilleryWindow = new(OnCloseArtilleryWindow, CanCloseArtilleryWindow);
        }

        private static bool CanDisplayFeature(object commandParameter)
        {
            return true;
        }

        private static void OnDisplayFeature(object commandParameter)
        {
            // TODO
        }

        private static bool CanCloseArtilleryWindow(object commandParameter)
        {
            return true;
        }

        private static void OnCloseArtilleryWindow(object commandParameter)
        {
            TaskManager.StopKeyCatcherTask();
        }
    }
}
