using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Manina.Windows.Forms
{
    [ToolboxItem(false)]
    [Designer(typeof(WizardPageDesigner))]
    [Docking(DockingBehavior.Never)]
    public class WizardPage : Control
    {
        #region Properties
        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Color), "Window")]
        [Description("Gets or sets the background color for the control.")]
        public override Color BackColor { get => base.BackColor; set => base.BackColor = value; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text { get => base.Text; set => base.Text = value; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override DockStyle Dock { get => base.Dock; set => base.Dock = value; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Point Location { get { return base.Location; } set { base.Location = value; } }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override AnchorStyles Anchor { get => base.Anchor; set => base.Anchor = value; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Size MinimumSize { get => new Size(0, 0); set => base.MinimumSize = new Size(0, 0); }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Size MaximumSize { get => new Size(0, 0); set => base.MinimumSize = new Size(0, 0); }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Size Size { get => base.Size; set => base.Size = value; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool Visible { get => base.Visible; set => base.Visible = value; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool Enabled { get => base.Enabled; set => base.Enabled = value; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool TabStop { get => base.TabStop; set => base.TabStop = value; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int TabIndex { get => base.TabIndex; set => base.TabIndex = value; }
        #endregion

        #region Overriden Methods
        protected override void OnPaint(PaintEventArgs e)
        {
            WizardControl control = (WizardControl)Parent;

            if (!control.OwnerDraw)
            {
                base.OnPaint(e);

                if (DesignMode)
                {
                    int index = control.Pages.IndexOf(this);
                    e.Graphics.DrawString(string.Format("{0}: {1}", index, Name), Font, SystemBrushes.GrayText, 5, 5);
                }
            }

            control.OnPagePaint(new WizardControl.PagePaintEventArgs(e.Graphics, this));
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="WizardPage"/> class.
        /// </summary>
        public WizardPage()
        {
            BackColor = SystemColors.Window;
        }
        #endregion

        #region ControlDesigner
        internal class WizardPageDesigner : ParentControlDesigner
        {
            #region Properties
            public override SelectionRules SelectionRules => SelectionRules.Locked;
            #endregion

            #region Parent/Child Relation
            public override bool CanBeParentedTo(IDesigner parentDesigner)
            {
                return (parentDesigner != null && parentDesigner.Component is TabControl);
            }
            #endregion

            #region Drag Events
            public new void OnDragEnter(DragEventArgs de)
            {
                base.OnDragEnter(de);
            }

            public new void OnDragOver(DragEventArgs de)
            {
                base.OnDragOver(de);
            }

            public new void OnDragLeave(EventArgs e)
            {
                base.OnDragLeave(e);
            }

            public new void OnDragDrop(DragEventArgs de)
            {
                base.OnDragDrop(de);
            }

            public new void OnGiveFeedback(GiveFeedbackEventArgs e)
            {
                base.OnGiveFeedback(e);
            }

            public new void OnDragComplete(DragEventArgs de)
            {
                base.OnDragComplete(de);
            }
            #endregion
        }
        #endregion
    }
}
