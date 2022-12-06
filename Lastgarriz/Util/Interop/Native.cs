using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;

namespace Lastgarriz.Util.Interop
{
    /// <summary>
    /// Helper class containing User32, KERNEL32, GDI32 API functions
    /// </summary>
    internal static class NativeWin // The mess
    {
        [DllImport("user32.dll")] internal static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll")] internal static extern ushort GetAsyncKeyState(int vKey);

        //[DllImport("user32.dll")] internal static extern IntPtr SetClipboardViewer(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)] internal static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        //[DllImport("user32.dll")] internal static extern bool ChangeClipboardChain(IntPtr hWnd, IntPtr hWndNext);
        //[DllImport("user32.dll", CharSet = CharSet.Unicode)] internal static extern IntPtr FindWindowEx(IntPtr parenthWnd, IntPtr childAfter, string lpClassName, string lpWindowName);
        internal const int WM_DRAWCLIPBOARD = 0x0308;
        internal const int WM_CHANGECBCHAIN = 0x030D;
        internal const int WM_KEYUP = 0x0101;
        internal const int WM_CLOSE = 0x0010;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)] internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")] internal static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")] internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")] internal static extern bool SetForegroundWindow(IntPtr hWnd);

        internal const int GWL_EXSTYLE = -20;
        internal const int WS_EX_NOACTIVATE = 0x08000000;

        //[DllImport("user32.dll")] internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        //[DllImport("user32.dll")] internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")] internal static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")] internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);


        [DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool SetProcessWorkingSetSize(IntPtr pProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);

        [DllImport("KERNEL32.DLL", EntryPoint = "GetCurrentProcess", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetCurrentProcess();
        /*
        [DllImport("user32.dll")] internal static extern uint GetWindowThreadProcessId(IntPtr hwnd, IntPtr proccess);
        [DllImport("user32.dll")] internal static extern IntPtr GetKeyboardLayout(uint thread);
        */

        //[DllImport("user32.dll")] internal static extern short GetKeyState(int nVirtKey);

        //[DllImport("user32.dll", SetLastError = true)] static public extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        internal const int WM_INPUT = 0x00FF;
        internal const int WM_HOTKEY = 0x312;
        internal const int VK_CONTROL = 0x11;

        internal const int VK_MBUTTON = 0x04;
        internal const int VK_LBUTTON = 0x01;
        internal const int VK_RBUTTON = 0x02;
        internal const int VK_XBUTTON1 = 0x05;
        internal const int VK_XBUTTON2 = 0x06;
        /// <summary>
        /// Struct representing a point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT
        {
            internal int X;
            internal int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")] internal static extern bool GetCursorPos(out POINT lpPoint);

        internal static Point GetCursorPosition()
        {
            GetCursorPos(out POINT lpPoint);
            // NOTE: If you need error handling
            // bool success = GetCursorPos(out lpPoint);
            // if (!success)

            return lpPoint;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            internal int left; // Specifies the x-coordinate of the upper-left corner of the rectangle.
            internal int top; // Specifies the y-coordinate of the upper-left corner of the rectangle.
            internal int right; // Specifies the x-coordinate of the lower-right corner of the rectangle.
            internal int bottom; // Specifies the y-coordinate of the lower-right corner of the rectangle.
            
            //public static implicit operator Rect(RECT point)
            //{
                //return new Rect(point.left, point.top, point.right, point.bottom);
            //}
        }

        [DllImport("user32.dll")] internal static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        internal static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, UIntPtr dwExtraInfo);
        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const uint MOUSEEVENTF_RIGHTUP = 0x10;

        internal static void SendMouseLeftClick()
        {
            Thread.Sleep(50);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            /*
            GetCursorPos(out POINT lpPoint);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)lpPoint.X, (uint)lpPoint.Y, 0, 0);*/
        }

        [DllImport("user32.dll")] internal static extern int GetWindowRect(IntPtr hWnd, out RECT lpRect);
        //[DllImport("user32.dll")] internal static extern bool GetClipCursor(out RECT lpRect);

        [DllImport("user32.dll")]
        internal static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        internal static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        internal static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

        const int WS_EX_TRANSPARENT = 0x00000020;

        [DllImport("user32.dll")]
        internal static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        internal static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        internal static void SetWindowExTransparent(IntPtr hwnd)
        {
            var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }

        /* GDI32 */
        internal const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter
        [DllImport("gdi32.dll")]
        internal static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
            int nWidth, int nHeight, IntPtr hObjectSource,
            int nXSrc, int nYSrc, int dwRop);
        [DllImport("gdi32.dll")]
        internal static extern bool StretchBlt(IntPtr hObject, int xDest, int yDest,
            int wDest, int hDest, IntPtr hObjectSource,
            int xSrc, int ySrc, int wSrc, int hSrc, int dwRop);
        [DllImport("gdi32.dll")]
        internal static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
            int nHeight);
        [DllImport("gdi32.dll")]
        internal static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        internal static extern bool DeleteDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        internal static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")]
        internal static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        //internal const int WM_ACTIVATEAPP = 0x1c;
        //internal const int WM_KILLFOCUS = 0x0008; 
        //internal const int WM_SETFOCUS = 0x0007;
        /*
        [DllImport("user32.dll")] 
        static public extern void mouse_event(uint dwFlags, uint dx, uint dy, int dwData, int dwExtraInfo);
        public const int MOUSEEVENTF_WHEEL = 0x0800;
        */
        // RAW INPUTS :
        /*
        [DllImport("User32.dll")]
        internal extern static bool RegisterRawInputDevices(RawInputDevice[] pRawInputDevice, uint uiNumDevices, uint cbSize);

        [DllImport("User32.dll")]
        internal extern static uint GetRawInputDeviceList(IntPtr pRawInputDeviceList, ref uint uiNumDevices, uint cbSize);

        [DllImport("User32.dll")]
        internal extern static uint GetRawInputDeviceInfo(IntPtr hDevice, uint uiCommand, IntPtr pData, ref uint pcbSize);

        [DllImport("User32.dll", SetLastError = true)]
        internal static extern int GetRawInputData(IntPtr hRawInput, DataCommand command, [Out] out InputData buffer, [In, Out] ref int size, int cbSizeHeader);

        [DllImport("User32.dll", SetLastError = true)]
        internal static extern int GetRawInputData(IntPtr hRawInput, DataCommand command, [Out] IntPtr pData, [In, Out] ref int size, int sizeHeader);

        [StructLayout(LayoutKind.Sequential)]
        internal struct RawInputDevice
        {
            internal HidUsagePage UsagePage;
            internal HidUsage Usage;
            internal RawInputDeviceFlags Flags;
            internal IntPtr Target;

            public override string ToString()
            {
                return string.Format("{0}/{1}, flags: {2}, target: {3}", UsagePage, Usage, Flags, Target);
            }
        }

        public enum DataCommand : uint
        {
            RID_HEADER = 0x10000005, // Get the header information from the RAWINPUT structure.
            RID_INPUT = 0x10000003   // Get the raw data from the RAWINPUT structure.
        }

        public enum HidUsagePage : ushort
        {
            UNDEFINED = 0x00,   // Unknown usage page
            GENERIC = 0x01,     // Generic desktop controls
            SIMULATION = 0x02,  // Simulation controls
            VR = 0x03,          // Virtual reality controls
            SPORT = 0x04,       // Sports controls
            GAME = 0x05,        // Games controls
            KEYBOARD = 0x07,    // Keyboard controls
        }

        public enum HidUsage : ushort
        {
            Undefined = 0x00,       // Unknown usage
            Pointer = 0x01,         // Pointer
            Mouse = 0x02,           // Mouse
            Joystick = 0x04,        // Joystick
            Gamepad = 0x05,         // Game Pad
            Keyboard = 0x06,        // Keyboard
            Keypad = 0x07,          // Keypad
            SystemControl = 0x80,   // Muilt-axis Controller
            Tablet = 0x80,          // Tablet PC controls
            Consumer = 0x0C,        // Consumer
        }

        public enum MouseButton : ushort // usButtonData
        {
            WheelUp = 0x0078,           // Mouse up wheel
            WheelDown = 0xff88,         // Mouse down wheel
        }

        [Flags]
        internal enum RawInputDeviceFlags
        {
            NONE = 0,                   // No flags
            REMOVE = 0x00000001,        // Removes the top level collection from the inclusion list. This tells the operating system to stop reading from a device which matches the top level collection. 
            EXCLUDE = 0x00000010,       // Specifies the top level collections to exclude when reading a complete usage page. This flag only affects a TLC whose usage page is already specified with PageOnly.
            PAGEONLY = 0x00000020,      // Specifies all devices whose top level collection is from the specified UsagePage. Note that Usage must be zero. To exclude a particular top level collection, use Exclude.
            NOLEGACY = 0x00000030,      // Prevents any devices specified by UsagePage or Usage from generating legacy messages. This is only for the mouse and keyboard.
            INPUTSINK = 0x00000100,     // Enables the caller to receive the input even when the caller is not in the foreground. Note that WindowHandle must be specified.
            CAPTUREMOUSE = 0x00000200,  // Mouse button click does not activate the other window.
            NOHOTKEYS = 0x00000200,     // Application-defined keyboard device hotkeys are not handled. However, the system hotkeys; for example, ALT+TAB and CTRL+ALT+DEL, are still handled. By default, all keyboard hotkeys are handled. NoHotKeys can be specified even if NoLegacy is not specified and WindowHandle is NULL.
            APPKEYS = 0x00000400,       // Application keys are handled.  NoLegacy must be specified.  Keyboard only.

            // Enables the caller to receive input in the background only if the foreground application does not process it. 
            // In other words, if the foreground application is not registered for raw input, then the background application that is registered will receive the input.
            EXINPUTSINK = 0x00001000,
            DEVNOTIFY = 0x00002000
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct RawData
        {
            [FieldOffset(0)]
            internal Rawmouse mouse;
            [FieldOffset(0)]
            internal Rawkeyboard keyboard;
            [FieldOffset(0)]
            internal Rawhid hid;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct InputData
        {
            public Rawinputheader header;           // 64 bit header size: 24  32 bit the header size: 16
            public RawData data;                    // Creating the rest in a struct allows the header size to align correctly for 32/64 bit
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Rawinputheader
        {
            public uint dwType;                     // Type of raw input (RIM_TYPEHID 2, RIM_TYPEKEYBOARD 1, RIM_TYPEMOUSE 0)
            public uint dwSize;                     // Size in bytes of the entire input packet of data. This includes RAWINPUT plus possible extra input reports in the RAWHID variable length array. 
            public IntPtr hDevice;                  // A handle to the device generating the raw input data. 
            public IntPtr wParam;                   // RIM_INPUT 0 if input occurred while application was in the foreground else RIM_INPUTSINK 1 if it was not.

            public override string ToString()
            {
                return string.Format("RawInputHeader\n dwType : {0}\n dwSize : {1}\n hDevice : {2}\n wParam : {3}", dwType, dwSize, hDevice, wParam);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Rawhid
        {
            public uint dwSizHid;
            public uint dwCount;
            public byte bRawData;

            public override string ToString()
            {
                return string.Format("Rawhib\n dwSizeHid : {0}\n dwCount : {1}\n bRawData : {2}\n", dwSizHid, dwCount, bRawData);
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct Rawmouse
        {
            [FieldOffset(0)]
            public ushort usFlags;
            [FieldOffset(4)]
            public uint ulButtons;
            [FieldOffset(4)]
            public ushort usButtonFlags;
            [FieldOffset(6)]
            public ushort usButtonData;
            [FieldOffset(8)]
            public uint ulRawButtons;
            [FieldOffset(12)]
            public int lLastX;
            [FieldOffset(16)]
            public int lLastY;
            [FieldOffset(20)]
            public uint ulExtraInformation;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Rawkeyboard
        {
            public ushort Makecode;                 // Scan code from the key depression
            public ushort Flags;                    // One or more of RI_KEY_MAKE, RI_KEY_BREAK, RI_KEY_E0, RI_KEY_E1
            private readonly ushort Reserved;       // Always 0    
            public ushort VKey;                     // Virtual Key Code
            public uint Message;                    // Corresponding Windows message for exmaple (WM_KEYDOWN, WM_SYASKEYDOWN etc)
            public uint ExtraInformation;           // The device-specific addition information for the event (seems to always be zero for keyboards)

            public override string ToString()
            {
                return string.Format("Rawkeyboard\n Makecode: {0}\n Makecode(hex) : {0:X}\n Flags: {1}\n Reserved: {2}\n VKeyName: {3}\n Message: {4}\n ExtraInformation {5}\n",
                                                    Makecode, Flags, Reserved, VKey, Message, ExtraInformation);
            }
        }
        */
    }
}
