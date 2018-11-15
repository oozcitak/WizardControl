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
            this.wizardPage1 = new Manina.Windows.Forms.WizardPage();
            this.wizardPage2 = new Manina.Windows.Forms.WizardPage();
            this.wizardPage3 = new Manina.Windows.Forms.WizardPage();
            this.wizardPage4 = new Manina.Windows.Forms.WizardPage();
            this.SuspendLayout();
            // 
            // wizardControl1
            // 
            this.wizardControl1.SelectedPage = this.wizardPage4;
            this.wizardControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardControl1.Location = new System.Drawing.Point(0, 0);
            this.wizardControl1.Name = "wizardControl1";
            this.wizardControl1.Size = new System.Drawing.Size(800, 450);
            this.wizardControl1.TabIndex = 0;
            this.wizardControl1.Pages.Add(wizardPage1);
            this.wizardControl1.Pages.Add(wizardPage2);
            this.wizardControl1.Pages.Add(wizardPage3);
            this.wizardControl1.Pages.Add(wizardPage4);
            // 
            // wizardPage1
            // 
            this.wizardPage1.Name = "wizardPage1";
            // 
            // wizardPage2
            // 
            this.wizardPage2.Name = "wizardPage2";
            // 
            // wizardPage3
            // 
            this.wizardPage3.Name = "wizardPage3";
            // 
            // wizardPage4
            // 
            this.wizardPage4.Name = "wizardPage4";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.wizardControl1);
            this.Name = "TestForm";
            this.Text = "Test Form";
            this.ResumeLayout(false);

        }

        #endregion

        private Manina.Windows.Forms.WizardControl wizardControl1;
        private Manina.Windows.Forms.WizardPage wizardPage1;
        private Manina.Windows.Forms.WizardPage wizardPage4;
        private Manina.Windows.Forms.WizardPage wizardPage2;
        private Manina.Windows.Forms.WizardPage wizardPage3;
    }
}

