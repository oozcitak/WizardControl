using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Manina.Windows.Forms
{
    [Designer(typeof(HorizontalLineDesigner))]
    public class HorizontalLine : Control
    {
        #region Properties
        /// <summary>
        /// Gets or sets the text associated with this control.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public override string Text { get => base.Text; set => base.Text = value; }
        #endregion

        #region Constructor
        public HorizontalLine()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.DoubleBuffer, true);
        }
        #endregion

        #region Overriden Methods
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, 2, specified);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, Border3DStyle.SunkenOuter);
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
