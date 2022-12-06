using Lastgarriz.Util;
using Lastgarriz.ViewModels.Command;

namespace Lastgarriz.ViewModels
{
    public sealed class TaskBarViewModel : BaseViewModel
    {
        private bool queue;
        private bool map;
        private bool artilleryUsGer;
        private bool artilleryUssr;
        private bool bazooka;
        private bool panzerschreck;

        public bool Queue { get => queue; set => SetProperty(ref queue, value); }
        public bool Map { get => map; set => SetProperty(ref map, value); }
        public bool ArtilleryUsGer { get => artilleryUsGer; set => SetProperty(ref artilleryUsGer, value); }
        public bool ArtilleryUssr { get => artilleryUssr; set => SetProperty(ref artilleryUssr, value); }
        public bool Bazooka { get => bazooka; set => SetProperty(ref bazooka, value); }
        public bool Panzerschreck { get => panzerschreck; set => SetProperty(ref panzerschreck, value); }

        //public TaskBarCommand Commands { get; private set; }
        public TaskBarViewModel()
        {
            //Commands = new(this);
        }
    }
}
