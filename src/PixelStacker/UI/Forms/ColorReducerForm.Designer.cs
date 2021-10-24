namespace PixelStacker.UI.Forms
{
    partial class ColorReducerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorReducerForm));
            this.lblInstructions = new System.Windows.Forms.Label();
            this.ddlAlgorithm = new System.Windows.Forms.ComboBox();
            this.ddlRgbBucketSize = new System.Windows.Forms.ComboBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ddlParallel = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblRgbBucketSize = new System.Windows.Forms.LinkLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbxEnableQuantizer = new System.Windows.Forms.CheckBox();
            this.lblAlgorithm = new System.Windows.Forms.LinkLabel();
            this.lblColorCache = new System.Windows.Forms.LinkLabel();
            this.ddlColorCache = new System.Windows.Forms.ComboBox();
            this.lblColorCount = new System.Windows.Forms.LinkLabel();
            this.ddlColorCount = new System.Windows.Forms.ComboBox();
            this.lblParallel = new System.Windows.Forms.LinkLabel();
            this.lblDither = new System.Windows.Forms.LinkLabel();
            this.ddlDither = new System.Windows.Forms.ComboBox();
            this.lblInstructionsTitle = new System.Windows.Forms.LinkLabel();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblInstructions
            // 
            this.lblInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInstructions.Location = new System.Drawing.Point(3, 9);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(467, 425);
            this.lblInstructions.TabIndex = 0;
            this.lblInstructions.Text = resources.GetString("lblInstructions.Text");
            // 
            // ddlAlgorithm
            // 
            this.ddlAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlAlgorithm.FormattingEnabled = true;
            this.ddlAlgorithm.Items.AddRange(new object[] {
            "A",
            "V",
            "C",
            "D",
            "E",
            "G",
            "SADSF",
            "SDDF"});
            this.ddlAlgorithm.Location = new System.Drawing.Point(3, 155);
            this.ddlAlgorithm.Name = "ddlAlgorithm";
            this.ddlAlgorithm.Size = new System.Drawing.Size(186, 28);
            this.ddlAlgorithm.TabIndex = 3;
            this.ddlAlgorithm.SelectedValueChanged += new System.EventHandler(this.ddlAlgorithm_SelectedValueChanged);
            // 
            // ddlRgbBucketSize
            // 
            this.ddlRgbBucketSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlRgbBucketSize.Location = new System.Drawing.Point(3, 33);
            this.ddlRgbBucketSize.Name = "ddlRgbBucketSize";
            this.ddlRgbBucketSize.Size = new System.Drawing.Size(186, 28);
            this.ddlRgbBucketSize.TabIndex = 4;
            this.ddlRgbBucketSize.SelectedValueChanged += new System.EventHandler(this.ddlRgbBucketSize_SelectedValueChanged);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 30000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.IsBalloon = true;
            this.toolTip.ReshowDelay = 100;
            // 
            // ddlParallel
            // 
            this.ddlParallel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlParallel.FormattingEnabled = true;
            this.ddlParallel.Location = new System.Drawing.Point(3, 362);
            this.ddlParallel.Name = "ddlParallel";
            this.ddlParallel.Size = new System.Drawing.Size(186, 28);
            this.ddlParallel.TabIndex = 7;
            this.ddlParallel.SelectedValueChanged += new System.EventHandler(this.ddlParallel_SelectedValueChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblInstructions);
            this.panel1.Location = new System.Drawing.Point(229, 32);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(475, 461);
            this.panel1.TabIndex = 9;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.lblRgbBucketSize);
            this.flowLayoutPanel1.Controls.Add(this.ddlRgbBucketSize);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Controls.Add(this.cbxEnableQuantizer);
            this.flowLayoutPanel1.Controls.Add(this.lblAlgorithm);
            this.flowLayoutPanel1.Controls.Add(this.ddlAlgorithm);
            this.flowLayoutPanel1.Controls.Add(this.lblColorCache);
            this.flowLayoutPanel1.Controls.Add(this.ddlColorCache);
            this.flowLayoutPanel1.Controls.Add(this.lblColorCount);
            this.flowLayoutPanel1.Controls.Add(this.ddlColorCount);
            this.flowLayoutPanel1.Controls.Add(this.lblParallel);
            this.flowLayoutPanel1.Controls.Add(this.ddlParallel);
            this.flowLayoutPanel1.Controls.Add(this.lblDither);
            this.flowLayoutPanel1.Controls.Add(this.ddlDither);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 9);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(211, 484);
            this.flowLayoutPanel1.TabIndex = 11;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // lblRgbBucketSize
            // 
            this.lblRgbBucketSize.AutoSize = true;
            this.lblRgbBucketSize.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblRgbBucketSize.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lblRgbBucketSize.LinkColor = System.Drawing.SystemColors.ControlText;
            this.lblRgbBucketSize.Location = new System.Drawing.Point(3, 10);
            this.lblRgbBucketSize.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblRgbBucketSize.Name = "lblRgbBucketSize";
            this.lblRgbBucketSize.Size = new System.Drawing.Size(137, 20);
            this.lblRgbBucketSize.TabIndex = 15;
            this.lblRgbBucketSize.TabStop = true;
            this.lblRgbBucketSize.Text = "Max unique colors";
            this.lblRgbBucketSize.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblRgbBucketSize_LinkClicked);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panel2.Location = new System.Drawing.Point(3, 67);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(186, 10);
            this.panel2.TabIndex = 16;
            // 
            // cbxEnableQuantizer
            // 
            this.cbxEnableQuantizer.AutoSize = true;
            this.cbxEnableQuantizer.Location = new System.Drawing.Point(3, 95);
            this.cbxEnableQuantizer.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.cbxEnableQuantizer.Name = "cbxEnableQuantizer";
            this.cbxEnableQuantizer.Size = new System.Drawing.Size(144, 24);
            this.cbxEnableQuantizer.TabIndex = 11;
            this.cbxEnableQuantizer.Text = "Enable Quantizer";
            this.cbxEnableQuantizer.UseVisualStyleBackColor = true;
            this.cbxEnableQuantizer.CheckedChanged += new System.EventHandler(this.cbxEnableQuantizer_CheckedChanged);
            // 
            // lblAlgorithm
            // 
            this.lblAlgorithm.AutoSize = true;
            this.lblAlgorithm.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblAlgorithm.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lblAlgorithm.LinkColor = System.Drawing.SystemColors.ControlText;
            this.lblAlgorithm.Location = new System.Drawing.Point(3, 132);
            this.lblAlgorithm.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblAlgorithm.Name = "lblAlgorithm";
            this.lblAlgorithm.Size = new System.Drawing.Size(153, 20);
            this.lblAlgorithm.TabIndex = 14;
            this.lblAlgorithm.TabStop = true;
            this.lblAlgorithm.Text = "Quantizer Algorithm";
            this.lblAlgorithm.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAlgorithm_LinkClicked);
            // 
            // lblColorCache
            // 
            this.lblColorCache.AutoSize = true;
            this.lblColorCache.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblColorCache.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lblColorCache.LinkColor = System.Drawing.SystemColors.ControlText;
            this.lblColorCache.Location = new System.Drawing.Point(3, 201);
            this.lblColorCache.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.lblColorCache.Name = "lblColorCache";
            this.lblColorCache.Size = new System.Drawing.Size(91, 20);
            this.lblColorCache.TabIndex = 7;
            this.lblColorCache.TabStop = true;
            this.lblColorCache.Text = "Color Cache";
            this.lblColorCache.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblColorCache_LinkClicked);
            // 
            // ddlColorCache
            // 
            this.ddlColorCache.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlColorCache.FormattingEnabled = true;
            this.ddlColorCache.Location = new System.Drawing.Point(3, 224);
            this.ddlColorCache.Name = "ddlColorCache";
            this.ddlColorCache.Size = new System.Drawing.Size(186, 28);
            this.ddlColorCache.TabIndex = 13;
            this.ddlColorCache.SelectedValueChanged += new System.EventHandler(this.ddlColorCache_SelectedValueChanged);
            // 
            // lblColorCount
            // 
            this.lblColorCount.AutoSize = true;
            this.lblColorCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblColorCount.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lblColorCount.LinkColor = System.Drawing.SystemColors.ControlText;
            this.lblColorCount.Location = new System.Drawing.Point(3, 270);
            this.lblColorCount.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.lblColorCount.Name = "lblColorCount";
            this.lblColorCount.Size = new System.Drawing.Size(164, 20);
            this.lblColorCount.TabIndex = 8;
            this.lblColorCount.TabStop = true;
            this.lblColorCount.Text = "Quantizer Color Count";
            this.lblColorCount.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblColorCount_LinkClicked);
            // 
            // ddlColorCount
            // 
            this.ddlColorCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlColorCount.FormattingEnabled = true;
            this.ddlColorCount.Location = new System.Drawing.Point(3, 293);
            this.ddlColorCount.Name = "ddlColorCount";
            this.ddlColorCount.Size = new System.Drawing.Size(186, 28);
            this.ddlColorCount.TabIndex = 13;
            this.ddlColorCount.SelectedValueChanged += new System.EventHandler(this.ddlColorCount_SelectedValueChanged);
            // 
            // lblParallel
            // 
            this.lblParallel.AutoSize = true;
            this.lblParallel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblParallel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lblParallel.LinkColor = System.Drawing.SystemColors.ControlText;
            this.lblParallel.Location = new System.Drawing.Point(3, 339);
            this.lblParallel.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.lblParallel.Name = "lblParallel";
            this.lblParallel.Size = new System.Drawing.Size(60, 20);
            this.lblParallel.TabIndex = 9;
            this.lblParallel.TabStop = true;
            this.lblParallel.Text = "Parallel";
            this.lblParallel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblParallel_LinkClicked);
            // 
            // lblDither
            // 
            this.lblDither.AutoSize = true;
            this.lblDither.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblDither.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lblDither.LinkColor = System.Drawing.SystemColors.ControlText;
            this.lblDither.Location = new System.Drawing.Point(3, 408);
            this.lblDither.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.lblDither.Name = "lblDither";
            this.lblDither.Size = new System.Drawing.Size(53, 20);
            this.lblDither.TabIndex = 10;
            this.lblDither.TabStop = true;
            this.lblDither.Text = "Dither";
            this.lblDither.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblDither_LinkClicked);
            // 
            // ddlDither
            // 
            this.ddlDither.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDither.FormattingEnabled = true;
            this.ddlDither.Location = new System.Drawing.Point(3, 431);
            this.ddlDither.Name = "ddlDither";
            this.ddlDither.Size = new System.Drawing.Size(186, 28);
            this.ddlDither.TabIndex = 12;
            this.ddlDither.SelectedValueChanged += new System.EventHandler(this.ddlDither_SelectedValueChanged);
            // 
            // lblInstructionsTitle
            // 
            this.lblInstructionsTitle.AutoSize = true;
            this.lblInstructionsTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblInstructionsTitle.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lblInstructionsTitle.LinkColor = System.Drawing.SystemColors.ControlText;
            this.lblInstructionsTitle.Location = new System.Drawing.Point(229, 9);
            this.lblInstructionsTitle.Name = "lblInstructionsTitle";
            this.lblInstructionsTitle.Size = new System.Drawing.Size(93, 20);
            this.lblInstructionsTitle.TabIndex = 12;
            this.lblInstructionsTitle.TabStop = true;
            this.lblInstructionsTitle.Text = "Instructions";
            this.lblInstructionsTitle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblInstructionsTitle_LinkClicked);
            // 
            // ColorReducerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 505);
            this.Controls.Add(this.lblInstructionsTitle);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "ColorReducerForm";
            this.Text = "Color Reducer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ColorReducerForm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.ComboBox ddlAlgorithm;
        private System.Windows.Forms.ComboBox ddlRgbBucketSize;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ComboBox ddlParallel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.LinkLabel lblColorCache;
        private System.Windows.Forms.LinkLabel lblColorCount;
        private System.Windows.Forms.LinkLabel lblParallel;
        private System.Windows.Forms.LinkLabel lblDither;
        private System.Windows.Forms.CheckBox cbxEnableQuantizer;
        private System.Windows.Forms.ComboBox ddlDither;
        private System.Windows.Forms.ComboBox ddlColorCache;
        private System.Windows.Forms.ComboBox ddlColorCount;
        private System.Windows.Forms.LinkLabel lblAlgorithm;
        private System.Windows.Forms.LinkLabel lblRgbBucketSize;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.LinkLabel lblInstructionsTitle;
    }
}