using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using Run.Util.Interop;
using TesseractOCR;

namespace Run.Util
{
    /// <summary>
    /// Centralize generalist methods necessary for the application.
    /// </summary>
    /// <remarks>This is a static class and can not be instancied.</remarks>
    internal static class Common
    {
        internal static string GetInnerExceptionMessages(Exception exp)
        {
            StringBuilder sbMessage = new();
            Exception innerException = exp;
            int watchdog = 0;
            do
            {
                if (!string.IsNullOrEmpty(innerException.Message))
                {
                    if (!sbMessage.ToString().Contains(innerException.Message, StringComparison.Ordinal))
                    {
                        sbMessage.AppendLine();
                        sbMessage.Append(innerException.Message);
                    }
                }
                innerException = innerException.InnerException;
                watchdog++;
            }
            while (innerException != null && watchdog <= 20);

            return sbMessage.ToString();
        }

        internal static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        internal static string GetFileVersion()
        {
            //string old = Process.GetCurrentProcess().MainModule.FileName;
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(Environment.ProcessPath);
            return fvi.FileVersion;
        }

        internal static double StrToDouble(string s, double def = 0)
        {
            if (s?.Length > 0)
            {
                try
                {
                    def = double.Parse(s, CultureInfo.InvariantCulture); // correction
                }
                catch (Exception ex)
                {
                    Helper.Debug.Trace("Exception using double parsing : " + ex.Message);
                }
            }

            return def;
        }

