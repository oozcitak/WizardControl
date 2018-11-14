using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    [ToolboxItem(typeof(WizardControlToolboxItem))]
    [Designer(typeof(WizardControlDesigner))]
    [DesignerSerializer(typeof(WizardControlSerializer), typeof(CodeDomSerializer))]
    [Docking(DockingBehavior.AutoDock)]
    public class WizardControl : Control
    {
        #region Events
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

        public delegate void PageEventHandler(object sender, PageEventArgs e);
        public delegate void PageChangingEventHandler(object sender, PageChangingEventArgs e);
        public delegate void PageChangedEventHandler(object sender, PageChangedEventArgs e);

        protected internal virtual void OnPageAdded(PageEventArgs e) { PageAdded?.Invoke(this, e); }
        protected internal virtual void OnPageRemoved(PageEventArgs e) { PageRemoved?.Invoke(this, e); }
        protected internal virtual void OnCurrentPageChanging(PageChangingEventArgs e) { CurrentPageChanging?.Invoke(this, e); }
        protected internal virtual void OnCurrentPageChanged(PageChangedEventArgs e) { CurrentPageChanged?.Invoke(this, e); }

        public event PageEventHandler PageAdded;
        public event PageEventHandler PageRemoved;
        public event PageChangingEventHandler CurrentPageChanging;
        public event PageChangedEventHandler CurrentPageChanged;
        #endregion

        #region Member Variables
        internal PageContainer pageContainer;
        internal HorizontalLine separator;
        internal Button backButton;
        internal Button nextButton;
        internal Button closeButton;

        private readonly WizardPageCollection pages;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the current page of the wizard.
        /// </summary>
        [Editor(typeof(WizardControlUITypeEditor), typeof(UITypeEditor))]
        [Category("Behavior")]
        [Description("Gets or sets the current page of the wizard.")]
        public WizardPage CurrentPage
        {
            get => pageContainer.CurrentPage;
            set
            {
                if (pageContainer.CurrentPage == value)
                    return;

                PageChangingEventArgs e = new PageChangingEventArgs(pageContainer.CurrentPage, value);
                OnCurrentPageChanging(e);
                if (!e.Cancel)
                {
                    var oldPage = pageContainer.CurrentPage;
                    pageContainer.CurrentPage = value;

                    UpdateNavigationControls();

                    OnCurrentPageChanged(new PageChangedEventArgs(oldPage, pageContainer.CurrentPage));
                }
            }
        }

        /// <summary>
        /// Gets or sets the zero-based index of the current page of the wizard.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets the zero-based index of the current page of the wizard.")]
        public int CurrentPageIndex
        {
            get => Pages.IndexOf(pageContainer.CurrentPage);
            set
            {
                int index = Pages.IndexOf(pageContainer.CurrentPage);
                if (index == value)
                    return;

                CurrentPage = Pages[value];
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
        [Category("Appearance"), Localizable(true)]
        [Description("Gets or sets the text of the back button.")]
        public string BackButtonText { get => backButton.Text; set => backButton.Text = value; }

        /// <summary>
        /// Gets or sets the text of the next button.
        /// </summary>
        [Category("Appearance"), Localizable(true)]
        [Description("Gets or sets the text of the next button.")]
        public string NextButtonText { get => nextButton.Text; set => nextButton.Text = value; }

        /// <summary>
        /// Gets or sets the text of the close button.
        /// </summary>
        [Category("Appearance"), Localizable(true)]
        [Description("Gets or sets the text of the close button.")]
        public string CloseButtonText { get => closeButton.Text; set => closeButton.Text = value; }

        /// <summary>
        /// Determines whether the wizard can navigate to the previous page.
        /// </summary>
        [Browsable(false)]
        public bool CanGoBack => !(ReferenceEquals(CurrentPage, pageContainer.Controls[0]));

        /// <summary>
        /// Determines whether the wizard can navigate to the next page.
        /// </summary>
        [Browsable(false)]
        public bool CanGoNext => !(ReferenceEquals(CurrentPage, pageContainer.Controls[pageContainer.Controls.Count - 1]));

        /// <summary>
        /// Gets the client rectangle where user interface controls are located.
        /// </summary>
        [Browsable(false)]
        public Rectangle UIArea => new Rectangle(ClientRectangle.Left, separator.Top, ClientRectangle.Width, ClientRectangle.Height - separator.Bottom);

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
            if (!CanGoBack)
                return;

            int index = pageContainer.Controls.IndexOf(CurrentPage);
            CurrentPage = (WizardPage)pageContainer.Controls[index - 1];

            UpdateNavigationControls();
        }

        /// <summary>
        /// Navigates to the next page.
        /// </summary>
        public void GoNext()
        {
            if (!CanGoNext)
                return;

            int index = pageContainer.Controls.IndexOf(CurrentPage);
            CurrentPage = (WizardPage)pageContainer.Controls[index + 1];

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
            Controls.Add(backButton);

            nextButton = new Button();
            nextButton.Text = "Next >";
            nextButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            Controls.Add(nextButton);

            closeButton = new Button();
            closeButton.Text = "Close";
            closeButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            Controls.Add(closeButton);

            ResizeControls();
        }

        private void ResizeControls()
        {
            var bounds = ClientRectangle;
            int buttonWidth = 75;
            int buttonHeight = 23;

            int separatorTop = bounds.Bottom - (2 + 12 + buttonHeight + 12);
            int buttonLeft = bounds.Right - (buttonWidth + buttonWidth + 12 + buttonWidth + 12);
            int buttonTop = bounds.Bottom - (buttonHeight + 12);

            pageContainer.SetBounds(bounds.Left, bounds.Top, bounds.Width, bounds.Height - (2 + 12 + buttonHeight + 12));
            separator.SetBounds(bounds.Left, separatorTop, bounds.Width, 2);
            backButton.SetBounds(buttonLeft, buttonTop, backButton.Width, backButton.Height);
            nextButton.SetBounds(buttonLeft + backButton.Width, buttonTop, nextButton.Width, nextButton.Height);
            closeButton.SetBounds(buttonLeft + backButton.Width + nextButton.Width + 12, buttonTop, closeButton.Width, closeButton.Height);
        }

        internal void UpdateNavigationControls()
        {
            backButton.Enabled = CanGoBack;
            nextButton.Enabled = CanGoNext;
        }
        #endregion

        #region Overriden Methods
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            ResizeControls();
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
                controls.Add(new WizardPage());
                for (int i = controls.Count - 2; i >= 0; i--)
                {
                    controls.RemoveAt(i);
                }
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
                foreach (Control control in removed)
                {
                    controls.Add(control);
                }

                owner.UpdateNavigationControls();
            }

            public bool Remove(WizardPage item)
            {
                bool exists = controls.Contains(item);

                if (controls.Count == 1)
                    controls.Add(new WizardPage());

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

                foreach (WizardPage page in control.pageContainer.Controls.Cast<WizardPage>())
                {
                    SelectorNode node = new SelectorNode(page.Name, page);
                    selector.Nodes.Add(node);

                    if (page == control.CurrentPage)
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
                Control oldPage = control.pageContainer.Controls[0];
                control.pageContainer.Controls.Add(page);
                control.pageContainer.Controls.Remove(oldPage);

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
