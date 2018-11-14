using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

namespace Manina.Windows.Forms
{
    internal class WizardControlDesigner : ParentControlDesigner
    {
        #region Member Variables
        private BehaviorService behaviorService;
        private ISelectionService selectionService;

        private DesignerVerb addPageVerb;
        private DesignerVerb removePageVerb;
        private DesignerVerb navigateBackVerb;
        private DesignerVerb navigateNextVerb;
        private DesignerVerbCollection verbs;

        private Adorner buttonAdorner;
        #endregion

        #region Properties
        public override DesignerVerbCollection Verbs => verbs;

        public new WizardControl Control => (WizardControl)base.Control;
        #endregion

        #region Glyph Icons
        private static PointF[] GetLeftArrowSign(float size)
        {
            float arrowHeadThickness = size;
            float arrowTailThickness = 0.375f * size;
            float arrowHeadLength = 0.5625f * size;
            float arrowTailLength = size - arrowHeadLength;

            return new PointF[] {
                new PointF(0, size / 2f),
                new PointF(arrowHeadLength, size / 2f - arrowHeadThickness / 2f),
                new PointF(arrowHeadLength, size / 2f - arrowTailThickness / 2f),
                new PointF(arrowHeadLength + arrowTailLength, size / 2f - arrowTailThickness / 2f),
                new PointF(arrowHeadLength + arrowTailLength, size / 2f + arrowTailThickness / 2f),
                new PointF(arrowHeadLength, size / 2f + arrowTailThickness / 2f),
                new PointF(arrowHeadLength, size / 2f + arrowHeadThickness / 2f),
            };
        }

        private static PointF[] GetRightArrowSign(float size)
        {
            float arrowHeadThickness = size;
            float arrowTailThickness = 0.375f * size;
            float arrowHeadLength = 0.5625f * size;
            float arrowTailLength = size - arrowHeadLength;

            return new PointF[] {
                new PointF(size, size / 2f),
                new PointF(size - arrowHeadLength, size / 2f - arrowHeadThickness / 2f),
                new PointF(size - arrowHeadLength, size / 2f - arrowTailThickness / 2f),
                new PointF(size - arrowHeadLength - arrowTailLength, size / 2f - arrowTailThickness / 2f),
                new PointF(size - arrowHeadLength - arrowTailLength, size / 2f + arrowTailThickness / 2f),
                new PointF(size - arrowHeadLength, size / 2f + arrowTailThickness / 2f),
                new PointF(size - arrowHeadLength, size / 2f + arrowHeadThickness / 2f),
            };
        }

        private static PointF[] GetPlusSign(float size)
        {
            float thickness = 0.375f * size;

            return new PointF[] {
                new PointF(0, size / 2f - thickness / 2f),
                new PointF(size / 2f - thickness / 2f, size / 2f - thickness / 2f),
                new PointF(size / 2f - thickness / 2f, 0),
                new PointF(size / 2f + thickness / 2f, 0),
                new PointF(size / 2f + thickness / 2f, size / 2f - thickness / 2f),
                new PointF(size, size / 2f - thickness / 2f),
                new PointF(size, size / 2f + thickness / 2f),
                new PointF(size / 2f + thickness / 2f, size / 2f + thickness / 2f),
                new PointF(size / 2f + thickness / 2f, size),
                new PointF(size / 2f - thickness / 2f, size),
                new PointF(size / 2f - thickness / 2f, size / 2f + thickness / 2f),
                new PointF(0, size / 2f + thickness / 2f),
            };
        }

        private static PointF[] GetMinusSign(float size)
        {
            float thickness = 0.375f * size;

            return new PointF[] {
                new PointF(0, size / 2f - thickness / 2f),
                new PointF(size, size / 2f - thickness / 2f),
                new PointF(size, size / 2f + thickness / 2f),
                new PointF(0, size / 2f + thickness / 2f),
            };
        }
        #endregion

        #region Initialize/Dispose
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            navigateBackVerb = new DesignerVerb("Previous page", new EventHandler(NavigateBackHandler));
            navigateNextVerb = new DesignerVerb("Next page", new EventHandler(NavigateNextHandler));
            addPageVerb = new DesignerVerb("Add page", new EventHandler(AddPageHandler));
            removePageVerb = new DesignerVerb("Remove page", new EventHandler(RemovePageHandler));

            verbs = new DesignerVerbCollection();
            verbs.AddRange(new DesignerVerb[] { navigateBackVerb, navigateNextVerb, addPageVerb, removePageVerb });