        internal static void CloseWindow(string windowName)
        {
            IntPtr pHwnd = NativeWin.FindWindow(null, windowName);
            if (pHwnd.ToInt32() > 0)
            {
                NativeWin.SendMessage(pHwnd, NativeWin.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
        }

        internal static bool IsKeyPushedDown(Keys vKey)
        {
            if (Global.ModifierKeyList.Contains(vKey))
            {
                return 0 != (NativeWin.GetAsyncKeyState((int)vKey));
            }
            return 0 != (NativeWin.GetAsyncKeyState((int)vKey) & 0x8000);
        }

        internal static bool IsLeftButtonMousePushedOnce()
        {
            return 0 != (NativeWin.GetAsyncKeyState(NativeWin.VK_LBUTTON) & 1);
        }

        internal static bool IsMiddleButtonMousePushedOnce()
        {
            return 0 != (NativeWin.GetAsyncKeyState(NativeWin.VK_MBUTTON) & 1);
        }

        internal static bool IsRightButtonMousePushedOnce()
        {
            return 0 != (NativeWin.GetAsyncKeyState(NativeWin.VK_RBUTTON) & 1);
        }

        internal static bool IsX1ButtonMousePushedOnce()
        {
            return 0 != (NativeWin.GetAsyncKeyState(NativeWin.VK_XBUTTON1) & 1);
        }

        internal static bool IsX2ButtonMousePushedOnce()
        {
            return 0 != (NativeWin.GetAsyncKeyState(NativeWin.VK_XBUTTON1) & 1);
        }

        internal static string ConvertWidthCursorPosition(int abscissa, int multiplier)
        {
            double calc = abscissa;
            calc = (calc / Global.HalfScreenWidth) * Global.SCALING_VALUE;
            int val = (Convert.ToInt32(calc) - Global.SCALING_VALUE) + Global.SCALING_VALUE * multiplier;
            if (val == 0)
            {
                return Strings.Aim.NOVALUE;
            }
            return val.ToString("G", CultureInfo.InvariantCulture);
        }

        private static readonly int VALMIN = -80;
        private static readonly int VALMAX = 80;

        internal static int UpdateCrosshairAbscissa(int abscissa, int multiplier)
        {
            double calc = abscissa;
            calc = (calc / Global.HalfScreenWidth) * Global.SCALING_VALUE;
            int val = (Convert.ToInt32(calc) - Global.SCALING_VALUE) + Global.SCALING_VALUE * multiplier;

            if (val < VALMIN)
            {
                return VALMIN;
            }
            if (val > VALMAX)
            {
                return VALMAX;
            }
            return val;
        }

        internal static string ConvertHeightCursorPosition(int ordinate, int multiplier)
        {
            double calc = ordinate;
            calc = (calc / Global.HalfScreenHeight) * Global.SCALING_VALUE;
            int val = (Convert.ToInt32(calc) - Global.SCALING_VALUE) + Global.SCALING_VALUE * multiplier;
            //Trace.WriteLine(val);
            if (val == 0)
            {
                return Strings.Aim.NOVALUE;
            }
            val = Global.DataJson.Config.Options.InvertedMouse ? val : val * -1;

            if(Global.DataJson.Config.Options.ConvertIndicator)
            {
                double convValue = val;
                if (!Global.DataJson.Config.Options.SteadyAim)
                {
                    convValue *= Global.RATIO_WITHOUT_STEADY;
                }
                convValue = Global.DataJson.Config.Options.SchreckZook ?
                    convValue * Global.RATIO_PANZERSCHRECK : convValue * Global.RATIO_BAZOOKA;

                val = Convert.ToInt32(convValue);

                return val.ToString("G", CultureInfo.InvariantCulture) + 'm';
                /*
                return Global.DataJson.Config.Options.SchreckZook ?
                    val.ToString() + (Global.DataJson.Config.Options.SteadyAim ? Strings.Aim.SCHRECK_METER_STEADY : Strings.Aim.SCHRECK_METER) 
                    : val.ToString() + (Global.DataJson.Config.Options.SteadyAim ? Strings.Aim.BAZOOKA_METER_STEADY : Strings.Aim.BAZOOKA_METER);
                */
            }
            return val.ToString("G", CultureInfo.InvariantCulture);
        }

        internal static string GetRocketIndicatorKind()
        {
            if (Global.DataJson.Config.Options.ConvertIndicator)
            {
                return Global.DataJson.Config.Options.SchreckZook ?
                    (Global.DataJson.Config.Options.SteadyAim ? Strings.Aim.SCHRECK_STEADY : Strings.Aim.SCHRECK)
                    : (Global.DataJson.Config.Options.SteadyAim ? Strings.Aim.BAZOOKA_STEADY : Strings.Aim.BAZOOKA);
            }
            return string.Empty;
        }

        internal static bool GetRocketKind()
        {
            return Global.DataJson.Config.Options.SchreckZook;
        }

        internal static bool IsHllLaunchedAndFocused()
        {
            IntPtr findHwnd = NativeWin.FindWindow(Strings.HllClass, Strings.HllCaption);
            if (findHwnd.ToInt32() > 0)
            {
                return NativeWin.GetForegroundWindow().Equals(findHwnd);
            }
            return false;
        }

        internal static void GenerateMapHistory(string mapFolderName, Stopwatch watch) // WIP
        {
            //List<Task> taskList = new();

            string path = Path.GetFullPath("MapHistory\\");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path += mapFolderName + "\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //string filePath = path + DateTime.UtcNow.ToString("yyMMddUTCHHmmss") + ".png";

            string elapsedTime = String.Format("{0:00}h{1:00}m{2:00}s", watch.Elapsed.Hours, watch.Elapsed.Minutes, watch.Elapsed.Seconds);
            string filePath = path + elapsedTime + ".jpg";

            if (!File.Exists(filePath))
            {
                try
                {
                    /* NOT NEEDED
                    IntPtr hllHwnd = Native.FindWindow(Strings.HllClass, Strings.HllCaption);
                    sc.CaptureWindowToFile(hllHwnd, filePath, ImageFormat.Png);
                    */
                    ScreenCapture.CaptureScreenToFile(filePath, ImageFormat.Jpeg, watch);
                }
                catch (Exception ex)
                {
                    WindowMessage.SendForeground(String.Format("{0} Error:  {1}\r\n\r\n{2}\r\n\r\n", ex.Source, ex.Message, ex.StackTrace), "GenerateMapHistory() encountered an error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                GC.Collect();
            }
        }

        internal static bool JoinQueue()
        {
            bool returnVal = false;
            try
            {
                var screen = ScreenCapture.CaptureScreen();
                //screen.Save("./test_screen.png", ImageFormat.Png);

                var resolution = new KeyValuePair<int, int>(screen.Width, screen.Height);
                HllArea area = new(resolution);
                Bitmap vip = ScreenCapture.CropImage(screen, area.Vip);

                using var stream = new MemoryStream();
                vip.Save(stream, ImageFormat.Png);
                //vip.Save("./test_vip.png", ImageFormat.Png);
                byte[] fileBytes = stream.ToArray();

                using var engine = new Engine(Strings.OcrPath, TesseractOCR.Enums.Language.English, TesseractOCR.Enums.EngineMode.Default);
                using var img = TesseractOCR.Pix.Image.LoadFromMemory(fileBytes);
                using var page = engine.Process(img);
                if (page.Text.Length > 0)
                {
                    int firstIdx = page.Text.LastIndexOf('(');
                    int secondIdx = page.Text.LastIndexOf(')');
                    if (firstIdx > 0 && secondIdx > firstIdx)
                    {
                        string extract = page.Text.Substring(firstIdx + 1, secondIdx - firstIdx - 1);
                        string[] queue = extract.Split('/');
                        if (queue.Length == 2)
                        {
                            if (Int32.TryParse(queue[0], out int peopleInQueue) && Int32.TryParse(queue[1], out int maxQueue))
                            {
                                //Console.WriteLine($"Total people in queue : {peopleInQueue} | Maximum queue : {maxQueue}");
                                if (peopleInQueue < maxQueue)
                                {
                                    // DO SOMETHING
                                    int x = area.Join.X + area.Join.Width / 2;
                                    int y = area.Join.Y + area.Join.Height / 2;
                                    NativeWin.SetCursorPos(x, y);
                                    NativeWin.SendMouseLeftClick();
                                    /*
                                    TestArea(screen, area.Ok, "ok");
                                    TestArea(screen, area.Cancel, "annuler");*/
                                }
                                returnVal = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WindowMessage.SendForeground(String.Format("{0} Error:  {1}\r\n\r\n{2}\r\n\r\n", ex.Source, ex.Message, ex.StackTrace), "JoinQueue() encountered an error", MessageBoxButton.OK, MessageBoxImage.Error);
                //throw;
            }
            GC.Collect();
            return returnVal;
        }

        internal static bool ByPassFullQueue()
        {
            bool returnVal = false;
            try
            {
                var screen = ScreenCapture.CaptureScreen();

                var resolution = new KeyValuePair<int, int>(screen.Width, screen.Height);
                HllArea area = new(resolution);
                Bitmap ok = ScreenCapture.CropImage(screen, area.Ok);

                using var stream = new MemoryStream();
                ok.Save(stream, ImageFormat.Png);
                //ok.Save("./test_ok.png", ImageFormat.Png);
                byte[] fileBytes = stream.ToArray();

                using var engine = new Engine(Strings.OcrPath, TesseractOCR.Enums.Language.English, TesseractOCR.Enums.EngineMode.Default);
                using var img = TesseractOCR.Pix.Image.LoadFromMemory(fileBytes);
                using var page = engine.Process(img);
                if (page.Text.Length > 0)
                {
                    foreach (string value in Global.BoxOk)
                    {
                        if (page.Text.ToLower().Contains(value, StringComparison.Ordinal)) // NEED TO TEST WITH ALL LANG
                        {
                            int x = area.Ok.X + area.Ok.Width / 2;
                            int y = area.Ok.Y + area.Ok.Height / 2;
                            NativeWin.SetCursorPos(x, y);
                            NativeWin.SendMouseLeftClick();

                            returnVal = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WindowMessage.SendForeground(String.Format("{0} Error:  {1}\r\n\r\n{2}\r\n\r\n", ex.Source, ex.Message, ex.StackTrace), "ByPassFullQueue() encountered an error", MessageBoxButton.OK, MessageBoxImage.Error);
                //throw;
            }
            GC.Collect();
            return returnVal;
        }

        internal static bool IsInQueue()
        {
            bool returnVal = false;
            try
            {
                var screen = ScreenCapture.CaptureScreen();
                var resolution = new KeyValuePair<int, int>(screen.Width, screen.Height);
                HllArea area = new(resolution);
                Bitmap cancel = ScreenCapture.CropImage(screen, area.Cancel);

                using var stream = new MemoryStream();
                cancel.Save(stream, ImageFormat.Png);
                //cancel.Save("./test_cancel.png", ImageFormat.Png);
                byte[] fileBytes = stream.ToArray();

                using var engine = new Engine(Strings.OcrPath, TesseractOCR.Enums.Language.English, TesseractOCR.Enums.EngineMode.Default);
                using var img = TesseractOCR.Pix.Image.LoadFromMemory(fileBytes);
                using var page = engine.Process(img);
                if (page.Text.Length > 0)
                {
                    foreach (string value in Global.BoxCancel)
                    {
                        if (page.Text.ToLower().Contains(value, StringComparison.Ordinal))
                        {
                            returnVal = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WindowMessage.SendForeground(String.Format("{0} Error:  {1}\r\n\r\n{2}\r\n\r\n", ex.Source, ex.Message, ex.StackTrace), "IsInQueue() encountered an error", MessageBoxButton.OK, MessageBoxImage.Error);
                //throw;
            }
            GC.Collect();
            return returnVal;
        }

        internal static int AdjustAbscissaValue(int val)
        {
            if (SystemParameters.PrimaryScreenHeight is 1080)
            {
                return val;
            }
            double ratio = SystemParameters.PrimaryScreenHeight / 1080;
            return Convert.ToInt32(val * ratio);
        }
        /*
        private static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = System.Windows.Media.VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
        */
    }
}
