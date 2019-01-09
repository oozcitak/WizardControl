using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Manina.Windows.Forms;

namespace WizardControlTest
{
    internal class WizardControlTest : WizardControl
    {
        private Button AddButton;
        private Button RemoveButton;
        private Button ClearLogButton;
        private List<Tuple<string, Color>> messages = new List<Tuple<string, Color>>();

        public WizardControlTest()
        {
            AddButton = new Button();
            AddButton.Text = "+";
            AddButton.Click += AddButton_Click;
            AddButton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            AddButton.BackColor = Color.PaleGoldenrod;

            RemoveButton = new Button();
            RemoveButton.Text = "-";
            RemoveButton.Click += RemoveButton_Click;
            RemoveButton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            RemoveButton.BackColor = Color.PaleGoldenrod;

            ClearLogButton = new Button();
            ClearLogButton.Text = "Clear Log";
            ClearLogButton.Click += ClearLogButton_Click;
            ClearLogButton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            ClearLogButton.BackColor = Color.PaleGoldenrod;
        }

        protected override void OnCreateUIControls(CreateUIControlsEventArgs e)
        {
            e.Controls = new List<Control>(e.Controls) { AddButton, RemoveButton, ClearLogButton }.ToArray();
            base.OnCreateUIControls(e);
        }

        protected override void OnUpdateUIControls(EventArgs e)
        {
            base.OnUpdateUIControls(e);

            int x = HelpButton.Right;
            int y = HelpButton.Top;
            AddButton.SetBounds(x + 2, y, 23, 23, BoundsSpecified.All);
            RemoveButton.SetBounds(x + 2 + AddButton.Width + 2, y, 23, 23, BoundsSpecified.All);
            ClearLogButton.SetBounds(x + 2 + AddButton.Width + 2 + RemoveButton.Width + 2, y, 75, 23, BoundsSpecified.All);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            Page page = new Page();
            int i = 1;
            string name = "page1";
            while (Pages.Any(p => p.Name == name))
            {
                i++;
                name = "page" + i.ToString();
            }
            page.Name = name;
            Pages.Add(page);
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (SelectedPage != null)
                Pages.Remove(SelectedPage);
        }

        private void ClearLogButton_Click(object sender, EventArgs e)
        {
            messages.Clear();
            Invalidate();
        }

        protected override void OnPagePaint(PagePaintEventArgs e)
        {
            base.OnPagePaint(e);
            PaintInfo(e.Graphics, e.Page.ClientRectangle, Color.White);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            PaintInfo(e.Graphics, DisplayRectangle, SystemColors.Control);
        }

        protected override void OnPageChanging(PageChangingEventArgs e)
        {
            base.OnPageChanging(e);
            AddMessage(string.Format("Page changing: {0} -> {1}", PageName(e.CurrentPage), PageName(e.NewPage)), Color.Green);
        }

        protected override void OnPageChanged(PageChangedEventArgs e)
        {
            base.OnPageChanged(e);
            AddMessage(string.Format("Page changed: {0} -> {1}", PageName(e.OldPage), PageName(e.CurrentPage)), Color.Green);
        }

        protected override void OnNextButtonClicked(CancelEventArgs e)
        {
            base.OnNextButtonClicked(e);
            AddMessage("Next button clicked", Color.Blue);
        }

        protected override void OnBackButtonClicked(CancelEventArgs e)
        {
            AddMessage("Back button clicked", Color.Blue);
        }

        protected override void OnHelpButtonClicked(EventArgs e)
        {
            base.OnHelpButtonClicked(e);
            AddMessage("Help button clicked", Color.Blue);
        }

        protected override void OnCloseButtonClicked(CancelEventArgs e)
        {
            base.OnCloseButtonClicked(e);
            AddMessage("Close button clicked", Color.Blue);
            e.Cancel = true;
        }

        protected override void OnPageAdded(PageEventArgs e)
        {
            base.OnPageAdded(e);
            AddMessage(string.Format("Page added: {0}", PageName(e.Page)), Color.DarkRed);
        }

        protected override void OnPageRemoved(PageEventArgs e)
        {
            base.OnPageRemoved(e);
            AddMessage(string.Format("Page removed: {0}", PageName(e.Page)), Color.DarkRed);
        }

        protected override void OnPageValidating(PageValidatingEventArgs e)
        {
            base.OnPageValidating(e);
            AddMessage(string.Format("Page validating: {0}", PageName(e.Page)));
        }

        protected override void OnPageValidated(PageEventArgs e)
        {
            base.OnPageValidated(e);
            AddMessage(string.Format("Page validated: {0}", PageName(e.Page)));
        }

        protected override void OnPageHidden(PageEventArgs e)
        {
            base.OnPageHidden(e);
            AddMessage(string.Format("Page hidden: {0}", PageName(e.Page)));
        }

        protected override void OnPageShown(PageEventArgs e)
        {
            base.OnPageShown(e);
            AddMessage(string.Format("Page shown: {0}", PageName(e.Page)));
        }

        private void PaintInfo(Graphics g, Rectangle bounds, Color backColor)
        {
            bounds.Inflate(-4, -4);
            g.Clip = new Region(bounds);
            g.Clear(backColor);

            var y = bounds.Top + 6;
            var burn = 0f;
            var burnStep = 0.9f / (bounds.Height / (g.MeasureString("M", Font).Height + 4));
            for (int i = messages.Count - 1; i >= 0; i--)
            {
                var message = messages[i].Item1;
                var color = messages[i].Item2;
                var h = (int)g.MeasureString(message, Font).Height;
                using (var brush = new SolidBrush(Color.FromArgb((int)(color.R + (255 - color.R) * burn), (int)(color.G + (255 - color.G) * burn), (int)(color.B + (255 - color.B) * burn))))
                {
                    g.DrawString(message, Font, brush, 20, y);
                }
                y += h + 4;
                burn += burnStep;
                if (burn > 0.9f) burn = 0.9f;
            }

            string currentPageStr = string.Format("Selected page: {0}", (SelectedPage != null) ? SelectedPage.Name : "<none>");
            string currentIndexStr = string.Format("Selected index: {0}", SelectedIndex);
            string pageCountStr = string.Format("Page count: {0}", Pages.Count);
            var size1 = g.MeasureString(currentPageStr, Font);
            var size2 = g.MeasureString(currentIndexStr, Font);
            var size3 = g.MeasureString(pageCountStr, Font);
            float maxWidth = Math.Max(size1.Width, Math.Max(size2.Width, size3.Width));
            g.DrawString(currentPageStr, Font, Brushes.Red, bounds.Right - 6 - maxWidth, bounds.Top + 6);
            g.DrawString(currentIndexStr, Font, Brushes.Red, bounds.Right - 6 - maxWidth, bounds.Top + 22);
            g.DrawString(pageCountStr, Font, Brushes.Red, bounds.Right - 6 - maxWidth, bounds.Top + 38);
        }

        private void AddMessage(string message, Color color)
        {
            messages.Add(Tuple.Create(message, color));
            Invalidate();
        }

        private void AddMessage(string message)
        {
            AddMessage(message, Color.Black);
        }

        private string PageName(Page page) => (page?.Name ?? "<none>");
    }
}
