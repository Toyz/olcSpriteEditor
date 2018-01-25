﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SPE.Engine
{
    public static class ColourHandler
    {
        public static List<Colour> Colours { get; }

        static ColourHandler()
        {
            Colours = new List<Colour>();

            if (Colours.Count <= 0)
            {
                foreach (var line in File.ReadAllLines("./colours.txt"))
                {
                    if (line.StartsWith("#") || string.IsNullOrEmpty(line)) continue;

                    Colours.Add(new Colour(line));
                }

                Colours.Sort(SortColors);

                // TODO: Fix Sorting by color
                // Colours.Sort(SortColors2);
            }
        }

        public static Colour ByHex(string hex)
        {
            return Colours.FirstOrDefault(x => x.Hex.Equals(hex));
        }

        public static Colour ByRgb(int r, int g, int b)
        {
            return Colours.FirstOrDefault(x => x.R == r && x.G == g && x.B == b);
        }

        public static Colour ByCode(short code)
        {
            return Colours.FirstOrDefault(x => x.Code == code);
        }

        private static int SortColors(Colour a, Colour b) => a.Color.GetBrightness().CompareTo(b.Color.GetBrightness());

        private static int SortColors2(Colour a, Colour b)
        {
            if (a.R < b.R)
                return -1;
            if (a.R > b.R)
                return 1;
            if (a.G < b.G)
                return -1;
            if (a.G > b.G)
                return 1;
            if (a.B < b.B)
                return -1;
            if (a.B > b.B)
                return 1;

            return 0;
        }
    }
}
