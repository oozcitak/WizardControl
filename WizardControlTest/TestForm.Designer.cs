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
            this.wizardControl1 = new Manina.Windows.Forms.WizardControl();
            this.wizardPage1 = new Manina.Windows.Forms.Page();
            this.wizardPage2 = new Manina.Windows.Forms.Page();
            this.wizardPage3 = new Manina.Windows.Forms.Page();
            this.wizardPage4 = new Manina.Windows.Forms.Page();
            this.wizardPage5 = new Manina.Windows.Forms.Page();
            this.wizardPage6 = new Manina.Windows.Forms.Page();
            this.wizardPage7 = new Manina.Windows.Forms.Page();
            this.wizardControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // wizardControl1
            // 
            this.wizardControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardControl1.HelpButtonVisible = true;
            this.wizardControl1.Location = new System.Drawing.Point(0, 0);
            this.wizardControl1.Name = "wizardControl1";
            this.wizardControl1.Pages.Add(this.wizardPage1);
            this.wizardControl1.Pages.Add(this.wizardPage2);
            this.wizardControl1.Pages.Add(this.wizardPage3);
            this.wizardControl1.Pages.Add(this.wizardPage4);
            this.wizardControl1.Pages.Add(this.wizardPage5);
            this.wizardControl1.Pages.Add(this.wizardPage6);
            this.wizardControl1.Pages.Add(this.wizardPage7);
            this.wizardControl1.Size = new System.Drawing.Size(773, 423);
            this.wizardControl1.TabIndex = 0;
            this.wizardControl1.BackButtonClicked += new Manina.Windows.Forms.WizardControl.ButtonClickEventHandler(this.wizardControl1_BackButtonClicked);
            this.wizardControl1.NextButtonClicked += new Manina.Windows.Forms.WizardControl.ButtonClickEventHandler(this.wizardControl1_NextButtonClicked);
            this.wizardControl1.CloseButtonClicked += new Manina.Windows.Forms.WizardControl.ButtonClickEventHandler(this.wizardControl1_CloseButtonClicked);
            this.wizardControl1.HelpButtonClicked += new System.EventHandler(this.wizardControl1_HelpButtonClicked);
            this.wizardControl1.PageAdded += new Manina.Windows.Forms.WizardControl.PageEventHandler(this.wizardControl1_PageAdded);
            this.wizardControl1.PageRemoved += new Manina.Windows.Forms.WizardControl.PageEventHandler(this.wizardControl1_PageRemoved);
            this.wizardControl1.PageChanging += new Manina.Windows.Forms.WizardControl.PageChangingEventHandler(this.wizardControl1_PageChanging);
            this.wizardControl1.PageChanged += new Manina.Windows.Forms.WizardControl.PageChangedEventHandler(this.wizardControl1_PageChanged);
            this.wizardControl1.PageValidating += new Manina.Windows.Forms.WizardControl.PageValidatingEventHandler(this.wizardControl1_PageValidating);
            this.wizardControl1.PageValidated += new Manina.Windows.Forms.WizardControl.PageEventHandler(this.wizardControl1_PageValidated);
            this.wizardControl1.PageHidden += new Manina.Windows.Forms.WizardControl.PageEventHandler(this.wizardControl1_PageHidden);
            this.wizardControl1.PageShown += new Manina.Windows.Forms.WizardControl.PageEventHandler(this.wizardControl1_PageShown);
            this.wizardControl1.PagePaint += new Manina.Windows.Forms.WizardControl.PagePaintEventHandler(this.wizardControl1_PagePaint);
            // 
            // wizardPage1
            // 
            this.wizardPage1.Name = "wizardPage1";
            this.wizardPage1.Size = new System.Drawing.Size(773, 374);
            // 
            // wizardPage2
            // 
            this.wizardPage2.Name = "wizardPage2";
            this.wizardPage2.Size = new System.Drawing.Size(773, 374);
            // 
            // wizardPage3
            // 
            this.wizardPage3.Name = "wizardPage3";
            this.wizardPage3.Size = new System.Drawing.Size(773, 374);
            // 
            // wizardPage4
            // 
            this.wizardPage4.Name = "wizardPage4";
            this.wizardPage4.Size = new System.Drawing.Size(773, 374);
            // 
            // wizardPage5
            // 
            this.wizardPage5.Name = "wizardPage5";
            this.wizardPage5.Size = new System.Drawing.Size(773, 374);
            // 
            // wizardPage6
            // 
            this.wizardPage6.Name = "wizardPage6";
            this.wizardPage6.Size = new System.Drawing.Size(773, 374);
            // 
            // wizardPage7
            // 
            this.wizardPage7.Name = "wizardPage7";
            this.wizardPage7.Size = new System.Drawing.Size(773, 374);
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

        private Manina.Windows.Forms.WizardControl wizardControl1;
        private Manina.Windows.Forms.Page wizardPage1;
        private Manina.Windows.Forms.Page wizardPage2;
        private Manina.Windows.Forms.Page wizardPage3;
        private Manina.Windows.Forms.Page wizardPage4;
        private Manina.Windows.Forms.Page wizardPage5;
        private Manina.Windows.Forms.Page wizardPage6;
        private Manina.Windows.Forms.Page wizardPage7;
    }
}

