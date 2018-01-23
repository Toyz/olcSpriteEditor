using System;
using System.Drawing;

namespace olcEngineSpriteEditor
{
    public static class Helpers
    {
        public static short ToShort(ConsoleColor backgroundColor, ConsoleColor forgroundColor)
        {
            return (short)(((byte)backgroundColor << 4) | (byte)forgroundColor);
        }

        public static void FromShort(short color, out ConsoleColor backgroundColor, out ConsoleColor forgroundColor)
        {
            byte fg = (byte)(color & 0x0f);
            byte bg = (byte)((color >> 4) & 0x0f);
            backgroundColor = (ConsoleColor)bg;
            forgroundColor = (ConsoleColor)fg;
        }

        public static ConsoleColor ToConsoleColor(this Color c)
        {
            int index = (c.R > 128 | c.G > 128 | c.B > 128) ? 8 : 0; // Bright bit
            index |= (c.R > 64) ? 4 : 0; // Red bit
            index |= (c.G > 64) ? 2 : 0; // Green bit
            index |= (c.B > 64) ? 1 : 0; // Blue bit
            return (ConsoleColor)index;
        }

        public static Color ToDrawingColor(this ConsoleColor color)
        {
            switch (color)
            {
                case ConsoleColor.Black:

                    return Color.Black;
                case ConsoleColor.Blue:

                    return Color.Blue;
                case ConsoleColor.Cyan:

                    return Color.Cyan;
                case ConsoleColor.DarkBlue:

                    return Color.DarkBlue;
                case ConsoleColor.DarkGray:

                    return Color.DarkGray;
                case ConsoleColor.DarkGreen:

                    return Color.DarkGreen;
                case ConsoleColor.DarkMagenta:

                    return Color.DarkMagenta;
                case ConsoleColor.DarkRed:

                    return Color.DarkRed;
                case ConsoleColor.DarkYellow:

                    return Color.FromArgb(255, 128, 128, 0);
                case ConsoleColor.Gray:

                    return Color.Gray;
                case ConsoleColor.Green:

                    return Color.Green;
                case ConsoleColor.Magenta:

                    return Color.Magenta;
                case ConsoleColor.Red:

                    return Color.Red;
                case ConsoleColor.White:

                    return Color.White;
                default:
                    return Color.Yellow;
            }
        }
    }
}
