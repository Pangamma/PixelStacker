namespace PixelStacker.UI
{
    partial class AboutForm
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
            this.Icon = global::PixelStacker.Resources.UIResources.wool;

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.lnkMyWebsite = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.lnkFontMaker = new System.Windows.Forms.LinkLabel();
            this.lnkWoolcityProject = new System.Windows.Forms.LinkLabel();
            this.lnkLinkedIn = new System.Windows.Forms.LinkLabel();
            this.lnkDonate = new System.Windows.Forms.LinkLabel();
            this.lnkDownload = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lnkMyWebsite
            // 
            resources.ApplyResources(this.lnkMyWebsite, "lnkMyWebsite");
            this.lnkMyWebsite.Name = "lnkMyWebsite";
            this.lnkMyWebsite.TabStop = true;
            this.lnkMyWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkMyWebsite_LinkClicked);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lnkFontMaker
            // 
            resources.ApplyResources(this.lnkFontMaker, "lnkFontMaker");
            this.lnkFontMaker.Name = "lnkFontMaker";
            this.lnkFontMaker.TabStop = true;
            this.lnkFontMaker.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFontMaker_LinkClicked);
            // 
            // lnkWoolcityProject
            // 
            resources.ApplyResources(this.lnkWoolcityProject, "lnkWoolcityProject");
            this.lnkWoolcityProject.Name = "lnkWoolcityProject";
            this.lnkWoolcityProject.TabStop = true;
            this.lnkWoolcityProject.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkWoolcityProject_LinkClicked);
            // 
            // lnkLinkedIn
            // 
            resources.ApplyResources(this.lnkLinkedIn, "lnkLinkedIn");
            this.lnkLinkedIn.Name = "lnkLinkedIn";
            this.lnkLinkedIn.TabStop = true;
            this.lnkLinkedIn.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLinkedIn_LinkClicked);
            // 
            // lnkDonate
            // 
            resources.ApplyResources(this.lnkDonate, "lnkDonate");
            this.lnkDonate.Name = "lnkDonate";
            this.lnkDonate.TabStop = true;
            this.lnkDonate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDonate_LinkClicked);
            // 
            // lnkDownload
            // 
            resources.ApplyResources(this.lnkDownload, "lnkDownload");
            this.lnkDownload.Name = "lnkDownload";
            this.lnkDownload.TabStop = true;
            this.lnkDownload.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDownload_LinkClicked);
            // 
            // AboutForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lnkDownload);
            this.Controls.Add(this.lnkDonate);
            this.Controls.Add(this.lnkLinkedIn);
            this.Controls.Add(this.lnkWoolcityProject);
            this.Controls.Add(this.lnkFontMaker);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lnkMyWebsite);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel lnkMyWebsite;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel lnkFontMaker;
        private System.Windows.Forms.LinkLabel lnkWoolcityProject;
        private System.Windows.Forms.LinkLabel lnkLinkedIn;
        private System.Windows.Forms.LinkLabel lnkDonate;
        private System.Windows.Forms.LinkLabel lnkDownload;
    }
}