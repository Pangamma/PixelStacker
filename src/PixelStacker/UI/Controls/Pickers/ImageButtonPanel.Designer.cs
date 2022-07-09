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
            this.components = new System.ComponentModel.Container();
            this.tilePanel = new PixelStacker.UI.Controls.CustomFlowLayoutPanel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // tilePanel
            // 
            this.tilePanel.AutoScroll = true;
            this.tilePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tilePanel.Location = new System.Drawing.Point(0, 0);
            this.tilePanel.Margin = new System.Windows.Forms.Padding(0);
            this.tilePanel.Name = "tilePanel";
            this.tilePanel.OnCommandKey = null;
            this.tilePanel.Size = new System.Drawing.Size(464, 342);
            this.tilePanel.TabIndex = 0;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 150;
            this.toolTip1.ReshowDelay = 100;
            // 
            // ImageButtonPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.tilePanel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ImageButtonPanel";
            this.Size = new System.Drawing.Size(464, 342);
            this.ResumeLayout(false);

        }

        #endregion

        private CustomFlowLayoutPanel tilePanel;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
