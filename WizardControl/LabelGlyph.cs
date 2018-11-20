using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design.Behavior;

namespace Manina.Windows.Forms
{
    public partial class WizardControl
    {
        internal class LabelGlyph : Glyph
        {
            #region Member Variables
            private readonly BehaviorService behaviorService;
            private readonly WizardControl control;
            private readonly WizardControlDesigner designer;
            private readonly Adorner adorner;

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
            public ContentAlignment Alignment { get; set; } = ContentAlignment.MiddleLeft;

            public string Text { get; set; } = "";

            public Color ForeColor { get; set; } = SystemColors.WindowText;

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
            #endregion

            #region Constructor
            public LabelGlyph(BehaviorService behaviorService, WizardControlDesigner designer, Adorner adorner)
                : base(new LabelGlyphBehavior())
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
                return null;
            }

            public override void Paint(PaintEventArgs pe)
            {
                pe.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                pe.Graphics.FillRectangle(Brushes.White, Bounds);
                Rectangle bounds = Bounds;

                TextFormatFlags flags = TextFormatFlags.SingleLine;
                switch(Alignment )
                {
                    case ContentAlignment.TopLeft:
                        flags |= TextFormatFlags.Top | TextFormatFlags.Left;
                        break;
                    case ContentAlignment.TopCenter:
                        flags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                        break;
                    case ContentAlignment.TopRight:
                        flags |= TextFormatFlags.Top | TextFormatFlags.Right;
                        break;
                    case ContentAlignment.MiddleLeft:
                        flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                        break;
                    case ContentAlignment.MiddleCenter:
                        flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                        break;
                    case ContentAlignment.MiddleRight:
                        flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                        break;
                    case ContentAlignment.BottomLeft:
                        flags |= TextFormatFlags.Bottom | TextFormatFlags.Left;
                        break;
                    case ContentAlignment.BottomCenter:
                        flags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                        break;
                    case ContentAlignment.BottomRight:
                        flags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
                        break;
                }
                TextRenderer.DrawText(pe.Graphics, Text, control.Font, bounds, ForeColor, flags);
            }
            #endregion

            #region Behavior
            internal class LabelGlyphBehavior : Behavior
            {
            }
            #endregion
        }
    }
}
