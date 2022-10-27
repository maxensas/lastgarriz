using Lastgarriz.ViewModels.Command;
using System.Windows.Media;

namespace Lastgarriz.ViewModels
{
    public sealed class RocketViewModel : BaseViewModel
    {
        private string indicator = string.Empty;
        private string kind = string.Empty;
        private bool showWindow;
        private bool showDisclaimer = true;
        private string disclaimerText = "Rocket indicator activated";
        private SolidColorBrush disclaimerColor = Brushes.Lime;
        private SolidColorBrush crosshairColor = Brushes.Lime;
        private int xhair = 0;

        public string Indicator { get => indicator; set => SetProperty(ref indicator, value); }
        public string Kind { get => kind; set => SetProperty(ref kind, value); }
        public bool ShowWindow { get => showWindow; set => SetProperty(ref showWindow, value); }
        public bool ShowDisclaimer { get => showDisclaimer; set => SetProperty(ref showDisclaimer, value); }
        public string DisclaimerText { get => disclaimerText; set => SetProperty(ref disclaimerText, value); }
        public SolidColorBrush DisclaimerColor { get => disclaimerColor; set => SetProperty(ref disclaimerColor, value); }
        public SolidColorBrush CrosshairColor { get => crosshairColor; set => SetProperty(ref crosshairColor, value); }
        public int Xhair { get => xhair; set => SetProperty(ref xhair, value); }

        public RocketCommand Commands { get; private set; }

        public RocketViewModel()
        {
            Commands = new(this);

            //TaskManager.StartMouseCatcherTask(this);
        }
    }
}
