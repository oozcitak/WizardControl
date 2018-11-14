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
    internal class PageContainer : Panel
    {
        #region Member Variables
        private WizardPage currentPage;
        #endregion

        #region Properties
        public WizardPage CurrentPage
        {
            get => currentPage;
            set
            {
                if (currentPage == value)
                    return;

                if (currentPage != null)
                    currentPage.Visible = false;

                currentPage = value;

                if (currentPage != null)
                    currentPage.Visible = true;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PageContainer"/> class.
        /// </summary>
        public PageContainer()
        {
            // Default page
            WizardPage page = new WizardPage();
            Controls.Add(page);
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
                    CurrentPage = page;
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
                if (Controls.Count == 0)
                {
                    var defaultPage = new WizardPage();
                    Controls.Add(defaultPage);
                    CurrentPage = defaultPage;
                }
                else if (ReferenceEquals(currentPage, page))
                {
                    CurrentPage = (WizardPage)Controls[0];
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
                return ((PageContainer)Control).CurrentPage;
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
