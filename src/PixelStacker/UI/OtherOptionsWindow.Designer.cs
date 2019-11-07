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
            ((System.ComponentModel.ISupportInitialize)(this.nbrGridSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbrMaxHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbrMaxWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // nbrGridSize
            // 
            this.nbrGridSize.Location = new System.Drawing.Point(111, 12);
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
            this.nbrGridSize.Size = new System.Drawing.Size(112, 20);
            this.nbrGridSize.TabIndex = 0;
            this.nbrGridSize.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.nbrGridSize.ValueChanged += new System.EventHandler(this.nbrGridSize_ValueChanged);
            // 
            // nbrMaxHeight
            // 
            this.nbrMaxHeight.Location = new System.Drawing.Point(111, 77);
            this.nbrMaxHeight.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.nbrMaxHeight.Name = "nbrMaxHeight";
            this.nbrMaxHeight.Size = new System.Drawing.Size(112, 20);
            this.nbrMaxHeight.TabIndex = 1;
            this.nbrMaxHeight.ValueChanged += new System.EventHandler(this.nbrMaxHeight_ValueChanged);
            // 
            // nbrMaxWidth
            // 
            this.nbrMaxWidth.Location = new System.Drawing.Point(111, 103);
            this.nbrMaxWidth.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.nbrMaxWidth.Name = "nbrMaxWidth";
            this.nbrMaxWidth.Size = new System.Drawing.Size(112, 20);
            this.nbrMaxWidth.TabIndex = 2;
            this.nbrMaxWidth.ValueChanged += new System.EventHandler(this.nbrMaxWidth_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Grid Size";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Max Height";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Max Width";
            // 
            // cbxIsSideView
            // 
            this.cbxIsSideView.AutoSize = true;
            this.cbxIsSideView.Location = new System.Drawing.Point(12, 129);
            this.cbxIsSideView.Name = "cbxIsSideView";
            this.cbxIsSideView.Size = new System.Drawing.Size(206, 17);
            this.cbxIsSideView.TabIndex = 8;
            this.cbxIsSideView.Text = "Side View? (Affects output orientation)";
            this.cbxIsSideView.UseVisualStyleBackColor = true;
            this.cbxIsSideView.CheckedChanged += new System.EventHandler(this.cbxIsSideView_CheckedChanged);
            // 
            // btnFactoryReset
            // 
            this.btnFactoryReset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFactoryReset.Location = new System.Drawing.Point(12, 152);
            this.btnFactoryReset.Name = "btnFactoryReset";
            this.btnFactoryReset.Size = new System.Drawing.Size(215, 23);
            this.btnFactoryReset.TabIndex = 9;
            this.btnFactoryReset.Text = "Factory Reset";
            this.btnFactoryReset.UseVisualStyleBackColor = true;
            this.btnFactoryReset.Click += new System.EventHandler(this.btnFactoryReset_Click);
            // 
            // lblGridColor
            // 
            this.lblGridColor.AutoSize = true;
            this.lblGridColor.Location = new System.Drawing.Point(12, 48);
            this.lblGridColor.Name = "lblGridColor";
            this.lblGridColor.Size = new System.Drawing.Size(53, 13);
            this.lblGridColor.TabIndex = 10;
            this.lblGridColor.Text = "Grid Color";
            // 
            // btnGridColor
            // 
            this.btnGridColor.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnGridColor.ForeColor = System.Drawing.Color.Black;
            this.btnGridColor.Location = new System.Drawing.Point(111, 43);
            this.btnGridColor.Name = "btnGridColor";
            this.btnGridColor.Size = new System.Drawing.Size(112, 23);
            this.btnGridColor.TabIndex = 12;
            this.btnGridColor.UseVisualStyleBackColor = false;
            this.btnGridColor.Click += new System.EventHandler(this.btnGridColor_Click);
            // 
            // OtherOptionsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(239, 339);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OtherOptionsWindow";
            this.Text = "Other Options";
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
    }
}