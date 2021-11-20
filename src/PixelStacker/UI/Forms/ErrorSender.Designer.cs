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
            this.cbxIncludeImage.AutoSize = true;
            this.cbxIncludeImage.Checked = true;
            this.cbxIncludeImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxIncludeImage.Location = new System.Drawing.Point(12, 271);
            this.cbxIncludeImage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbxIncludeImage.Name = "cbxIncludeImage";
            this.cbxIncludeImage.Size = new System.Drawing.Size(485, 24);
            this.cbxIncludeImage.TabIndex = 0;
            this.cbxIncludeImage.Text = "Are you willing to upload your current image for analysis? (Optional)";
            this.cbxIncludeImage.UseVisualStyleBackColor = true;
            // 
            // cbxIsUploadSavedSettings
            // 
            this.cbxIsUploadSavedSettings.AutoSize = true;
            this.cbxIsUploadSavedSettings.Checked = true;
            this.cbxIsUploadSavedSettings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxIsUploadSavedSettings.Enabled = false;
            this.cbxIsUploadSavedSettings.Location = new System.Drawing.Point(12, 146);
            this.cbxIsUploadSavedSettings.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbxIsUploadSavedSettings.Name = "cbxIsUploadSavedSettings";
            this.cbxIsUploadSavedSettings.Size = new System.Drawing.Size(352, 24);
            this.cbxIsUploadSavedSettings.TabIndex = 1;
            this.cbxIsUploadSavedSettings.Text = "Upload current saved option settings. (Required)";
            this.cbxIsUploadSavedSettings.UseVisualStyleBackColor = true;
            // 
            // cbxIsStackTraceEnabled
            // 
            this.cbxIsStackTraceEnabled.AutoSize = true;
            this.cbxIsStackTraceEnabled.Checked = true;
            this.cbxIsStackTraceEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxIsStackTraceEnabled.Enabled = false;
            this.cbxIsStackTraceEnabled.Location = new System.Drawing.Point(12, 180);
            this.cbxIsStackTraceEnabled.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbxIsStackTraceEnabled.Name = "cbxIsStackTraceEnabled";
            this.cbxIsStackTraceEnabled.Size = new System.Drawing.Size(345, 24);
            this.cbxIsStackTraceEnabled.TabIndex = 2;
            this.cbxIsStackTraceEnabled.Text = "Upload stacktrace + error messages. (Required)";
            this.cbxIsStackTraceEnabled.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(283, 31);
            this.label1.TabIndex = 3;
            this.label1.Text = "An error has occurred.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(474, 60);
            this.label2.TabIndex = 4;
            this.label2.Text = "Are you willing to send me some information to help me fix this error?\nIf yes, th" +
    "e following info will be zipped. Then you can send the info to \nme as a ";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(12, 214);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(287, 24);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Current PixelStacker version (Required)";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // btnYes
            // 
            this.btnYes.Location = new System.Drawing.Point(12, 324);
            this.btnYes.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(205, 122);
            this.btnYes.TabIndex = 6;
            this.btnYes.Text = "Yes! Zip some info so the issue can be fixed.";
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // btnNo
            // 
            this.btnNo.Location = new System.Drawing.Point(235, 324);
            this.btnNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(220, 122);
            this.btnNo.TabIndex = 7;
            this.btnNo.Text = "No, skip the report for now.";
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(72, 104);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(92, 20);
            this.linkLabel1.TabIndex = 10;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Github issue.";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // ErrorSender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 466);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbxIsStackTraceEnabled);
            this.Controls.Add(this.cbxIsUploadSavedSettings);
            this.Controls.Add(this.cbxIncludeImage);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ErrorSender";
            this.Text = "Error Reporter";
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