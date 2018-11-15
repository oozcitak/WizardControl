using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    [ToolboxBitmap(typeof(WizardControl))]
    [ToolboxItem(typeof(WizardControlToolboxItem))]
    [Designer(typeof(WizardControlDesigner))]
    [DesignerSerializer(typeof(WizardControlSerializer), typeof(CodeDomSerializer))]
    [Docking(DockingBehavior.AutoDock)]
    [DefaultEvent("CurrentPageChanged")]
    [DefaultProperty("CurrentPage")]
    public class WizardControl : Control
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
            public WizardPage NewPage { get; private set; }
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

        public delegate void ButtonClickEventHandler(object sender, ButtonClickEventArgs e);
        public delegate void PageEventHandler(object sender, PageEventArgs e);
        public delegate void PageChangingEventHandler(object sender, PageChangingEventArgs e);
        public delegate void PageChangedEventHandler(object sender, PageChangedEventArgs e);

        protected internal virtual void OnBackButtonClicked(ButtonClickEventArgs e) { BackButtonClicked?.Invoke(this, e); }
        protected internal virtual void OnNextButtonClicked(ButtonClickEventArgs e) { NextButtonClicked?.Invoke(this, e); }
        protected internal virtual void OnCloseButtonClicked(ButtonClickEventArgs e) { CloseButtonClicked?.Invoke(this, e); }
        protected internal virtual void OnPageAdded(PageEventArgs e) { PageAdded?.Invoke(this, e); }
        protected internal virtual void OnPageRemoved(PageEventArgs e) { PageRemoved?.Invoke(this, e); }
        protected internal virtual void OnCurrentPageChanging(PageChangingEventArgs e) { PageChanging?.Invoke(this, e); }
        protected internal virtual void OnCurrentPageChanged(PageChangedEventArgs e) { PageChanged?.Invoke(this, e); }

        [Category("Behavior")]
        public event ButtonClickEventHandler BackButtonClicked;
        [Category("Behavior")]
        public event ButtonClickEventHandler NextButtonClicked;
        [Category("Behavior")]
        public event ButtonClickEventHandler CloseButtonClicked;
        [Category("Behavior")]
        public event PageEventHandler PageAdded;
        [Category("Behavior")]
        public event PageEventHandler PageRemoved;
        [Category("Behavior")]
        public event PageChangingEventHandler PageChanging;
        [Category("Behavior")]
        public event PageChangedEventHandler PageChanged;
        #endregion

        #region Member Variables
        private PageContainer pageContainer;
        private HorizontalLine separator;
        private Button backButton;
        private Button nextButton;
        private Button closeButton;

        private bool backButtonEnabled = true;
        private bool nextButtonEnabled = true;
        private bool closeButtonEnabled = true;

        private readonly WizardPageCollection pages;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the current page of the wizard.
        /// </summary>
        [Editor(typeof(WizardControlUITypeEditor), typeof(UITypeEditor))]
        [Category("Behavior")]
        [Description("Gets or sets the current page of the wizard.")]
        public WizardPage SelectedPage
        {
            get => pageContainer.SelectedPage;
            set
            {
                if (pageContainer.SelectedPage == value)
                    return;

                PageChangingEventArgs e = new PageChangingEventArgs(pageContainer.SelectedPage, value);
                OnCurrentPageChanging(e);
                if (!e.Cancel)
                {
                    var oldPage = pageContainer.SelectedPage;
                    pageContainer.SelectedPage = value;

                    UpdateNavigationControls();

                    OnCurrentPageChanged(new PageChangedEventArgs(oldPage, pageContainer.SelectedPage));
                }
            }
        }

        /// <summary>
        /// Gets or sets the zero-based index of the current page of the wizard.
        /// </summary>
        [Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Gets or sets the zero-based index of the current page of the wizard.")]
        public int SelectedIndex
        {
            get => Pages.Count == 0 ? -1 : Pages.IndexOf(pageContainer.SelectedPage);
            set
            {
                if (Pages.Count == 0) return;

                if (Pages.IndexOf(pageContainer.SelectedPage) == value)
                    return;

                SelectedPage = Pages[value];
            }
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
        [Category("Appearance"), Localizable(true), DefaultValue("< Back")]
        [Description("Gets or sets the text of the back button.")]
        public string BackButtonText { get => backButton.Text; set => backButton.Text = value; }

        /// <summary>
        /// Gets or sets the text of the next button.
        /// </summary>
        [Category("Appearance"), Localizable(true), DefaultValue("Next >")]
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
        /// Gets the client rectangle where wizard are located.
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
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="WizardControl"/> class.
        /// </summary>
        public WizardControl()
        {
            CreateChildControls();

            pages = new WizardPageCollection(this);

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

            UpdateNavigationControls();
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

            UpdateNavigationControls();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Create the UI controls of the wizard.
        /// </summary>
        private void CreateChildControls()
        {
            Controls.Clear();

            pageContainer = new PageContainer();
            pageContainer.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            Controls.Add(pageContainer);

            separator = new HorizontalLine();
            separator.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            Controls.Add(separator);

            backButton = new Button();
            backButton.Text = "< Back";
            backButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            backButton.Click += BackButton_Click;
            Controls.Add(backButton);

            nextButton = new Button();
            nextButton.Text = "Next >";
            nextButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom; nextButton.Click += NextButton_Click;
            Controls.Add(nextButton);

            closeButton = new Button();
            closeButton.Text = "Close";
            closeButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            closeButton.Click += CloseButton_Click;
            Controls.Add(closeButton);

            ResizeControls();
        }

        private void ResizeControls()
        {
            var pageBounds = PageArea;
            var uiBounds = UIArea;

            int buttonWidth = 75;
            int buttonHeight = 23;

            int buttonLeft = uiBounds.Right - (buttonWidth + buttonWidth + 12 + buttonWidth + 12);
            int buttonTop = uiBounds.Bottom - (buttonHeight + 12);

            pageContainer.SetBounds(pageBounds.Left, pageBounds.Top, pageBounds.Width, pageBounds.Height);
            separator.SetBounds(uiBounds.Left, uiBounds.Top, uiBounds.Width, 2);
            backButton.SetBounds(buttonLeft, buttonTop, 0, 0, BoundsSpecified.Location);
            nextButton.SetBounds(buttonLeft + buttonWidth, buttonTop, 0, 0, BoundsSpecified.Location);
            closeButton.SetBounds(buttonLeft + backButton.Width + nextButton.Width + 12, buttonTop, 0, 0, BoundsSpecified.Location);
        }

        internal void UpdateNavigationControls()
        {
            backButton.Enabled = backButtonEnabled && CanGoBack;
            nextButton.Enabled = nextButtonEnabled && CanGoNext;
            closeButton.Enabled = closeButtonEnabled;
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
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            ResizeControls();
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

        #region PageCollection
        public class WizardPageCollection : IList<WizardPage>
        {
            private WizardControl owner;
            private ControlCollection controls;

            public WizardPage this[int index]
            {
                get => (WizardPage)controls[index];
                set
                {
                    Insert(index, value);
                    RemoveAt(index + 1);
                }
            }
            public int Count => controls.Count;
            public bool IsReadOnly => controls.IsReadOnly;

            public WizardPageCollection(WizardControl control)
            {
                owner = control;
                controls = control.pageContainer.Controls;
            }

            public void Add(WizardPage item)
            {
                controls.Add(item);
                owner.UpdateNavigationControls();
            }

            public void Clear()
            {
                controls.Clear();
                owner.UpdateNavigationControls();
            }

            public bool Contains(WizardPage item)
            {
                return controls.Contains(item);
            }

            public void CopyTo(WizardPage[] array, int arrayIndex)
            {
                controls.CopyTo(array, arrayIndex);
            }

            public IEnumerator<WizardPage> GetEnumerator()
            {
                var iterator = controls.GetEnumerator();
                while (iterator.MoveNext())
                {
                    yield return (WizardPage)iterator.Current;
                }
            }

            public int IndexOf(WizardPage item)
            {
                return controls.IndexOf(item);
            }

            public void Insert(int index, WizardPage item)
            {
                controls.Add(item);

                List<Control> removed = new List<Control>();
                for (int i = controls.Count - 2; i >= index; i--)
                {
                    removed.Add(controls[i]);
                    controls.RemoveAt(i);
                }
                for (int i = removed.Count - 1; i >= 0; i--)
                {
                    controls.Add(removed[i]);
                }

                owner.UpdateNavigationControls();
            }

            public bool Remove(WizardPage item)
            {
                bool exists = controls.Contains(item);

                controls.Remove(item);

                owner.UpdateNavigationControls();

                return exists;
            }

            public void RemoveAt(int index)
            {
                controls.RemoveAt(index);
                owner.UpdateNavigationControls();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
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

        #region ToolboxItem
        internal class WizardControlToolboxItem : ToolboxItem
        {
            public WizardControlToolboxItem() : base(typeof(WizardControl))
            {
            }

            public WizardControlToolboxItem(SerializationInfo info, StreamingContext context)
            {
                Deserialize(info, context);
            }

            protected override IComponent[] CreateComponentsCore(IDesignerHost host)
            {
                WizardControl control = (WizardControl)host.CreateComponent(typeof(WizardControl));
                WizardPage page = (WizardPage)host.CreateComponent(typeof(WizardPage));
                control.Pages.Add(page);

                return new IComponent[] { control };
            }
        }
        #endregion

        #region CodeDomSerializer
        internal class WizardControlSerializer : CodeDomSerializer
        {
            public override object Serialize(IDesignerSerializationManager manager, object value)
            {
                CodeDomSerializer baseSerializer = (CodeDomSerializer)manager.GetSerializer(
                            typeof(WizardControl).BaseType,
                            typeof(CodeDomSerializer));
                // Let the base class do its work first.
                object codeObject = baseSerializer.Serialize(manager, value);

                // Let us now add our own initialization code.
                if (codeObject is CodeStatementCollection)
                {
                    CodeStatementCollection statements = (CodeStatementCollection)codeObject;
                    // This is the code reference to our ImageListView instance.
                    CodeExpression codeControl = base.SerializeToExpression(manager, value);
                    if (codeControl != null && value is WizardControl wizardControl)
                    {
                        // Walk through pages
                        foreach (var page in wizardControl.Pages)
                        {
                            // Create a line of code that will invoke Pages.Add(page);
                            var codeGetPages = new CodePropertyReferenceExpression(codeControl, "Pages");
                            var codeAddToPages = new CodeMethodInvokeExpression(codeGetPages, "Add", new CodeVariableReferenceExpression(page.Name));
                            // Add to the list of code statements.
                            statements.Add(codeAddToPages);
                        }
                    }
                    return codeObject;
                }

                return base.Serialize(manager, value);
            }

            public override object Deserialize(IDesignerSerializationManager manager, object codeObject)
            {
                // Let the base class handle deserialization.
                CodeDomSerializer baseSerializer = (CodeDomSerializer)manager.GetSerializer(
                    typeof(WizardControl).BaseType,
                    typeof(CodeDomSerializer));

                return baseSerializer.Deserialize(manager, codeObject);
            }
        }
        #endregion
    }
}
