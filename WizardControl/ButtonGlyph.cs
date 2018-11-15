using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design.Behavior;

namespace Manina.Windows.Forms
{
    public partial class WizardControl
    {
        internal class ButtonGlyph : Glyph
        {
            #region Member Variables
            private readonly BehaviorService behaviorService;
            private readonly WizardControl control;
            private readonly WizardControlDesigner designer;
            private readonly Adorner adorner;

            private bool isHot;

            private Point location = new Point(0, 0);
            private Size size = new Size(16, 16);
            private Padding margins = new Padding();
            #endregion

            #region Properties
            public Point Location
            {
                get => location;
                set
                {
                    var oldLocation = location;
                    location = value;

                    margins.Left += location.X - oldLocation.X;
                    margins.Right -= location.X - oldLocation.X;
                    margins.Top += location.Y - oldLocation.Y;
                    margins.Bottom -= location.Y - oldLocation.Y;
                }
            }
            public Size Size
            {
                get => size;
                set
                {
                    var oldSize = size;
                    size = value;

                    if ((Anchor & AnchorStyles.Left) != AnchorStyles.None)
                        margins.Right -= size.Width - oldSize.Width;
                    else
                        margins.Left -= size.Width - oldSize.Width;

                    if ((Anchor & AnchorStyles.Top) != AnchorStyles.None)
                        margins.Bottom -= size.Height - oldSize.Height;
                    else
                        margins.Top -= size.Height - oldSize.Height;

                }
            }
            public AnchorStyles Anchor { get; set; } = AnchorStyles.Left | AnchorStyles.Top;

            public bool Enabled { get; set; } = true;

            public PointF[] Path { get; set; } = new PointF[0];

            public Color BackColor { get; set; } = SystemColors.Window;
            public Color ForeColor { get; set; } = SystemColors.WindowText;
            public Color HotBackColor { get; set; } = Color.FromArgb(205, 229, 247);
            public Color DisabledBackColor { get; set; } = SystemColors.Control;
            public Color DisabledForeColor { get; set; } = SystemColors.GrayText;

            public override Rectangle Bounds
            {
                get
                {
                    Point pt = behaviorService.ControlToAdornerWindow(control);

                    var margins = Margins;
                    int left = ((Anchor & AnchorStyles.Left) != AnchorStyles.None) ? pt.X + margins.Left : pt.X + control.Width - margins.Right - Size.Width;
                    int top = ((Anchor & AnchorStyles.Top) != AnchorStyles.None) ? pt.Y + margins.Top : pt.Y + control.Height - margins.Bottom - Size.Height;

                    return new Rectangle(left, top, Size.Width, Size.Height);
                }
            }

            public Padding Margins => margins;

            public static PointF[] GetDefaultPath(float size)
            {
                return new PointF[]
                {
                    new PointF(0, 0),
                    new PointF(size, 0),
                    new PointF(size, size),
                    new PointF(0,size),
                };
            }
            #endregion

            #region Events
            public event EventHandler Click;

            protected internal virtual void OnClick(EventArgs e)
            {
                Click?.Invoke(this, e);
            }
            #endregion

            #region Constructor
            public ButtonGlyph(BehaviorService behaviorService, WizardControlDesigner designer, Adorner adorner)
                : base(new ButtonGlyphBehavior())
            {
                this.behaviorService = behaviorService;
                this.designer = designer;
                this.control = (WizardControl)designer.Component;
                this.adorner = adorner;

                margins = new Padding(Location.X, Location.Y, control.Width - Location.X - Size.Width, control.Height - Location.Y - Size.Height);
            }
            #endregion

            #region Overriden Methods
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
                var path = Path;
                if (path == null || path.Length == 0) path = GetDefaultPath(Size.Width);

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
            #endregion

            #region Behavior
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
            #endregion
        }
    }
}
