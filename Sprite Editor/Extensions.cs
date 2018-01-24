using System;
using System.Windows.Media;

namespace SPE
{
    public static class Extensions
    {
        public static System.Drawing.SolidBrush ToSolidBrush(this Color color)
        {
            return new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        // From https://stackoverflow.com/a/2683487
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            return val.CompareTo(min) < 0 ? min : (val.CompareTo(max) > 0 ? max : val);
        }
    }
}
