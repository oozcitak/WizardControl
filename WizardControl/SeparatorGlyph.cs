using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    internal class SeparatorGlyph : BaseGlyph
    {
        private Size lineSize;

        public override Size Size
        {
            get
            {
                lineSize = new Size(1, Parent.DefaultIconSize.Height);

                return lineSize + Padding + Padding;
            }
        }

        public override void Paint(PaintEventArgs pe)
        {
            pe.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            using (Brush backBrush = new SolidBrush(Parent.ButtonBackColor))
            using (Pen linePen = new Pen(Parent.SeparatorColor))
            {
                Rectangle bounds = Bounds;

                pe.Graphics.FillRectangle(backBrush, bounds);
                Rectangle lineBounds = GetCenteredRectangle(lineSize);
                pe.Graphics.DrawLine(linePen, lineBounds.Left, lineBounds.Top, lineBounds.Left, lineBounds.Bottom);
            }
        }
    }
}
