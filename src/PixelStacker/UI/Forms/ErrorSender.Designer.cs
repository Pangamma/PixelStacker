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
            cbxIncludeImage = new System.Windows.Forms.CheckBox();
            cbxIsUploadSavedSettings = new System.Windows.Forms.CheckBox();
            cbxIsStackTraceEnabled = new System.Windows.Forms.CheckBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            checkBox1 = new System.Windows.Forms.CheckBox();
            btnYes = new System.Windows.Forms.Button();
            btnNo = new System.Windows.Forms.Button();
            linkLabel1 = new System.Windows.Forms.LinkLabel();
            SuspendLayout();
            // 
            // cbxIncludeImage
            // 
            cbxIncludeImage.AutoSize = true;
            cbxIncludeImage.Checked = true;
            cbxIncludeImage.CheckState = System.Windows.Forms.CheckState.Checked;
            cbxIncludeImage.Location = new System.Drawing.Point(12, 271);
            cbxIncludeImage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            cbxIncludeImage.Name = "cbxIncludeImage";
            cbxIncludeImage.Size = new System.Drawing.Size(485, 24);
            cbxIncludeImage.TabIndex = 0;
            cbxIncludeImage.Text = "Are you willing to upload your current image for analysis? (Optional)";
            cbxIncludeImage.UseVisualStyleBackColor = true;
            // 
            // cbxIsUploadSavedSettings
            // 
            cbxIsUploadSavedSettings.AutoSize = true;
            cbxIsUploadSavedSettings.Checked = true;
            cbxIsUploadSavedSettings.CheckState = System.Windows.Forms.CheckState.Checked;
            cbxIsUploadSavedSettings.Enabled = false;
            cbxIsUploadSavedSettings.Location = new System.Drawing.Point(12, 146);
            cbxIsUploadSavedSettings.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            cbxIsUploadSavedSettings.Name = "cbxIsUploadSavedSettings";
            cbxIsUploadSavedSettings.Size = new System.Drawing.Size(352, 24);
            cbxIsUploadSavedSettings.TabIndex = 1;
            cbxIsUploadSavedSettings.Text = "Upload current saved option settings. (Required)";
            cbxIsUploadSavedSettings.UseVisualStyleBackColor = true;
            // 
            // cbxIsStackTraceEnabled
            // 
            cbxIsStackTraceEnabled.AutoSize = true;
            cbxIsStackTraceEnabled.Checked = true;
            cbxIsStackTraceEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            cbxIsStackTraceEnabled.Enabled = false;
            cbxIsStackTraceEnabled.Location = new System.Drawing.Point(12, 180);
            cbxIsStackTraceEnabled.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            cbxIsStackTraceEnabled.Name = "cbxIsStackTraceEnabled";
            cbxIsStackTraceEnabled.Size = new System.Drawing.Size(345, 24);
            cbxIsStackTraceEnabled.TabIndex = 2;
            cbxIsStackTraceEnabled.Text = "Upload stacktrace + error messages. (Required)";
            cbxIsStackTraceEnabled.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            label1.Location = new System.Drawing.Point(12, 11);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(283, 31);
            label1.TabIndex = 3;
            label1.Text = "An error has occurred.";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(15, 61);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(474, 60);
            label2.TabIndex = 4;
            label2.Text = "Are you willing to send me some information to help me fix this error?\nIf yes, the following info will be zipped. Then you can send the info to \nme as a ";
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Checked = true;
            checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBox1.Enabled = false;
            checkBox1.Location = new System.Drawing.Point(12, 214);
            checkBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(287, 24);
            checkBox1.TabIndex = 5;
            checkBox1.Text = "Current PixelStacker version (Required)";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // btnYes
            // 
            btnYes.Location = new System.Drawing.Point(12, 324);
            btnYes.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnYes.Name = "btnYes";
            btnYes.Size = new System.Drawing.Size(205, 122);
            btnYes.TabIndex = 6;
            btnYes.Text = "Yes! Zip some info so the issue can be fixed.";
            btnYes.UseVisualStyleBackColor = true;
            btnYes.Click += btnYes_Click;
            // 
            // btnNo
            // 
            btnNo.Location = new System.Drawing.Point(235, 324);
            btnNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnNo.Name = "btnNo";
            btnNo.Size = new System.Drawing.Size(220, 122);
            btnNo.TabIndex = 7;
            btnNo.Text = "No, skip the report for now.";
            btnNo.UseVisualStyleBackColor = true;
            btnNo.Click += btnNo_Click;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new System.Drawing.Point(72, 104);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new System.Drawing.Size(92, 20);
            linkLabel1.TabIndex = 10;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Github issue.";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // ErrorSender
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(477, 466);
            Controls.Add(linkLabel1);
            Controls.Add(btnNo);
            Controls.Add(btnYes);
            Controls.Add(checkBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(cbxIsStackTraceEnabled);
            Controls.Add(cbxIsUploadSavedSettings);
            Controls.Add(cbxIncludeImage);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "ErrorSender";
            Text = "Error Reporter";
            ResumeLayout(false);
            PerformLayout();
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