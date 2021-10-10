
namespace PixelStacker.UI
{
    partial class MaterialSelectTile
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
            // MaterialSelectTile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MaterialSelectTile";
            this.Size = new System.Drawing.Size(64, 64);
            this.Click += new System.EventHandler(this.MaterialSelectTile_Click);
            this.Enter += new System.EventHandler(this.MaterialSelectTile_Enter);
            this.Leave += new System.EventHandler(this.MaterialSelectTile_Leave);
            this.MouseEnter += new System.EventHandler(this.MaterialSelectTile_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.MaterialSelectTile_MouseLeave);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
