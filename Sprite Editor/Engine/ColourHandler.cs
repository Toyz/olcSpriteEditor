using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SPE.Engine
{
    public static class ColourHandler
    {
        public static List<Colour> SystemColours;

        static ColourHandler()
        {
            SystemColours = new List<Colour>();

            if (SystemColours.Count <= 0)
            {
                foreach (var line in File.ReadAllLines("./colours.txt"))
                {
                    if (line.StartsWith("#") || string.IsNullOrEmpty(line)) continue;

                    SystemColours.Add(new Colour(line));
                }
            }
        }

        public static Colour ByHex(string hex)
        {
            return SystemColours.FirstOrDefault(x => x.Hex.Equals(hex));
        }

        public static Colour ByRgb(int r, int g, int b)
        {
            return SystemColours.FirstOrDefault(x => x.R == r && x.G == g && x.B == b);
        }

        public static Colour ByCode(short code)
        {
            return SystemColours.FirstOrDefault(x => x.Code == code);
        }
    }
}
