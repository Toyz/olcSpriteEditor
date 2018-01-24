using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using static SPE.Properties.Settings;
using Brush = System.Windows.Media.Brush;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace SPE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Sprite LoadedSprite { get; set; }
        public static List<Colour> SystemColours = new List<Colour>();

        public readonly WindowDataContext WindowDataContext = new WindowDataContext();

        private Colour _activeColour;
        private readonly Brush _gridColorBrush = new SolidColorBrush(Colors.White);
        private readonly Brush _colorBorder = new SolidColorBrush(Colors.LightGray);
        private readonly Brush _hoverBrush = new SolidColorBrush(Colors.Brown);
        private readonly Brush _activeColor = new SolidColorBrush(Colors.Red);

        public MainWindow()
        {
            InitializeComponent();
            DataContext = WindowDataContext;

            LoadRecentsFilesList();

            ToggleCanvasGrid.IsChecked = Default.UseGridOnCanvas;

            foreach (var line in File.ReadAllLines("./colours.txt"))
            {
                if(line.StartsWith("#") || string.IsNullOrEmpty(line)) continue;
                
                SystemColours.Add(new Colour(line));
            }

            CreateColorPalletWindow();
           
            LoadedSprite = new Sprite(10, 10);
            UpdateCanvas();

            WindowDataContext.CurrentProgramStatus = "Loaded Empty Sprite";
        }

        private void CreateColorPalletWindow()
        {
            _activeColour = SystemColours.FirstOrDefault(x => x.Hex == "000000");
            ColorViewCanvas.Width = ColorScrollViewer.Width;

            var width = (int)(ColorViewCanvas.Width / Sprite.SpriteBlockSize);
            var height = SystemColours.Count / width;
            ColorViewCanvas.Height = height * Sprite.SpriteBlockSize;

            var colorIdx = 0;
            for (var j = 0; j < height; j++)
            {
                for (var i = 0; i < width; i++)
                {
                    var c = SystemColours[colorIdx];

                    var rect = new Rectangle
                    {
                        Width = Sprite.SpriteBlockSize,
                        Height = Sprite.SpriteBlockSize,
                        Fill = new SolidColorBrush(c.Color),
                        Stroke = _colorBorder,
                        StrokeThickness = 1.2
                    };

                    rect.MouseEnter += (sender, args) =>
                    {
                        var brush = (SolidColorBrush)rect.Stroke;
                        if (Equals(brush.Color, Colors.Red)) return;

                        rect.Stroke = _hoverBrush;
                    };

                    rect.MouseLeave += (sender, args) =>
                    {
                        var brush = (SolidColorBrush)rect.Stroke;
                        if (Equals(brush.Color, Colors.Red)) return;

                        rect.Stroke = _colorBorder;
                    };

                    rect.MouseUp += (sender, args) =>
                    {
                        var p = (Rectangle)sender;

                        p.Stroke = _activeColor;
                        _activeColour = c;

                        foreach (var child in ColorViewCanvas.Children)
                        {
                            var r = (Rectangle)child;
                            if (Equals(r, p)) continue;

                            if (!Equals(r.Stroke, _activeColor)) continue;
                            r.Stroke = _colorBorder;
                            break;

                        }
                    };

                    if (_activeColour != null && _activeColour.Hex == c.Hex)
                    {
                        rect.Stroke = _activeColor;
                    }

                    ColorViewCanvas.Children.Add(rect);
                    Canvas.SetLeft(rect, i * Sprite.SpriteBlockSize);
                    Canvas.SetTop(rect, j * Sprite.SpriteBlockSize);

                    colorIdx++;
                }
            }
        }

        private void UpdateCanvas()
        {
            SpriteViewCanvas.IsEnabled = false;
            SpriteViewCanvas.Width = LoadedSprite.Width * Sprite.SpriteBlockSize;
            SpriteViewCanvas.Height = LoadedSprite.Height * Sprite.SpriteBlockSize;

            for (var i = 0; i < LoadedSprite.Height; i++)
            {
                for (var j = 0; j < LoadedSprite.Width; j++)
                {
                    var i1 = j;
                    var j1 = i;

                    var c = LoadedSprite.GetColour(j, i);
                    var correctColor = SystemColours.FirstOrDefault(x => x.Code == c);

                    var rect = new Rectangle
                    {
                        Width = Sprite.SpriteBlockSize,
                        Height = Sprite.SpriteBlockSize,
                        Fill = new SolidColorBrush(correctColor.Color),    
                        StrokeThickness = 2
                    };

                    if (Default.UseGridOnCanvas)
                    {
                        rect.Stroke = _gridColorBrush;
                    }

                    rect.MouseEnter += (sender, args) =>
                    {
                        rect.Stroke = _hoverBrush;
                    };

                    rect.MouseDown += (sender, args) =>
                    {
                        rect.Fill = new SolidColorBrush(_activeColour.Color);
                        LoadedSprite.SetColour(i1, j1, _activeColour);
                        LoadedSprite.SetGlyph(i1, j1, _activeColour.PT);
                    };

                    rect.MouseLeave += (sender, args) =>
                    {
                        if (Default.UseGridOnCanvas)
                        {
                            rect.Stroke = _gridColorBrush;
                            return;
                        }
                        rect.Stroke = null;
                    };

                    SpriteViewCanvas.Children.Add(rect);
                    Canvas.SetTop(rect, i * Sprite.SpriteBlockSize);
                    Canvas.SetLeft(rect, j * Sprite.SpriteBlockSize);
                }
            }

            SpriteViewCanvas.IsEnabled = true;
        }

        private void FileOptionClicked(object sender, ExecutedRoutedEventArgs e)
        {
            var command = ((RoutedCommand)e.Command).Name;

            switch (command)
            {
                case "SaveSprite":
                    if (string.IsNullOrEmpty(LoadedSprite.File))
                    {
                        SaveSpriteAs();
                        return;
                    }
                    LoadedSprite.Save();
                    break;
                case "SaveAsSprite":
                    SaveSpriteAs();
                    break;
                case "OpenSprite":
                    var ofd = new OpenFileDialog
                    {
                        Title = "Open Sprite File",
                        Multiselect = false,
                        Filter = "Sprite File (*.spr)|*.spr"
                    };

                    if (ofd.ShowDialog() == true)
                    {
                        LoadedSprite = new Sprite(ofd.FileName);
                        SpriteViewCanvas.Children.Clear();
                        SaveToRecentsList(ofd.FileName);
                        WindowDataContext.CurrentProgramStatus = $"Loaded: {Path.GetFileName(ofd.FileName)}";
                        UpdateCanvas();
                    }

                    break;
                case "ToggleCanvasGrid":
                case "ToggleGridView":
                    ToggleSpriteGrid();
                    break;
                case "ExportSprite":
                    ExportSpriteAsImage();
                    break;
                case "NewSprite":
                    CreateNewSprite();
                    break;
            }
        }

        private void CreateNewSprite()
        {
            var newSpriteDialog = new Dialogs.NewSpriteDialog(this) {Owner = this};

            if (newSpriteDialog.ShowDialog() == true)
            {
                // Dp thing with the data...
                SpriteViewCanvas.Children.Clear();
                UpdateCanvas();
            }
        }

        private void ExportSpriteAsImage()
        {
            var sfd = new SaveFileDialog
            {
                Title = "Export Sprite File as PNG",
                Filter = "PNG File (*.png)|*.png|JPG File (*.jpg)|*.jpg",
                FileName = "Export",
                AddExtension = true
            };

            if (sfd.ShowDialog() == true)
            {
                var file = sfd.FileName;

                Bitmap flag = new Bitmap(LoadedSprite.Width * Sprite.SpriteBlockSize, LoadedSprite.Height * Sprite.SpriteBlockSize);
                flag.SetResolution(100, 100);
                Graphics flagGraphics = Graphics.FromImage(flag);

                for (var i = 0; i < LoadedSprite.Width; i++)
                {
                    for (var j = 0; j < LoadedSprite.Height; j++)
                    {
                        var c = LoadedSprite.GetColour(i, j);
                        var correctColor = SystemColours.First(x => x.Code == c);

                        flagGraphics.FillRectangle(correctColor.Color.ToSolidBrush(), new RectangleF(i * Sprite.SpriteBlockSize, j * Sprite.SpriteBlockSize, Sprite.SpriteBlockSize, Sprite.SpriteBlockSize));
                    }
                }

                switch (Path.GetExtension(file).Split('.').Last().ToLower())
                {
                    case "png":
                        flag.Save(file, ImageFormat.Png);
                        break;
                    case "jpg":
                        flag.Save(file, ImageFormat.Jpeg);
                        break;
                }
            }
        }

        private void SaveToRecentsList(string file)
        {
            if (Default.RecentFiles.Contains(file)) return;

            Default.RecentFiles.Insert(0, file);

            if (Default.RecentFiles.Count > 6)
            {
                Default.RecentFiles.RemoveAt(Default.RecentFiles.Count - 1);
            }

            RecentFilesList.Items.Insert(0, CreateRecentItem(file));
            if (RecentFilesList.Items.Count > 6)
            {
                RecentFilesList.Items.RemoveAt(RecentFilesList.Items.Count - 1);
            }

            Default.Save();
        }

        private void SaveSpriteAs()
        {
            var sfd = new SaveFileDialog
            {
                Title = "Save Sprite File",
                Filter = "Sprite File (*.spr)|*.spr",
                FileName = "Default.spr",
                AddExtension = true
            };

            if (sfd.ShowDialog() == true)
            {
                var file = sfd.FileName;

                SaveToRecentsList(file);
                LoadedSprite.Save(file);
                WindowDataContext.CurrentProgramStatus = $"Saved: {Path.GetFileName(file)}";

            }
        }

        private void ToggleSpriteGrid()
        {
            ToggleCanvasGrid.IsChecked = !Default.UseGridOnCanvas;
            Default.UseGridOnCanvas = ToggleCanvasGrid.IsChecked;

            if (Default.UseGridOnCanvas)
            {
                foreach (var child in SpriteViewCanvas.Children)
                {
                    var rect = (Rectangle) child;
                    rect.Stroke = _gridColorBrush;
                }
            }
            else
            {
                foreach (var child in SpriteViewCanvas.Children)
                {
                    var rect = (Rectangle)child;
                    var brush = (SolidColorBrush) rect.Stroke;
                    if (brush.Color == ((SolidColorBrush) _hoverBrush).Color) continue;

                    rect.Stroke = null;
                }
            }

            Default.Save();
        }

        private void LoadRecentsFilesList()
        {
            if (Default.RecentFiles == null)
            {
                Default.RecentFiles = new StringCollection();
                Default.Save();
            }

            foreach (var item in Default.RecentFiles)
            {
                RecentFilesList.Items.Insert(0, CreateRecentItem(item));
            }
        }

        private MenuItem CreateRecentItem(string item)
        {
            var mItem = new MenuItem
            {
                Header = Path.GetFileName(item),
                Tag = item
            };

            mItem.Click += (sender, args) =>
            {
                var file = ((MenuItem) sender).Tag.ToString();

                if (!File.Exists(file))
                {
                    RecentFilesList.Items.Remove(sender);
                    MessageBox.Show($"{file} no longer exist", "Sprite Loading Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                LoadedSprite = new Sprite(file);
                WindowDataContext.CurrentProgramStatus = $"Loaded: {Path.GetFileName(file)}";

                SpriteViewCanvas.Children.Clear();
                UpdateCanvas();
            };

            return mItem;
        }
    }
}
