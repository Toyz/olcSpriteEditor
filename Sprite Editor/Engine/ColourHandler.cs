using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SPE.Engine
{
    public static class ColourHandler
    {
        public static List<Colour> Colours { get; }
        private static List<Colour> AllColours { get; }

        static ColourHandler()
        {
            Colours = AllColours = new List<Colour>();

            if (Colours.Count <= 0)
            {
                foreach (var line in File.ReadAllLines("./colours.txt"))
                {
                    if (line.StartsWith("#") || string.IsNullOrEmpty(line)) continue;

                    AllColours.Add(new Colour(line));
                }

                //Colours.Sort(SortColors);

                // TODO: Fix Sorting by color
                // Colours.Sort(SortColors2);
            }

            Colours.Insert(0, new Colour(0, 0, 0, 150, Engine.Colours.BG_BLACK, Engine.Colours.BG_BLACK, Pixal.PIXEL_SPACE));

            Colours = new List<Colour>(AllColours);
        }

        public static void SwapColours(bool all)
        {
            Colours.Clear();

            if (all)
            {
                foreach (var c in AllColours)
                {
                    Colours.Add(c);
                }
            }
            else
            {
                foreach(var c in AllColours.Take(17).ToList())
                    Colours.Add(c);
            }
        }

        public static Colour ByHex(string hex, Pixal pixal)
        {
            return Colours.FirstOrDefault(x => x.Hex.Equals(hex) && x.PT == pixal);
        }

        public static Colour ByHex(string hex)
        {
            return Colours.FirstOrDefault(x => x.Hex.Equals(hex));
        }

        public static Colour ByRgb(int r, int g, int b)
        {
            return Colours.FirstOrDefault(x => x.R == r && x.G == g && x.B == b);
        }

        public static Colour ByRgb(int r, int b, int g, int a)
        {
            return Colours.FirstOrDefault(x => x.R == r && x.G == g && x.B == b && x.A == a);
        }

        public static Colour ByCode(short code)
        {
            return Colours.FirstOrDefault(x => x.Code == code);
        }

        public static Colour ByCode(short code, Pixal pixal)
        {
            return Colours.FirstOrDefault(x => x.Code == code && x.PT == pixal);
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
