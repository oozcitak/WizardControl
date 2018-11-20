using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    internal class ButtonGlyph : BaseGlyph
    {
        public PointF[] Path { get; set; } = new PointF[0];
        public string Text { get; set; } = "";

        private Size iconSize;
        private Size textSize;

        public override Size Size
        {
            get
            {
                bool hasIcon = (Path != null && Path.Length != 0);
                bool hasText = !string.IsNullOrEmpty(Text);

                iconSize = (hasIcon ? Parent.DefaultIconSize : Size.Empty);
                textSize = (hasText ? TextRenderer.MeasureText(Text, Parent.Control.Font) : Size.Empty);

                return new Size(iconSize.Width + textSize.Width + (hasIcon && hasText ? 2 : 0),
                    Math.Max(iconSize.Height, textSize.Height)) + Padding + Padding + new Size(2, 2);
            }
        }

        public override void Paint(PaintEventArgs pe)
        {
            pe.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            using (Brush backBrush = new SolidBrush(Enabled && IsHot ? Parent.HotButtonBackColor : Parent.ButtonBackColor))
            using (Pen borderPen = new Pen(IsHot ? Parent.HotButtonBorderColor : Parent.ButtonBorderColor))
            using (Brush pathBrush = new SolidBrush(IsHot ? Parent.HotButtonFillColor : !Enabled ? Parent.DisabledButtonFillColor : Parent.ButtonFillColor))
            using (Pen pathPen = new Pen(!Enabled ? Parent.DisabledButtonForeColor : Parent.ButtonForeColor))
            {
                Rectangle bounds = Bounds;

                pe.Graphics.FillRectangle(backBrush, bounds);
                pe.Graphics.DrawRectangle(borderPen, bounds);

                if (Path != null && Path.Length != 0)
                {
                    Rectangle iconBounds = GetCenteredRectangle(iconSize);

                    var oldTrans = pe.Graphics.Transform;
                    pe.Graphics.TranslateTransform(iconBounds.Left, iconBounds.Top);
                    pe.Graphics.FillPolygon(pathBrush, Path);
                    pe.Graphics.DrawPolygon(pathPen, Path);
                    pe.Graphics.Transform = oldTrans;
                }
                if (!string.IsNullOrEmpty(Text))
                {
                    Rectangle textBounds = GetCenteredRectangle(textSize);

                    TextRenderer.DrawText(pe.Graphics, Text, Parent.Control.Font, textBounds,
                        (!Enabled ? Parent.DisabledButtonForeColor : Parent.ButtonForeColor),
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
                }
            }
        }
    }
}
