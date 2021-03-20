
namespace PixelStacker.UI
{
    partial class PreRenderOptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreRenderOptionsForm));
            this.ddlAlgorithm = new System.Windows.Forms.ComboBox();
            this.lblAlgorithm = new System.Windows.Forms.Label();
            this.lblColorCache = new System.Windows.Forms.Label();
            this.ddlColorCache = new System.Windows.Forms.ComboBox();
            this.ddlColorCount = new System.Windows.Forms.ComboBox();
            this.lblColorCount = new System.Windows.Forms.Label();
            this.ddlDither = new System.Windows.Forms.ComboBox();
            this.lblDither = new System.Windows.Forms.Label();
            this.ddlParallel = new System.Windows.Forms.ComboBox();
            this.lblParallel = new System.Windows.Forms.Label();
            this.btnEnablePreRender = new System.Windows.Forms.Button();
            this.tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.lblColorCacheSize = new System.Windows.Forms.Label();
            this.ddlMaxNormalizedColorCount = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // ddlAlgorithm
            // 
            this.ddlAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlAlgorithm.FormattingEnabled = true;
            this.ddlAlgorithm.Items.AddRange(new object[] {
            "HSL distinct selection",
            "Uniform quantization",
            "Popularity algorithm",
            "Median cut algorithm",
            "Octree quantization",
            "Wu\'s color quantizer",
            "NeuQuant quantizer",
            "Optimal palette"});
            this.ddlAlgorithm.Location = new System.Drawing.Point(135, 77);
            this.ddlAlgorithm.Margin = new System.Windows.Forms.Padding(4);
            this.ddlAlgorithm.Name = "ddlAlgorithm";
            this.ddlAlgorithm.Size = new System.Drawing.Size(263, 24);
            this.ddlAlgorithm.TabIndex = 0;
            this.ddlAlgorithm.SelectedValueChanged += new System.EventHandler(this.ddlAlgorithm_SelectedValueChanged);
            // 
            // lblAlgorithm
            // 
            this.lblAlgorithm.AutoSize = true;
            this.lblAlgorithm.Location = new System.Drawing.Point(17, 81);
            this.lblAlgorithm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAlgorithm.Name = "lblAlgorithm";
            this.lblAlgorithm.Size = new System.Drawing.Size(67, 17);
            this.lblAlgorithm.TabIndex = 1;
            this.lblAlgorithm.Text = "Algorithm";
            // 
            // lblColorCache
            // 
            this.lblColorCache.AutoSize = true;
            this.lblColorCache.Location = new System.Drawing.Point(17, 113);
            this.lblColorCache.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColorCache.Name = "lblColorCache";
            this.lblColorCache.Size = new System.Drawing.Size(85, 17);
            this.lblColorCache.TabIndex = 2;
            this.lblColorCache.Text = "Color Cache";
            // 
            // ddlColorCache
            // 
            this.ddlColorCache.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlColorCache.FormattingEnabled = true;
            this.ddlColorCache.Items.AddRange(new object[] {
            "Euclidean distance",
            "Locality-sensitive hash",
            "Octree search"});
            this.ddlColorCache.Location = new System.Drawing.Point(135, 111);
            this.ddlColorCache.Margin = new System.Windows.Forms.Padding(4);
            this.ddlColorCache.Name = "ddlColorCache";
            this.ddlColorCache.Size = new System.Drawing.Size(263, 24);
            this.ddlColorCache.TabIndex = 3;
            this.ddlColorCache.SelectedValueChanged += new System.EventHandler(this.ddlColorCache_SelectedValueChanged);
            // 
            // ddlColorCount
            // 
            this.ddlColorCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlColorCount.FormattingEnabled = true;
            this.ddlColorCount.Items.AddRange(new object[] {
            "2",
            "4",
            "8",
            "16",
            "32",
            "64",
            "128",
            "256"});
            this.ddlColorCount.Location = new System.Drawing.Point(135, 143);
            this.ddlColorCount.Margin = new System.Windows.Forms.Padding(4);
            this.ddlColorCount.Name = "ddlColorCount";
            this.ddlColorCount.Size = new System.Drawing.Size(263, 24);
            this.ddlColorCount.TabIndex = 5;
            this.ddlColorCount.SelectedValueChanged += new System.EventHandler(this.ddlColorCount_SelectedValueChanged);
            // 
            // lblColorCount
            // 
            this.lblColorCount.AutoSize = true;
            this.lblColorCount.Location = new System.Drawing.Point(17, 146);
            this.lblColorCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColorCount.Name = "lblColorCount";
            this.lblColorCount.Size = new System.Drawing.Size(82, 17);
            this.lblColorCount.TabIndex = 4;
            this.lblColorCount.Text = "Color Count";
            // 
            // ddlDither
            // 
            this.ddlDither.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDither.FormattingEnabled = true;
            this.ddlDither.Items.AddRange(new object[] {
            "No dithering",
            "--[ Ordered ]--",
            "Bayer dithering (4x4)",
            "Bayer dithering (8x8)",
            "Clustered dot (4x4)",
            "Dot halftoning (8x8)",
            "--[ Error diffusion ]--",
            "Fan dithering (7x3)",
            "Shiau dithering (5x3)",
            "Sierra dithering (5x3)",
            "Stucki dithering (5x5)",
            "Burkes dithering (5x3)",
            "Atkinson dithering (5x5)",
            "Two-row Sierra dithering (5x3)",
            "Floyd–Steinberg dithering (3x3)",
            "Jarvis-Judice-Ninke dithering (5x5)"});
            this.ddlDither.Location = new System.Drawing.Point(135, 207);
            this.ddlDither.Margin = new System.Windows.Forms.Padding(4);
            this.ddlDither.Name = "ddlDither";
            this.ddlDither.Size = new System.Drawing.Size(263, 24);
            this.ddlDither.TabIndex = 7;
            this.ddlDither.SelectedValueChanged += new System.EventHandler(this.ddlDither_SelectedValueChanged);
            // 
            // lblDither
            // 
            this.lblDither.AutoSize = true;
            this.lblDither.Location = new System.Drawing.Point(17, 214);
            this.lblDither.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDither.Name = "lblDither";
            this.lblDither.Size = new System.Drawing.Size(46, 17);
            this.lblDither.TabIndex = 6;
            this.lblDither.Text = "Dither";
            // 
            // ddlParallel
            // 
            this.ddlParallel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlParallel.FormattingEnabled = true;
            this.ddlParallel.Items.AddRange(new object[] {
            "1",
            "2",
            "4",
            "8",
            "16",
            "32",
            "64"});
            this.ddlParallel.Location = new System.Drawing.Point(135, 175);
            this.ddlParallel.Margin = new System.Windows.Forms.Padding(4);
            this.ddlParallel.Name = "ddlParallel";
            this.ddlParallel.Size = new System.Drawing.Size(263, 24);
            this.ddlParallel.TabIndex = 9;
            this.ddlParallel.SelectedValueChanged += new System.EventHandler(this.ddlParallel_SelectedValueChanged);
            // 
            // lblParallel
            // 
            this.lblParallel.AutoSize = true;
            this.lblParallel.Location = new System.Drawing.Point(17, 179);
            this.lblParallel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblParallel.Name = "lblParallel";
            this.lblParallel.Size = new System.Drawing.Size(55, 17);
            this.lblParallel.TabIndex = 10;
            this.lblParallel.Text = "Parallel";
            // 
            // btnEnablePreRender
            // 
            this.btnEnablePreRender.Location = new System.Drawing.Point(20, 9);
            this.btnEnablePreRender.Margin = new System.Windows.Forms.Padding(4);
            this.btnEnablePreRender.Name = "btnEnablePreRender";
            this.btnEnablePreRender.Size = new System.Drawing.Size(378, 28);
            this.btnEnablePreRender.TabIndex = 11;
            this.btnEnablePreRender.Text = "Enable Quantizer";
            this.btnEnablePreRender.UseVisualStyleBackColor = true;
            this.btnEnablePreRender.Click += new System.EventHandler(this.btnEnablePreRender_Click);
            // 
            // tooltip
            // 
            this.tooltip.AutoPopDelay = 15000;
            this.tooltip.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tooltip.InitialDelay = 500;
            this.tooltip.IsBalloon = true;
            this.tooltip.ReshowDelay = 100;
            // 
            // lblColorCacheSize
            // 
            this.lblColorCacheSize.AutoSize = true;
            this.lblColorCacheSize.Location = new System.Drawing.Point(17, 49);
            this.lblColorCacheSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColorCacheSize.Name = "lblColorCacheSize";
            this.lblColorCacheSize.Size = new System.Drawing.Size(116, 17);
            this.lblColorCacheSize.TabIndex = 13;
            this.lblColorCacheSize.Text = "Color Cache Size";
            // 
            // ddlMaxNormalizedColorCount
            // 
            this.ddlMaxNormalizedColorCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlMaxNormalizedColorCount.FormattingEnabled = true;
            this.ddlMaxNormalizedColorCount.Items.AddRange(new object[] {
            "255^3",
            "51^3",
            "17^3",
            "15^3",
            "5^3"});
            this.ddlMaxNormalizedColorCount.Location = new System.Drawing.Point(160, 45);
            this.ddlMaxNormalizedColorCount.Margin = new System.Windows.Forms.Padding(4);
            this.ddlMaxNormalizedColorCount.Name = "ddlMaxNormalizedColorCount";
            this.ddlMaxNormalizedColorCount.Size = new System.Drawing.Size(238, 24);
            this.ddlMaxNormalizedColorCount.TabIndex = 12;
            this.ddlMaxNormalizedColorCount.SelectedValueChanged += new System.EventHandler(this.ddlMaxNormalizedColorCount_SelectedValueChanged);
            // 
            // PreRenderOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(411, 252);
            this.Controls.Add(this.lblColorCacheSize);
            this.Controls.Add(this.ddlMaxNormalizedColorCount);
            this.Controls.Add(this.btnEnablePreRender);
            this.Controls.Add(this.lblParallel);
            this.Controls.Add(this.ddlParallel);
            this.Controls.Add(this.ddlDither);
            this.Controls.Add(this.lblDither);
            this.Controls.Add(this.ddlColorCount);
            this.Controls.Add(this.lblColorCount);
            this.Controls.Add(this.ddlColorCache);
            this.Controls.Add(this.lblColorCache);
            this.Controls.Add(this.lblAlgorithm);
            this.Controls.Add(this.ddlAlgorithm);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PreRenderOptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Quantizer Settings";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblAlgorithm;
        private System.Windows.Forms.Label lblColorCache;
        private System.Windows.Forms.Label lblColorCount;
        private System.Windows.Forms.Label lblDither;
        private System.Windows.Forms.Label lblParallel;
        private System.Windows.Forms.Button btnEnablePreRender;
        internal System.Windows.Forms.ComboBox ddlAlgorithm;
        internal System.Windows.Forms.ComboBox ddlColorCache;
        internal System.Windows.Forms.ComboBox ddlColorCount;
        internal System.Windows.Forms.ComboBox ddlParallel;
        internal System.Windows.Forms.ComboBox ddlDither;
        private System.Windows.Forms.ToolTip tooltip;
        private System.Windows.Forms.Label lblColorCacheSize;
        internal System.Windows.Forms.ComboBox ddlMaxNormalizedColorCount;
    }
}