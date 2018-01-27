using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using SPE.Engine;
using static SPE.Properties.Settings;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace SPE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public Sprite LoadedSprite { get; set; }

        public readonly WindowDataContext WindowDataContext = new WindowDataContext();

        private bool _isLeftClickHeldDown;
        private bool _isRightClickHeldDown;

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

            WindowDataContext.ToggleCanvasGrid = Default.UseGridOnCanvas;

           
            LoadedSprite = new Sprite(10, 10, this);
            UpdateCanvas();

            WindowDataContext.CurrentProgramStatus = "Loaded Empty Sprite";

            if (Default.ShowAllColours)
            {
                WindowDataContext.ModeAllColours = true;
                ColourHandler.SwapColours(true);
            }
            else
            {
                WindowDataContext.ModeSystemColours = true;
                ColourHandler.SwapColours(false);
            }

            CreateColorPalletWindow();
        }

        private void CreateColorPalletWindow()
        {
            _activeColour = ColourHandler.ByHex("FF000000", Pixal.PIXEL_SOLID);

            ColorViewCanvas.Width = ColorScrollViewer.Width;

            var width = (int)(ColorViewCanvas.Width / Sprite.SpriteBlockSize);
            var height = (int)Math.Ceiling((decimal)ColourHandler.Colours.Count / width);
            Console.WriteLine(height);
            ColorViewCanvas.Height = height * Sprite.SpriteBlockSize;

            var colorIdx = 0;
            for (var j = 0; j < height; j++)
            {
                for (var i = 0; i < width; i++)
                {
                    if (colorIdx == ColourHandler.Colours.Count)
                    {
                        break;
                    }

                    var c = ColourHandler.Colours[colorIdx];

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

                    var tt = new ToolTip
                    {
                        Content = $"Hex: #{c.Hex}{Environment.NewLine}" +
                                  $"RGB: {c.R},{c.G},{c.B}" +
                                  $"{(c.A < 255 ? $"{Environment.NewLine}Transparent" : "")}{Environment.NewLine}" +
                                  $"IDX: {colorIdx}{Environment.NewLine}" +
                                  $"Pixal: {(char)c.PT}",
                        Background = new SolidColorBrush(Color.FromArgb(200, 255, 255, 255))
                    };

                    rect.ToolTip = tt;

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
                    var g = LoadedSprite.GetGlyph(j, i);                    
                    var correctColor = ColourHandler.ByCode(c, (Pixal)g);

                    if(correctColor == null) continue;
                    
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
                        var rr = (Rectangle) sender;
                        var ca = _activeColour;
                        if (_isRightClickHeldDown)
                        {
                            ca = ColourHandler.Colours[0];
                        }

                        if (_isLeftClickHeldDown || _isRightClickHeldDown)
                        {
                            UpdateRect(rr, i1, j1, ca);
                        }

                        Colour rectColour = GetColourFromRect(rr);
                        var rrT = (ToolTip) rr.ToolTip;
                        rrT.Content += $"{Environment.NewLine}Hex: #{rectColour.Hex}" +
                                       $"{Environment.NewLine}Pixal: {(char)rectColour.PT}";
                        rect.Stroke = _hoverBrush;
                    };

                    rect.MouseLeave += (sender, args) =>
                    {
                        var rr = (Rectangle)sender;
                        var rrT = (ToolTip)rr.ToolTip;
                        rrT.Content = $"Pos: {(j1 + 1)}, {(i1 + 1)}";

                        if (Default.UseGridOnCanvas)
                        {
                            rect.Stroke = _gridColorBrush;
                            return;
                        }

                        rect.Stroke = null;
                    };

                    rect.MouseUp += (sender, args) =>
                    {
                        if (args.LeftButton == MouseButtonState.Released)
                        {
                            _isLeftClickHeldDown = false;
                            WindowDataContext.CurrentSystemTool = "";
                        }

                        if (args.RightButton == MouseButtonState.Released)
                        {
                            _isRightClickHeldDown = false;
                            WindowDataContext.CurrentSystemTool = "";
                        }
                    };

                    rect.MouseDown += (sender, args) =>
                    {
                        var ca = _activeColour;
                        if (args.LeftButton == MouseButtonState.Pressed)
                        {
                            _isRightClickHeldDown = false;
                            _isLeftClickHeldDown = true;
                            WindowDataContext.CurrentSystemTool = "Dragging Mode";

                        }
                        else if (args.RightButton == MouseButtonState.Pressed)
                        {
                            _isLeftClickHeldDown = false;
                            _isRightClickHeldDown = true;
                            WindowDataContext.CurrentSystemTool = "Erasing Mode";
                            ca = ColourHandler.Colours[0];
                        }
                        
                        UpdateRect((Rectangle)sender, i1, j1, ca);
                    };

                    var tt = new ToolTip
                    {
                        Content = $"{(j + 1)}, {(i + 1)}"
                    };

                    rect.ToolTip = tt;

                    SpriteViewCanvas.Children.Add(rect);
                    Canvas.SetTop(rect, i * Sprite.SpriteBlockSize);
                    Canvas.SetLeft(rect, j * Sprite.SpriteBlockSize);
                }
            }

            SpriteViewCanvas.IsEnabled = true;
        }

        private Colour GetColourFromRect(Rectangle rr)
        {
            var s = (SolidColorBrush) rr.Fill;
            return ColourHandler.ByRgb(s.Color.R, s.Color.B, s.Color.G, s.Color.A);

        }

        private void UpdateRect(Rectangle rect, int i1, int j1, Colour c = null)
        {
            if (c == null)
            {
                c = _activeColour;
            }
            rect.Fill = new SolidColorBrush(c.Color);
            LoadedSprite.SetColour(i1, j1, c);
            LoadedSprite.SetGlyph(i1, j1, c.PT);
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
                        LoadedSprite = new Sprite(ofd.FileName, this);
                        if (!LoadedSprite.FailedToLoad)
                        {
                            SpriteViewCanvas.Children.Clear();
                            SaveToRecentsList(ofd.FileName);
                            WindowDataContext.CurrentProgramStatus = $"Loaded: {Path.GetFileName(ofd.FileName)}";
                            UpdateCanvas();
                        }
                        else
                        {
                            LoadedSprite = new Sprite(10, 10, this);
                            SpriteViewCanvas.Children.Clear();
                            UpdateCanvas();
                        }
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
                case "ShowSystemColours":
                    Default.ShowAllColours = false;
                    Default.Save();

                    WindowDataContext.ModeAllColours = false;
                    WindowDataContext.ModeSystemColours = true;

                    ColourHandler.SwapColours(Default.ShowAllColours);
                    ColorViewCanvas.Children.Clear();
                    CreateColorPalletWindow();
                    break;
                case "ShowAllColours":
                    Default.ShowAllColours = true;
                    Default.Save();

                    WindowDataContext.ModeAllColours = true;
                    WindowDataContext.ModeSystemColours = false;

                    ColourHandler.SwapColours(Default.ShowAllColours);
                    ColorViewCanvas.Children.Clear();
                    CreateColorPalletWindow();
                    break;
            }
        }

        private void CreateNewSprite()
        {
            var newSpriteDialog = new Dialogs.NewSpriteDialog(this) {Owner = this};

            if (newSpriteDialog.ShowDialog() == true)
            {
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
                        var correctColor = ColourHandler.ByCode(c);

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

            if (sfd.ShowDialog() != true) return;
            var file = sfd.FileName;

            SaveToRecentsList(file);
            LoadedSprite.Save(file);
            WindowDataContext.CurrentProgramStatus = $"Saved: {Path.GetFileName(file)}";
        }

        private void ToggleSpriteGrid()
        {
            WindowDataContext.ToggleCanvasGrid = !Default.UseGridOnCanvas;
            Default.UseGridOnCanvas = WindowDataContext.ToggleCanvasGrid;

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

                LoadedSprite = new Sprite(file, this);

                if (LoadedSprite.FailedToLoad)
                {
                    RecentFilesList.Items.Remove(sender);
                    LoadedSprite = new Sprite(10, 10, this);
                }
                else
                {
                    WindowDataContext.CurrentProgramStatus = $"Loaded: {Path.GetFileName(file)}";
                }

                SpriteViewCanvas.Children.Clear();
                UpdateCanvas();
            };

            return mItem;
        }
    }
}
