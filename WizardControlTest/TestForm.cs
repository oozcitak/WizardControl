using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WizardControlTest
{
    public partial class TestForm : Form
    {
        private List<Tuple<string, Color>> messages = new List<Tuple<string, Color>>();

        public TestForm()
        {
            InitializeComponent();
        }

        private void wizardControl1_PagePaint(object sender, Manina.Windows.Forms.WizardControl.PagePaintEventArgs e)
        {
            var bounds = e.Page.DisplayRectangle;
            bounds.Inflate(-10, -10);
            e.Graphics.DrawRectangle(Pens.Red, bounds);
            bounds.Inflate(-4, -4);
            e.Graphics.Clip = new Region(bounds);

            var y = bounds.Top + 6;
            var fade = 1f;
            for (int i = messages.Count - 1; i >= 0; i--)
            {
                var message = messages[i].Item1;
                var color = messages[i].Item2;
                var h = (int)e.Graphics.MeasureString(message, e.Page.Font).Height;
                using (var brush = new SolidBrush(Color.FromArgb((int)(color.R * fade), (int)(color.G * fade), (int)(color.B * fade))))
                {
                    e.Graphics.DrawString(message, e.Page.Font, brush, 20, y);
                }
                y += h + 4;
                fade -= 0.05f;
                if (fade < 0.2f) fade = 0.2f;
            }

            string currentPageStr = string.Format("Selected page: {0}", (wizardControl1.SelectedPage != null) ? wizardControl1.SelectedPage.Name : "<none>");
            var size = e.Graphics.MeasureString(currentPageStr, e.Page.Font);
            e.Graphics.DrawString(currentPageStr, e.Page.Font, Brushes.Red, bounds.Right - 6 - size.Width, bounds.Top + 6);
        }

        private void AddMessage(string message, Color color)
        {
            messages.Add(Tuple.Create(message, color));
            wizardControl1.SelectedPage?.Refresh();
        }

        private void AddMessage(string message)
        {
            AddMessage(message, Color.Black);
        }

        private void wizardControl1_PageChanging(object sender, Manina.Windows.Forms.WizardControl.PageChangingEventArgs e)
        {
            AddMessage(string.Format("Page changing: {0} -> {1}", e.CurrentPage.Name, e.NewPage.Name), Color.Green);
        }

        private void wizardControl1_PageChanged(object sender, Manina.Windows.Forms.WizardControl.PageChangedEventArgs e)
        {
            AddMessage(string.Format("Page changed: {0} -> {1}", e.OldPage.Name, e.CurrentPage.Name), Color.Green);
        }

        private void wizardControl1_NextButtonClicked(object sender, Manina.Windows.Forms.WizardControl.ButtonClickEventArgs e)
        {
            AddMessage("Next button clicked", Color.Blue);
        }

        private void wizardControl1_BackButtonClicked(object sender, Manina.Windows.Forms.WizardControl.ButtonClickEventArgs e)
        {
            AddMessage("Back button clicked", Color.Blue);
        }

        private void wizardControl1_HelpButtonClicked(object sender, System.EventArgs e)
        {
            AddMessage("Help button clicked", Color.Blue);
        }

        private void wizardControl1_CloseButtonClicked(object sender, Manina.Windows.Forms.WizardControl.ButtonClickEventArgs e)
        {
            AddMessage("Close button clicked", Color.Blue);
            e.Cancel = true;
        }

        private void wizardControl1_PageAdded(object sender, Manina.Windows.Forms.WizardControl.PageEventArgs e)
        {
            AddMessage(string.Format("Page added: {0}", e.Page.Name), Color.DarkRed);
        }

        private void wizardControl1_PageRemoved(object sender, Manina.Windows.Forms.WizardControl.PageEventArgs e)
        {
            AddMessage(string.Format("Page removed: {0}", e.Page.Name), Color.DarkRed);
        }

        private void wizardControl1_PageValidating(object sender, Manina.Windows.Forms.WizardControl.PageValidatingEventArgs e)
        {
            AddMessage(string.Format("Page validating: {0}", e.Page.Name));
        }

        private void wizardControl1_PageValidated(object sender, Manina.Windows.Forms.WizardControl.PageEventArgs e)
        {
            AddMessage(string.Format("Page validated: {0}", e.Page.Name));
        }

        private void wizardControl1_PageShown(object sender, Manina.Windows.Forms.WizardControl.PageEventArgs e)
        {
            AddMessage(string.Format("Page shown: {0}", e.Page.Name));
        }
    }
}
