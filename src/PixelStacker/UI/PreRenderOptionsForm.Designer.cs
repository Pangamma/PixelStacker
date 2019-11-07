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
            this.lblColorCache = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ddlColorCache = new System.Windows.Forms.ComboBox();
            this.ddlColorCount = new System.Windows.Forms.ComboBox();
            this.lblColorCount = new System.Windows.Forms.Label();
            this.ddlDither = new System.Windows.Forms.ComboBox();
            this.lblDither = new System.Windows.Forms.Label();
            this.ddlParallel = new System.Windows.Forms.ComboBox();
            this.lblParallel = new System.Windows.Forms.Label();
            this.btnEnablePreRender = new System.Windows.Forms.Button();
            this.tooltip = new System.Windows.Forms.ToolTip(this.components);
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
            this.ddlAlgorithm.Location = new System.Drawing.Point(86, 36);
            this.ddlAlgorithm.Name = "ddlAlgorithm";
            this.ddlAlgorithm.Size = new System.Drawing.Size(198, 21);
            this.ddlAlgorithm.TabIndex = 0;
            this.tooltip.SetToolTip(this.ddlAlgorithm, "Algorithm determines HOW the \r\nimage colors will be reduced.");
            this.ddlAlgorithm.SelectedValueChanged += new System.EventHandler(this.ddlAlgorithm_SelectedValueChanged);
            // 
            // lblColorCache
            // 
            this.lblColorCache.AutoSize = true;
            this.lblColorCache.Location = new System.Drawing.Point(12, 39);
            this.lblColorCache.Name = "lblColorCache";
            this.lblColorCache.Size = new System.Drawing.Size(50, 13);
            this.lblColorCache.TabIndex = 1;
            this.lblColorCache.Text = "Algorithm";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Color Cache";
            // 
            // ddlColorCache
            // 
            this.ddlColorCache.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlColorCache.FormattingEnabled = true;
            this.ddlColorCache.Items.AddRange(new object[] {
            "Euclidean distance",
            "Locality-sensitive hash",
            "Octree search"});
            this.ddlColorCache.Location = new System.Drawing.Point(86, 62);
            this.ddlColorCache.Name = "ddlColorCache";
            this.ddlColorCache.Size = new System.Drawing.Size(198, 21);
            this.ddlColorCache.TabIndex = 3;
            this.tooltip.SetToolTip(this.ddlColorCache, "Changes how color differences are stored.");
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
            this.ddlColorCount.Location = new System.Drawing.Point(86, 89);
            this.ddlColorCount.Name = "ddlColorCount";
            this.ddlColorCount.Size = new System.Drawing.Size(198, 21);
            this.ddlColorCount.TabIndex = 5;
            this.tooltip.SetToolTip(this.ddlColorCount, "Decide how many different colors can appear\r\n in the rendered image.");
            this.ddlColorCount.SelectedValueChanged += new System.EventHandler(this.ddlColorCount_SelectedValueChanged);
            // 
            // lblColorCount
            // 
            this.lblColorCount.AutoSize = true;
            this.lblColorCount.Location = new System.Drawing.Point(12, 92);
            this.lblColorCount.Name = "lblColorCount";
            this.lblColorCount.Size = new System.Drawing.Size(62, 13);
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
            this.ddlDither.Location = new System.Drawing.Point(86, 144);
            this.ddlDither.Name = "ddlDither";
            this.ddlDither.Size = new System.Drawing.Size(198, 21);
            this.ddlDither.TabIndex = 7;
            this.tooltip.SetToolTip(this.ddlDither, "For if you\'re into that kind of thing.");
            this.ddlDither.SelectedValueChanged += new System.EventHandler(this.ddlDither_SelectedValueChanged);
            // 
            // lblDither
            // 
            this.lblDither.AutoSize = true;
            this.lblDither.Location = new System.Drawing.Point(12, 147);
            this.lblDither.Name = "lblDither";
            this.lblDither.Size = new System.Drawing.Size(35, 13);
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
            this.ddlParallel.Location = new System.Drawing.Point(86, 116);
            this.ddlParallel.Name = "ddlParallel";
            this.ddlParallel.Size = new System.Drawing.Size(198, 21);
            this.ddlParallel.TabIndex = 9;
            this.tooltip.SetToolTip(this.ddlParallel, "Some quantizing algorithms can improve \r\nefficiency by using multithreading.");
            this.ddlParallel.SelectedValueChanged += new System.EventHandler(this.ddlParallel_SelectedValueChanged);
            // 
            // lblParallel
            // 
            this.lblParallel.AutoSize = true;
            this.lblParallel.Location = new System.Drawing.Point(12, 119);
            this.lblParallel.Name = "lblParallel";
            this.lblParallel.Size = new System.Drawing.Size(41, 13);
            this.lblParallel.TabIndex = 10;
            this.lblParallel.Text = "Parallel";
            // 
            // btnEnablePreRender
            // 
            this.btnEnablePreRender.Location = new System.Drawing.Point(15, 7);
            this.btnEnablePreRender.Name = "btnEnablePreRender";
            this.btnEnablePreRender.Size = new System.Drawing.Size(269, 23);
            this.btnEnablePreRender.TabIndex = 11;
            this.btnEnablePreRender.Text = "Enable Quantizer";
            this.tooltip.SetToolTip(this.btnEnablePreRender, "The quantizer reduces the color count for \r\nfaster rendering. It can also be used" +
        " to add\r\ndithering effects. If the quantizer is disabled,\r\nrendering will be slo" +
        "wer but also higher quality.");
            this.btnEnablePreRender.UseVisualStyleBackColor = true;
            this.btnEnablePreRender.Click += new System.EventHandler(this.btnEnablePreRender_Click);
            // 
            // tooltip
            // 
            this.tooltip.AutomaticDelay = 1000;
            this.tooltip.AutoPopDelay = 20000;
            this.tooltip.InitialDelay = 1000;
            this.tooltip.ReshowDelay = 200;
            // 
            // PreRenderOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 180);
            this.Controls.Add(this.btnEnablePreRender);
            this.Controls.Add(this.lblParallel);
            this.Controls.Add(this.ddlParallel);
            this.Controls.Add(this.ddlDither);
            this.Controls.Add(this.lblDither);
            this.Controls.Add(this.ddlColorCount);
            this.Controls.Add(this.lblColorCount);
            this.Controls.Add(this.ddlColorCache);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblColorCache);
            this.Controls.Add(this.ddlAlgorithm);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PreRenderOptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Quantizer Settings";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblColorCache;
        private System.Windows.Forms.Label label1;
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
    }
}