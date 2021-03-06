﻿using System;
using System.Collections.Generic;
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

        public static float GetHue(this Color c) =>
            System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B).GetHue();

        public static float GetBrightness(this Color c) =>
            System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B).GetBrightness();

        public static float GetSaturation(this Color c) =>
            System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B).GetSaturation();

        public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (predicate == null) throw new ArgumentNullException("predicate");

            int retVal = 0;
            foreach (var item in items)
            {
                if (predicate(item)) return retVal;
                retVal++;
            }
            return -1;
        }

        public static int IndexOf<T>(this IEnumerable<T> items, T item) { return items.FindIndex(i => EqualityComparer<T>.Default.Equals(item, i)); }
    }
}
