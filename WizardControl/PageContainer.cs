using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Manina.Windows.Forms
{
    [ToolboxItem(false)]
    [Designer(typeof(PageContainerDesigner))]
    internal class PageContainer : Control
    {
        #region Member Variables
        private WizardPage selectedPage;
        #endregion

        #region Properties
        public WizardPage SelectedPage
        {
            get => selectedPage;
            set
            {
                if (selectedPage == value)
                    return;

                if (selectedPage != null)
                    selectedPage.Visible = false;

                selectedPage = value;

                if (selectedPage != null)
                    selectedPage.Visible = true;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PageContainer"/> class.
        /// </summary>
        public PageContainer()
        {

        }
        #endregion

        #region Overriden Methods
        protected override void OnControlAdded(ControlEventArgs e)
        {
            if (e.Control is WizardPage page)
            {
                var bounds = ClientRectangle;
                page.SetBounds(bounds.Left, bounds.Top, bounds.Width, bounds.Height);
                page.Dock = DockStyle.Fill;

                base.OnControlAdded(e);

                if (Controls.Count == 1)
                    SelectedPage = page;
                else
                    page.Visible = false;

                if (Parent != null)
                    ((WizardControl)Parent).OnPageAdded(new WizardControl.PageEventArgs(page));
            }
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);

            if (e.Control is WizardPage page)
            {
                if (ReferenceEquals(selectedPage, page))
                {
                    if (Controls.Count != 0)
                        SelectedPage = (WizardPage)Controls[0];
                    else
                        SelectedPage = null;
                }

                if (Parent != null)
                    ((WizardControl)Parent).OnPageRemoved(new WizardControl.PageEventArgs(page));
            }
        }
        #endregion

        #region ControlDesigner
        internal class PageContainerDesigner : ParentControlDesigner
        {
            #region Helper Methods
            /// <summary>
            /// Gets the current wizard page.
            /// </summary>
            /// <returns>The wizard page currently active in the designer.</returns>
            private WizardPage GetCurrentPage()
            {
                return ((PageContainer)Control).SelectedPage;
            }

            /// <summary>
            /// Gets the designer of the current page.
            /// </summary>
            /// <returns>The designer of the wizard page currently active in the designer.</returns>
            private WizardPage.WizardPageDesigner GetCurrentPageDesigner()
            {
                var page = GetCurrentPage();
                if (page != null)
                {
                    IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
                    if (host != null)
                        return (WizardPage.WizardPageDesigner)host.GetDesigner(page);
                }
                return null;
            }
            #endregion

            #region Parent/Child Relation
            public override bool CanBeParentedTo(IDesigner parentDesigner)
            {
                return (parentDesigner != null && parentDesigner.Component is WizardControl);
            }

            public override bool CanParent(Control control)
            {
                return (control != null && control is WizardPage && !Control.Contains(control));
            }
            #endregion

            #region Delegate All Drag Events To The WizardPage
            protected override void OnDragEnter(DragEventArgs de)
            {
                GetCurrentPageDesigner().OnDragEnter(de);
            }

            protected override void OnDragOver(DragEventArgs de)
            {
                Point pt = Control.PointToClient(new Point(de.X, de.Y));

                if (!Control.DisplayRectangle.Contains(pt))
                    de.Effect = DragDropEffects.None;
                else
                    GetCurrentPageDesigner().OnDragOver(de);
            }

            protected override void OnDragLeave(EventArgs e)
            {
                GetCurrentPageDesigner().OnDragLeave(e);
            }

            protected override void OnDragDrop(DragEventArgs de)
            {
                GetCurrentPageDesigner().OnDragDrop(de);
            }

            protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
            {
                GetCurrentPageDesigner().OnGiveFeedback(e);
            }

            protected override void OnDragComplete(DragEventArgs de)
            {
                GetCurrentPageDesigner().OnDragComplete(de);
            }
            #endregion
        }
        #endregion
    }
}
