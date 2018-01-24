using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace SPE.Engine
{
    public enum Colours : short
    {
        FG_BLACK = 0x0000,
        FG_DARK_BLUE = 0x0001,
        FG_DARK_GREEN = 0x0002,
        FG_DARK_CYAN = 0x0003,
        FG_DARK_RED = 0x0004,
        FG_DARK_MAGENTA = 0x0005,
        FG_DARK_YELLOW = 0x0006,
        FG_GREY = 0x0007, // Thanks MS :-/
        FG_DARK_GREY = 0x0008,
        FG_BLUE = 0x0009,
        FG_GREEN = 0x000A,
        FG_CYAN = 0x000B,
        FG_RED = 0x000C,
        FG_MAGENTA = 0x000D,
        FG_YELLOW = 0x000E,
        FG_WHITE = 0x000F,
        BG_BLACK = 0x0000,
        BG_DARK_BLUE = 0x0010,
        BG_DARK_GREEN = 0x0020,
        BG_DARK_CYAN = 0x0030,
        BG_DARK_RED = 0x0040,
        BG_DARK_MAGENTA = 0x0050,
        BG_DARK_YELLOW = 0x0060,
        BG_GREY = 0x0070,
        BG_DARK_GREY = 0x0080,
        BG_BLUE = 0x0090,
        BG_GREEN = 0x00A0,
        BG_CYAN = 0x00B0,
        BG_RED = 0x00C0,
        BG_MAGENTA = 0x00D0,
        BG_YELLOW = 0x00E0,
        BG_WHITE = 0x00F0,
    };

    public enum Pixal : short
    {
        PIXEL_SOLID = 0x2588,
        PIXEL_THREEQUARTERS = 0x2593,
        PIXEL_HALF = 0x2592,
        PIXEL_QUARTER = 0x2591
    }

    public class Colour : IEquatable<Colour>
    {
        public Colour(int r, int g, int b, Colours fg, Colours bg, Pixal type)
        {
            R = r;
            G = g;
            B = b;
            FG = fg;
            BG = bg;
            PT = type;
        }

        public Colour(string line)
        {
            var lineData = line.ToUpper().Split(',');

            R = (int) (float.Parse(lineData[0].Trim()) * 255);
            G = (int) (float.Parse(lineData[1].Trim()) * 255);
            B = (int) (float.Parse(lineData[2].Trim()) * 255);

            FG = (Colours)Enum.Parse(typeof(Colours), lineData[3].Trim());
            BG = (Colours)Enum.Parse(typeof(Colours), lineData[4].Trim());
            PT = (Pixal)Enum.Parse(typeof(Pixal), lineData[5].Trim());
        }

        public int R { get; }
        public int G { get; }
        public int B { get; }

        public Colours FG { get; }
        public Colours BG { get; }
        public Pixal PT { get; }

        public string Hex => $"{R:X2}{G:X2}{B:X2}";
        public Color Color => Color.FromRgb((byte)R, (byte)G, (byte)B);
        public short Code => (short)((byte)BG | (byte)FG);

        public override bool Equals(object obj)
        {
            return Equals(obj as Colour);
        }

        public bool Equals(Colour other)
        {
            return other != null &&
                   R == other.R &&
                   G == other.G &&
                   B == other.B &&
                   FG == other.FG &&
                   BG == other.BG &&
                   PT == other.PT &&
                   Hex == other.Hex &&
                   Color.Equals(other.Color) &&
                   Code == other.Code;
        }

        public override int GetHashCode()
        {
            var hashCode = 29083839;
            hashCode = hashCode * -1521134295 + R.GetHashCode();
            hashCode = hashCode * -1521134295 + G.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            hashCode = hashCode * -1521134295 + FG.GetHashCode();
            hashCode = hashCode * -1521134295 + BG.GetHashCode();
            hashCode = hashCode * -1521134295 + PT.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Hex);
            hashCode = hashCode * -1521134295 + EqualityComparer<Color>.Default.GetHashCode(Color);
            hashCode = hashCode * -1521134295 + Code.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"HEX: {Hex} RGB: {R}, {G}, {B} Code: {Code}";
        }
    }
}
