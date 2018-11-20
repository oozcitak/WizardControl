using System;
using System.Drawing;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    internal abstract class BaseGlyph
    {
        #region Properties
        internal GlyphToolBar Parent { get; set; }

        public Point Location { get; set; } = new Point();
        public abstract Size Size { get; }

        public bool Enabled { get; set; } = true;
        public bool IsHot { get; set; } = false;

        public Size Padding { get; set; } = new Size(2, 2);

        public Rectangle Bounds { get; set; }
        #endregion

        #region Events
        public event EventHandler Click;

        protected internal virtual void OnClick(EventArgs e)
        {
            Click?.Invoke(this, e);
        }
        #endregion

        #region Virtual Methods
        public abstract void Paint(PaintEventArgs pe);
        #endregion

        #region Helper Methods
        protected Rectangle GetCenteredRectangle(Size size)
        {
            Rectangle bounds = Bounds;
            return new Rectangle(bounds.Left + (bounds.Width - size.Width) / 2,
                bounds.Top + (bounds.Height - size.Height) / 2,
                size.Width,
                size.Height);
        }
        #endregion
    }
}
