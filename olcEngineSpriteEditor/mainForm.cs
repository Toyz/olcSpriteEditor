using System;
using System.Drawing;
using System.Windows.Forms;

namespace olcEngineSpriteEditor
{
    public partial class MainForm : Form
    {
        public Sprite LoadedSprite;
        public readonly int CellSpriteSize = 32;
        private readonly int CellColorSize = 32;
        private Color _selectedColor = Color.Black;
        private Color _borderGridColor = Color.Transparent;
        private bool _showGrid;

        public MainForm()
        {
            InitializeComponent();

            LoadedSprite = new Sprite(drawingPanel.Width / CellSpriteSize, drawingPanel.Height / CellSpriteSize);
            DrawSpriteInPanel();
           
            var index = 0;

            var maxColors = Enum.GetValues(typeof(ConsoleColor)).Length;

            for (var i = 0; i < supportedColors.Width / CellColorSize; i++)
            {
                for (var j = 0; j < supportedColors.Height / CellColorSize; j++)
                {
                    var color = Color.Transparent;

                    if (index < maxColors)
                    {
                        color = ((ConsoleColor) index).ToDrawingColor();
                    }

                    var cell = createPanel(color);

                    cell.Location = new Point(i * CellColorSize, j * CellColorSize);
                    cell.Width = CellColorSize;
                    cell.Height = CellColorSize;
                    cell.Click += CellOnClick;

                    if (color == _selectedColor)
                    {
                        cell.BorderColor = Color.LightBlue;
                        cell.Clicked = true;
                        cell.Clicked = true;
                    }

                    supportedColors.Controls.Add(cell);

                    index++;
                }
            }
        }

        private void CellOnClick(object o, EventArgs eventArgs)
        {
            var c = (Cell) o;

            _selectedColor = c.BackColor;
            c.BorderColor = Color.LightBlue;
            c.Clicked = true;
            c.Refresh();

            for (var i = 0; i < supportedColors.Controls.Count; i++)
            {
                var cell = (Cell)supportedColors.Controls[i];

                if(cell == c) continue;
                
                cell.BorderColor = Color.Transparent;
                cell.Clicked = false;
                cell.Refresh();
            }
        }

        private void openSpriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Sprite File(*.spr)|*spr";
                ofd.Title = "Select Sprite File";
                ofd.Multiselect = false;

                if (ofd.ShowDialog() != DialogResult.OK) return;
                LoadedSprite = new Sprite(ofd.FileName);

                Console.WriteLine(LoadedSprite);

                DrawSpriteInPanel();
            }
        }

        public void DrawSpriteInPanel()
        {
            drawingPanel.Controls.Clear();

            for (var i = 0; i < LoadedSprite.Width; i++)
            {
                for (var j = 0; j < LoadedSprite.Height; j++)
                {
                    var cColor = (ConsoleColor) LoadedSprite.GetColour(i, j);
                    var color = cColor.ToDrawingColor();
                    var cell = createPanel(color);
                    cell.Location = new Point(i * CellSpriteSize, j * CellSpriteSize);
                    cell.BackColor = color;

                    cell.Click += (sender, args) =>
                    {
                        var c = (Cell) sender;
                        c.BackColor = _selectedColor;

                        LoadedSprite.SetColour(c.Location.X / CellSpriteSize, c.Location.Y / CellSpriteSize, (short)c.BackColor.ToConsoleColor());
                        c.Refresh();
                    };
                    
                    drawingPanel.Controls.Add(cell);
                }
            }
        }

        private Cell createPanel(Color color)
        {
            var panel = new Cell
            {
                BackColor = color,
                Width = CellSpriteSize,
                Height = CellSpriteSize
            };

            if (_showGrid)
            {
                panel.BorderColor = _borderGridColor;
                panel.BorderSize = 1;
            }

            panel.MouseEnter += (sender, args) =>
            {
                if (panel.Clicked) return;
                panel.BorderColor = Color.Blue;
                panel.BorderSize = 4;
                panel.Refresh();
            };
            panel.MouseLeave += (sender, args) =>
            {
                if (panel.Clicked) return;
                panel.BorderColor = _borderGridColor;
                panel.BorderSize = 1;
                panel.Refresh();
            };


            return panel;
        }

        private void saveAsSpriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LoadedSprite == null) return;

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Sprite File(*.spr)|*spr";
                sfd.Title = "Save Sprite File";

                if (sfd.ShowDialog() != DialogResult.OK) return;

                var fileName = sfd.FileName;
                if (!fileName.EndsWith(".spr"))
                {
                    fileName += ".spr";
                }

                LoadedSprite.Save(fileName);
            }
        }

        private void saveSpriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(LoadedSprite.File))
            {
                saveAsSpriteToolStripMenuItem_Click(sender, e);
                return;
            }
            LoadedSprite?.Save();
        }

        private void toggleGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = (ToolStripMenuItem)sender;
            _showGrid = item.Checked = !item.Checked;
  
            _borderGridColor = item.Checked ? Color.WhiteSmoke : Color.Transparent;

            foreach (var cell in drawingPanel.Controls)
            {
                var subCell = (Cell)cell;
                subCell.BorderColor = _borderGridColor;
                subCell.BorderSize = 1;
                subCell.Refresh();
            }
        }

        private void newSpriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var newDialog = new NewSprite(this))
            {                
                if (newDialog.ShowDialog() == DialogResult.OK)
                {
                    DrawSpriteInPanel();
                    newDialog.Close();
                }
            }
        }
    }
}
