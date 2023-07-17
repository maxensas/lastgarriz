using System;
using System.Collections.Generic;
using System.Drawing;

namespace Run.Util
{
    /// <summary>
    /// Contain specific area data (HLL Server browser) used for image cropping / OCR.
    /// </summary>
    /// <remarks>This class should be instancied.</remarks>
    internal class HllArea
    {
        internal Rectangle Vip { get; private set; }
        internal Rectangle Join { get; private set; }
        internal Rectangle Box { get; private set; }
        internal Rectangle Ok { get; private set; }
        internal Rectangle Cancel { get; private set; }

        internal HllArea(KeyValuePair<int, int> resolution)
        {
            Vip = new(Convert.ToInt32(resolution.Key * 0.65), Convert.ToInt32(resolution.Value * 0.48), Convert.ToInt32(resolution.Key * 0.26), Convert.ToInt32(resolution.Value * 0.03));
            Join = new(Convert.ToInt32(resolution.Key * 0.645), Convert.ToInt32(resolution.Value * 0.348), Convert.ToInt32(resolution.Key * 0.269), Convert.ToInt32(resolution.Value * 0.044));
            Box = new(Convert.ToInt32(resolution.Key * 0.368), Convert.ToInt32(resolution.Value * 0.392), Convert.ToInt32(resolution.Key * 0.263), Convert.ToInt32(resolution.Value * 0.213));
            Ok = new(Convert.ToInt32(resolution.Key * 0.474), Convert.ToInt32(resolution.Value * 0.544), Convert.ToInt32(resolution.Key * 0.052), Convert.ToInt32(resolution.Value * 0.041));
            Cancel = new(Convert.ToInt32(resolution.Key * 0.448), Convert.ToInt32(resolution.Value * 0.548), Convert.ToInt32(resolution.Key * 0.103), Convert.ToInt32(resolution.Value * 0.027));
        }
    }
}
