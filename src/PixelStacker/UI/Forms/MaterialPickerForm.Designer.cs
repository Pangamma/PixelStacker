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
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbxSearchFilter = new System.Windows.Forms.ComboBox();
            this.lblBottomMaterial = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lblTopMaterial = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.imgBottomMaterial = new PixelStacker.UI.Controls.ImageButton();
            this.imgTopMaterial = new PixelStacker.UI.Controls.ImageButton();
            this.imgMaterialsCombined = new PixelStacker.UI.Controls.ImageButton();
            this.customPanel1 = new PixelStacker.UI.Controls.CustomPanel();
            this.lblFilter = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.Controls.Add(this.lblFilter);
            this.panel1.Controls.Add(this.tbxSearchFilter);
            this.panel1.Controls.Add(this.lblBottomMaterial);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.lblTopMaterial);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.imgBottomMaterial);
            this.panel1.Controls.Add(this.imgTopMaterial);
            this.panel1.Controls.Add(this.imgMaterialsCombined);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(202, 426);
            this.panel1.TabIndex = 0;
            // 
            // tbxSearchFilter
            // 
            this.tbxSearchFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.tbxSearchFilter.FormattingEnabled = true;
            this.tbxSearchFilter.Location = new System.Drawing.Point(3, 230);
            this.tbxSearchFilter.Name = "tbxSearchFilter";
            this.tbxSearchFilter.Size = new System.Drawing.Size(194, 28);
            this.tbxSearchFilter.TabIndex = 7;
            // 
            // lblBottomMaterial
            // 
            this.lblBottomMaterial.Location = new System.Drawing.Point(33, 166);
            this.lblBottomMaterial.Name = "lblBottomMaterial";
            this.lblBottomMaterial.Size = new System.Drawing.Size(164, 25);
            this.lblBottomMaterial.TabIndex = 6;
            this.lblBottomMaterial.Text = "Bottom material";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::PixelStacker.Resources.UIResources.layers_bottom;
            this.pictureBox2.Location = new System.Drawing.Point(3, 167);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(24, 24);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // lblTopMaterial
            // 
            this.lblTopMaterial.Location = new System.Drawing.Point(33, 137);
            this.lblTopMaterial.Name = "lblTopMaterial";
            this.lblTopMaterial.Size = new System.Drawing.Size(164, 25);
            this.lblTopMaterial.TabIndex = 4;
            this.lblTopMaterial.Text = "Top material";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::PixelStacker.Resources.UIResources.layers_top;
            this.pictureBox1.Location = new System.Drawing.Point(3, 137);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 24);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // imgBottomMaterial
            // 
            this.imgBottomMaterial.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imgBottomMaterial.Image = null;
            this.imgBottomMaterial.IsChecked = false;
            this.imgBottomMaterial.Location = new System.Drawing.Point(137, 69);
            this.imgBottomMaterial.Name = "imgBottomMaterial";
            this.imgBottomMaterial.PushState = PixelStacker.UI.Controls.ImageButtonPushState.Normal;
            this.imgBottomMaterial.Size = new System.Drawing.Size(60, 60);
            this.imgBottomMaterial.TabIndex = 2;
            this.imgBottomMaterial.Click += new System.EventHandler(this.imgBottomMaterial_Click);
            // 
            // imgTopMaterial
            // 
            this.imgTopMaterial.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imgTopMaterial.Image = null;
            this.imgTopMaterial.IsChecked = false;
            this.imgTopMaterial.Location = new System.Drawing.Point(137, 3);
            this.imgTopMaterial.Name = "imgTopMaterial";
            this.imgTopMaterial.PushState = PixelStacker.UI.Controls.ImageButtonPushState.Normal;
            this.imgTopMaterial.Size = new System.Drawing.Size(60, 60);
            this.imgTopMaterial.TabIndex = 1;
            this.imgTopMaterial.Click += new System.EventHandler(this.imgTopMaterial_Click);
            // 
            // imgMaterialsCombined
            // 
            this.imgMaterialsCombined.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imgMaterialsCombined.Image = null;
            this.imgMaterialsCombined.IsChecked = true;
            this.imgMaterialsCombined.Location = new System.Drawing.Point(3, 3);
            this.imgMaterialsCombined.Name = "imgMaterialsCombined";
            this.imgMaterialsCombined.PushState = PixelStacker.UI.Controls.ImageButtonPushState.Normal;
            this.imgMaterialsCombined.Size = new System.Drawing.Size(128, 128);
            this.imgMaterialsCombined.TabIndex = 0;
            this.imgMaterialsCombined.Click += new System.EventHandler(this.imgMaterialsCombined_Click);
            // 
            // customPanel1
            // 
            this.customPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.customPanel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.customPanel1.Location = new System.Drawing.Point(220, 12);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.OnCommandKey = null;
            this.customPanel1.Size = new System.Drawing.Size(617, 426);
            this.customPanel1.TabIndex = 1;
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(3, 207);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(42, 20);
            this.lblFilter.TabIndex = 8;
            this.lblFilter.Text = "Filter";
            // 
            // MaterialPickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 450);
            this.Controls.Add(this.customPanel1);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "MaterialPickerForm";
            this.ShowInTaskbar = false;
            this.Text = "MaterialPickerForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Controls.ImageButton imgTopMaterial;
        private Controls.ImageButton imgMaterialsCombined;
        private Controls.ImageButton imgBottomMaterial;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblBottomMaterial;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lblTopMaterial;
        private System.Windows.Forms.ComboBox tbxSearchFilter;
        private Controls.CustomPanel customPanel1;
        private System.Windows.Forms.Label lblFilter;
    }
}