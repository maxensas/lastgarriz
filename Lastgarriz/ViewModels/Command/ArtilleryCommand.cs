using System.Windows.Input;

namespace Lastgarriz.ViewModels.Command
{
    public sealed class ArtilleryCommand
    {
        private static ArtilleryViewModel Vm { get; set; }

        private readonly DelegateCommand displayFeature;

        public ICommand DisplayFeature => displayFeature;

        public ArtilleryCommand(ArtilleryViewModel vm)
        {
            Vm = vm;
            displayFeature = new(OnDisplayFeature, CanDisplayFeature);
        }

        private static bool CanDisplayFeature(object commandParameter)
        {
            return true;
        }

        private static void OnDisplayFeature(object commandParameter)
        {
            // TODO
        }
    }
}
