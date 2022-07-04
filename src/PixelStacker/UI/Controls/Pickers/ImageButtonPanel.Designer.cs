namespace PixelStacker.UI.Controls.Pickers
{
    partial class ImageButtonPanel
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
            this.tilePanel = new PixelStacker.UI.Controls.CustomFlowLayoutPanel();
            this.SuspendLayout();
            // 
            // tilePanel
            // 
            this.tilePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tilePanel.AutoScroll = true;
            this.tilePanel.Location = new System.Drawing.Point(0, 0);
            this.tilePanel.Name = "tilePanel";
            this.tilePanel.OnCommandKey = null;
            this.tilePanel.Size = new System.Drawing.Size(461, 342);
            this.tilePanel.TabIndex = 0;
            // 
            // ImageButtonPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.tilePanel);
            this.Name = "ImageButtonPanel";
            this.Size = new System.Drawing.Size(464, 342);
            this.ResumeLayout(false);

        }

        #endregion

        private CustomFlowLayoutPanel tilePanel;
    }
}