            behaviorService = (BehaviorService)GetService(typeof(BehaviorService));
            selectionService = (ISelectionService)GetService(typeof(ISelectionService));

            buttonAdorner = new Adorner();
            behaviorService.Adorners.Add(buttonAdorner);
            int glyphSize = 16;
            int glyphXOffset = glyphSize + 8;
            int glyphX = 8;
            int glyphY = Control.UIArea.Top + (Control.UIArea.Height - glyphSize) / 2;
            buttonAdorner.Glyphs.Add(new ButtonGlyph(behaviorService, selectionService, this, buttonAdorner, "prev_page", GetLeftArrowSign(glyphSize), glyphX, glyphY, glyphSize, AnchorStyles.Left | AnchorStyles.Bottom));
            buttonAdorner.Glyphs.Add(new ButtonGlyph(behaviorService, selectionService, this, buttonAdorner, "next_page", GetRightArrowSign(glyphSize), glyphX += glyphXOffset, glyphY, glyphSize, AnchorStyles.Left | AnchorStyles.Bottom));
            buttonAdorner.Glyphs.Add(new ButtonGlyph(behaviorService, selectionService, this, buttonAdorner, "add_page", GetPlusSign(glyphSize), glyphX += glyphXOffset, glyphY, glyphSize, AnchorStyles.Left | AnchorStyles.Bottom));
            buttonAdorner.Glyphs.Add(new ButtonGlyph(behaviorService, selectionService, this, buttonAdorner, "remove_page", GetMinusSign(glyphSize), glyphX += glyphXOffset, glyphY, glyphSize, AnchorStyles.Left | AnchorStyles.Bottom));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (behaviorService != null)
                    behaviorService.Adorners.Remove(buttonAdorner);
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Gets the designer of the current page.
        /// </summary>
        /// <returns>The designer of the wizard page currently active in the designer.</returns>
        private WizardPage.WizardPageDesigner GetCurrentPageDesigner()
        {
            var page = Control.CurrentPage;
            if (page != null)
            {
                IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
                if (host != null)
                    return (WizardPage.WizardPageDesigner)host.GetDesigner(page);
            }
            return null;
        }
        #endregion

        #region Verb Handlers
        /// <summary>
        /// Adds a new wizard page.
        /// </summary>
        protected void AddPageHandler(object sender, EventArgs e)
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));

            if (host != null)
            {
                WizardPage page = (WizardPage)host.CreateComponent(typeof(WizardPage));
                Control.Pages.Add(page);
                Control.CurrentPage = page;

                selectionService.SetSelectedComponents(new Component[] { Control });
            }
        }

        /// <summary>
        /// Removes the current wizard page.
        /// </summary>
        protected void RemovePageHandler(object sender, EventArgs e)
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));

            if (host != null)
            {
                if (Control.Pages.Count > 1)
                {
                    WizardPage page = Control.CurrentPage;
                    int index = Control.CurrentPageIndex;
                    host.DestroyComponent(page);
                    if (index == Control.Pages.Count)
                        index = Control.Pages.Count - 1;
                    Control.CurrentPage = (WizardPage)Control.Pages[index];

                    selectionService.SetSelectedComponents(new Component[] { Control });
                }
            }
        }

        /// <summary>
        /// Navigates to the previous wizard page.
        /// </summary>
        protected void NavigateBackHandler(object sender, EventArgs e)
        {
            WizardControl control = Control;

            if (control.CanGoBack)
                control.GoBack();

            selectionService.SetSelectedComponents(new Component[] { Control });
        }

        /// <summary>
        /// Navigates to the next wizard page.
        /// </summary>
        protected void NavigateNextHandler(object sender, EventArgs e)
        {
            WizardControl control = Control;

            if (control.CanGoNext)
                control.GoNext();

            selectionService.SetSelectedComponents(new Component[] { Control });
        }

        /// <summary>
        /// Occurs when a glyph is clicked on the designer surface.
        /// </summary>
        /// <param name="key">Glyph key.</param>
        public void OnGlyphClick(string key)
        {
            WizardControl control = Control;

            switch (key)
            {
                case "prev_page":
                    NavigateBackHandler(null, null);
                    break;
                case "next_page":
                    NavigateNextHandler(null, null);
                    break;
                case "add_page":
                    AddPageHandler(null, null);
                    break;
                case "remove_page":
                    RemovePageHandler(null, null);
                    break;
            }
        }
        #endregion

        #region Delegate All Drag Events To The PageContainer
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
}
