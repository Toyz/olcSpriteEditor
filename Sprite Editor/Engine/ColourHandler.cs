using System.Collections.Generic;
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
    }
}
