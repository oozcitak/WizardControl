using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design.Behavior;

namespace Manina.Windows.Forms
{
    public partial class WizardControl
    {
        internal class ButtonGlyphBehavior : Behavior
        {
            public override bool OnMouseDown(Glyph g, MouseButtons button, Point mouseLoc)
            {
                if (g.Bounds.Contains(mouseLoc))
                {
                    ButtonGlyph glyph = (ButtonGlyph)g;

                    if (glyph.Enabled)
                        glyph.OnClick(new EventArgs());

                    return true;
                }

                return false;
            }
        }

        internal class ButtonGlyph : Glyph
        {
            private readonly BehaviorService behaviorService;
            private readonly WizardControl control;
            private readonly WizardControlDesigner designer;
            private readonly Adorner adorner;

            private readonly PointF[] path;
            private readonly int size;
            private readonly AnchorStyles anchor;
            private readonly Padding margins;
            private bool isHot;

            public bool Enabled { get; set; }

            public Color BackColor => SystemColors.Window;
            public Color ForeColor => SystemColors.WindowText;
            public Color HotBackColor => Color.FromArgb(205, 229, 247);
            public Color DisabledBackColor => SystemColors.Control;
            public Color DisabledForeColor => SystemColors.GrayText;

            public ButtonGlyph(BehaviorService behaviorService, WizardControlDesigner designer, Adorner adorner, PointF[] path, int left, int top, int size, AnchorStyles anchor = AnchorStyles.Left | AnchorStyles.Top)
                : base(new ButtonGlyphBehavior())
            {
                this.behaviorService = behaviorService;
                this.designer = designer;
                this.control = (WizardControl)designer.Component;
                this.adorner = adorner;

                this.path = path;
                this.size = size;
                this.anchor = anchor;

                Enabled = true;

                margins = new Padding(left, top, control.Width - left - size, control.Height - top - size);
            }

            public override Rectangle Bounds
            {
                get
                {
                    Point pt = behaviorService.ControlToAdornerWindow(control);

                    int left = ((anchor & AnchorStyles.Left) != AnchorStyles.None) ? pt.X + margins.Left : pt.X + control.Width - margins.Right - size;
                    int top = ((anchor & AnchorStyles.Top) != AnchorStyles.None) ? pt.Y + margins.Top : pt.Y + control.Height - margins.Bottom - size;

                    return new Rectangle(left, top, size, size);
                }
            }

            public override Cursor GetHitTest(Point p)
            {
                if (!Enabled) return null;

                bool newIshot = Bounds.Contains(p);
                if (isHot != newIshot)
                {
                    isHot = newIshot;
                    adorner.Invalidate();
                }
                return isHot ? Cursors.Default : null;
            }

            public override void Paint(PaintEventArgs pe)
            {
                pe.Graphics.SmoothingMode = SmoothingMode.HighQuality;

                Rectangle bounds = Bounds;

                using (Brush brush = new SolidBrush(!Enabled ? DisabledBackColor : isHot ? HotBackColor : BackColor))
                using (Pen pen = new Pen(!Enabled ? DisabledForeColor : ForeColor))
                {
                    var oldTrans = pe.Graphics.Transform;
                    pe.Graphics.TranslateTransform(bounds.Left, bounds.Top);
                    pe.Graphics.FillPolygon(brush, path);
                    pe.Graphics.DrawPolygon(pen, path);
                    pe.Graphics.Transform = oldTrans;
                }
            }

            public event EventHandler Click;

            internal virtual void OnClick(EventArgs e)
            {
                Click?.Invoke(this, e);
            }
        }
    }
}
