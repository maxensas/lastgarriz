using System;
using System.Windows.Forms;

namespace Lastgarriz.Util.Hook
{
    internal sealed class SpongeWindow : NativeWindow
    {
        internal event EventHandler<Message> WndProcCalled;

        internal SpongeWindow()
        {
            CreateHandle(new CreateParams());
        }

        protected override void WndProc(ref Message m)
        {
            WndProcCalled?.Invoke(this, m);
            base.WndProc(ref m);
        }
    }
}
