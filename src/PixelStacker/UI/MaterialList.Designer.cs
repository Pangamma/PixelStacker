namespace PixelStacker.UI
{
    partial class MaterialList
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnToggleAll = new System.Windows.Forms.Button();
            this.lblCategory = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(153, 4);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(871, 116);
            this.flowLayoutPanel1.TabIndex = 7;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // btnToggleAll
            // 
            this.btnToggleAll.Location = new System.Drawing.Point(4, 53);
            this.btnToggleAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnToggleAll.Name = "btnToggleAll";
            this.btnToggleAll.Size = new System.Drawing.Size(123, 38);
            this.btnToggleAll.TabIndex = 2;
            this.btnToggleAll.Text = "Disable All";
            this.btnToggleAll.UseVisualStyleBackColor = true;
            this.btnToggleAll.Click += new System.EventHandler(this.btnToggleAll_Click);
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblCategory.Location = new System.Drawing.Point(4, 7);
            this.lblCategory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(123, 17);
            this.lblCategory.TabIndex = 9;
            this.lblCategory.Text = "Glazed Terracotta";
            this.lblCategory.Click += new System.EventHandler(this.btnToggleAll_Click);
            // 
            // MaterialList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.btnToggleAll);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(0, 123);
            this.Name = "MaterialList";
            this.Size = new System.Drawing.Size(1028, 123);
            this.Load += new System.EventHandler(this.MaterialList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnToggleAll;
        private System.Windows.Forms.Label lblCategory;
    }
}
