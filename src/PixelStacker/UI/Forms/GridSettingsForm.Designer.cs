namespace PixelStacker.UI.Forms
{
    partial class GridSettingsForm
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
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.tbxGridSize = new System.Windows.Forms.NumericUpDown();
            this.lblGridSize = new System.Windows.Forms.Label();
            this.lblGridColor = new System.Windows.Forms.Label();
            this.btnGridColor = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tbxGridSize)).BeginInit();
            this.SuspendLayout();
            // 
            // tbxGridSize
            // 
            this.tbxGridSize.Location = new System.Drawing.Point(104, 12);
            this.tbxGridSize.Name = "tbxGridSize";
            this.tbxGridSize.Size = new System.Drawing.Size(150, 27);
            this.tbxGridSize.TabIndex = 0;
            this.tbxGridSize.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.tbxGridSize.ValueChanged += new System.EventHandler(this.tbxGridSize_ValueChanged);
            // 
            // lblGridSize
            // 
            this.lblGridSize.AutoSize = true;
            this.lblGridSize.Location = new System.Drawing.Point(12, 14);
            this.lblGridSize.Name = "lblGridSize";
            this.lblGridSize.Size = new System.Drawing.Size(68, 20);
            this.lblGridSize.TabIndex = 1;
            this.lblGridSize.Text = "Grid Size";
            // 
            // lblGridColor
            // 
            this.lblGridColor.AutoSize = true;
            this.lblGridColor.Location = new System.Drawing.Point(12, 49);
            this.lblGridColor.Name = "lblGridColor";
            this.lblGridColor.Size = new System.Drawing.Size(45, 20);
            this.lblGridColor.TabIndex = 2;
            this.lblGridColor.Text = "Color";
            // 
            // btnGridColor
            // 
            this.btnGridColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnGridColor.Location = new System.Drawing.Point(104, 49);
            this.btnGridColor.Name = "btnGridColor";
            this.btnGridColor.Size = new System.Drawing.Size(150, 20);
            this.btnGridColor.TabIndex = 3;
            this.btnGridColor.UseVisualStyleBackColor = true;
            this.btnGridColor.Click += new System.EventHandler(this.btnGridColor_Click);
            // 
            // GridSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 81);
            this.Controls.Add(this.btnGridColor);
            this.Controls.Add(this.lblGridColor);
            this.Controls.Add(this.lblGridSize);
            this.Controls.Add(this.tbxGridSize);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "GridSettingsForm";
            this.ShowInTaskbar = false;
            this.Text = "Grid Settings";
            this.Load += new System.EventHandler(this.GridSettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbxGridSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.NumericUpDown tbxGridSize;
        private System.Windows.Forms.Label lblGridSize;
        private System.Windows.Forms.Label lblGridColor;
        private System.Windows.Forms.Button btnGridColor;
    }
}