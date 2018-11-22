using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Manina.Windows.Forms
{
    [ToolboxItem(false), DesignTimeVisible(false)]
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
        public override DockStyle Dock { get => DockStyle.None; set { } }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Point Location { get => new Point(0, 0); set { } }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override AnchorStyles Anchor { get => AnchorStyles.None; set { } }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Size MinimumSize { get => new Size(0, 0); set { } }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Size MaximumSize { get => new Size(0, 0); set { } }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool Visible { get => base.Visible; set => base.Visible = value; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool Enabled { get => base.Enabled; set => base.Enabled = value; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool TabStop { get => true; set => base.TabStop = true; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int TabIndex { get => 0; set => base.TabIndex = 0; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Padding Margin { get => base.Margin; set { } }

        [Browsable(false), Category("Layout"), Localizable(true)]
        public new Size Size { get => base.Size; set { } }
        #endregion

        #region Overriden Methods
        protected override ControlCollection CreateControlsInstance()
        {
            return new WizardPageControlCollection(this);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            WizardControl control = (WizardControl)Parent;

            if (!control.OwnerDraw)
            {
                base.OnPaint(e);
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
            SetStyle(ControlStyles.ResizeRedraw, true);
            BackColor = SystemColors.Window;
        }
        #endregion

        #region WizardPageControlCollection
        internal class WizardPageControlCollection : ControlCollection
        {
            public WizardPageControlCollection(WizardPage ownerControl) : base(ownerControl)
            {
            }

            public override void Add(Control value)
            {
                if (value is WizardPage)
                {
                    throw new ArgumentException("Cannot add a WizardPage as a child control of another WizardPage.");
                }

                base.Add(value);
            }
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
                return (parentDesigner != null && parentDesigner.Component is WizardControl);
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
