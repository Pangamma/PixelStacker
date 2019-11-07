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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.lnkLumenGaming = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.lnkFontMaker = new System.Windows.Forms.LinkLabel();
            this.lnkWoolcityProject = new System.Windows.Forms.LinkLabel();
            this.lnkLinkedIn = new System.Windows.Forms.LinkLabel();
            this.lnkDonate = new System.Windows.Forms.LinkLabel();
            this.lnkDownload = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lnkLumenGaming
            // 
            this.lnkLumenGaming.AutoSize = true;
            this.lnkLumenGaming.Location = new System.Drawing.Point(23, 241);
            this.lnkLumenGaming.Name = "lnkLumenGaming";
            this.lnkLumenGaming.Size = new System.Drawing.Size(117, 13);
            this.lnkLumenGaming.TabIndex = 0;
            this.lnkLumenGaming.TabStop = true;
            this.lnkLumenGaming.Text = "LumenGaming Website";
            this.lnkLumenGaming.Click += new System.EventHandler(this.lnkLumenGaming_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(404, 169);
            this.label1.TabIndex = 1;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // lnkFontMaker
            // 
            this.lnkFontMaker.AutoSize = true;
            this.lnkFontMaker.Location = new System.Drawing.Point(187, 213);
            this.lnkFontMaker.Name = "lnkFontMaker";
            this.lnkFontMaker.Size = new System.Drawing.Size(61, 13);
            this.lnkFontMaker.TabIndex = 2;
            this.lnkFontMaker.TabStop = true;
            this.lnkFontMaker.Text = "Font Maker";
            this.lnkFontMaker.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFontMaker_LinkClicked);
            // 
            // lnkWoolcityProject
            // 
            this.lnkWoolcityProject.AutoSize = true;
            this.lnkWoolcityProject.Location = new System.Drawing.Point(23, 213);
            this.lnkWoolcityProject.Name = "lnkWoolcityProject";
            this.lnkWoolcityProject.Size = new System.Drawing.Size(128, 13);
            this.lnkWoolcityProject.TabIndex = 3;
            this.lnkWoolcityProject.TabStop = true;
            this.lnkWoolcityProject.Text = "Visit the Wool City Project";
            this.lnkWoolcityProject.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkWoolcityProject_LinkClicked);
            // 
            // lnkLinkedIn
            // 
            this.lnkLinkedIn.AutoSize = true;
            this.lnkLinkedIn.Location = new System.Drawing.Point(187, 241);
            this.lnkLinkedIn.Name = "lnkLinkedIn";
            this.lnkLinkedIn.Size = new System.Drawing.Size(87, 13);
            this.lnkLinkedIn.TabIndex = 4;
            this.lnkLinkedIn.TabStop = true;
            this.lnkLinkedIn.Text = "Taylor\'s LinkedIn";
            this.lnkLinkedIn.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLinkedIn_LinkClicked);
            // 
            // lnkDonate
            // 
            this.lnkDonate.AutoSize = true;
            this.lnkDonate.Location = new System.Drawing.Point(299, 213);
            this.lnkDonate.Name = "lnkDonate";
            this.lnkDonate.Size = new System.Drawing.Size(78, 13);
            this.lnkDonate.TabIndex = 5;
            this.lnkDonate.TabStop = true;
            this.lnkDonate.Text = "Buy me a beer!";
            this.lnkDonate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDonate_LinkClicked);
            // 
            // lnkDownload
            // 
            this.lnkDownload.AutoSize = true;
            this.lnkDownload.Location = new System.Drawing.Point(299, 241);
            this.lnkDownload.Name = "lnkDownload";
            this.lnkDownload.Size = new System.Drawing.Size(99, 13);
            this.lnkDownload.TabIndex = 6;
            this.lnkDownload.TabStop = true;
            this.lnkDownload.Text = "Download Location";
            this.lnkDownload.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDownload_LinkClicked);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 278);
            this.Controls.Add(this.lnkDownload);
            this.Controls.Add(this.lnkDonate);
            this.Controls.Add(this.lnkLinkedIn);
            this.Controls.Add(this.lnkWoolcityProject);
            this.Controls.Add(this.lnkFontMaker);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lnkLumenGaming);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.Text = "About Pangamma\'s PixelStacker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel lnkLumenGaming;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel lnkFontMaker;
        private System.Windows.Forms.LinkLabel lnkWoolcityProject;
        private System.Windows.Forms.LinkLabel lnkLinkedIn;
        private System.Windows.Forms.LinkLabel lnkDonate;
        private System.Windows.Forms.LinkLabel lnkDownload;
    }
}