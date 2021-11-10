namespace PixelStacker.UI.Controls
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.btnFill = new System.Windows.Forms.Button();
            this.btnErase = new System.Windows.Forms.Button();
            this.btnPaintBrush = new System.Windows.Forms.Button();
            this.btnPencil = new System.Windows.Forms.Button();
            this.btnPicker = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackgroundImage = global::PixelStacker.Resources.UIResources.close_normal;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(3, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(31, 33);
            this.button1.TabIndex = 0;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnFill
            // 
            this.btnFill.BackgroundImage = global::PixelStacker.Resources.UIResources.fill;
            this.btnFill.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnFill.FlatAppearance.BorderSize = 0;
            this.btnFill.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFill.Location = new System.Drawing.Point(3, 188);
            this.btnFill.Name = "btnFill";
            this.btnFill.Size = new System.Drawing.Size(32, 32);
            this.btnFill.TabIndex = 4;
            this.btnFill.UseVisualStyleBackColor = false;
            // 
            // btnErase
            // 
            this.btnErase.BackgroundImage = global::PixelStacker.Resources.UIResources.eraser;
            this.btnErase.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnErase.FlatAppearance.BorderSize = 0;
            this.btnErase.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnErase.Location = new System.Drawing.Point(3, 226);
            this.btnErase.Name = "btnErase";
            this.btnErase.Size = new System.Drawing.Size(32, 32);
            this.btnErase.TabIndex = 5;
            this.btnErase.UseVisualStyleBackColor = false;
            // 
            // btnPaintBrush
            // 
            this.btnPaintBrush.BackgroundImage = global::PixelStacker.Resources.UIResources.paintbrush;
            this.btnPaintBrush.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPaintBrush.FlatAppearance.BorderSize = 0;
            this.btnPaintBrush.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPaintBrush.Location = new System.Drawing.Point(3, 74);
            this.btnPaintBrush.Name = "btnPaintBrush";
            this.btnPaintBrush.Size = new System.Drawing.Size(32, 32);
            this.btnPaintBrush.TabIndex = 1;
            this.btnPaintBrush.UseVisualStyleBackColor = false;
            // 
            // btnPencil
            // 
            this.btnPencil.BackgroundImage = global::PixelStacker.Resources.UIResources.pencil;
            this.btnPencil.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPencil.FlatAppearance.BorderSize = 0;
            this.btnPencil.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPencil.Location = new System.Drawing.Point(3, 36);
            this.btnPencil.Name = "btnPencil";
            this.btnPencil.Size = new System.Drawing.Size(32, 32);
            this.btnPencil.TabIndex = 0;
            this.btnPencil.UseVisualStyleBackColor = false;
            // 
            // btnPicker
            // 
            this.btnPicker.BackgroundImage = global::PixelStacker.Resources.UIResources.picker;
            this.btnPicker.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPicker.FlatAppearance.BorderSize = 0;
            this.btnPicker.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPicker.Location = new System.Drawing.Point(3, 150);
            this.btnPicker.Name = "btnPicker";
            this.btnPicker.Size = new System.Drawing.Size(32, 32);
            this.btnPicker.TabIndex = 3;
            this.btnPicker.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackgroundImage = global::PixelStacker.Resources.UIResources.wand;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Location = new System.Drawing.Point(3, 112);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(32, 32);
            this.button2.TabIndex = 2;
            this.button2.UseVisualStyleBackColor = false;
            // 
            // CanvasTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnPicker);
            this.Controls.Add(this.btnPencil);
            this.Controls.Add(this.btnPaintBrush);
            this.Controls.Add(this.btnErase);
            this.Controls.Add(this.btnFill);
            this.Controls.Add(this.button1);
            this.Name = "CanvasTools";
            this.Size = new System.Drawing.Size(39, 262);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnFill;
        private System.Windows.Forms.Button btnErase;
        private System.Windows.Forms.Button btnPaintBrush;
        private System.Windows.Forms.Button btnPencil;
        private System.Windows.Forms.Button btnPicker;
        private System.Windows.Forms.Button button2;
    }
}
