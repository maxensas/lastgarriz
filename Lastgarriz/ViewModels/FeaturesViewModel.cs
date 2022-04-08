namespace Lastgarriz.ViewModels
{
    public sealed class FeaturesViewModel : BaseViewModel
    {
        private HotkeyViewModel artillery_usger = new();
        private HotkeyViewModel artillery_ru = new();
        private HotkeyViewModel configuration = new();
        private HotkeyViewModel autoqueue = new();
        private HotkeyViewModel browser = new();
        //private HotkeyViewModel close = new();

        public HotkeyViewModel Artillery_usger { get => artillery_usger; set => SetProperty(ref artillery_usger, value); }
        public HotkeyViewModel Artillery_ru { get => artillery_ru; set => SetProperty(ref artillery_ru, value); }
        public HotkeyViewModel Configuration { get => configuration; set => SetProperty(ref configuration, value); }
        public HotkeyViewModel Autoqueue { get => autoqueue; set => SetProperty(ref autoqueue, value); }
        public HotkeyViewModel Browser { get => browser; set => SetProperty(ref browser, value); }
        //public HotkeyViewModel Close { get => close; set => SetProperty(ref close, value); }
    }
}
