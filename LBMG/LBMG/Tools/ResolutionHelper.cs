using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Xna.Framework;
using System.Windows.Forms;

namespace LBMG.Tools
{
    public static class ResolutionHelper
    {
        public static Point GetResolution()
        {
            Point resolution = new Point
            {
                X = Screen.PrimaryScreen.Bounds.Width,
                Y = Screen.PrimaryScreen.Bounds.Height
            };

            return resolution;
        }
    }
}
