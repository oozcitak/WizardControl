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
        public class ButtonClickEventArgs : EventArgs
        {
            public bool Cancel { get; set; }

            public ButtonClickEventArgs()
            {
                Cancel = false;
            }
        }

        public class PageEventArgs : EventArgs
        {
            public WizardPage Page { get; private set; }

            public PageEventArgs(WizardPage page)
            {
                Page = page;
            }
        }

        public class PageChangingEventArgs : EventArgs
        {
            public WizardPage CurrentPage { get; private set; }
            public WizardPage NewPage { get; set; }
            public bool Cancel { get; set; }

            public PageChangingEventArgs(WizardPage currentPage, WizardPage newPage)
            {
                CurrentPage = currentPage;
                NewPage = newPage;

                Cancel = false;
            }
        }

        public class PageChangedEventArgs : EventArgs
        {
            public WizardPage OldPage { get; private set; }
            public WizardPage CurrentPage { get; private set; }

            public PageChangedEventArgs(WizardPage oldPage, WizardPage currentPage)
            {
                OldPage = oldPage;
                CurrentPage = currentPage;
            }
        }

        public class PageValidatingEventArgs : EventArgs
        {
            public WizardPage Page { get; private set; }
            public bool Cancel { get; set; }

            public PageValidatingEventArgs(WizardPage page)
            {
                Page = page;
                Cancel = false;
            }
        }

        public class PagePaintEventArgs : EventArgs
        {
            public Graphics Graphics { get; private set; }
            public WizardPage Page { get; private set; }

            public PagePaintEventArgs(Graphics graphics, WizardPage page)
            {
                Graphics = graphics;
                Page = page;
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
        protected internal virtual void OnPageShown(PageEventArgs e) { PageShown?.Invoke(this, e); }
        protected internal virtual void OnPagePaint(PagePaintEventArgs e) { PagePaint?.Invoke(this, e); }

        [Category("Behavior")]
        public event ButtonClickEventHandler BackButtonClicked;
        [Category("Behavior")]
        public event ButtonClickEventHandler NextButtonClicked;
        [Category("Behavior")]
        public event ButtonClickEventHandler CloseButtonClicked;
        [Category("Behavior")]
        public event EventHandler HelpButtonClicked;
        [Category("Behavior")]
        public event PageEventHandler PageAdded;
        [Category("Behavior")]
        public event PageEventHandler PageRemoved;
        [Category("Behavior")]
        public event PageChangingEventHandler PageChanging;
        [Category("Behavior")]
        public event PageChangedEventHandler PageChanged;
        [Category("Behavior")]
        public event PageValidatingEventHandler PageValidating;
        [Category("Behavior")]
        public event PageEventHandler PageValidated;
        [Category("Behavior")]
        public event PageEventHandler PageShown;
        [Category("Appearance")]
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
        internal bool creatingUIControls;
        private readonly WizardPageCollection pages;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the index of the first tab in the controls collection.
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
                var oldPage = (selectedIndex == -1 || selectedIndex < 0 || selectedIndex > PageCount - 1 ? null : Pages[selectedIndex]);
                var newPage = value;

                if (newPage != null && !pages.Contains(newPage))
                    throw new ArgumentException("Page is not found in the page collection.");

                if (oldPage == value)
                    return;

                PageValidatingEventArgs pve = new PageValidatingEventArgs(oldPage);
                OnPageValidating(pve);
                if (pve.Cancel) return;

                OnPageValidated(new PageEventArgs(oldPage));

                PageChangingEventArgs pce = new PageChangingEventArgs(oldPage, newPage);
                OnCurrentPageChanging(pce);
                if (pce.Cancel) return;

                selectedIndex = (newPage == null ? -1 : Pages.IndexOf(newPage));
                if (oldPage != null) oldPage.Visible = false;
                if (newPage != null) newPage.Visible = true;

                UpdateNavigationControls();

                OnCurrentPageChanged(new PageChangedEventArgs(oldPage, newPage));

                OnPageShown(new PageEventArgs(newPage));
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
            set { SelectedPage = (value == -1 ? null : pages[value]); }
        }

        /// <summary>
        /// Gets or sets the collection pages in the wizard.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets the collection pages in the wizard.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public WizardPageCollection Pages => pages;

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
        public Rectangle PageArea => new Rectangle(ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Width, ClientRectangle.Height - UIAreaHeight);

        /// <summary>
        /// Gets the client rectangle where user interface controls are located.
        /// </summary>
        [Browsable(false)]
        public Rectangle UIArea => new Rectangle(ClientRectangle.Left, ClientRectangle.Height - UIAreaHeight, ClientRectangle.Width, UIAreaHeight);

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override string Text { get => base.Text; set => base.Text = value; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ControlCollection Controls => base.Controls;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="WizardControl"/> class.
        /// </summary>
        public WizardControl()
        {
            creatingUIControls = false;
            pages = new WizardPageCollection(this);
            selectedIndex = -1;
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
            var pageBounds = PageArea;
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
            var pageBounds = PageArea;

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
        protected override Control.ControlCollection CreateControlsInstance()
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
