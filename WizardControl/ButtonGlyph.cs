using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design.Behavior;

namespace Manina.Windows.Forms
{
    internal class ButtonGlyphBehavior : Behavior
    {
        private readonly WizardControlDesigner designer;

        public ButtonGlyphBehavior(WizardControlDesigner designer)
        {
            this.designer = designer;
        }

        public override bool OnMouseDown(Glyph g, MouseButtons button, Point mouseLoc)
        {
            if (g.Bounds.Contains(mouseLoc))
            {
                ButtonGlyph glyph = (ButtonGlyph)g;

                designer.OnGlyphClick(glyph.Key);

                return true;
            }

            return false;
        }
    }

    internal class ButtonGlyph : Glyph
    {
        private readonly BehaviorService behaviorService;
        private readonly ISelectionService selectionService;
        private readonly WizardControl control;
        private readonly IDesigner designer;
        private readonly Adorner adorner;

        private readonly string key;
        private readonly PointF[] path;
        private readonly int size;
        private readonly AnchorStyles anchor;
        private readonly Padding margins;
        private bool isHot;

        public string Key => key;

        public ButtonGlyph(BehaviorService behaviorService, ISelectionService selectionService, WizardControlDesigner designer, Adorner adorner, string key, PointF[] path, int left, int top, int size, AnchorStyles anchor = AnchorStyles.Left | AnchorStyles.Top)
            : base(new ButtonGlyphBehavior(designer))
        {
            this.behaviorService = behaviorService;
            this.selectionService = selectionService;
            this.designer = designer;
            this.control = (WizardControl)designer.Component;
            this.adorner = adorner;

            this.key = key;
            this.path = path;
            this.size = size;
            this.anchor = anchor;

            margins = new Padding(left, top, control.Width - left - size, control.Height - top - size);

            selectionService.SelectionChanged += SelectionService_SelectionChanged;
        }

        private void SelectionService_SelectionChanged(object sender, EventArgs e)
        {
            adorner.Enabled = ReferenceEquals(selectionService.PrimarySelection, control);
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

            using (Brush brush = new SolidBrush(isHot ? Color.FromArgb(205, 229, 247) : SystemColors.Window))
            using (Pen pen = new Pen(SystemColors.WindowText))
            {
                var oldTrans = pe.Graphics.Transform;
                pe.Graphics.TranslateTransform(bounds.Left, bounds.Top);
                pe.Graphics.FillPolygon(brush, path);
                pe.Graphics.DrawPolygon(pen, path);
                pe.Graphics.Transform = oldTrans;
            }
        }
    }
}
