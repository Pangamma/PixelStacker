namespace PixelStacker.UI
{
    partial class TextPrompt
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
            this.lblPromptText = new System.Windows.Forms.Label();
            this.tbText = new System.Windows.Forms.TextBox();
            this.btnContinue = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblPromptText
            // 
            this.lblPromptText.AutoSize = true;
            this.lblPromptText.Location = new System.Drawing.Point(22, 9);
            this.lblPromptText.Name = "lblPromptText";
            this.lblPromptText.Size = new System.Drawing.Size(114, 17);
            this.lblPromptText.TabIndex = 0;
            this.lblPromptText.Text = "LABEL PROMPT";
            // 
            // tbText
            // 
            this.tbText.Location = new System.Drawing.Point(25, 39);
            this.tbText.Name = "tbText";
            this.tbText.Size = new System.Drawing.Size(265, 22);
            this.tbText.TabIndex = 1;
            // 
            // btnContinue
            // 
            this.btnContinue.Location = new System.Drawing.Point(169, 67);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(121, 29);
            this.btnContinue.TabIndex = 2;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.BtnSubmitText_Click);
            // 
            // TextPrompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(311, 107);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.tbText);
            this.Controls.Add(this.lblPromptText);
            this.Name = "TextPrompt";
            this.Text = "TextPrompt";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPromptText;
        private System.Windows.Forms.TextBox tbText;
        private System.Windows.Forms.Button btnContinue;
    }
}