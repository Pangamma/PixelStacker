namespace PixelStacker.UI
{
    partial class MaterialPickerWindow
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
            this.panelFlow = new System.Windows.Forms.FlowLayoutPanel();
            this.materialPickerOption1 = new PixelStacker.UI.MaterialPickerOption();
            this.materialPickerOption2 = new PixelStacker.UI.MaterialPickerOption();
            this.materialPickerOption3 = new PixelStacker.UI.MaterialPickerOption();
            this.panelFlow.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelFlow
            // 
            this.panelFlow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFlow.AutoScroll = true;
            this.panelFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelFlow.Controls.Add(this.materialPickerOption1);
            this.panelFlow.Controls.Add(this.materialPickerOption2);
            this.panelFlow.Controls.Add(this.materialPickerOption3);
            this.panelFlow.Cursor = System.Windows.Forms.Cursors.Default;
            this.panelFlow.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelFlow.Location = new System.Drawing.Point(12, 12);
            this.panelFlow.Name = "panelFlow";
            this.panelFlow.Size = new System.Drawing.Size(679, 426);
            this.panelFlow.TabIndex = 0;
            // 
            // materialPickerOption1
            // 
            this.materialPickerOption1.IsFocused = false;
            this.materialPickerOption1.Location = new System.Drawing.Point(3, 3);
            this.materialPickerOption1.Name = "materialPickerOption1";
            this.materialPickerOption1.Size = new System.Drawing.Size(292, 72);
            this.materialPickerOption1.TabIndex = 0;
            // 
            // materialPickerOption2
            // 
            this.materialPickerOption2.IsFocused = false;
            this.materialPickerOption2.Location = new System.Drawing.Point(301, 3);
            this.materialPickerOption2.Name = "materialPickerOption2";
            this.materialPickerOption2.Size = new System.Drawing.Size(292, 72);
            this.materialPickerOption2.TabIndex = 1;
            this.materialPickerOption2.Load += new System.EventHandler(this.materialPickerOption2_Load);
            // 
            // materialPickerOption3
            // 
            this.materialPickerOption3.IsFocused = false;
            this.materialPickerOption3.Location = new System.Drawing.Point(3, 81);
            this.materialPickerOption3.Name = "materialPickerOption3";
            this.materialPickerOption3.Size = new System.Drawing.Size(292, 72);
            this.materialPickerOption3.TabIndex = 2;
            // 
            // MaterialPickerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(703, 450);
            this.ControlBox = false;
            this.Controls.Add(this.panelFlow);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MaterialPickerWindow";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ShowInTaskbar = false;
            this.Text = "MaterialPickerWindow";
            this.Deactivate += new System.EventHandler(this.MaterialPickerWindow_Deactivate);
            this.panelFlow.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private MaterialPickerOption materialPickerOption1;
        private MaterialPickerOption materialPickerOption2;
        private MaterialPickerOption materialPickerOption3;
        public System.Windows.Forms.FlowLayoutPanel panelFlow;
    }
}