using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
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
            Page page = new Page();
            int i = 1;
            string name = "page1";
            while (wizardControl1.Pages.Any(p => p.Name == name))
            {
                i++;
                name = "page" + i.ToString();
            }
            page.Name = name;
            wizardControl1.Pages.Add(page);
        }

        private void WizardControl1_RemoveButtonClick(object sender, EventArgs e)
        {
            if (wizardControl1.SelectedPage != null)
                wizardControl1.Pages.Remove(wizardControl1.SelectedPage);
        }

        private void wizardControl1_PagePaint(object sender, PagedControl.PagePaintEventArgs e)
        {
            PaintInfo(e.Graphics, e.Page.ClientRectangle, Color.White);
        }

        private void wizardControl1_Paint(object sender, PaintEventArgs e)
        {
            PaintInfo(e.Graphics, wizardControl1.DisplayRectangle, SystemColors.Control);
        }

        private void PaintInfo(Graphics g, Rectangle bounds, Color backColor)
        {
            g.Clear(backColor);

            bounds.Inflate(-4, -4);
            g.Clip = new Region(bounds);

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

            string currentPageStr = string.Format("Selected page: {0}", (wizardControl1.SelectedPage != null) ? wizardControl1.SelectedPage.Name : "<none>");
            string currentIndexStr = string.Format("Selected index: {0}", wizardControl1.SelectedIndex);
            string pageCountStr = string.Format("Page count: {0}", wizardControl1.Pages.Count);
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
            wizardControl1.Invalidate(true);
        }

        private void AddMessage(string message)
        {
            AddMessage(message, Color.Black);
        }

        private string PageName(Page page) => (page?.Name ?? "<none>");

        private void wizardControl1_PageChanging(object sender, PagedControl.PageChangingEventArgs e)
        {
            AddMessage(string.Format("Page changing: {0} -> {1}", PageName(e.CurrentPage), PageName(e.NewPage)), Color.Green);
        }

        private void wizardControl1_PageChanged(object sender, PagedControl.PageChangedEventArgs e)
        {
            AddMessage(string.Format("Page changed: {0} -> {1}", PageName(e.OldPage), PageName(e.CurrentPage)), Color.Green);
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
            AddMessage(string.Format("Page added: {0}", PageName(e.Page)), Color.DarkRed);
        }

        private void wizardControl1_PageRemoved(object sender, PagedControl.PageEventArgs e)
        {
            AddMessage(string.Format("Page removed: {0}", PageName(e.Page)), Color.DarkRed);
        }

        private void wizardControl1_PageValidating(object sender, PagedControl.PageValidatingEventArgs e)
        {
            AddMessage(string.Format("Page validating: {0}", PageName(e.Page)));
        }

        private void wizardControl1_PageValidated(object sender, PagedControl.PageEventArgs e)
        {
            AddMessage(string.Format("Page validated: {0}", PageName(e.Page)));
        }

        private void wizardControl1_PageHidden(object sender, PagedControl.PageEventArgs e)
        {
            AddMessage(string.Format("Page hidden: {0}", PageName(e.Page)));
        }

        private void wizardControl1_PageShown(object sender, PagedControl.PageEventArgs e)
        {
            AddMessage(string.Format("Page shown: {0}", PageName(e.Page)));
        }
    }
}
