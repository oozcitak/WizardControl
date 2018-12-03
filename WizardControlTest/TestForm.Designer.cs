namespace WizardControlTest
{
    partial class TestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.wizardControl1 = new WizardControlTest.TestForm.DemoWizardControl();
            this.page1 = new Manina.Windows.Forms.Page();
            this.page2 = new Manina.Windows.Forms.Page();
            this.page3 = new Manina.Windows.Forms.Page();
            this.page4 = new Manina.Windows.Forms.Page();
            this.page5 = new Manina.Windows.Forms.Page();
            this.page6 = new Manina.Windows.Forms.Page();
            this.page7 = new Manina.Windows.Forms.Page();
            this.wizardControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // wizardControl1
            // 
            this.wizardControl1.Controls.Add(this.page1);
            this.wizardControl1.Controls.Add(this.page2);
            this.wizardControl1.Controls.Add(this.page3);
            this.wizardControl1.Controls.Add(this.page4);
            this.wizardControl1.Controls.Add(this.page5);
            this.wizardControl1.Controls.Add(this.page6);
            this.wizardControl1.Controls.Add(this.page7);
            this.wizardControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardControl1.HelpButtonVisible = true;
            this.wizardControl1.Location = new System.Drawing.Point(0, 0);
            this.wizardControl1.Name = "wizardControl1";
            this.wizardControl1.Size = new System.Drawing.Size(773, 423);
            this.wizardControl1.TabIndex = 0;
            this.wizardControl1.BackButtonClicked += new Manina.Windows.Forms.WizardControl.ButtonClickEventHandler(this.wizardControl1_BackButtonClicked);
            this.wizardControl1.NextButtonClicked += new Manina.Windows.Forms.WizardControl.ButtonClickEventHandler(this.wizardControl1_NextButtonClicked);
            this.wizardControl1.CloseButtonClicked += new Manina.Windows.Forms.WizardControl.ButtonClickEventHandler(this.wizardControl1_CloseButtonClicked);
            this.wizardControl1.HelpButtonClicked += new System.EventHandler(this.wizardControl1_HelpButtonClicked);
            this.wizardControl1.PageAdded += new Manina.Windows.Forms.PagedControl.PageEventHandler(this.wizardControl1_PageAdded);
            this.wizardControl1.PageRemoved += new Manina.Windows.Forms.PagedControl.PageEventHandler(this.wizardControl1_PageRemoved);
            this.wizardControl1.PageChanging += new Manina.Windows.Forms.PagedControl.PageChangingEventHandler(this.wizardControl1_PageChanging);
            this.wizardControl1.PageChanged += new Manina.Windows.Forms.PagedControl.PageChangedEventHandler(this.wizardControl1_PageChanged);
            this.wizardControl1.PageValidating += new Manina.Windows.Forms.PagedControl.PageValidatingEventHandler(this.wizardControl1_PageValidating);
            this.wizardControl1.PageValidated += new Manina.Windows.Forms.PagedControl.PageEventHandler(this.wizardControl1_PageValidated);
            this.wizardControl1.PageHidden += new Manina.Windows.Forms.PagedControl.PageEventHandler(this.wizardControl1_PageHidden);
            this.wizardControl1.PageShown += new Manina.Windows.Forms.PagedControl.PageEventHandler(this.wizardControl1_PageShown);
            this.wizardControl1.PagePaint += new Manina.Windows.Forms.PagedControl.PagePaintEventHandler(this.wizardControl1_PagePaint);
            // 
            // page1
            // 
            this.page1.Location = new System.Drawing.Point(1, 1);
            this.page1.Name = "page1";
            this.page1.Size = new System.Drawing.Size(771, 371);
            // 
            // page2
            // 
            this.page2.Location = new System.Drawing.Point(1, 1);
            this.page2.Name = "page2";
            this.page2.Size = new System.Drawing.Size(771, 371);
            // 
            // page3
            // 
            this.page3.Location = new System.Drawing.Point(1, 1);
            this.page3.Name = "page3";
            this.page3.Size = new System.Drawing.Size(771, 371);
            // 
            // page4
            // 
            this.page4.Location = new System.Drawing.Point(1, 1);
            this.page4.Name = "page4";
            this.page4.Size = new System.Drawing.Size(771, 371);
            // 
            // page5
            // 
            this.page5.Location = new System.Drawing.Point(1, 1);
            this.page5.Name = "page5";
            this.page5.Size = new System.Drawing.Size(771, 371);
            // 
            // page6
            // 
            this.page6.Location = new System.Drawing.Point(1, 1);
            this.page6.Name = "page6";
            this.page6.Size = new System.Drawing.Size(771, 371);
            // 
            // page7
            // 
            this.page7.Location = new System.Drawing.Point(1, 1);
            this.page7.Name = "page7";
            this.page7.Size = new System.Drawing.Size(771, 371);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 423);
            this.Controls.Add(this.wizardControl1);
            this.Name = "TestForm";
            this.Text = "Test Form";
            this.wizardControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DemoWizardControl wizardControl1;
        private Manina.Windows.Forms.Page page1;
        private Manina.Windows.Forms.Page page2;
        private Manina.Windows.Forms.Page page3;
        private Manina.Windows.Forms.Page page4;
        private Manina.Windows.Forms.Page page5;
        private Manina.Windows.Forms.Page page6;
        private Manina.Windows.Forms.Page page7;
    }
}

