using Lastgarriz.Util;
using Lastgarriz.ViewModels.Command;

namespace Lastgarriz.ViewModels
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
