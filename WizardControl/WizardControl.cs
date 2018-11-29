using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    [ToolboxBitmap(typeof(WizardControl))]
    [Docking(DockingBehavior.AutoDock)]
    public class WizardControl : PagedControl
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

        public delegate void ButtonClickEventHandler(object sender, ButtonClickEventArgs e);

        protected internal virtual void OnBackButtonClicked(ButtonClickEventArgs e) { BackButtonClicked?.Invoke(this, e); }
        protected internal virtual void OnNextButtonClicked(ButtonClickEventArgs e) { NextButtonClicked?.Invoke(this, e); }
        protected internal virtual void OnCloseButtonClicked(ButtonClickEventArgs e) { CloseButtonClicked?.Invoke(this, e); }
        protected internal virtual void OnHelpButtonClicked(EventArgs e) { HelpButtonClicked?.Invoke(this, e); }

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

        private Control[] uiControls = new Control[0];
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the background color of the control.
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Color), "Control")]
        [Description("Gets or sets the background color of the control.")]
        public override Color BackColor { get => base.BackColor; set => base.BackColor = value; }

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

        /// <summary>
        /// Gets the array of user interface controls.
        /// </summary>
        public override Control[] UIControls
        {
            get
            {
                if (uiControls == null || uiControls.Length == 0)
                {
                    helpButton = new Button();
                    helpButton.Text = "Help";
                    helpButton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
                    helpButton.Click += HelpButton_Click;
                    helpButton.Visible = false;

                    backButton = new Button();
                    backButton.Text = "Back";
                    backButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                    backButton.Click += BackButton_Click;

                    nextButton = new Button();
                    nextButton.Text = "Next";
                    nextButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                    nextButton.Click += NextButton_Click;

                    closeButton = new Button();
                    closeButton.Text = "Close";
                    closeButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                    closeButton.Click += CloseButton_Click;

                    uiControls = new Control[] { helpButton, backButton, nextButton, closeButton };
                }
                    
                return uiControls;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="WizardControl"/> class.
        /// </summary>
        public WizardControl()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            BackColor = SystemColors.Control;
        }
        #endregion

        #region Button Event Handlers
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
        protected override void OnUpdateUIControls(EventArgs e)
        {
            base.OnUpdateUIControls(e);

            backButton.Enabled = backButtonEnabled && CanGoBack;
            nextButton.Enabled = nextButtonEnabled && CanGoNext;
            closeButton.Enabled = closeButtonEnabled;
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

            helpButton.SetBounds(helpButtonLeft, buttonTop, 0, 0, BoundsSpecified.Location);
            backButton.SetBounds(buttonLeft, buttonTop, 0, 0, BoundsSpecified.Location);
            nextButton.SetBounds(buttonLeft + buttonWidth, buttonTop, 0, 0, BoundsSpecified.Location);
            closeButton.SetBounds(buttonLeft + backButton.Width + nextButton.Width + 12, buttonTop, 0, 0, BoundsSpecified.Location);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Brush backBrush = new SolidBrush(BackColor))
            using (Pen separatorPen = new Pen(Color.FromArgb(223, 223, 223)))
            {
                var uiBounds = UIArea;
                e.Graphics.FillRectangle(backBrush, UIArea);
                e.Graphics.DrawLine(separatorPen, uiBounds.Left + 2, uiBounds.Top, uiBounds.Right - 2, uiBounds.Top);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                helpButton.Click -= HelpButtonClicked;
                backButton.Click -= BackButton_Click;
                nextButton.Click -= NextButton_Click;
                closeButton.Click -= CloseButton_Click;
            }

            base.Dispose(disposing);
        }
        #endregion
    }
}
