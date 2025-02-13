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
            components = new System.ComponentModel.Container();
            panel1 = new System.Windows.Forms.Panel();
            lblHtmlCode = new System.Windows.Forms.Label();
            tbxHtmlColorCode = new System.Windows.Forms.TextBox();
            lblFilter = new System.Windows.Forms.Label();
            tbxMaterialFilter = new System.Windows.Forms.ComboBox();
            lblBottomMaterial = new System.Windows.Forms.Label();
            pictureBox2 = new System.Windows.Forms.PictureBox();
            lblTopMaterial = new System.Windows.Forms.Label();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            imgBottomMaterial = new Controls.ImageButton();
            imgTopMaterial = new Controls.ImageButton();
            imgMaterialsCombined = new Controls.ImageButton();
            tcMaterials = new System.Windows.Forms.TabControl();
            tabTop = new System.Windows.Forms.TabPage();
            pnlTopMats = new Controls.Pickers.ImageButtonPanel();
            tabBottom = new System.Windows.Forms.TabPage();
            pnlBottomMats = new Controls.Pickers.ImageButtonPanel();
            tabSimilarCombinations = new System.Windows.Forms.TabPage();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            ttTop = new System.Windows.Forms.ToolTip(components);
            ttBottom = new System.Windows.Forms.ToolTip(components);
            pnlSimilarCombinations = new Controls.Pickers.ImageButtonPanel();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tcMaterials.SuspendLayout();
            tabTop.SuspendLayout();
            tabBottom.SuspendLayout();
            tabSimilarCombinations.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            panel1.Controls.Add(lblHtmlCode);
            panel1.Controls.Add(tbxHtmlColorCode);
            panel1.Controls.Add(lblFilter);
            panel1.Controls.Add(tbxMaterialFilter);
            panel1.Controls.Add(lblBottomMaterial);
            panel1.Controls.Add(pictureBox2);
            panel1.Controls.Add(lblTopMaterial);
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(imgBottomMaterial);
            panel1.Controls.Add(imgTopMaterial);
            panel1.Controls.Add(imgMaterialsCombined);
            panel1.Location = new System.Drawing.Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(202, 497);
            panel1.TabIndex = 0;
            // 
            // lblHtmlCode
            // 
            lblHtmlCode.AutoSize = true;
            lblHtmlCode.Location = new System.Drawing.Point(4, 263);
            lblHtmlCode.Name = "lblHtmlCode";
            lblHtmlCode.Size = new System.Drawing.Size(127, 20);
            lblHtmlCode.TabIndex = 10;
            lblHtmlCode.Text = "HTML Color Code";
            // 
            // tbxHtmlColorCode
            // 
            tbxHtmlColorCode.Location = new System.Drawing.Point(3, 286);
            tbxHtmlColorCode.Name = "tbxHtmlColorCode";
            tbxHtmlColorCode.ReadOnly = true;
            tbxHtmlColorCode.Size = new System.Drawing.Size(194, 27);
            tbxHtmlColorCode.TabIndex = 9;
            // 
            // lblFilter
            // 
            lblFilter.AutoSize = true;
            lblFilter.Location = new System.Drawing.Point(3, 198);
            lblFilter.Name = "lblFilter";
            lblFilter.Size = new System.Drawing.Size(42, 20);
            lblFilter.TabIndex = 8;
            lblFilter.Text = "Filter";
            // 
            // tbxMaterialFilter
            // 
            tbxMaterialFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            tbxMaterialFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            tbxMaterialFilter.FormattingEnabled = true;
            tbxMaterialFilter.Location = new System.Drawing.Point(3, 221);
            tbxMaterialFilter.Name = "tbxMaterialFilter";
            tbxMaterialFilter.Size = new System.Drawing.Size(194, 28);
            tbxMaterialFilter.TabIndex = 7;
            tbxMaterialFilter.TextChanged += tbxSearchFilter_TextChanged;
            // 
            // lblBottomMaterial
            // 
            lblBottomMaterial.Location = new System.Drawing.Point(33, 166);
            lblBottomMaterial.Name = "lblBottomMaterial";
            lblBottomMaterial.Size = new System.Drawing.Size(164, 25);
            lblBottomMaterial.TabIndex = 6;
            lblBottomMaterial.Text = "Bottom material";
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Resources.UIResources.layers_bottom;
            pictureBox2.Location = new System.Drawing.Point(3, 167);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new System.Drawing.Size(24, 24);
            pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 5;
            pictureBox2.TabStop = false;
            // 
            // lblTopMaterial
            // 
            lblTopMaterial.Location = new System.Drawing.Point(33, 137);
            lblTopMaterial.Name = "lblTopMaterial";
            lblTopMaterial.Size = new System.Drawing.Size(164, 25);
            lblTopMaterial.TabIndex = 4;
            lblTopMaterial.Text = "Top material";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Resources.UIResources.layers_top;
            pictureBox1.Location = new System.Drawing.Point(3, 137);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(24, 24);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // imgBottomMaterial
            // 
            imgBottomMaterial.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            imgBottomMaterial.IsChecked = false;
            imgBottomMaterial.Location = new System.Drawing.Point(137, 69);
            imgBottomMaterial.Name = "imgBottomMaterial";
            imgBottomMaterial.PushState = UI.Controls.ImageButtonPushState.Normal;
            imgBottomMaterial.Size = new System.Drawing.Size(60, 60);
            imgBottomMaterial.TabIndex = 2;
            imgBottomMaterial.Click += imgBottomMaterial_Click;
            // 
            // imgTopMaterial
            // 
            imgTopMaterial.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            imgTopMaterial.IsChecked = true;
            imgTopMaterial.Location = new System.Drawing.Point(137, 3);
            imgTopMaterial.Name = "imgTopMaterial";
            imgTopMaterial.PushState = UI.Controls.ImageButtonPushState.Normal;
            imgTopMaterial.Size = new System.Drawing.Size(60, 60);
            imgTopMaterial.TabIndex = 1;
            imgTopMaterial.Click += imgTopMaterial_Click;
            // 
            // imgMaterialsCombined
            // 
            imgMaterialsCombined.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            imgMaterialsCombined.IsChecked = false;
            imgMaterialsCombined.Location = new System.Drawing.Point(3, 3);
            imgMaterialsCombined.Name = "imgMaterialsCombined";
            imgMaterialsCombined.PushState = UI.Controls.ImageButtonPushState.Normal;
            imgMaterialsCombined.Size = new System.Drawing.Size(128, 128);
            imgMaterialsCombined.TabIndex = 0;
            // 
            // tcMaterials
            // 
            tcMaterials.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tcMaterials.Controls.Add(tabTop);
            tcMaterials.Controls.Add(tabBottom);
            tcMaterials.Controls.Add(tabSimilarCombinations);
            tcMaterials.Location = new System.Drawing.Point(217, 12);
            tcMaterials.Name = "tcMaterials";
            tcMaterials.SelectedIndex = 0;
            tcMaterials.Size = new System.Drawing.Size(386, 497);
            tcMaterials.TabIndex = 2;
            tcMaterials.SelectedIndexChanged += TabControlMaterials_SelectedIndexChanged;
            // 
            // tabTop
            // 
            tabTop.Controls.Add(pnlTopMats);
            tabTop.Location = new System.Drawing.Point(4, 29);
            tabTop.Name = "tabTop";
            tabTop.Padding = new System.Windows.Forms.Padding(3);
            tabTop.Size = new System.Drawing.Size(378, 464);
            tabTop.TabIndex = 0;
            tabTop.Text = "Top";
            tabTop.UseVisualStyleBackColor = true;
            // 
            // pnlTopMats
            // 
            pnlTopMats.AutoScroll = true;
            pnlTopMats.BackColor = System.Drawing.SystemColors.ControlDark;
            pnlTopMats.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlTopMats.Location = new System.Drawing.Point(3, 3);
            pnlTopMats.Margin = new System.Windows.Forms.Padding(0);
            pnlTopMats.Name = "pnlTopMats";
            pnlTopMats.OnCommandKey = null;
            pnlTopMats.Size = new System.Drawing.Size(372, 458);
            pnlTopMats.TabIndex = 0;
            pnlTopMats.TileClicked += pnlTopMats_TileClicked;
            // 
            // tabBottom
            // 
            tabBottom.Controls.Add(pnlBottomMats);
            tabBottom.Location = new System.Drawing.Point(4, 29);
            tabBottom.Name = "tabBottom";
            tabBottom.Padding = new System.Windows.Forms.Padding(3);
            tabBottom.Size = new System.Drawing.Size(378, 464);
            tabBottom.TabIndex = 1;
            tabBottom.Text = "Bottom";
            tabBottom.UseVisualStyleBackColor = true;
            // 
            // pnlBottomMats
            // 
            pnlBottomMats.AutoScroll = true;
            pnlBottomMats.BackColor = System.Drawing.SystemColors.ControlDark;
            pnlBottomMats.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlBottomMats.Location = new System.Drawing.Point(3, 3);
            pnlBottomMats.Margin = new System.Windows.Forms.Padding(0);
            pnlBottomMats.Name = "pnlBottomMats";
            pnlBottomMats.OnCommandKey = null;
            pnlBottomMats.Size = new System.Drawing.Size(372, 458);
            pnlBottomMats.TabIndex = 0;
            pnlBottomMats.TileClicked += pnlBottomMats_TileClicked;
            // 
            // tabSimilarCombinations
            // 
            tabSimilarCombinations.Controls.Add(pnlSimilarCombinations);
            tabSimilarCombinations.Location = new System.Drawing.Point(4, 29);
            tabSimilarCombinations.Name = "tabSimilarCombinations";
            tabSimilarCombinations.Padding = new System.Windows.Forms.Padding(3);
            tabSimilarCombinations.Size = new System.Drawing.Size(378, 464);
            tabSimilarCombinations.TabIndex = 2;
            tabSimilarCombinations.Text = "Similar Combinations";
            tabSimilarCombinations.UseVisualStyleBackColor = true;
            // 
            // pnlSimilarCombinations
            // 
            pnlSimilarCombinations.AutoScroll = true;
            pnlSimilarCombinations.BackColor = System.Drawing.SystemColors.ControlDark;
            pnlSimilarCombinations.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlSimilarCombinations.Location = new System.Drawing.Point(3, 3);
            pnlSimilarCombinations.Margin = new System.Windows.Forms.Padding(0);
            pnlSimilarCombinations.Name = "pnlSimilarCombinations";
            pnlSimilarCombinations.OnCommandKey = null;
            pnlSimilarCombinations.Size = new System.Drawing.Size(372, 458);
            pnlSimilarCombinations.TabIndex = 0;
            pnlSimilarCombinations.TileClicked += pnlSimilarCombinations_TileClicked;
            // 
            // toolTip1
            // 
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 200;
            toolTip1.ReshowDelay = 100;
            // 
            // ttTop
            // 
            ttTop.AutoPopDelay = 5000;
            ttTop.InitialDelay = 200;
            ttTop.ReshowDelay = 100;
            // 
            // ttBottom
            // 
            ttBottom.AutoPopDelay = 5000;
            ttBottom.InitialDelay = 200;
            ttBottom.ReshowDelay = 100;
            // 
            // MaterialPickerForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(606, 521);
            Controls.Add(tcMaterials);
            Controls.Add(panel1);
            DoubleBuffered = true;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            Name = "MaterialPickerForm";
            ShowInTaskbar = false;
            Text = "Material Picker";
            FormClosing += MaterialPickerForm_FormClosing;
            ResizeBegin += MaterialPickerForm_ResizeBegin;
            ResizeEnd += MaterialPickerForm_ResizeEnd;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tcMaterials.ResumeLayout(false);
            tabTop.ResumeLayout(false);
            tabBottom.ResumeLayout(false);
            tabSimilarCombinations.ResumeLayout(false);
            ResumeLayout(false);
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
        private System.Windows.Forms.TabPage tabSimilarCombinations;
        private Controls.Pickers.ImageButtonPanel pnlSimilarCombinations;
    }
}