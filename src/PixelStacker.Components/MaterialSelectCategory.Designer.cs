
namespace PixelStacker.UI
{
    public partial class MaterialSelectCategory<TModel>
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
            this.cbxEnableAll = new System.Windows.Forms.CheckBox();
            this.customFlowLayoutPanel1 = new PixelStacker.Components.CustomFlowLayoutPanel();
            this.SuspendLayout();
            // 
            // cbxEnableAll
            // 
            this.cbxEnableAll.AutoSize = true;
            this.cbxEnableAll.Location = new System.Drawing.Point(0, 3);
            this.cbxEnableAll.Name = "cbxEnableAll";
            this.cbxEnableAll.Size = new System.Drawing.Size(98, 21);
            this.cbxEnableAll.TabIndex = 2;
            this.cbxEnableAll.Text = "checkBox1";
            this.cbxEnableAll.UseVisualStyleBackColor = true;
            this.cbxEnableAll.CheckedChanged += new System.EventHandler(this.cbxEnableAll_CheckedChanged);
            // 
            // customFlowLayoutPanel1
            // 
            this.customFlowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.customFlowLayoutPanel1.AutoSize = true;
            this.customFlowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.customFlowLayoutPanel1.Location = new System.Drawing.Point(133, 0);
            this.customFlowLayoutPanel1.Name = "customFlowLayoutPanel1";
            this.customFlowLayoutPanel1.OnCommandKey = null;
            this.customFlowLayoutPanel1.Size = new System.Drawing.Size(422, 66);
            this.customFlowLayoutPanel1.TabIndex = 1;
            // 
            // MaterialSelectCategory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.cbxEnableAll);
            this.Controls.Add(this.customFlowLayoutPanel1);
            this.Name = "MaterialSelectCategory";
            this.Size = new System.Drawing.Size(555, 66);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Components.CustomFlowLayoutPanel customFlowLayoutPanel1;
        protected System.Windows.Forms.CheckBox cbxEnableAll;
    }
}
