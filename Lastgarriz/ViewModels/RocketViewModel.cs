using Lastgarriz.ViewModels.Command;
using System.Windows.Media;

namespace Lastgarriz.ViewModels
{
    public sealed class RocketViewModel : BaseViewModel
    {
        private string indicator = string.Empty;
        private bool showWindow;
        private bool showDisclaimer = true;
        private string disclaimerText = "Rocket indicator activated";
        private SolidColorBrush disclaimerColor = Brushes.Lime;

        public string Indicator { get => indicator; set => SetProperty(ref indicator, value); }
        public bool ShowWindow { get => showWindow; set => SetProperty(ref showWindow, value); }
        public bool ShowDisclaimer { get => showDisclaimer; set => SetProperty(ref showDisclaimer, value); }
        public string DisclaimerText { get => disclaimerText; set => SetProperty(ref disclaimerText, value); }
        public SolidColorBrush DisclaimerColor { get => disclaimerColor; set => SetProperty(ref disclaimerColor, value); }

        public RocketCommand Commands { get; private set; }

        public RocketViewModel()
        {
            Commands = new(this);

            //TaskManager.StartMouseCatcherTask(this);
        }
    }
}
