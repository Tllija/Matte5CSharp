using System;
using System.Collections.Generic;
using System.Text;

namespace Matte5
{
    public static class MathExt
    {
        public static double Clamp(this double n, double min, double max)
        {
            return Math.Clamp(n, min, max);
        }

        public static double Round(this double n, byte decimals)
        {
            return Math.Round(n, decimals);
        }
    }
}
