using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Diagnostics;
using Run.Util.Interop;
using System.Drawing.Drawing2D;
using System.IO;
using TesseractOCR;

namespace Run.Util
{
    /// <summary>
    /// Provides methods to capture screen or a particular window, crop image, check hll map, save to file.
    /// </summary>
    /// <remarks>This is a static class and can not be instancied.</remarks>
    internal static class ScreenCapture
    {
        /// <summary>
        /// Creates an Image object containing a screen shot of the entire desktop
        /// </summary>
        /// <returns></returns>
        internal static Image CaptureScreen()
        {
            return CaptureWindow(NativeWin.GetDesktopWindow());
        }
        /// <summary>
        /// Creates an Image object containing a screen shot of a specific window
        /// </summary>
        /// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
        /// <returns></returns>
        internal static Image CaptureWindow(IntPtr handle)
        {
            // get te hDC of the target window
            IntPtr hdcSrc = NativeWin.GetWindowDC(handle);
            // get the size
            //NativeWin.RECT windowRect = new();
            NativeWin.GetWindowRect(handle, out NativeWin.RECT windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            IntPtr hdcDest = NativeWin.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = NativeWin.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = NativeWin.SelectObject(hdcDest, hBitmap);
            // bitblt over
            NativeWin.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, NativeWin.SRCCOPY);
            //NativeWin.StretchBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, width, height, NativeWin.SRCCOPY);

            // restore selection
            NativeWin.SelectObject(hdcDest, hOld);
            // clean up
            NativeWin.DeleteDC(hdcDest);
            NativeWin.ReleaseDC(handle, hdcSrc);
            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);

            // free up the Bitmap object
            NativeWin.DeleteObject(hBitmap);
            return img;
        }

        internal static Image CaptureWindowBis(IntPtr handle)
        {
            NativeWin.GetWindowRect(handle, out NativeWin.RECT rc);
            int width = rc.right - rc.left;
            int height = rc.bottom - rc.top;
            Bitmap bmp = new(width, height);
            Graphics gfxBmp = Graphics.FromImage(bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();

            NativeWin.PrintWindow(handle, hdcBitmap, 1);

            gfxBmp.ReleaseHdc(hdcBitmap);
            gfxBmp.Dispose();

            return bmp;
        }
        /// <summary>
        /// Captures a screen shot of a specific window, and saves it to a file
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        internal static void CaptureWindowToFile(IntPtr handle, string filename, ImageFormat format)
        {
            Image img = CaptureWindow(handle);
            img.Save(filename, format);
        }
        /// <summary>
        /// Captures a screen shot of the entire desktop, and saves it to a file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        internal static void CaptureScreenToFile(string filename, ImageFormat format, Stopwatch watch)
        {
            Image screen = CaptureScreen();
            var resolution = new KeyValuePair<int, int>(screen.Width, screen.Height);
            if (Global.MapSizeList.ContainsKey(resolution))
            {
                var mapRect = Global.MapSizeList.GetValueOrDefault(resolution);
                Image map = CropImage(screen, mapRect);
                if (IsMap(map, mapRect))
                {
                    //Image map = CropImage(screen, mapRect, new(0, 0, screen.Width, screen.Height));
                    map.Save(filename, format);
                    Global.LastMapSaveTime = watch.ElapsedMilliseconds;
                }
            }
        }

        internal static Bitmap CropImage(Image imgIn, Rectangle srcRect)
        {
            Bitmap imgTmp = new(imgIn);
            Graphics g = Graphics.FromImage(imgTmp);
            Rectangle dstRect = new(0, 0, imgTmp.Width, imgTmp.Height);
            g.DrawImage(imgTmp, dstRect, srcRect, GraphicsUnit.Pixel); // ZOOM
            Bitmap imgOut = new(srcRect.Width, srcRect.Height);

            g = Graphics.FromImage(imgOut);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawImage(imgTmp, new RectangleF(0, 0, imgOut.Width, imgOut.Height)); // RESIZE
            g.Dispose();
            return imgOut;
        }

        internal static bool IsMap(Image map, Rectangle mapRect)
        {
            int headerHeight = Convert.ToInt32((mapRect.Height / 100) * 2.2);
            Image mapHeader = CropImage(map, new Rectangle(0, 0, mapRect.Width, headerHeight));

            // OCR
            using var ms = new MemoryStream();
            mapHeader.Save(ms, ImageFormat.Png);
            using var engine = new Engine(Strings.OcrPath, TesseractOCR.Enums.Language.English, TesseractOCR.Enums.EngineMode.Default);
            using var img = TesseractOCR.Pix.Image.LoadFromMemory(ms.ToArray());
            using var page = engine.Process(img);

            // page.Text.Contains("min");  
            return page.Text.Length > 20;
        }
    }
}
