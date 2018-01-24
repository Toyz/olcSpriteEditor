using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
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
    public partial class MainWindow
    {
        public static readonly RoutedCommand OpenCommand = new RoutedUICommand("Open", "OpenSprite", typeof(MenuItem), new InputGestureCollection(new InputGesture[]
        {
               new KeyGesture(Key.O, ModifierKeys.Control)
        }));

        public static readonly RoutedCommand ExportCommand = new RoutedUICommand("Export", "ExportSprite", typeof(MenuItem), new InputGestureCollection(new InputGesture[]
        {
            new KeyGesture(Key.E, ModifierKeys.Control | ModifierKeys.Alt)
        }));

        public static readonly RoutedCommand SaveCommand = new RoutedUICommand("Save", "SaveSprite", typeof(MenuItem), new InputGestureCollection(new InputGesture[]
        {
            new KeyGesture(Key.S, ModifierKeys.Control)
        }));

        public static readonly RoutedCommand SaveAsCommand = new RoutedUICommand("Save As", "SaveAsSprite", typeof(MenuItem), new InputGestureCollection(new InputGesture[]
        {
            new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift)
        }));

        public static readonly RoutedCommand ToggleGridCommand = new RoutedUICommand("Toggle Canvas Grid", "ToggleGridView", typeof(MenuItem), new InputGestureCollection(new InputGesture[]
        {
            new KeyGesture(Key.T, ModifierKeys.Control | ModifierKeys.Alt)
        }));

        public Sprite LoadedSprite { get; private set; }
        public static List<Colour> SystemColours = new List<Colour>();

        private Colour _activeColour;
        private readonly Brush _gridColorBrush = new SolidColorBrush(Colors.White);
        private readonly Brush _colorBorder = new SolidColorBrush(Colors.LightGray);
        private readonly Brush _hoverBrush = new SolidColorBrush(Colors.Brown);
        private readonly Brush _activeColor = new SolidColorBrush(Colors.Red);

        public MainWindow()
        {
            InitializeComponent();
            LoadRecentsFilesList();

            ToggleCanvasGrid.IsChecked = Default.UseGridOnCanvas;

            foreach (var line in File.ReadAllLines("./colours.txt"))
            {
                if(line.StartsWith("#") || string.IsNullOrEmpty(line)) continue;
                
                SystemColours.Add(new Colour(line));
            }

            _activeColour = SystemColours.FirstOrDefault(x => x.Hex == "000000");

            ColorViewCanvas.Width = ColorScrollViewer.Width;

            var width = (int) (ColorViewCanvas.Width / Sprite.SpriteBlockSize);
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
                        Stroke = _colorBorder
                    };

                    rect.MouseEnter += (sender, args) =>
                    {
                        var brush = rect.Stroke as SolidColorBrush;
                        if (Equals(brush.Color, Colors.Red)) return;

                        rect.Stroke = _hoverBrush;
                    };

                    rect.MouseLeave += (sender, args) =>
                    {
                        var brush = rect.Stroke as SolidColorBrush;
                        if (Equals(brush.Color, Colors.Red)) return;

                        rect.Stroke = _colorBorder;
                    };

                    rect.MouseUp += (sender, args) =>
                    {
                        var p = (Rectangle) sender;

                        p.Stroke = _activeColor;
                        _activeColour = c;

                        foreach (var child in ColorViewCanvas.Children)
                        {
                            var r = (Rectangle) child;
                            if(Equals(r, p)) continue; 

                            if (Equals(r.Stroke, _activeColor))
                            {
                                r.Stroke = _colorBorder;
                                break;
                            }

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

            LoadedSprite = new Sprite(10, 10);
            Draw();
        }

        private void Draw()
        {
            SpriteViewCanvas.IsEnabled = false;
            SpriteViewCanvas.Width = LoadedSprite.Width * Sprite.SpriteBlockSize;
            SpriteViewCanvas.Height = LoadedSprite.Height * Sprite.SpriteBlockSize;

            for (var i = 0; i < LoadedSprite.Height; i++)
            {
                for (var j = 0; j < LoadedSprite.Width; j++)
                {
                    var c = LoadedSprite.GetColour(j, i);
                    var correctColor = SystemColours.FirstOrDefault(x => x.Code == c);

                    var rect = new Rectangle
                    {
                        Width = Sprite.SpriteBlockSize,
                        Height = Sprite.SpriteBlockSize,
                        Fill = new SolidColorBrush(correctColor.Color),    
                    };

                    if (Default.UseGridOnCanvas)
                    {
                        rect.Stroke = _gridColorBrush;
                    }

                    rect.MouseEnter += (sender, args) =>
                    {
                        rect.Stroke = _hoverBrush;
                    };

                    var i1 = j;
                    var j1 = i;
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

                        Draw();
                    }

                    break;
                case "ToggleCanvasGrid":
                case "ToggleGridView":
                    ToggleSpriteGrid();
                    break;
                case "ExportSprite":
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
                                var correctColor = SystemColours.FirstOrDefault(x => x.Code == c);

                                flagGraphics.FillRectangle(correctColor.Color.ToSolidBrush(), new RectangleF(i * Sprite.SpriteBlockSize, j * Sprite.SpriteBlockSize, Sprite.SpriteBlockSize , Sprite.SpriteBlockSize));
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
                        //Helpers.SaveCanvas(this, SpriteViewCanvas, 100, file);
                    }
                    break;
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
                Header = System.IO.Path.GetFileName(item),
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
                SpriteViewCanvas.Children.Clear();
                Draw();
            };

            return mItem;
        }

    }
}
