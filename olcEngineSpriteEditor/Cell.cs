using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace olcEngineSpriteEditor
{
    class Cell : Panel
    {
        public Cell()
        {
            SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (BorderColor == Color.Transparent) return;
            e.Graphics.DrawRectangle(
                new Pen(
                    new SolidBrush(BorderColor), BorderSize),
                e.ClipRectangle);

            /*            base.OnPaint(e);

            e.Graphics.DrawRectangle(
                new Pen(
                    new SolidBrush(BorderColor), BorderSize),
                e.ClipRectangle);
                */
        }

        public Color BorderColor { get; set; } = Color.Transparent;
        public int BorderSize { get; set; } = 4;
        public bool Clicked { get; set; } = false;
    }
}
