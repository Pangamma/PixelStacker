namespace PixelStacker
{
    partial class ErrorSender
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorSender));
            this.cbxIncludeImage = new System.Windows.Forms.CheckBox();
            this.cbxIsUploadSavedSettings = new System.Windows.Forms.CheckBox();
            this.cbxIsStackTraceEnabled = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // cbxIncludeImage
            // 
            resources.ApplyResources(this.cbxIncludeImage, "cbxIncludeImage");
            this.cbxIncludeImage.Checked = true;
            this.cbxIncludeImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxIncludeImage.Name = "cbxIncludeImage";
            this.cbxIncludeImage.UseVisualStyleBackColor = true;
            // 
            // cbxIsUploadSavedSettings
            // 
            resources.ApplyResources(this.cbxIsUploadSavedSettings, "cbxIsUploadSavedSettings");
            this.cbxIsUploadSavedSettings.Checked = true;
            this.cbxIsUploadSavedSettings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxIsUploadSavedSettings.Name = "cbxIsUploadSavedSettings";
            this.cbxIsUploadSavedSettings.UseVisualStyleBackColor = true;
            // 
            // cbxIsStackTraceEnabled
            // 
            resources.ApplyResources(this.cbxIsStackTraceEnabled, "cbxIsStackTraceEnabled");
            this.cbxIsStackTraceEnabled.Checked = true;
            this.cbxIsStackTraceEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxIsStackTraceEnabled.Name = "cbxIsStackTraceEnabled";
            this.cbxIsStackTraceEnabled.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // btnYes
            // 
            resources.ApplyResources(this.btnYes, "btnYes");
            this.btnYes.Name = "btnYes";
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // btnNo
            // 
            resources.ApplyResources(this.btnNo, "btnNo");
            this.btnNo.Name = "btnNo";
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // linkLabel1
            // 
            resources.ApplyResources(this.linkLabel1, "linkLabel1");
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.TabStop = true;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // ErrorSender
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbxIsStackTraceEnabled);
            this.Controls.Add(this.cbxIsUploadSavedSettings);
            this.Controls.Add(this.cbxIncludeImage);
            this.Name = "ErrorSender";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbxIncludeImage;
        private System.Windows.Forms.CheckBox cbxIsUploadSavedSettings;
        private System.Windows.Forms.CheckBox cbxIsStackTraceEnabled;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}