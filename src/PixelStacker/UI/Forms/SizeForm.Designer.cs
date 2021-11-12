namespace PixelStacker.UI
{
    partial class SizeForm
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
            this.lblInstructions = new System.Windows.Forms.Label();
            this.lblWidth = new System.Windows.Forms.Label();
            this.tbxMaxWidth = new System.Windows.Forms.TextBox();
            this.lblHeight = new System.Windows.Forms.Label();
            this.tbxMaxHeight = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblInstructions
            // 
            this.lblInstructions.Location = new System.Drawing.Point(12, 9);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(225, 53);
            this.lblInstructions.TabIndex = 0;
            this.lblInstructions.Text = "Enter a max width or max height to limit the output size.";
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new System.Drawing.Point(12, 75);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(81, 20);
            this.lblWidth.TabIndex = 1;
            this.lblWidth.Text = "Max Width";
            // 
            // tbxMaxWidth
            // 
            this.tbxMaxWidth.Location = new System.Drawing.Point(116, 68);
            this.tbxMaxWidth.Name = "tbxMaxWidth";
            this.tbxMaxWidth.Size = new System.Drawing.Size(133, 27);
            this.tbxMaxWidth.TabIndex = 2;
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(12, 112);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(86, 20);
            this.lblHeight.TabIndex = 3;
            this.lblHeight.Text = "Max Height";
            // 
            // tbxMaxHeight
            // 
            this.tbxMaxHeight.Location = new System.Drawing.Point(116, 109);
            this.tbxMaxHeight.Name = "tbxMaxHeight";
            this.tbxMaxHeight.Size = new System.Drawing.Size(133, 27);
            this.tbxMaxHeight.TabIndex = 4;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 156);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(237, 29);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // SizeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(261, 197);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbxMaxHeight);
            this.Controls.Add(this.lblHeight);
            this.Controls.Add(this.tbxMaxWidth);
            this.Controls.Add(this.lblWidth);
            this.Controls.Add(this.lblInstructions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SizeForm";
            this.ShowInTaskbar = false;
            this.Text = "Output size?";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.TextBox tbxMaxWidth;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.TextBox tbxMaxHeight;
        private System.Windows.Forms.Button btnSave;
    }
}