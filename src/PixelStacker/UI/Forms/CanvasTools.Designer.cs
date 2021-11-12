namespace PixelStacker.UI.Forms
{
    partial class CanvasTools
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
            this.btnFill = new System.Windows.Forms.Button();
            this.btnErase = new System.Windows.Forms.Button();
            this.btnPicker = new System.Windows.Forms.Button();
            this.btnPencil = new System.Windows.Forms.Button();
            this.btnBrush = new System.Windows.Forms.Button();
            this.btnPan = new System.Windows.Forms.Button();
            this.btnWorldEditOrigin = new System.Windows.Forms.Button();
            this.btnMagicWand = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnFill
            // 
            this.btnFill.BackgroundImage = global::PixelStacker.Resources.UIResources.paint_bucket;
            this.btnFill.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnFill.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnFill.FlatAppearance.BorderSize = 0;
            this.btnFill.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFill.Location = new System.Drawing.Point(10, 10);
            this.btnFill.Margin = new System.Windows.Forms.Padding(1);
            this.btnFill.Name = "btnFill";
            this.btnFill.Size = new System.Drawing.Size(32, 32);
            this.btnFill.TabIndex = 0;
            this.btnFill.Text = "c";
            this.btnFill.UseVisualStyleBackColor = true;
            this.btnFill.Click += new System.EventHandler(this.btnFill_Click);
            // 
            // btnErase
            // 
            this.btnErase.BackgroundImage = global::PixelStacker.Resources.UIResources.eraser;
            this.btnErase.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnErase.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnErase.FlatAppearance.BorderSize = 0;
            this.btnErase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnErase.Location = new System.Drawing.Point(60, 112);
            this.btnErase.Margin = new System.Windows.Forms.Padding(1);
            this.btnErase.Name = "btnErase";
            this.btnErase.Size = new System.Drawing.Size(32, 32);
            this.btnErase.TabIndex = 1;
            this.btnErase.UseVisualStyleBackColor = true;
            this.btnErase.Click += new System.EventHandler(this.btnEraser_Click);
            // 
            // btnPicker
            // 
            this.btnPicker.BackgroundImage = global::PixelStacker.Resources.UIResources.dropper;
            this.btnPicker.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPicker.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnPicker.FlatAppearance.BorderSize = 0;
            this.btnPicker.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPicker.Location = new System.Drawing.Point(10, 112);
            this.btnPicker.Margin = new System.Windows.Forms.Padding(1);
            this.btnPicker.Name = "btnPicker";
            this.btnPicker.Size = new System.Drawing.Size(32, 32);
            this.btnPicker.TabIndex = 2;
            this.btnPicker.UseVisualStyleBackColor = true;
            this.btnPicker.Click += new System.EventHandler(this.btnPicker_Click);
            // 
            // btnPencil
            // 
            this.btnPencil.BackgroundImage = global::PixelStacker.Resources.UIResources.pencil_1;
            this.btnPencil.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPencil.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnPencil.FlatAppearance.BorderSize = 0;
            this.btnPencil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPencil.Location = new System.Drawing.Point(10, 56);
            this.btnPencil.Margin = new System.Windows.Forms.Padding(1);
            this.btnPencil.Name = "btnPencil";
            this.btnPencil.Size = new System.Drawing.Size(32, 32);
            this.btnPencil.TabIndex = 3;
            this.btnPencil.UseVisualStyleBackColor = true;
            this.btnPencil.Click += new System.EventHandler(this.btnPencil_Click);
            // 
            // btnBrush
            // 
            this.btnBrush.BackgroundImage = global::PixelStacker.Resources.UIResources.paintbrush;
            this.btnBrush.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnBrush.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnBrush.FlatAppearance.BorderSize = 0;
            this.btnBrush.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrush.Location = new System.Drawing.Point(60, 56);
            this.btnBrush.Margin = new System.Windows.Forms.Padding(1);
            this.btnBrush.Name = "btnBrush";
            this.btnBrush.Size = new System.Drawing.Size(32, 32);
            this.btnBrush.TabIndex = 4;
            this.btnBrush.UseVisualStyleBackColor = true;
            this.btnBrush.Click += new System.EventHandler(this.btnBrush_Click);
            // 
            // btnPan
            // 
            this.btnPan.BackgroundImage = global::PixelStacker.Resources.UIResources.all_directions;
            this.btnPan.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPan.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnPan.FlatAppearance.BorderSize = 0;
            this.btnPan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPan.Location = new System.Drawing.Point(60, 10);
            this.btnPan.Margin = new System.Windows.Forms.Padding(1);
            this.btnPan.Name = "btnPan";
            this.btnPan.Size = new System.Drawing.Size(32, 32);
            this.btnPan.TabIndex = 6;
            this.btnPan.UseVisualStyleBackColor = true;
            this.btnPan.Click += new System.EventHandler(this.btnPanZoom_Click);
            // 
            // btnWorldEditOrigin
            // 
            this.btnWorldEditOrigin.BackgroundImage = global::PixelStacker.Resources.UIResources.compass;
            this.btnWorldEditOrigin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnWorldEditOrigin.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnWorldEditOrigin.FlatAppearance.BorderSize = 0;
            this.btnWorldEditOrigin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWorldEditOrigin.Location = new System.Drawing.Point(10, 163);
            this.btnWorldEditOrigin.Margin = new System.Windows.Forms.Padding(1);
            this.btnWorldEditOrigin.Name = "btnWorldEditOrigin";
            this.btnWorldEditOrigin.Size = new System.Drawing.Size(32, 32);
            this.btnWorldEditOrigin.TabIndex = 7;
            this.btnWorldEditOrigin.UseVisualStyleBackColor = true;
            this.btnWorldEditOrigin.Click += new System.EventHandler(this.btnWorldEditOrigin_Click);
            // 
            // btnMagicWand
            // 
            this.btnMagicWand.BackgroundImage = global::PixelStacker.Resources.UIResources.magic_wand_1;
            this.btnMagicWand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMagicWand.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnMagicWand.FlatAppearance.BorderSize = 0;
            this.btnMagicWand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMagicWand.Location = new System.Drawing.Point(60, 163);
            this.btnMagicWand.Margin = new System.Windows.Forms.Padding(1);
            this.btnMagicWand.Name = "btnMagicWand";
            this.btnMagicWand.Size = new System.Drawing.Size(32, 32);
            this.btnMagicWand.TabIndex = 8;
            this.btnMagicWand.UseVisualStyleBackColor = true;
            // 
            // CanvasTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(106, 217);
            this.Controls.Add(this.btnMagicWand);
            this.Controls.Add(this.btnWorldEditOrigin);
            this.Controls.Add(this.btnPan);
            this.Controls.Add(this.btnBrush);
            this.Controls.Add(this.btnPencil);
            this.Controls.Add(this.btnPicker);
            this.Controls.Add(this.btnErase);
            this.Controls.Add(this.btnFill);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CanvasTools";
            this.ShowInTaskbar = false;
            this.Text = "Toolbox";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnFill;
        private System.Windows.Forms.Button btnErase;
        private System.Windows.Forms.Button btnPicker;
        private System.Windows.Forms.Button btnPencil;
        private System.Windows.Forms.Button btnBrush;
        private System.Windows.Forms.Button btnPan;
        private System.Windows.Forms.Button btnWorldEditOrigin;
        private System.Windows.Forms.Button btnMagicWand;
    }
}