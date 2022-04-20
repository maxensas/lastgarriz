namespace Lastgarriz.ViewModels
{
    public sealed class FeaturesViewModel : BaseViewModel
    {
        private HotkeyViewModel artillery_usger = new();
        private HotkeyViewModel artillery_ru = new();
        private HotkeyViewModel artillery_validate = new();
        private HotkeyViewModel rocket_start = new();
        private HotkeyViewModel rocket_enable = new();
        private HotkeyViewModel configuration = new();
        private HotkeyViewModel autoqueue = new();
        private HotkeyViewModel browser = new();
        //private HotkeyViewModel close = new();

        public HotkeyViewModel Artillery_usger { get => artillery_usger; set => SetProperty(ref artillery_usger, value); }
        public HotkeyViewModel Artillery_ru { get => artillery_ru; set => SetProperty(ref artillery_ru, value); }
        public HotkeyViewModel Artillery_validate { get => artillery_validate; set => SetProperty(ref artillery_validate, value); }
        public HotkeyViewModel Rocket_start { get => rocket_start; set => SetProperty(ref rocket_start, value); }
        public HotkeyViewModel Rocket_enable { get => rocket_enable; set => SetProperty(ref rocket_enable, value); }
        public HotkeyViewModel Configuration { get => configuration; set => SetProperty(ref configuration, value); }
        public HotkeyViewModel Autoqueue { get => autoqueue; set => SetProperty(ref autoqueue, value); }
        public HotkeyViewModel Browser { get => browser; set => SetProperty(ref browser, value); }
        //public HotkeyViewModel Close { get => close; set => SetProperty(ref close, value); }
    }
}
