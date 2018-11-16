using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Manina.Windows.Forms
{
    [Designer(typeof(HorizontalLineDesigner))]
    internal class HorizontalLine : Control
    {
        #region Member Variables
        private BorderStyle borderStyle;
        private Color borderColor;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the text associated with this control.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public override string Text { get => base.Text; set => base.Text = value; }

        /// <summary>
        /// Gets or sets the border style of the line.
        /// </summary>
        [Category("Appearance"), DefaultValue(BorderStyle.Fixed3D)]
        [Description("Gets or sets the border style of the line.")]
        public BorderStyle BorderStyle { get => borderStyle; set { borderStyle = value; Invalidate(); } }

        /// <summary>
        /// Gets or sets the border color of the line.
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Color), "223, 223, 223")]
        [Description("Gets or sets the border color of the line.")]
        public Color BorderColor { get => borderColor; set { borderColor = value; Invalidate(); } }
        #endregion

        #region Constructor
        public HorizontalLine()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.DoubleBuffer, true);

            borderStyle = BorderStyle.Fixed3D;
            borderColor = Color.FromArgb(223, 223, 223);
        }
        #endregion

        #region Overriden Methods
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, (BorderStyle == BorderStyle.Fixed3D ? 2 : 1), specified);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (BorderStyle == BorderStyle.Fixed3D)
                ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, Border3DStyle.SunkenOuter);
            else
                e.Graphics.Clear(BorderColor);
        }
        #endregion

        #region Control Designer
        internal class HorizontalLineDesigner : ControlDesigner
        {
            public override SelectionRules SelectionRules
            {
                get
                {
                    return SelectionRules.LeftSizeable | SelectionRules.RightSizeable | SelectionRules.Moveable;
                }
            }
        }
        #endregion
    }
}
