using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    [ToolboxBitmap(typeof(WizardControl))]
    [Docking(DockingBehavior.AutoDock)]
    public class WizardControl : PagedControl
    {
        #region Virtual Functions for Events
        protected internal virtual void OnBackButtonClicked(CancelEventArgs e) { BackButtonClicked?.Invoke(this, e); }
        protected internal virtual void OnNextButtonClicked(CancelEventArgs e) { NextButtonClicked?.Invoke(this, e); }
        protected internal virtual void OnCloseButtonClicked(CancelEventArgs e) { CloseButtonClicked?.Invoke(this, e); }
        protected internal virtual void OnHelpButtonClicked(EventArgs e) { HelpButtonClicked?.Invoke(this, e); }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when the back button is clicked.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the back button is clicked.")]
        public event EventHandler<CancelEventArgs> BackButtonClicked;
        /// <summary>
        /// Occurs when the next button is clicked.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the next button is clicked.")]
        public event EventHandler<CancelEventArgs> NextButtonClicked;
        /// <summary>
        /// Occurs when the close button is clicked.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the close button is clicked.")]
        public event EventHandler<CancelEventArgs> CloseButtonClicked;
        /// <summary>
        /// Occurs when the help button is clicked.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the help button is clicked.")]
        public event EventHandler HelpButtonClicked;
        #endregion

        #region Member Variables
        private bool backButtonEnabled = true;
        private bool nextButtonEnabled = true;
        private bool closeButtonEnabled = true;
        private bool helpButtonEnabled = true;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the "Back" button.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected Button BackButton { get; private set; }
        /// <summary>
        /// Gets the "Next" button.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected Button NextButton { get; private set; }
        /// <summary>
        /// Gets the "Close" button.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected Button CloseButton { get; private set; }
        /// <summary>
        /// Gets the "Help" button.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected Button HelpButton { get; private set; }

        /// <summary>
        /// Gets or sets the text of the back button.
        /// </summary>
        [Category("Appearance"), Localizable(true), DefaultValue("Back")]
        [Description("Gets or sets the text of the back button.")]
        public string BackButtonText { get => BackButton.Text; set => BackButton.Text = value; }

        /// <summary>
        /// Gets or sets the text of the next button.
        /// </summary>
        [Category("Appearance"), Localizable(true), DefaultValue("Next")]
        [Description("Gets or sets the text of the next button.")]
        public string NextButtonText { get => NextButton.Text; set => NextButton.Text = value; }

        /// <summary>
        /// Gets or sets the text of the close button.
        /// </summary>
        [Category("Appearance"), Localizable(true), DefaultValue("Close")]
        [Description("Gets or sets the text of the close button.")]
        public string CloseButtonText { get => CloseButton.Text; set => CloseButton.Text = value; }

        /// <summary>
        /// Gets or sets whether the back button is enabled by user code.
        /// </summary>
        [Category("Behavior"), Localizable(true), DefaultValue(true)]
        [Description("Gets or sets whether the back button is enabled by user code.")]
        public bool BackButtonEnabled { get => backButtonEnabled; set { backButtonEnabled = value; OnUpdateUIControls(EventArgs.Empty); } }

        /// <summary>
        /// Gets or sets whether the next button is enabled by user code.
        /// </summary>
        [Category("Behavior"), Localizable(true), DefaultValue(true)]
        [Description("Gets or sets whether the next button is enabled by user code.")]
        public bool NextButtonEnabled { get => nextButtonEnabled; set { nextButtonEnabled = value; OnUpdateUIControls(EventArgs.Empty); } }

        /// <summary>
        /// Gets or sets whether the close button is enabled by user code.
        /// </summary>
        [Category("Behavior"), Localizable(true), DefaultValue(true)]
        [Description("Gets or sets whether the close button is enabled by user code.")]
        public bool CloseButtonEnabled { get => closeButtonEnabled; set { closeButtonEnabled = value; OnUpdateUIControls(EventArgs.Empty); } }

        /// <summary>
        /// Gets or sets whether the help button is enabled by user code.
        /// </summary>
        [Category("Behavior"), Localizable(true), DefaultValue(true)]
        [Description("Gets or sets whether the help button is enabled by user code.")]
        public bool HelpButtonEnabled { get => helpButtonEnabled; set { helpButtonEnabled = value; OnUpdateUIControls(EventArgs.Empty); } }

        /// <summary>
        /// Gets or sets whether the back button is visible.
        /// </summary>
        [Category("Appearance"), DefaultValue(true)]
        [Description("Gets or sets whether the back button is visible.")]
        public bool BackButtonVisible { get => BackButton.Visible; set { BackButton.Visible = value; } }

        /// <summary>
        /// Gets or sets whether the next button is visible.
        /// </summary>
        [Category("Appearance"), DefaultValue(true)]
        [Description("Gets or sets whether the next button is visible.")]
        public bool NextButtonVisible { get => NextButton.Visible; set { NextButton.Visible = value; } }

        /// <summary>
        /// Gets or sets whether the close button is visible.
        /// </summary>
        [Category("Appearance"), DefaultValue(true)]
        [Description("Gets or sets whether the close button is visible.")]
        public bool CloseButtonVisible { get => CloseButton.Visible; set { CloseButton.Visible = value; } }

        /// <summary>
        /// Gets or sets whether the help button is visible.
        /// </summary>
        [Category("Appearance"), DefaultValue(false)]
        [Description("Gets or sets whether the help button is visible.")]
        public bool HelpButtonVisible { get => HelpButton.Visible; set { HelpButton.Visible = value; } }

        /// <summary>
        /// Gets the height of the area where user interface controls are located.
        /// </summary>
        [Browsable(false)]
        internal int UIAreaHeight => 2 + 12 + 23 + 12;

        /// <summary>
        /// Gets the client rectangle where wizard pages are located.
        /// </summary>
        [Browsable(false)]
        public override Rectangle DisplayRectangle => new Rectangle(ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Width, ClientRectangle.Height - UIAreaHeight - 1);

        /// <summary>
        /// Gets the client rectangle where user interface controls are located.
        /// </summary>
        [Browsable(false)]
        public Rectangle UIArea => new Rectangle(ClientRectangle.Left, ClientRectangle.Height - UIAreaHeight, ClientRectangle.Width, UIAreaHeight);
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="WizardControl"/> class.
        /// </summary>
        public WizardControl()
        {
            HelpButton = new Button();
            HelpButton.Text = "Help";
            HelpButton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            HelpButton.Click += HelpButton_Click;
            HelpButton.Visible = false;

            BackButton = new Button();
            BackButton.Text = "Back";
            BackButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            BackButton.Click += BackButton_Click;

            NextButton = new Button();
            NextButton.Text = "Next";
            NextButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            NextButton.Click += NextButton_Click;

            CloseButton = new Button();
            CloseButton.Text = "Close";
            CloseButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            CloseButton.Click += CloseButton_Click;
        }
        #endregion

        #region Button Event Handlers
        private void HelpButton_Click(object sender, EventArgs e)
        {
            OnHelpButtonClicked(new EventArgs());
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            CancelEventArgs be = new CancelEventArgs();
            OnBackButtonClicked(be);
            if (!be.Cancel) GoBack();
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            CancelEventArgs be = new CancelEventArgs();
            OnNextButtonClicked(be);
            if (!be.Cancel) GoNext();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            CancelEventArgs be = new CancelEventArgs();
            OnCloseButtonClicked(be);
            if (!be.Cancel) FindForm().Close();
        }
        #endregion

        #region Overriden Methods
        protected override void OnCreateUIControls(CreateUIControlsEventArgs e)
        {
            e.Controls = new List<Control>(e.Controls) { HelpButton, BackButton, NextButton, CloseButton }.ToArray();

            base.OnCreateUIControls(e);
        }

        protected override void OnUpdateUIControls(EventArgs e)
        {
            base.OnUpdateUIControls(e);

            BackButton.Enabled = backButtonEnabled && CanGoBack;
            NextButton.Enabled = nextButtonEnabled && CanGoNext;
            CloseButton.Enabled = closeButtonEnabled;
            HelpButton.Enabled = helpButtonEnabled;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            var uiBounds = UIArea;

            int buttonWidth = 75;
            int buttonHeight = 23;

            int buttonLeft = uiBounds.Right - (buttonWidth + buttonWidth + 12 + buttonWidth + 12);
            int buttonTop = uiBounds.Bottom - (buttonHeight + 12);
            int helpButtonLeft = uiBounds.Left + 12;

            HelpButton.SetBounds(helpButtonLeft, buttonTop, 0, 0, BoundsSpecified.Location);
            BackButton.SetBounds(buttonLeft, buttonTop, 0, 0, BoundsSpecified.Location);
            NextButton.SetBounds(buttonLeft + buttonWidth, buttonTop, 0, 0, BoundsSpecified.Location);
            CloseButton.SetBounds(buttonLeft + BackButton.Width + NextButton.Width + 12, buttonTop, 0, 0, BoundsSpecified.Location);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var oldClip = e.Graphics.Clip;
            e.Graphics.ResetClip();

            using (Pen separatorPen = new Pen(Color.FromArgb(223, 223, 223)))
            {
                var uiBounds = UIArea;
                e.Graphics.DrawLine(separatorPen, uiBounds.Left + 2, uiBounds.Top, uiBounds.Right - 2, uiBounds.Top);
            }

            e.Graphics.Clip = oldClip;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                HelpButton.Click -= HelpButtonClicked;
                BackButton.Click -= BackButton_Click;
                NextButton.Click -= NextButton_Click;
                CloseButton.Click -= CloseButton_Click;
            }

            base.Dispose(disposing);
        }
        #endregion
    }
}
