namespace PixelStacker.UI.Controls
{
    partial class MaterialPicker
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
            this.imgSelectedCombo = new PixelStacker.UI.Controls.ImageButton();
            this.lblTop = new System.Windows.Forms.Label();
            this.lblBottom = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.cbxMultiLayer = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // imgSelectedCombo
            // 
            this.imgSelectedCombo.IsChecked = false;
            this.imgSelectedCombo.Location = new System.Drawing.Point(3, 3);
            this.imgSelectedCombo.Name = "imgSelectedCombo";
            this.imgSelectedCombo.PushState = PixelStacker.UI.Controls.ImageButtonPushState.Normal;
            this.imgSelectedCombo.Size = new System.Drawing.Size(80, 80);
            this.imgSelectedCombo.TabIndex = 0;
            // 
            // lblTop
            // 
            this.lblTop.AutoSize = true;
            this.lblTop.Location = new System.Drawing.Point(89, 3);
            this.lblTop.Name = "lblTop";
            this.lblTop.Size = new System.Drawing.Size(34, 20);
            this.lblTop.TabIndex = 1;
            this.lblTop.Text = "Top";
            // 
            // lblBottom
            // 
            this.lblBottom.AutoSize = true;
            this.lblBottom.Location = new System.Drawing.Point(89, 23);
            this.lblBottom.Name = "lblBottom";
            this.lblBottom.Size = new System.Drawing.Size(59, 20);
            this.lblBottom.TabIndex = 2;
            this.lblBottom.Text = "Bottom";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(89, 56);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(213, 27);
            this.textBox1.TabIndex = 3;
            // 
            // cbxMultiLayer
            // 
            this.cbxMultiLayer.AutoSize = true;
            this.cbxMultiLayer.Location = new System.Drawing.Point(3, 119);
            this.cbxMultiLayer.Name = "cbxMultiLayer";
            this.cbxMultiLayer.Size = new System.Drawing.Size(177, 24);
            this.cbxMultiLayer.TabIndex = 4;
            this.cbxMultiLayer.Text = "Enable multiple layers";
            this.cbxMultiLayer.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(3, 89);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(177, 24);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Enable multiple layers";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // MaterialPicker
            // 
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.cbxMultiLayer);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lblBottom);
            this.Controls.Add(this.lblTop);
            this.Controls.Add(this.imgSelectedCombo);
            this.Name = "MaterialPicker";
            this.Size = new System.Drawing.Size(305, 438);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ImageButton imgSelectedCombo;
        private System.Windows.Forms.Label lblTop;
        private System.Windows.Forms.Label lblBottom;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox cbxMultiLayer;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}
