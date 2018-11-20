using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

namespace Manina.Windows.Forms
{
    internal class GlyphToolBar : Glyph
    {
        #region Member Variables
        private readonly BehaviorService behaviorService;
        private readonly ControlDesigner designer;
        private readonly Adorner adorner;

        private readonly List<BaseGlyph> buttons = new List<BaseGlyph>();

        private Rectangle bounds;
        #endregion

        #region Properties
        public Control Control { get; private set; }

        public Size Padding { get; set; } = new Size(2, 2);
        public Size DefaultIconSize = new Size(16, 16);

        public Point Location { get; set; }
        public Size Size { get; private set; }
        public override Rectangle Bounds => bounds;

        public Color BackColor { get; set; } = Color.White;
        public Color BorderColor { get; set; } = Color.FromArgb(128, 128, 128);

        public Color ButtonBackColor { get; set; } = Color.White;
        public Color HotButtonBackColor { get; set; } = Color.FromArgb(253, 244, 191);

        public Color ButtonBorderColor { get; set; } = Color.White;
        public Color HotButtonBorderColor { get; set; } = Color.FromArgb(229, 195, 101);

        public Color ButtonForeColor { get; set; } = Color.Black;
        public Color DisabledButtonForeColor { get; set; } = Color.FromArgb(109, 109, 109);
        public Color ButtonFillColor { get; set; } = Color.FromArgb(249, 224, 126);
        public Color HotButtonFillColor { get; set; } = Color.FromArgb(231, 186, 10);
        public Color DisabledButtonFillColor { get; set; } = Color.FromArgb(186, 186, 186);

        public Color SeparatorColor { get; set; } = Color.FromArgb(109, 109, 109);
        #endregion

        #region Constructor
        public GlyphToolBar(BehaviorService behaviorService, ControlDesigner designer, Adorner adorner)
            : base(new GlyphToolBarBehavior())
        {
            this.behaviorService = behaviorService;
            this.designer = designer;
            this.Control = (Control)designer.Component;
            this.adorner = adorner;
        }
        #endregion

        #region Behavior
        internal class GlyphToolBarBehavior : Behavior
        {
            public override bool OnMouseDown(Glyph g, MouseButtons mouseButton, Point mouseLoc)
            {
                if (mouseButton == MouseButtons.Left)
                {
                    GlyphToolBar toolbar = (GlyphToolBar)g;

                    foreach (var button in toolbar.buttons)
                    {
                        if (button.Enabled && button.Bounds.Contains(mouseLoc))
                        {
                            button.OnClick(EventArgs.Empty);
                            return true;
                        }

                    }
                }

                return false;
            }
        }
        #endregion

        #region Instance Methods
        public void AddButton(BaseGlyph button)
        {
            button.Parent = this;
            buttons.Add(button);
        }

        public void UpdateLayout()
        {
            Point pt = behaviorService.ControlToAdornerWindow(Control);

            // calculate toolbar size
            int width = 0;
            int height = 0;

            foreach (var button in buttons)
            {
                Size size = button.Size;

                height = Math.Max(height, size.Height);
                width += size.Width + 1;
            }

            Size = new Size(width - 1, height) + Padding + Padding + new Size(2, 2);
            bounds = new Rectangle(pt.X + Location.X, pt.Y + Location.Y, Size.Width, Size.Height);

            // update button locations
            int x = pt.X + Location.X + Padding.Width + 1;
            int y = pt.Y + Location.Y + Padding.Height + 1;

            foreach (var button in buttons)
            {
                Size size = button.Size;

                button.Bounds = new Rectangle(x, y, size.Width, height);
                x += size.Width + 1;
            }
        }
        #endregion

        #region Overriden Methods
        public override Cursor GetHitTest(Point p)
        {
            bool needsPaint = false;
            bool hasHit = false;

            foreach (var button in buttons)
            {
                if (!button.Enabled)
                {
                    if (button.IsHot)
                    {
                        button.IsHot = false;
                        needsPaint = true;
                    }
                    continue;
                }

                bool newIsHot = button.Bounds.Contains(p);
                if (newIsHot) hasHit = true;
                if (button.IsHot != newIsHot)
                {
                    button.IsHot = newIsHot;
                    needsPaint = true;
                }
            }

            if (needsPaint) adorner.Invalidate();

            return hasHit ? Cursors.Default : null;
        }

        public override void Paint(PaintEventArgs pe)
        {
            UpdateLayout();
            Rectangle bounds = Bounds;

            using (Brush brush = new SolidBrush(BackColor))
            using (Pen pen = new Pen(BorderColor))
            {
                pe.Graphics.FillRectangle(brush, bounds);
                pe.Graphics.DrawRectangle(pen, bounds);
            }

            foreach (var button in buttons)
            {
                button.Paint(pe);
            }
        }
        #endregion
    }
}
