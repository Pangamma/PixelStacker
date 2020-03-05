namespace PixelStacker.UI
{
    partial class MaterialSelectWindow
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
            this.tbxMaterialFilter = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.cbxIsMultiLayer = new System.Windows.Forms.CheckBox();
            this.cbxIsSideView = new System.Windows.Forms.CheckBox();
            this.flowLayout = new PixelStacker.UI.CustomFlowLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbxMaterialFilter
            // 
            this.tbxMaterialFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.tbxMaterialFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbxMaterialFilter.FormattingEnabled = true;
            this.tbxMaterialFilter.Location = new System.Drawing.Point(48, 3);
            this.tbxMaterialFilter.Name = "tbxMaterialFilter";
            this.tbxMaterialFilter.Size = new System.Drawing.Size(228, 24);
            this.tbxMaterialFilter.TabIndex = 0;
            this.tbxMaterialFilter.TextChanged += new System.EventHandler(this.ddlMaterialSearch_TextChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 30);
            this.label1.TabIndex = 1;
            this.label1.Text = "Filter";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(544, 0);
            this.lblInfo.MinimumSize = new System.Drawing.Size(200, 30);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(200, 30);
            this.lblInfo.TabIndex = 3;
            this.lblInfo.Text = "lblInfo";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxIsMultiLayer
            // 
            this.cbxIsMultiLayer.AutoSize = true;
            this.cbxIsMultiLayer.Location = new System.Drawing.Point(282, 3);
            this.cbxIsMultiLayer.Name = "cbxIsMultiLayer";
            this.cbxIsMultiLayer.Size = new System.Drawing.Size(159, 21);
            this.cbxIsMultiLayer.TabIndex = 4;
            this.cbxIsMultiLayer.Text = "Enable second layer";
            this.cbxIsMultiLayer.UseVisualStyleBackColor = true;
            this.cbxIsMultiLayer.CheckedChanged += new System.EventHandler(this.cbxIsMultiLayer_CheckedChanged);
            // 
            // cbxIsSideView
            // 
            this.cbxIsSideView.AutoSize = true;
            this.cbxIsSideView.Location = new System.Drawing.Point(447, 3);
            this.cbxIsSideView.Name = "cbxIsSideView";
            this.cbxIsSideView.Size = new System.Drawing.Size(91, 21);
            this.cbxIsSideView.TabIndex = 5;
            this.cbxIsSideView.Text = "Side View";
            this.cbxIsSideView.UseVisualStyleBackColor = true;
            this.cbxIsSideView.CheckedChanged += new System.EventHandler(this.cbxIsSideView_CheckedChanged);
            // 
            // flowLayout
            // 
            this.flowLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayout.AutoScroll = true;
            this.flowLayout.BackColor = System.Drawing.SystemColors.ControlLight;
            this.flowLayout.Location = new System.Drawing.Point(0, 70);
            this.flowLayout.Name = "flowLayout";
            this.flowLayout.OnCommandKey = null;
            this.flowLayout.Size = new System.Drawing.Size(800, 407);
            this.flowLayout.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.tbxMaterialFilter);
            this.flowLayoutPanel1.Controls.Add(this.cbxIsMultiLayer);
            this.flowLayoutPanel1.Controls.Add(this.cbxIsSideView);
            this.flowLayoutPanel1.Controls.Add(this.lblInfo);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(800, 64);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // MaterialSelectWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 477);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.flowLayout);
            this.Name = "MaterialSelectWindow";
            this.Text = "Material Selection";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MaterialSelectWindow_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.MaterialSelectWindow_VisibleChanged);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox tbxMaterialFilter;
        private System.Windows.Forms.Label label1;
        private CustomFlowLayoutPanel flowLayout;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.CheckBox cbxIsMultiLayer;
        private System.Windows.Forms.CheckBox cbxIsSideView;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}