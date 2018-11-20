using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    internal class LabelGlyph : BaseGlyph
    {
        public string Text { get; set; } = "";

        private Size textSize;

        public override Size Size
        {
            get
            {
                bool hasText = !string.IsNullOrEmpty(Text);

                textSize = (hasText ? TextRenderer.MeasureText(Text, Parent.Control.Font) : Size.Empty);

                return textSize + Padding + Padding;
            }
        }

        public override void Paint(PaintEventArgs pe)
        {
            pe.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            using (Brush backBrush = new SolidBrush(Parent.ButtonBackColor))
            {
                Rectangle bounds = Bounds;

                pe.Graphics.FillRectangle(backBrush, bounds);

                if (!string.IsNullOrEmpty(Text))
                {
                    Rectangle textBounds = GetCenteredRectangle(textSize);

                    TextRenderer.DrawText(pe.Graphics, Text, Parent.Control.Font, textBounds, Parent.ButtonForeColor,
                        TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.SingleLine);
                }
            }
        }
    }
}
