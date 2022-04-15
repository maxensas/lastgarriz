using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using Lastgarriz.Util.Interop;

namespace Lastgarriz.Util
{
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
            IntPtr pHwnd = Native.FindWindow(null, windowName);
            if (pHwnd.ToInt32() > 0)
            {
                Native.SendMessage(pHwnd, Native.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
        }

        internal static bool IsKeyPushedDown(Keys vKey)
        {
            if (Global.ModifierKeyList.Contains(vKey))
            {
                return 0 != (Native.GetAsyncKeyState((int)vKey));
            }
            return 0 != (Native.GetAsyncKeyState((int)vKey) & 0x8000);
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
