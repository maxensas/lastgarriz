namespace Run.ViewModels
{
    public sealed class HotkeyViewModel : BaseViewModel
    {
        private bool isEnable;
        private string hotkey = string.Empty;

        public bool IsEnable { get => isEnable; set => SetProperty(ref isEnable, value); }
        public string Hotkey { get => hotkey; set => SetProperty(ref hotkey, value); }
    }
}
