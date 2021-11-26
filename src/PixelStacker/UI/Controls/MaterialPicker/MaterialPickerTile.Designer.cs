namespace PixelStacker.UI.Controls.MaterialPicker
{
    partial class MaterialPickerTile
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
            this.SuspendLayout();
            // 
            // MaterialPickerTile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "MaterialPickerTile";
            this.Size = new System.Drawing.Size(64, 64);
            this.Click += new System.EventHandler(this.MaterialPickerTile_Click);
            this.Enter += new System.EventHandler(this.MaterialPickerTile_Enter);
            this.Leave += new System.EventHandler(this.MaterialPickerTile_Leave);
            this.MouseEnter += new System.EventHandler(this.MaterialPickerTile_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.MaterialPickerTile_MouseLeave);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
