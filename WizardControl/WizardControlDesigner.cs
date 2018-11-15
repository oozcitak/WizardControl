using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

namespace Manina.Windows.Forms
{
    public partial class WizardControl
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

            private ButtonGlyph addPageButton;
            private ButtonGlyph removePageButton;
            private ButtonGlyph navigateBackButton;
            private ButtonGlyph navigateNextButton;

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

                navigateBackButton = new ButtonGlyph(behaviorService, this, buttonAdorner, GetLeftArrowSign(glyphSize), glyphX, glyphY, glyphSize, AnchorStyles.Left | AnchorStyles.Bottom);
                navigateNextButton = new ButtonGlyph(behaviorService, this, buttonAdorner, GetRightArrowSign(glyphSize), glyphX += glyphXOffset, glyphY, glyphSize, AnchorStyles.Left | AnchorStyles.Bottom);
                addPageButton = new ButtonGlyph(behaviorService, this, buttonAdorner, GetPlusSign(glyphSize), glyphX += glyphXOffset, glyphY, glyphSize, AnchorStyles.Left | AnchorStyles.Bottom);
                removePageButton = new ButtonGlyph(behaviorService, this, buttonAdorner, GetMinusSign(glyphSize), glyphX += glyphXOffset, glyphY, glyphSize, AnchorStyles.Left | AnchorStyles.Bottom);

                navigateBackButton.Click += NavigateBackButton_Click;
                navigateNextButton.Click += NavigateNextButton_Click;
                addPageButton.Click += AddPageButton_Click;
                removePageButton.Click += RemovePageButton_Click;

                buttonAdorner.Glyphs.Add(navigateBackButton);
                buttonAdorner.Glyphs.Add(navigateNextButton);
                buttonAdorner.Glyphs.Add(addPageButton);
                buttonAdorner.Glyphs.Add(removePageButton);

                Control.PageChanged += Control_CurrentPageChanged;
                Control.PageAdded += Control_PageAdded;
                Control.PageRemoved += Control_PageRemoved;
            }

            private void Control_CurrentPageChanged(object sender, WizardControl.PageChangedEventArgs e)
            {
                UpdateGlyphs();
            }

            private void Control_PageAdded(object sender, WizardControl.PageEventArgs e)
            {
                UpdateGlyphs();
            }

            private void Control_PageRemoved(object sender, WizardControl.PageEventArgs e)
            {
                UpdateGlyphs();
            }

            private void NavigateBackButton_Click(object sender, EventArgs e)
            {
                NavigateBackHandler(null, null);
            }

            private void NavigateNextButton_Click(object sender, EventArgs e)
            {
                NavigateNextHandler(null, null);
            }

            private void AddPageButton_Click(object sender, EventArgs e)
            {
                AddPageHandler(null, null);
            }

            private void RemovePageButton_Click(object sender, EventArgs e)
            {
                RemovePageHandler(null, null);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    navigateBackButton.Click -= NavigateBackButton_Click;
                    navigateNextButton.Click -= NavigateNextButton_Click;
                    addPageButton.Click -= AddPageButton_Click;
                    removePageButton.Click -= RemovePageButton_Click;

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
                var page = Control.SelectedPage;
                if (page != null)
                {
                    IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
                    if (host != null)
                        return (WizardPage.WizardPageDesigner)host.GetDesigner(page);
                }
                return null;
            }

            /// <summary>
            /// Updates the visual states of glyphs.
            /// </summary>
            private void UpdateGlyphs()
            {
                removePageVerb.Enabled = removePageButton.Enabled = (Control.Pages.Count > 1);
                navigateBackVerb.Enabled = navigateBackButton.Enabled = (Control.SelectedIndex > 0);
                navigateNextVerb.Enabled = navigateNextButton.Enabled = (Control.SelectedIndex < Control.Pages.Count - 1);

                buttonAdorner.Invalidate();
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
                    Control.SelectedPage = page;

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
                        WizardPage page = Control.SelectedPage;
                        int index = Control.SelectedIndex;
                        host.DestroyComponent(page);
                        if (index == Control.Pages.Count)
                            index = Control.Pages.Count - 1;
                        Control.SelectedPage = Control.Pages[index];

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
}
