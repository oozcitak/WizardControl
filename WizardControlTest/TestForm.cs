using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Manina.Windows.Forms;

namespace WizardControlTest
{
    public partial class TestForm : Form
    {
        public class DemoWizardControl : WizardControl
        {
            private Button addButton;
            private Button removeButton;
            private Control[] uiControls = new Control[0];

            public event EventHandler AddButtonClick;
            public event EventHandler RemoveButtonClick;

            public override Control[] UIControls
            {
                get
                {
                    if (uiControls.Length == 0)
                    {
                        List<Control> controls = new List<Control>(base.UIControls);

                        addButton = new Button();
                        addButton.Text = "+";
                        addButton.Click += AddButton_Click;
                        controls.Add(addButton);

                        removeButton = new Button();
                        removeButton.Text = "-";
                        removeButton.Click += RemoveButton_Click;
                        controls.Add(removeButton);

                        uiControls = controls.ToArray();
                    }

                    return uiControls;
                }
            }

            private void AddButton_Click(object sender, EventArgs e)
            {
                AddButtonClick?.Invoke(this, EventArgs.Empty);
            }

            private void RemoveButton_Click(object sender, EventArgs e)
            {
                RemoveButtonClick?.Invoke(this, EventArgs.Empty);
            }

            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);

                int x = uiControls[0].Right;
                int y = uiControls[0].Top;
                addButton.SetBounds(x + 2, y, 23, 23);
                removeButton.SetBounds(x + 2 + 23 + 2, y, 23, 23);
            }
        }

        private List<Tuple<string, Color>> messages = new List<Tuple<string, Color>>();

        public TestForm()
        {
            InitializeComponent();

            wizardControl1.AddButtonClick += WizardControl1_AddButtonClick;
            wizardControl1.RemoveButtonClick += WizardControl1_RemoveButtonClick;
        }

        private void WizardControl1_AddButtonClick(object sender, EventArgs e)
        {
            wizardControl1.Pages.Add(new Page());
        }

        private void WizardControl1_RemoveButtonClick(object sender, EventArgs e)
        {
            if (wizardControl1.SelectedPage != null)
                wizardControl1.Pages.Remove(wizardControl1.SelectedPage);
        }

        private void wizardControl1_PagePaint(object sender, PagedControl.PagePaintEventArgs e)
        {
            var bounds = e.Page.DisplayRectangle;
            bounds.Inflate(-10, -10);
            e.Graphics.DrawRectangle(Pens.Red, bounds);
            bounds.Inflate(-4, -4);
            e.Graphics.Clip = new Region(bounds);

            var y = bounds.Top + 6;
            var burn = 0f;
            var burnStep = 0.9f / (bounds.Height / (e.Graphics.MeasureString("M", e.Page.Font).Height + 4));
            for (int i = messages.Count - 1; i >= 0; i--)
            {
                var message = messages[i].Item1;
                var color = messages[i].Item2;
                var h = (int)e.Graphics.MeasureString(message, e.Page.Font).Height;
                using (var brush = new SolidBrush(Color.FromArgb((int)(color.R + (255 - color.R) * burn), (int)(color.G + (255 - color.G) * burn), (int)(color.B + (255 - color.B) * burn))))
                {
                    e.Graphics.DrawString(message, e.Page.Font, brush, 20, y);
                }
                y += h + 4;
                burn += burnStep;
                if (burn > 0.9f) burn = 0.9f;
            }

            string currentPageStr = string.Format("Selected page: {0}", (wizardControl1.SelectedPage != null) ? wizardControl1.SelectedPage.Name : "<none>");
            var size = e.Graphics.MeasureString(currentPageStr, e.Page.Font);
            e.Graphics.DrawString(currentPageStr, e.Page.Font, Brushes.Red, bounds.Right - 6 - size.Width, bounds.Top + 6);
        }

        private void AddMessage(string message, Color color)
        {
            messages.Add(Tuple.Create(message, color));
            if (wizardControl1.SelectedPage != null)
                wizardControl1.SelectedPage?.Refresh();
        }

        private void AddMessage(string message)
        {
            AddMessage(message, Color.Black);
        }

        private void wizardControl1_PageChanging(object sender, PagedControl.PageChangingEventArgs e)
        {
            AddMessage(string.Format("Page changing: {2}: {0} -> {3}: {1}", e.CurrentPage.Name, e.NewPage.Name, e.CurrentPageIndex, e.NewPageIndex), Color.Green);
        }

        private void wizardControl1_PageChanged(object sender, PagedControl.PageChangedEventArgs e)
        {
            AddMessage(string.Format("Page changed: {2}: {0} -> {3}: {1}", e.OldPage.Name, e.CurrentPage.Name, e.OldPageIndex, e.CurrentPageIndex), Color.Green);
        }

        private void wizardControl1_NextButtonClicked(object sender, CancelEventArgs e)
        {
            AddMessage("Next button clicked", Color.Blue);
        }

        private void wizardControl1_BackButtonClicked(object sender, CancelEventArgs e)
        {
            AddMessage("Back button clicked", Color.Blue);
        }

        private void wizardControl1_HelpButtonClicked(object sender, EventArgs e)
        {
            AddMessage("Help button clicked", Color.Blue);
        }

        private void wizardControl1_CloseButtonClicked(object sender, CancelEventArgs e)
        {
            AddMessage("Close button clicked", Color.Blue);
            e.Cancel = true;
        }

        private void wizardControl1_PageAdded(object sender, PagedControl.PageEventArgs e)
        {
            AddMessage(string.Format("Page added: {0}", e.Page.Name), Color.DarkRed);
        }

        private void wizardControl1_PageRemoved(object sender, PagedControl.PageEventArgs e)
        {
            AddMessage(string.Format("Page removed: {0}", e.Page.Name), Color.DarkRed);
        }

        private void wizardControl1_PageValidating(object sender, PagedControl.PageValidatingEventArgs e)
        {
            AddMessage(string.Format("Page validating: {0}", e.Page.Name));
        }

        private void wizardControl1_PageValidated(object sender, PagedControl.PageEventArgs e)
        {
            AddMessage(string.Format("Page validated: {0}", e.Page.Name));
        }

        private void wizardControl1_PageHidden(object sender, PagedControl.PageEventArgs e)
        {
            AddMessage(string.Format("Page hidden: {0}", e.Page.Name));
        }

        private void wizardControl1_PageShown(object sender, PagedControl.PageEventArgs e)
        {
            AddMessage(string.Format("Page shown: {0}", e.Page.Name));
        }
    }
}
