using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace SPE.Engine
{
    public class Sprite : IEquatable<Sprite>
    {
        private readonly MainWindow _mainWindow;

        public bool FailedToLoad { get; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public short[] Colours { get; private set; }
        public short[] Glyphs { get; private set; }

        public string File { get; private set; } = string.Empty;
        private readonly System.Windows.Threading.DispatcherTimer _dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        // Load Sprite from file
        public Sprite(string file, MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            File = file;

            FailedToLoad = Load();

            StartAutoSaving();
        }

        public Sprite(int width, int height, MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            Width = width;
            Height = height;
            FailedToLoad = false;

            Colours = new short[Width * Height];

            var black = ColourHandler.ByHex("FF000000", Pixal.PIXEL_SOLID);
        
            for (var i = 0; i < Colours.Length; i++) Colours[i] = black.Code;

            Glyphs = new short[Width * Height];
            for (var i = 0; i < Glyphs.Length; i++) Glyphs[i] = (int)Pixal.PIXEL_SOLID;

            StartAutoSaving();
        }

        ~Sprite()
        {
            _dispatcherTimer.Stop();
        }

        private void StartAutoSaving()
        {
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(30);
            _dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(File)) return;
            if (!System.IO.File.Exists(File) || FailedToLoad) return;
            Save();
            _mainWindow.WindowDataContext.CurrentProgramStatus = $"Auto Saved Project - {DateTime.Now:h:mm:ss tt}";
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

        public int GetGlyph(int x, int y)
        {
            return x < 0 || x > Width || y < 0 || y > Height ? 0 : Glyphs[y * Width + x];
        }

        public void SetGlyph(int x, int y, Pixal glyph)
        {
            if (x < 0 || x > Width || y < 0 || y > Height)
                return;
            Glyphs[y * Width + x] = (short) glyph;
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
                    var pixal = (short)ColourHandler.ByCode(Colours[i], (Pixal)Glyphs[i]).PT;
                    writer.Write(pixal);
                }

            }
        }

        private bool Load()
        {
            try
            {
                using (var reader = new BinaryReader(System.IO.File.Open(File, FileMode.Open)))
                {

                    Width = reader.ReadInt32();
                    Height = reader.ReadInt32();
                    Colours = new short[Width * Height];
                    Glyphs = new short[Width * Height];

                    for (var i = 0; i < Width * Height; i++)
                    {
                        Colours[i] = reader.ReadInt16();
                    }

                    for (var i = 0; i < Width * Height; i++)
                    {
                        var g = reader.ReadInt16();
                        Glyphs[i] = g;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Corrupted sprite has been detected...", "Failed to load", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return true;
            }

            return false;
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
                   EqualityComparer<short[]>.Default.Equals(Glyphs, other.Glyphs) &&
                   File == other.File;
        }

        public override int GetHashCode()
        {
            var hashCode = 265894640;
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<short[]>.Default.GetHashCode(Colours);
            hashCode = hashCode * -1521134295 + EqualityComparer<short[]>.Default.GetHashCode(Glyphs);
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
