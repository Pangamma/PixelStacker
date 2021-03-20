namespace PixelStacker.UI
{
    partial class OtherOptionsWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OtherOptionsWindow));
            this.nbrGridSize = new System.Windows.Forms.NumericUpDown();
            this.nbrMaxHeight = new System.Windows.Forms.NumericUpDown();
            this.nbrMaxWidth = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbxIsSideView = new System.Windows.Forms.CheckBox();
            this.btnFactoryReset = new System.Windows.Forms.Button();
            this.lblGridColor = new System.Windows.Forms.Label();
            this.colorDialogue = new System.Windows.Forms.ColorDialog();
            this.btnGridColor = new System.Windows.Forms.Button();
            this.cbxIsFrugalWithMaterials = new System.Windows.Forms.CheckBox();
            this.cbxSkipShadowRendering = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nbrGridSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbrMaxHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbrMaxWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // nbrGridSize
            // 
            resources.ApplyResources(this.nbrGridSize, "nbrGridSize");
            this.nbrGridSize.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.nbrGridSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nbrGridSize.Name = "nbrGridSize";
            this.nbrGridSize.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.nbrGridSize.ValueChanged += new System.EventHandler(this.nbrGridSize_ValueChanged);
            // 
            // nbrMaxHeight
            // 
            resources.ApplyResources(this.nbrMaxHeight, "nbrMaxHeight");
            this.nbrMaxHeight.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.nbrMaxHeight.Name = "nbrMaxHeight";
            this.nbrMaxHeight.ValueChanged += new System.EventHandler(this.nbrMaxHeight_ValueChanged);
            // 
            // nbrMaxWidth
            // 
            resources.ApplyResources(this.nbrMaxWidth, "nbrMaxWidth");
            this.nbrMaxWidth.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.nbrMaxWidth.Name = "nbrMaxWidth";
            this.nbrMaxWidth.ValueChanged += new System.EventHandler(this.nbrMaxWidth_ValueChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cbxIsSideView
            // 
            resources.ApplyResources(this.cbxIsSideView, "cbxIsSideView");
            this.cbxIsSideView.Name = "cbxIsSideView";
            this.cbxIsSideView.UseVisualStyleBackColor = true;
            this.cbxIsSideView.CheckedChanged += new System.EventHandler(this.cbxIsSideView_CheckedChanged);
            // 
            // btnFactoryReset
            // 
            resources.ApplyResources(this.btnFactoryReset, "btnFactoryReset");
            this.btnFactoryReset.Name = "btnFactoryReset";
            this.btnFactoryReset.UseVisualStyleBackColor = true;
            this.btnFactoryReset.Click += new System.EventHandler(this.btnFactoryReset_Click);
            // 
            // lblGridColor
            // 
            resources.ApplyResources(this.lblGridColor, "lblGridColor");
            this.lblGridColor.Name = "lblGridColor";
            // 
            // btnGridColor
            // 
            this.btnGridColor.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnGridColor.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btnGridColor, "btnGridColor");
            this.btnGridColor.Name = "btnGridColor";
            this.btnGridColor.UseVisualStyleBackColor = false;
            this.btnGridColor.Click += new System.EventHandler(this.btnGridColor_Click);
            // 
            // cbxIsFrugalWithMaterials
            // 
            resources.ApplyResources(this.cbxIsFrugalWithMaterials, "cbxIsFrugalWithMaterials");
            this.cbxIsFrugalWithMaterials.BackColor = System.Drawing.Color.PaleTurquoise;
            this.cbxIsFrugalWithMaterials.Name = "cbxIsFrugalWithMaterials";
            this.cbxIsFrugalWithMaterials.UseVisualStyleBackColor = false;
            this.cbxIsFrugalWithMaterials.CheckedChanged += new System.EventHandler(this.cbxIsFrugalWithMaterials_CheckedChanged);
            // 
            // cbxSkipShadowRendering
            // 
            resources.ApplyResources(this.cbxSkipShadowRendering, "cbxSkipShadowRendering");
            this.cbxSkipShadowRendering.Name = "cbxSkipShadowRendering";
            this.cbxSkipShadowRendering.UseVisualStyleBackColor = true;
            this.cbxSkipShadowRendering.CheckedChanged += new System.EventHandler(this.cbxSkipShadowRendering_CheckedChanged);
            // 
            // OtherOptionsWindow
            // 
            resources.ApplyResources(this, "$this");
            this.Text = Resources.Text.OtherOptions_Title;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbxSkipShadowRendering);
            this.Controls.Add(this.cbxIsFrugalWithMaterials);
            this.Controls.Add(this.btnGridColor);
            this.Controls.Add(this.lblGridColor);
            this.Controls.Add(this.btnFactoryReset);
            this.Controls.Add(this.cbxIsSideView);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nbrMaxWidth);
            this.Controls.Add(this.nbrMaxHeight);
            this.Controls.Add(this.nbrGridSize);
            this.Icon = global::PixelStacker.Resources.UIResources.wool;
            this.Name = "OtherOptionsWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OtherOptionsWindow_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.nbrGridSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbrMaxHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbrMaxWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nbrGridSize;
        private System.Windows.Forms.NumericUpDown nbrMaxHeight;
        private System.Windows.Forms.NumericUpDown nbrMaxWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbxIsSideView;
        private System.Windows.Forms.Button btnFactoryReset;
        private System.Windows.Forms.Label lblGridColor;
        private System.Windows.Forms.ColorDialog colorDialogue;
        private System.Windows.Forms.Button btnGridColor;
        private System.Windows.Forms.CheckBox cbxIsFrugalWithMaterials;
        private System.Windows.Forms.CheckBox cbxSkipShadowRendering;
    }
}