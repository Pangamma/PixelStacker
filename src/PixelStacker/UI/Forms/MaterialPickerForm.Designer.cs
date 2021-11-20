namespace PixelStacker.UI.Forms
{
    partial class MaterialPickerForm
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
            this.materialPicker1 = new PixelStacker.UI.Controls.MaterialPicker();
            this.SuspendLayout();
            // 
            // materialPicker1
            // 
            this.materialPicker1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialPicker1.Location = new System.Drawing.Point(0, 0);
            this.materialPicker1.Name = "materialPicker1";
            this.materialPicker1.Size = new System.Drawing.Size(441, 450);
            this.materialPicker1.TabIndex = 0;
            // 
            // MaterialPickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 450);
            this.Controls.Add(this.materialPicker1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "MaterialPickerForm";
            this.ShowInTaskbar = false;
            this.Text = "MaterialPickerForm";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.MaterialPicker materialPicker1;
    }
}