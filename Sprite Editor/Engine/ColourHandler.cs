using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SPE.Engine
{
    public static class ColourHandler
    {
        public static List<Colour> Colours { get; set; }
        private static List<Colour> AllColours { get; set; }

        static ColourHandler()
        {
            if (AllColours != null) return;

            LoadColours();
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
            return Colours.FirstOrDefault(x => x.Hex.Equals(hex) && x.Pixal == pixal);
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
            return Colours.FirstOrDefault(x => x.Code == code && x.Pixal == pixal);
        }

        private static void LoadColours()
        {
            using (Stream stream = File.Open("colours.bin", FileMode.Open))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                AllColours = (List<Colour>)bformatter.Deserialize(stream);
            }

            Colours = new List<Colour>(AllColours);
        }
    }
}
