using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    [ToolboxBitmap(typeof(WizardControl))]
    [Designer(typeof(WizardControlDesigner))]
    [Docking(DockingBehavior.AutoDock)]
    [DefaultEvent("PageChanged")]
    [DefaultProperty("SelectedPage")]
    public partial class WizardControl : Control
    {
        #region Events
        /// <summary>
        /// Contains event data for button events.
        /// </summary>
        public class ButtonClickEventArgs : CancelEventArgs
        {
            public ButtonClickEventArgs() : base(false)
            {

            }
        }

        /// <summary>
        /// Contains event data for events related to a singe page.
        /// </summary>
        public class PageEventArgs : EventArgs
        {
            /// <summary>
            /// The page causing the event.
            /// </summary>
            public WizardPage Page { get; private set; }

            public PageEventArgs(WizardPage page)
            {
                Page = page;
            }
        }

        /// <summary>
        /// Contains event data for the <see cref="PageChanging"/> event.
        /// </summary>
        public class PageChangingEventArgs : CancelEventArgs
        {
            /// <summary>
            /// Current page.
            /// </summary>
            public WizardPage CurrentPage { get; private set; }
            /// <summary>
            /// The page that will become the current page after the event.
            /// </summary>
            public WizardPage NewPage { get; set; }

            public PageChangingEventArgs(WizardPage currentPage, WizardPage newPage) : base(false)
            {
                CurrentPage = currentPage;
                NewPage = newPage;
            }
        }

        /// <summary>
        /// Contains event data for the <see cref="PageChanged"/> event.
        /// </summary>
        public class PageChangedEventArgs : EventArgs
        {
            /// <summary>
            /// The page that was the current page before the event.
            /// </summary>
            public WizardPage OldPage { get; private set; }
            /// <summary>
            /// Current page.
            /// </summary>
            public WizardPage CurrentPage { get; private set; }

            public PageChangedEventArgs(WizardPage oldPage, WizardPage currentPage)
            {
                OldPage = oldPage;
                CurrentPage = currentPage;
            }
        }

        /// <summary>
        /// Contains event data for the <see cref="PageValidating"/> event.
        /// </summary>
        public class PageValidatingEventArgs : CancelEventArgs
        {
            /// <summary>
            /// The page causing the event.
            /// </summary>
            public WizardPage Page { get; private set; }

            public PageValidatingEventArgs(WizardPage page) : base(false)
            {
                Page = page;
            }
        }

        /// <summary>
        /// Contains event data for the <see cref="PagePaint"/> event.
        /// </summary>
        public class PagePaintEventArgs : PageEventArgs
        {
            /// <summary>
            /// Gets the graphics used to paint.
            /// </summary>
            public Graphics Graphics { get; private set; }

            public PagePaintEventArgs(Graphics graphics, WizardPage page) : base(page)
            {
                Graphics = graphics;
            }
        }

        public delegate void ButtonClickEventHandler(object sender, ButtonClickEventArgs e);
        public delegate void PageEventHandler(object sender, PageEventArgs e);
        public delegate void PageChangingEventHandler(object sender, PageChangingEventArgs e);
        public delegate void PageChangedEventHandler(object sender, PageChangedEventArgs e);
        public delegate void PageValidatingEventHandler(object sender, PageValidatingEventArgs e);
        public delegate void PagePaintEventHandler(object sender, PagePaintEventArgs e);

        protected internal virtual void OnBackButtonClicked(ButtonClickEventArgs e) { BackButtonClicked?.Invoke(this, e); }
        protected internal virtual void OnNextButtonClicked(ButtonClickEventArgs e) { NextButtonClicked?.Invoke(this, e); }
        protected internal virtual void OnCloseButtonClicked(ButtonClickEventArgs e) { CloseButtonClicked?.Invoke(this, e); }
        protected internal virtual void OnHelpButtonClicked(EventArgs e) { HelpButtonClicked?.Invoke(this, e); }
        protected internal virtual void OnPageAdded(PageEventArgs e) { PageAdded?.Invoke(this, e); }
        protected internal virtual void OnPageRemoved(PageEventArgs e) { PageRemoved?.Invoke(this, e); }
        protected internal virtual void OnCurrentPageChanging(PageChangingEventArgs e) { PageChanging?.Invoke(this, e); }
        protected internal virtual void OnCurrentPageChanged(PageChangedEventArgs e) { PageChanged?.Invoke(this, e); }
        protected internal virtual void OnPageValidating(PageValidatingEventArgs e) { PageValidating?.Invoke(this, e); }
        protected internal virtual void OnPageValidated(PageEventArgs e) { PageValidated?.Invoke(this, e); }
        protected internal virtual void OnPageHidden(PageEventArgs e) { PageHidden?.Invoke(this, e); }
        protected internal virtual void OnPageShown(PageEventArgs e) { PageShown?.Invoke(this, e); }
        protected internal virtual void OnPagePaint(PagePaintEventArgs e) { PagePaint?.Invoke(this, e); }

        /// <summary>
        /// Occurs when the back button is clicked.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the back button is clicked.")]
        public event ButtonClickEventHandler BackButtonClicked;
        /// <summary>
        /// Occurs when the next button is clicked.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the next button is clicked.")]
        public event ButtonClickEventHandler NextButtonClicked;
        /// <summary>
        /// Occurs when the close button is clicked.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the close button is clicked.")]
        public event ButtonClickEventHandler CloseButtonClicked;
        /// <summary>
        /// Occurs when the help button is clicked.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the help button is clicked.")]
        public event EventHandler HelpButtonClicked;
        /// <summary>
        /// Occurs when a new page is added.
        /// </summary>
        [Category("Behavior"), Description("Occurs when a new page is added.")]
        public event PageEventHandler PageAdded;
        /// <summary>
        /// Occurs when a page is removed.
        /// </summary>
        [Category("Behavior"), Description("Occurs when a page is removed.")]
        public event PageEventHandler PageRemoved;
        /// <summary>
        /// Occurs before the current page is changed.
        /// </summary>
        [Category("Behavior"), Description("Occurs before the current page is changed.")]
        public event PageChangingEventHandler PageChanging;
        /// <summary>
        /// Occurs after the current page is changed.
        /// </summary>
        [Category("Behavior"), Description("Occurs after the current page is changed.")]
        public event PageChangedEventHandler PageChanged;
        /// <summary>
        /// Occurs when the current page is validating.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the current page is validating.")]
        public event PageValidatingEventHandler PageValidating;
        /// <summary>
        /// Occurs after the current page is successfully validated.
        /// </summary>
        [Category("Behavior"), Description("Occurs after the current page is successfully validated.")]
        public event PageEventHandler PageValidated;
        /// <summary>
        /// Occurs while the current page is changing and the previous current page is hidden.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the current page is changing and the previous current page is hidden.")]
        public event PageEventHandler PageHidden;
        /// <summary>
        /// Occurs while the current page is changing and the new current page is shown.
        /// </summary>
        [Category("Behavior"), Description("Occurs while the current page is changing and the new current page is shown.")]
        public event PageEventHandler PageShown;
        /// <summary>
        /// Occurs when a page is painted.
        /// </summary>
        [Category("Appearance"), Description("Occurs when a page is painted.")]
        public event PagePaintEventHandler PagePaint;
        #endregion

        #region Member Variables
        private Button backButton;
        private Button nextButton;
        private Button closeButton;
        private Button helpButton;

        private bool backButtonEnabled = true;
        private bool nextButtonEnabled = true;
        private bool closeButtonEnabled = true;
        private bool helpButtonEnabled = true;

        private int selectedIndex;
        private WizardPage lastSelectedPage;
        internal bool creatingUIControls;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the index of the first page in the controls collection.
        /// </summary>
        internal int FirstPageIndex => 4;
        /// <summary>
        /// Gets the number of pages.
        /// </summary>
        internal int PageCount => Controls.Count - 4;

        /// <summary>
        /// Gets or sets the current page of the wizard.
        /// </summary>
        [Editor(typeof(WizardControlUITypeEditor), typeof(UITypeEditor))]
        [Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Gets or sets the current page of the wizard.")]
        public WizardPage SelectedPage
        {
            get
            {
                return (selectedIndex == -1 ? null : Pages[selectedIndex]);
            }
            set
            {
                var oldPage = lastSelectedPage;
                var newPage = value;

                if (newPage != null && !Pages.Contains(newPage))
                    throw new ArgumentException("Page is not found in the page collection.");

                if (oldPage == value)
                    return;

                if (oldPage != null && oldPage.CausesValidation)
                {
                    PageValidatingEventArgs pve = new PageValidatingEventArgs(oldPage);
                    OnPageValidating(pve);
                    if (pve.Cancel) return;

                    OnPageValidated(new PageEventArgs(oldPage));
                }

                if (oldPage != null && newPage != null)
                {
                    PageChangingEventArgs pce = new PageChangingEventArgs(oldPage, newPage);
                    OnCurrentPageChanging(pce);
                    if (pce.Cancel) return;
                }

                selectedIndex = (newPage == null ? -1 : Pages.IndexOf(newPage));
                if (oldPage != null) oldPage.Visible = false;
                if (newPage != null) newPage.Visible = true;

                lastSelectedPage = newPage;

                UpdateNavigationControls();

                if (oldPage != null)
                    OnPageHidden(new PageEventArgs(oldPage));

                if (newPage != null)
                    OnPageShown(new PageEventArgs(newPage));

                if (oldPage != null && newPage != null)
                    OnCurrentPageChanged(new PageChangedEventArgs(oldPage, newPage));
            }
        }

        /// <summary>
        /// Gets or sets the zero-based index of the current page of the wizard.
        /// </summary>
        [Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Gets or sets the zero-based index of the current page of the wizard.")]
        public int SelectedIndex
        {
            get => selectedIndex;
            set { SelectedPage = (value == -1 ? null : Pages[value]); }
        }

        /// <summary>
        /// Gets or sets the collection pages in the wizard.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets the collection pages in the wizard.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public WizardPageCollection Pages { get; }

        /// <summary>
        /// Gets or sets the text of the back button.
        /// </summary>
        [Category("Appearance"), Localizable(true), DefaultValue("Back")]
        [Description("Gets or sets the text of the back button.")]
        public string BackButtonText { get => backButton.Text; set => backButton.Text = value; }

        /// <summary>
        /// Gets or sets the text of the next button.
        /// </summary>
        [Category("Appearance"), Localizable(true), DefaultValue("Next")]
        [Description("Gets or sets the text of the next button.")]
        public string NextButtonText { get => nextButton.Text; set => nextButton.Text = value; }

        /// <summary>
        /// Gets or sets the text of the close button.
        /// </summary>
        [Category("Appearance"), Localizable(true), DefaultValue("Close")]
        [Description("Gets or sets the text of the close button.")]
        public string CloseButtonText { get => closeButton.Text; set => closeButton.Text = value; }

        /// <summary>
        /// Gets or sets whether the back button is enabled by user code.
        /// </summary>
        [Category("Behavior"), Localizable(true), DefaultValue(true)]
        [Description("Gets or sets whether the back button is enabled by user code.")]
        public bool BackButtonEnabled { get => backButtonEnabled; set { backButtonEnabled = value; UpdateNavigationControls(); } }

        /// <summary>
        /// Gets or sets whether the next button is enabled by user code.
        /// </summary>
        [Category("Behavior"), Localizable(true), DefaultValue(true)]
        [Description("Gets or sets whether the next button is enabled by user code.")]
        public bool NextButtonEnabled { get => nextButtonEnabled; set { nextButtonEnabled = value; UpdateNavigationControls(); } }

        /// <summary>
        /// Gets or sets whether the close button is enabled by user code.
        /// </summary>
        [Category("Behavior"), Localizable(true), DefaultValue(true)]
        [Description("Gets or sets whether the close button is enabled by user code.")]
        public bool CloseButtonEnabled { get => closeButtonEnabled; set { closeButtonEnabled = value; UpdateNavigationControls(); } }

        /// <summary>
        /// Gets or sets whether the help button is enabled by user code.
        /// </summary>
        [Category("Behavior"), Localizable(true), DefaultValue(true)]
        [Description("Gets or sets whether the help button is enabled by user code.")]
        public bool HelpButtonEnabled { get => helpButtonEnabled; set { helpButtonEnabled = value; UpdateNavigationControls(); } }

        /// <summary>
        /// Gets or sets whether the back button is visible.
        /// </summary>
        [Category("Appearance"), DefaultValue(true)]
        [Description("Gets or sets whether the back button is visible.")]
        public bool BackButtonVisible { get => backButton.Visible; set { backButton.Visible = value; } }

        /// <summary>
        /// Gets or sets whether the next button is visible.
        /// </summary>
        [Category("Appearance"), DefaultValue(true)]
        [Description("Gets or sets whether the next button is visible.")]
        public bool NextButtonVisible { get => nextButton.Visible; set { nextButton.Visible = value; } }

        /// <summary>
        /// Gets or sets whether the close button is visible.
        /// </summary>
        [Category("Appearance"), DefaultValue(true)]
        [Description("Gets or sets whether the close button is visible.")]
        public bool CloseButtonVisible { get => closeButton.Visible; set { closeButton.Visible = value; } }

        /// <summary>
        /// Gets or sets whether the help button is visible.
        /// </summary>
        [Category("Appearance"), DefaultValue(false)]
        [Description("Gets or sets whether the help button is visible.")]
        public bool HelpButtonVisible { get => helpButton.Visible; set { helpButton.Visible = value; } }

        /// <summary>
        /// Gets or sets the drawing mode for the control.
        /// </summary>
        [Category("Behavior"), DefaultValue(false)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description("Gets or sets the drawing mode for the control.")]
        public bool OwnerDraw { get; set; }

        /// <summary>
        /// Gets the size of the control when it is initially created.
        /// </summary>
        protected override Size DefaultSize => new Size(300, 200);

        /// <summary>
        /// Determines whether the wizard can navigate to the previous page.
        /// </summary>
        [Browsable(false)]
        public bool CanGoBack => (Pages.Count != 0) && !(ReferenceEquals(SelectedPage, Pages[0]));

        /// <summary>
        /// Determines whether the wizard can navigate to the next page.
        /// </summary>
        [Browsable(false)]
        public bool CanGoNext => (Pages.Count != 0) && !(ReferenceEquals(SelectedPage, Pages[Pages.Count - 1]));

        /// <summary>
        /// Gets the height of the area where user interface controls are located.
        /// </summary>
        [Browsable(false)]
        internal int UIAreaHeight => 2 + 12 + 23 + 12;

        /// <summary>
        /// Gets the client rectangle where wizard pages are located.
        /// </summary>
        [Browsable(false)]
        public override Rectangle DisplayRectangle => new Rectangle(ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Width, ClientRectangle.Height - UIAreaHeight);

        /// <summary>
        /// Gets the client rectangle where user interface controls are located.
        /// </summary>
        [Browsable(false)]
        public Rectangle UIArea => new Rectangle(ClientRectangle.Left, ClientRectangle.Height - UIAreaHeight, ClientRectangle.Width, UIAreaHeight);
        #endregion

        #region Unused Methods - Hide From User
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), Bindable(false)]
        public override string Text { get => base.Text; set => base.Text = value; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ControlCollection Controls => base.Controls;

#pragma warning disable CS0067
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        new public event EventHandler TextChanged;
#pragma warning restore CS0067
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="WizardControl"/> class.
        /// </summary>
        public WizardControl()
        {
            creatingUIControls = false;
            Pages = new WizardPageCollection(this);
            selectedIndex = -1;
            lastSelectedPage = null;
            SetStyle(ControlStyles.ResizeRedraw, true);

            CreateChildControls();

            UpdateNavigationControls();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Navigates to the previous page.
        /// </summary>
        public void GoBack()
        {
            if (Pages.Count == 0) return;
            if (!CanGoBack) return;

            int index = SelectedIndex;
            if (index == -1) return;

            SelectedIndex = index - 1;
        }

        /// <summary>
        /// Navigates to the next page.
        /// </summary>
        public void GoNext()
        {
            if (Pages.Count == 0) return;
            if (!CanGoNext) return;

            int index = SelectedIndex;
            if (index == -1) return;

            SelectedIndex = index + 1;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Create the UI controls of the wizard.
        /// </summary>
        private void CreateChildControls()
        {
            creatingUIControls = true;

            helpButton = new Button();
            helpButton.Text = "Help";
            helpButton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            helpButton.Click += HelpButton_Click;
            helpButton.Visible = false;
            Controls.Add(helpButton);

            backButton = new Button();
            backButton.Text = "Back";
            backButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            backButton.Click += BackButton_Click;
            Controls.Add(backButton);

            nextButton = new Button();
            nextButton.Text = "Next";
            nextButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom; nextButton.Click += NextButton_Click;
            Controls.Add(nextButton);

            closeButton = new Button();
            closeButton.Text = "Close";
            closeButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            closeButton.Click += CloseButton_Click;
            Controls.Add(closeButton);

            ResizeControls();

            creatingUIControls = false;
        }

        internal void ResizeControls()
        {
            var pageBounds = DisplayRectangle;
            var uiBounds = UIArea;

            int buttonWidth = 75;
            int buttonHeight = 23;

            int buttonLeft = uiBounds.Right - (buttonWidth + buttonWidth + 12 + buttonWidth + 12);
            int buttonTop = uiBounds.Bottom - (buttonHeight + 12);
            int helpButtonLeft = uiBounds.Left + 12;

            helpButton.SetBounds(helpButtonLeft, buttonTop, 0, 0, BoundsSpecified.Location);
            backButton.SetBounds(buttonLeft, buttonTop, 0, 0, BoundsSpecified.Location);
            nextButton.SetBounds(buttonLeft + buttonWidth, buttonTop, 0, 0, BoundsSpecified.Location);
            closeButton.SetBounds(buttonLeft + backButton.Width + nextButton.Width + 12, buttonTop, 0, 0, BoundsSpecified.Location);

            UpdatePages();
        }

        internal void UpdateNavigationControls()
        {
            backButton.Enabled = backButtonEnabled && CanGoBack;
            nextButton.Enabled = nextButtonEnabled && CanGoNext;
            closeButton.Enabled = closeButtonEnabled;
        }

        internal void UpdatePages()
        {
            var pageBounds = DisplayRectangle;

            for (int i = 0; i < Pages.Count; i++)
            {
                WizardPage page = Pages[i];
                page.SetBounds(pageBounds.Left, pageBounds.Top, pageBounds.Width, pageBounds.Height, BoundsSpecified.All);
                page.Visible = (i == selectedIndex);
            }
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            OnHelpButtonClicked(new EventArgs());
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            ButtonClickEventArgs be = new ButtonClickEventArgs();
            OnBackButtonClicked(be);
            if (!be.Cancel) GoBack();
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            ButtonClickEventArgs be = new ButtonClickEventArgs();
            OnNextButtonClicked(be);
            if (!be.Cancel) GoNext();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            ButtonClickEventArgs be = new ButtonClickEventArgs();
            OnCloseButtonClicked(be);
            if (!be.Cancel) FindForm().Close();
        }
        #endregion

        #region Overriden Methods
        protected override ControlCollection CreateControlsInstance()
        {
            return new WizardControlCollection(this);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            ResizeControls();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Pen pen = new Pen(Color.FromArgb(223, 223, 223)))
            {
                var uiBounds = UIArea;
                e.Graphics.DrawLine(pen, uiBounds.Left, uiBounds.Top, uiBounds.Right, uiBounds.Top);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                backButton.Click -= BackButton_Click;
                nextButton.Click -= NextButton_Click;
                closeButton.Click -= CloseButton_Click;
            }

            base.Dispose(disposing);
        }
        #endregion

        #region WizardControlCollection
        internal class WizardControlCollection : ControlCollection
        {
            private readonly WizardControl owner;

            public WizardControlCollection(WizardControl ownerControl) : base(ownerControl)
            {
                owner = ownerControl;
            }

            public override void Add(Control value)
            {
                if (owner.creatingUIControls)
                {
                    base.Add(value);
                    return;
                }

                if (!(value is WizardPage))
                {
                    throw new ArgumentException("Only a WizardPage can be hosted in a WizardControl.");
                }

                base.Add(value);
                if (owner.PageCount == 1) owner.SelectedIndex = 0;

                owner.UpdateNavigationControls();
                owner.UpdatePages();

                // site the page
                ISite site = owner.Site;
                if (site != null && value.Site == null)
                {
                    IContainer container = site.Container;
                    if (container != null)
                    {
                        container.Add(value);
                    }
                }
            }

            public override void Remove(Control value)
            {
                if (!base.Contains(value))
                {
                    throw new ArgumentException("Control not found in collection.", "value");
                }

                if (owner.creatingUIControls)
                {
                    base.Remove(value);
                    return;
                }

                if (!(value is WizardPage))
                {
                    throw new ArgumentException("Only a WizardPage can be hosted in a WizardControl.");
                }

                base.Remove(value);

                if (owner.PageCount == 0)
                    owner.SelectedIndex = -1;
                else if (owner.SelectedIndex > owner.PageCount - 1)
                    owner.SelectedIndex = 0;

                owner.UpdateNavigationControls();
                owner.UpdatePages();
            }

            public override Control this[int index] => base[index];
        }
        #endregion

        #region UITypeEditor
        internal class WizardControlUITypeEditor : ObjectSelectorEditor
        {
            protected override void FillTreeWithData(Selector selector, ITypeDescriptorContext context, IServiceProvider provider)
            {
                base.FillTreeWithData(selector, context, provider);

                WizardControl control = (WizardControl)context.Instance;

                foreach (var page in control.Pages)
                {
                    SelectorNode node = new SelectorNode(page.Name, page);
                    selector.Nodes.Add(node);

                    if (page == control.SelectedPage)
                        selector.SelectedNode = node;
                }
            }
        }
        #endregion
    }
}
