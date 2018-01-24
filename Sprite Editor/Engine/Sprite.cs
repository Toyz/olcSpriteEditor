using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SPE.Engine
{
    public class Sprite : IEquatable<Sprite>
    {
        public static int SpriteBlockSize { get; } = 32;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public short[] Colours { get; private set; }
        public char[] Glyphs { get; private set; }

        public string File { get; private set; } = string.Empty;

        // Load Sprite from file
        public Sprite(string file)
        {
            File = file;

            Load();
        }

        public Sprite(int width, int height)
        {
            Width = width;
            Height = height;

            Colours = new short[Width * Height];

            var black = ColourHandler.GetByHex("000000");
        
            for (var i = 0; i < Colours.Length; i++) Colours[i] = black.Code;

            Glyphs = new char[Width * Height];
        }

        public short GetColour(int x, int y)
        {
            return x < 0 || x > Width || y < 0 || y > Height ? (short)0 : Colours[y * Width + x];
        }

        public void SetColour(int x, int y, Colour color)
        {
            if (x < 0 || x > Width || y < 0 || y > Height)
                return;
            Colours[y * Width + x] = color.Code;
        }

        public char GetGlyph(int x, int y)
        {
            return x < 0 || x > Width || y < 0 || y > Height ? (char)0 : Glyphs[y * Width + x];
        }

        public void SetGlyph(int x, int y, Pixal glyph)
        {
            if (x < 0 || x > Width || y < 0 || y > Height)
                return;
            Glyphs[y * Width + x] = (char) glyph;
        }

        public void Save()
        {
            Save(File);
        }

        public void Save(string file)
        {
            File = file;

            using (var writer = new BinaryWriter(System.IO.File.Open(file, FileMode.OpenOrCreate)))
            {
                writer.Write(Width);
                writer.Write(Height);

                for (var i = 0; i < Width * Height; i++)
                {
                    writer.Write(Colours[i]);
                }

                for (var i = 0; i < Width * Height; i++)
                {
                    writer.Write(Glyphs[i]);
                }
            }
        }

        private void Load()
        {
            using (var reader = new BinaryReader(System.IO.File.Open(File, FileMode.Open)))
            {
                Width = reader.ReadInt32();
                Height = reader.ReadInt32();
                Colours = new short[Width * Height];
                Glyphs = new char[Width * Height];

                for (var i = 0; i < Width * Height; i++)
                {
                    Colours[i] = reader.ReadInt16();
                }

                for (var i = 0; i < Width * Height; i++)
                {
                    Glyphs[i] = reader.ReadChar();
                }
            }
        }

        public override string ToString()
        {
            return $"Width: {Width} Height: {Height} " +
                   $"Colours: {string.Join(",", Colours)} " +
                   $"Glyphs: {string.Join(",", Glyphs)}";
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Sprite);
        }

        public bool Equals(Sprite other)
        {
            return other != null &&
                   Width == other.Width &&
                   Height == other.Height &&
                   EqualityComparer<short[]>.Default.Equals(Colours, other.Colours) &&
                   EqualityComparer<char[]>.Default.Equals(Glyphs, other.Glyphs) &&
                   File == other.File;
        }

        public override int GetHashCode()
        {
            var hashCode = 265894640;
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<short[]>.Default.GetHashCode(Colours);
            hashCode = hashCode * -1521134295 + EqualityComparer<char[]>.Default.GetHashCode(Glyphs);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(File);
            return hashCode;
        }

        public static bool operator ==(Sprite x, Sprite y)
        {
            return x != null && x.Equals(y);
        }

        public static bool operator !=(Sprite x, Sprite y)
        {
            return !(x == y);
        }
    }

}
