namespace PixelStacker.UI
{
    partial class MaterialPickerOption
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MaterialsText = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // MaterialsText
            // 
            this.MaterialsText.AutoSize = true;
            this.MaterialsText.Location = new System.Drawing.Point(73, 3);
            this.MaterialsText.MaximumSize = new System.Drawing.Size(0, 64);
            this.MaterialsText.Name = "MaterialsText";
            this.MaterialsText.Size = new System.Drawing.Size(49, 13);
            this.MaterialsText.TabIndex = 1;
            this.MaterialsText.Text = "Materials";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.MaterialPickerOption_Click);
            this.pictureBox1.MouseEnter += new System.EventHandler(this.MaterialPickerOption_MouseEnter);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.MaterialPickerOption_MouseLeave);
            // 
            // MaterialPickerOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MaterialsText);
            this.Controls.Add(this.pictureBox1);
            this.Name = "MaterialPickerOption";
            this.Size = new System.Drawing.Size(292, 72);
            this.Click += new System.EventHandler(this.MaterialPickerOption_Click);
            this.MouseEnter += new System.EventHandler(this.MaterialPickerOption_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.MaterialPickerOption_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.Label MaterialsText;
    }
}
