using System.Windows.Media;

namespace SPE
{
    public static class Extensions
    {
        public static System.Drawing.SolidBrush ToSolidBrush(this Color color)
        {
            return new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B));
        }
    }
}
