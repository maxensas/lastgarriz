namespace Run.ViewModels
{
    public sealed class FeaturesViewModel : BaseViewModel
    {
        private HotkeyViewModel artillery_usger = new();
        private HotkeyViewModel artillery_ru = new();
        private HotkeyViewModel artillery_validate = new();
        private HotkeyViewModel rocket_start = new();
        private HotkeyViewModel rocket_us = new();
        private HotkeyViewModel rocket_ger = new();
        private HotkeyViewModel map_open = new();
        private HotkeyViewModel map_record = new();
        private HotkeyViewModel configuration = new();
        private HotkeyViewModel autoqueue = new();
        private HotkeyViewModel browser = new();
        //private HotkeyViewModel close = new();

        public HotkeyViewModel Artillery_usger { get => artillery_usger; set => SetProperty(ref artillery_usger, value); }
        public HotkeyViewModel Artillery_ru { get => artillery_ru; set => SetProperty(ref artillery_ru, value); }
        public HotkeyViewModel Artillery_validate { get => artillery_validate; set => SetProperty(ref artillery_validate, value); }
        public HotkeyViewModel Rocket_start { get => rocket_start; set => SetProperty(ref rocket_start, value); }
        public HotkeyViewModel Rocket_us { get => rocket_us; set => SetProperty(ref rocket_us, value); }
        public HotkeyViewModel Rocket_ger { get => rocket_ger; set => SetProperty(ref rocket_ger, value); }
        public HotkeyViewModel Map_open { get => map_open; set => SetProperty(ref map_open, value); }
        public HotkeyViewModel Map_record { get => map_record; set => SetProperty(ref map_record, value); }
        public HotkeyViewModel Configuration { get => configuration; set => SetProperty(ref configuration, value); }
        public HotkeyViewModel Autoqueue { get => autoqueue; set => SetProperty(ref autoqueue, value); }
        public HotkeyViewModel Browser { get => browser; set => SetProperty(ref browser, value); }
        //public HotkeyViewModel Close { get => close; set => SetProperty(ref close, value); }
    }
}
