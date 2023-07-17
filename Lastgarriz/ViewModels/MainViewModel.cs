using Run.Util;
using Run.ViewModels.Command;

namespace Run.ViewModels
{
    public sealed class MainViewModel : BaseViewModel
    {
        public MainCommand Commands { get; private set; }

        public MainViewModel()
        {
            Commands = new(this);

            Global.InitGlobals(/*ViewModel*/);
        }
    }
}
