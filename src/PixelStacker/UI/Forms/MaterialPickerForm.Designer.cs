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
            this.lblPoolPicker = new System.Windows.Forms.Label();
            this.ddlColorPool = new System.Windows.Forms.ComboBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.tbxMaterialFilter = new System.Windows.Forms.ComboBox();
            this.lblBottomMaterial = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lblTopMaterial = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.imgBottomMaterial = new PixelStacker.UI.Controls.ImageButton();
            this.imgTopMaterial = new PixelStacker.UI.Controls.ImageButton();
            this.imgMaterialsCombined = new PixelStacker.UI.Controls.ImageButton();
            this.customPanel1 = new PixelStacker.UI.Controls.CustomPanel();
            this.tcSearchOptions = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tcSearchOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.Controls.Add(this.tcSearchOptions);
            this.panel1.Controls.Add(this.lblPoolPicker);
            this.panel1.Controls.Add(this.ddlColorPool);
            this.panel1.Controls.Add(this.lblFilter);
            this.panel1.Controls.Add(this.tbxMaterialFilter);
            this.panel1.Controls.Add(this.lblBottomMaterial);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.lblTopMaterial);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.imgBottomMaterial);
            this.panel1.Controls.Add(this.imgTopMaterial);
            this.panel1.Controls.Add(this.imgMaterialsCombined);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(202, 435);
            this.panel1.TabIndex = 0;
            // 
            // lblPoolPicker
            // 
            this.lblPoolPicker.AutoSize = true;
            this.lblPoolPicker.Location = new System.Drawing.Point(3, 282);
            this.lblPoolPicker.Name = "lblPoolPicker";
            this.lblPoolPicker.Size = new System.Drawing.Size(103, 20);
            this.lblPoolPicker.TabIndex = 10;
            this.lblPoolPicker.Text = "Materials Pool";
            // 
            // ddlColorPool
            // 
            this.ddlColorPool.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlColorPool.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlColorPool.FormattingEnabled = true;
            this.ddlColorPool.Location = new System.Drawing.Point(3, 305);
            this.ddlColorPool.Name = "ddlColorPool";
            this.ddlColorPool.Size = new System.Drawing.Size(196, 28);
            this.ddlColorPool.TabIndex = 9;
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
            // tbxMaterialFilter
            // 
            this.tbxMaterialFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.tbxMaterialFilter.FormattingEnabled = true;
            this.tbxMaterialFilter.Location = new System.Drawing.Point(3, 230);
            this.tbxMaterialFilter.Name = "tbxMaterialFilter";
            this.tbxMaterialFilter.Size = new System.Drawing.Size(194, 28);
            this.tbxMaterialFilter.TabIndex = 7;
            this.tbxMaterialFilter.TextChanged += new System.EventHandler(this.tbxSearchFilter_TextChanged);
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
            this.customPanel1.Size = new System.Drawing.Size(580, 435);
            this.customPanel1.TabIndex = 1;
            // 
            // tcSearchOptions
            // 
            this.tcSearchOptions.Controls.Add(this.tabPage1);
            this.tcSearchOptions.Controls.Add(this.tabPage2);
            this.tcSearchOptions.Location = new System.Drawing.Point(3, 339);
            this.tcSearchOptions.Name = "tcSearchOptions";
            this.tcSearchOptions.SelectedIndex = 0;
            this.tcSearchOptions.Size = new System.Drawing.Size(199, 125);
            this.tcSearchOptions.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(191, 92);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(191, 92);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // MaterialPickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 459);
            this.Controls.Add(this.customPanel1);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "MaterialPickerForm";
            this.ShowInTaskbar = false;
            this.Text = "MaterialPickerForm";
            this.Load += new System.EventHandler(this.MaterialPickerForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tcSearchOptions.ResumeLayout(false);
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
        private System.Windows.Forms.ComboBox tbxMaterialFilter;
        private Controls.CustomPanel customPanel1;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.ComboBox ddlColorPool;
        private System.Windows.Forms.Label lblPoolPicker;
        private System.Windows.Forms.TabControl tcSearchOptions;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}