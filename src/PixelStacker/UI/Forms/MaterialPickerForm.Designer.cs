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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblHtmlCode = new System.Windows.Forms.Label();
            this.tbxHtmlColorCode = new System.Windows.Forms.TextBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.tbxMaterialFilter = new System.Windows.Forms.ComboBox();
            this.lblBottomMaterial = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lblTopMaterial = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.imgBottomMaterial = new PixelStacker.UI.Controls.ImageButton();
            this.imgTopMaterial = new PixelStacker.UI.Controls.ImageButton();
            this.imgMaterialsCombined = new PixelStacker.UI.Controls.ImageButton();
            this.tcMaterials = new System.Windows.Forms.TabControl();
            this.tabTop = new System.Windows.Forms.TabPage();
            this.pnlTopMats = new PixelStacker.UI.Controls.Pickers.ImageButtonPanel();
            this.tabBottom = new System.Windows.Forms.TabPage();
            this.pnlBottomMats = new PixelStacker.UI.Controls.Pickers.ImageButtonPanel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ttTop = new System.Windows.Forms.ToolTip(this.components);
            this.ttBottom = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tcMaterials.SuspendLayout();
            this.tabTop.SuspendLayout();
            this.tabBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.Controls.Add(this.lblHtmlCode);
            this.panel1.Controls.Add(this.tbxHtmlColorCode);
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
            this.panel1.Size = new System.Drawing.Size(202, 497);
            this.panel1.TabIndex = 0;
            // 
            // lblHtmlCode
            // 
            this.lblHtmlCode.AutoSize = true;
            this.lblHtmlCode.Location = new System.Drawing.Point(4, 263);
            this.lblHtmlCode.Name = "lblHtmlCode";
            this.lblHtmlCode.Size = new System.Drawing.Size(127, 20);
            this.lblHtmlCode.TabIndex = 10;
            this.lblHtmlCode.Text = "HTML Color Code";
            // 
            // tbxHtmlColorCode
            // 
            this.tbxHtmlColorCode.Location = new System.Drawing.Point(3, 286);
            this.tbxHtmlColorCode.Name = "tbxHtmlColorCode";
            this.tbxHtmlColorCode.ReadOnly = true;
            this.tbxHtmlColorCode.Size = new System.Drawing.Size(194, 27);
            this.tbxHtmlColorCode.TabIndex = 9;
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(3, 198);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(42, 20);
            this.lblFilter.TabIndex = 8;
            this.lblFilter.Text = "Filter";
            // 
            // tbxMaterialFilter
            // 
            this.tbxMaterialFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.tbxMaterialFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbxMaterialFilter.FormattingEnabled = true;
            this.tbxMaterialFilter.Location = new System.Drawing.Point(3, 221);
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
            this.imgTopMaterial.IsChecked = true;
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
            this.imgMaterialsCombined.IsChecked = false;
            this.imgMaterialsCombined.Location = new System.Drawing.Point(3, 3);
            this.imgMaterialsCombined.Name = "imgMaterialsCombined";
            this.imgMaterialsCombined.PushState = PixelStacker.UI.Controls.ImageButtonPushState.Normal;
            this.imgMaterialsCombined.Size = new System.Drawing.Size(128, 128);
            this.imgMaterialsCombined.TabIndex = 0;
            // 
            // tcMaterials
            // 
            this.tcMaterials.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcMaterials.Controls.Add(this.tabTop);
            this.tcMaterials.Controls.Add(this.tabBottom);
            this.tcMaterials.Location = new System.Drawing.Point(217, 12);
            this.tcMaterials.Name = "tcMaterials";
            this.tcMaterials.SelectedIndex = 0;
            this.tcMaterials.Size = new System.Drawing.Size(386, 497);
            this.tcMaterials.TabIndex = 2;
            this.tcMaterials.SelectedIndexChanged += new System.EventHandler(this.TabControlMaterials_SelectedIndexChanged);
            // 
            // tabTop
            // 
            this.tabTop.Controls.Add(this.pnlTopMats);
            this.tabTop.Location = new System.Drawing.Point(4, 29);
            this.tabTop.Name = "tabTop";
            this.tabTop.Padding = new System.Windows.Forms.Padding(3);
            this.tabTop.Size = new System.Drawing.Size(378, 464);
            this.tabTop.TabIndex = 0;
            this.tabTop.Text = "Top";
            this.tabTop.UseVisualStyleBackColor = true;
            // 
            // pnlTopMats
            // 
            this.pnlTopMats.AutoScroll = true;
            this.pnlTopMats.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pnlTopMats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTopMats.Location = new System.Drawing.Point(3, 3);
            this.pnlTopMats.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTopMats.Name = "pnlTopMats";
            this.pnlTopMats.OnCommandKey = null;
            this.pnlTopMats.Size = new System.Drawing.Size(372, 458);
            this.pnlTopMats.TabIndex = 0;
            this.pnlTopMats.TileClicked += new System.EventHandler<PixelStacker.UI.Controls.Pickers.ImageButtonClickEventArgs>(this.pnlTopMats_TileClicked);
            // 
            // tabBottom
            // 
            this.tabBottom.Controls.Add(this.pnlBottomMats);
            this.tabBottom.Location = new System.Drawing.Point(4, 29);
            this.tabBottom.Name = "tabBottom";
            this.tabBottom.Padding = new System.Windows.Forms.Padding(3);
            this.tabBottom.Size = new System.Drawing.Size(378, 464);
            this.tabBottom.TabIndex = 1;
            this.tabBottom.Text = "Bottom";
            this.tabBottom.UseVisualStyleBackColor = true;
            // 
            // pnlBottomMats
            // 
            this.pnlBottomMats.AutoScroll = true;
            this.pnlBottomMats.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pnlBottomMats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottomMats.Location = new System.Drawing.Point(3, 3);
            this.pnlBottomMats.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBottomMats.Name = "pnlBottomMats";
            this.pnlBottomMats.OnCommandKey = null;
            this.pnlBottomMats.Size = new System.Drawing.Size(372, 458);
            this.pnlBottomMats.TabIndex = 0;
            this.pnlBottomMats.TileClicked += new System.EventHandler<PixelStacker.UI.Controls.Pickers.ImageButtonClickEventArgs>(this.pnlBottomMats_TileClicked);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 200;
            this.toolTip1.ReshowDelay = 100;
            // 
            // ttTop
            // 
            this.ttTop.AutoPopDelay = 5000;
            this.ttTop.InitialDelay = 200;
            this.ttTop.ReshowDelay = 100;
            // 
            // ttBottom
            // 
            this.ttBottom.AutoPopDelay = 5000;
            this.ttBottom.InitialDelay = 200;
            this.ttBottom.ReshowDelay = 100;
            // 
            // MaterialPickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 521);
            this.Controls.Add(this.tcMaterials);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "MaterialPickerForm";
            this.ShowInTaskbar = false;
            this.Text = "Material Picker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MaterialPickerForm_FormClosing);
            this.ResizeBegin += new System.EventHandler(this.MaterialPickerForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.MaterialPickerForm_ResizeEnd);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tcMaterials.ResumeLayout(false);
            this.tabTop.ResumeLayout(false);
            this.tabBottom.ResumeLayout(false);
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
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.TabControl tcMaterials;
        private System.Windows.Forms.TabPage tabTop;
        private System.Windows.Forms.TabPage tabBottom;
        private Controls.Pickers.ImageButtonPanel pnlTopMats;
        private Controls.Pickers.ImageButtonPanel pnlBottomMats;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip ttTop;
        private System.Windows.Forms.ToolTip ttBottom;
        private System.Windows.Forms.Label lblHtmlCode;
        private System.Windows.Forms.TextBox tbxHtmlColorCode;
    }
}