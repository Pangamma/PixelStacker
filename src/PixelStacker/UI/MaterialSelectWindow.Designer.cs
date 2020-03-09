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
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.flowVertical = new PixelStacker.UI.CustomFlowLayoutPanel();
            this.flowRow1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelFilter = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxMaterialFilter = new System.Windows.Forms.ComboBox();
            this.panelColorProfile = new System.Windows.Forms.Panel();
            this.btnEditColorProfiles = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ddlColorProfile = new System.Windows.Forms.ComboBox();
            this.customFlowLayoutPanel1 = new PixelStacker.UI.CustomFlowLayoutPanel();
            this.cbxIsMultiLayer = new System.Windows.Forms.CheckBox();
            this.cbxIsSideView = new System.Windows.Forms.CheckBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.flowLayout = new PixelStacker.UI.CustomFlowLayoutPanel();
            this.flowVertical.SuspendLayout();
            this.flowRow1.SuspendLayout();
            this.panelFilter.SuspendLayout();
            this.panelColorProfile.SuspendLayout();
            this.customFlowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dlgSave
            // 
            this.dlgSave.DefaultExt = "json";
            this.dlgSave.Filter = "json|*.json";
            // 
            // flowVertical
            // 
            this.flowVertical.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowVertical.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowVertical.BackColor = System.Drawing.Color.Transparent;
            this.flowVertical.Controls.Add(this.flowRow1);
            this.flowVertical.Controls.Add(this.customFlowLayoutPanel1);
            this.flowVertical.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowVertical.Location = new System.Drawing.Point(3, 9);
            this.flowVertical.Margin = new System.Windows.Forms.Padding(0);
            this.flowVertical.Name = "flowVertical";
            this.flowVertical.OnCommandKey = null;
            this.flowVertical.Size = new System.Drawing.Size(860, 85);
            this.flowVertical.TabIndex = 8;
            // 
            // flowRow1
            // 
            this.flowRow1.AutoSize = true;
            this.flowRow1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowRow1.BackColor = System.Drawing.Color.Transparent;
            this.flowRow1.Controls.Add(this.panelFilter);
            this.flowRow1.Controls.Add(this.panelColorProfile);
            this.flowRow1.Location = new System.Drawing.Point(3, 3);
            this.flowRow1.Name = "flowRow1";
            this.flowRow1.Size = new System.Drawing.Size(714, 39);
            this.flowRow1.TabIndex = 0;
            // 
            // panelFilter
            // 
            this.panelFilter.BackColor = System.Drawing.SystemColors.Control;
            this.panelFilter.Controls.Add(this.label1);
            this.panelFilter.Controls.Add(this.tbxMaterialFilter);
            this.panelFilter.Location = new System.Drawing.Point(3, 3);
            this.panelFilter.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.Padding = new System.Windows.Forms.Padding(5);
            this.panelFilter.Size = new System.Drawing.Size(290, 33);
            this.panelFilter.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Location = new System.Drawing.Point(8, -1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 33);
            this.label1.TabIndex = 1;
            this.label1.Text = "Filter";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // panelColorProfile
            // 
            this.panelColorProfile.BackColor = System.Drawing.SystemColors.Control;
            this.panelColorProfile.Controls.Add(this.btnEditColorProfiles);
            this.panelColorProfile.Controls.Add(this.label2);
            this.panelColorProfile.Controls.Add(this.ddlColorProfile);
            this.panelColorProfile.Location = new System.Drawing.Point(316, 3);
            this.panelColorProfile.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.panelColorProfile.Name = "panelColorProfile";
            this.panelColorProfile.Padding = new System.Windows.Forms.Padding(5);
            this.panelColorProfile.Size = new System.Drawing.Size(388, 33);
            this.panelColorProfile.TabIndex = 1;
            // 
            // btnEditColorProfiles
            // 
            this.btnEditColorProfiles.Location = new System.Drawing.Point(300, 2);
            this.btnEditColorProfiles.Name = "btnEditColorProfiles";
            this.btnEditColorProfiles.Size = new System.Drawing.Size(69, 25);
            this.btnEditColorProfiles.TabIndex = 1;
            this.btnEditColorProfiles.Text = "Save";
            this.btnEditColorProfiles.UseVisualStyleBackColor = true;
            this.btnEditColorProfiles.Click += new System.EventHandler(this.btnEditColorProfiles_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "Color Profile";
            // 
            // ddlColorProfile
            // 
            this.ddlColorProfile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ddlColorProfile.FormattingEnabled = true;
            this.ddlColorProfile.Items.AddRange(new object[] {
            "Test",
            "Test2"});
            this.ddlColorProfile.Location = new System.Drawing.Point(94, 3);
            this.ddlColorProfile.MaximumSize = new System.Drawing.Size(200, 0);
            this.ddlColorProfile.MinimumSize = new System.Drawing.Size(200, 0);
            this.ddlColorProfile.Name = "ddlColorProfile";
            this.ddlColorProfile.Size = new System.Drawing.Size(200, 24);
            this.ddlColorProfile.TabIndex = 2;
            this.ddlColorProfile.SelectedValueChanged += new System.EventHandler(this.ddlColorProfile_SelectedValueChanged);
            // 
            // customFlowLayoutPanel1
            // 
            this.customFlowLayoutPanel1.AutoSize = true;
            this.customFlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.customFlowLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.customFlowLayoutPanel1.Controls.Add(this.cbxIsMultiLayer);
            this.customFlowLayoutPanel1.Controls.Add(this.cbxIsSideView);
            this.customFlowLayoutPanel1.Controls.Add(this.lblInfo);
            this.customFlowLayoutPanel1.Location = new System.Drawing.Point(3, 48);
            this.customFlowLayoutPanel1.Name = "customFlowLayoutPanel1";
            this.customFlowLayoutPanel1.OnCommandKey = null;
            this.customFlowLayoutPanel1.Size = new System.Drawing.Size(468, 27);
            this.customFlowLayoutPanel1.TabIndex = 1;
            // 
            // cbxIsMultiLayer
            // 
            this.cbxIsMultiLayer.AutoSize = true;
            this.cbxIsMultiLayer.Location = new System.Drawing.Point(3, 3);
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
            this.cbxIsSideView.Location = new System.Drawing.Point(168, 3);
            this.cbxIsSideView.Name = "cbxIsSideView";
            this.cbxIsSideView.Size = new System.Drawing.Size(91, 21);
            this.cbxIsSideView.TabIndex = 5;
            this.cbxIsSideView.Text = "Side View";
            this.cbxIsSideView.UseVisualStyleBackColor = true;
            this.cbxIsSideView.CheckedChanged += new System.EventHandler(this.cbxIsSideView_CheckedChanged);
            // 
            // lblInfo
            // 
            this.lblInfo.Location = new System.Drawing.Point(265, 0);
            this.lblInfo.MinimumSize = new System.Drawing.Size(200, 21);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(200, 24);
            this.lblInfo.TabIndex = 3;
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flowLayout
            // 
            this.flowLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayout.AutoScroll = true;
            this.flowLayout.BackColor = System.Drawing.SystemColors.ControlLight;
            this.flowLayout.Location = new System.Drawing.Point(3, 97);
            this.flowLayout.Name = "flowLayout";
            this.flowLayout.OnCommandKey = null;
            this.flowLayout.Size = new System.Drawing.Size(860, 380);
            this.flowLayout.TabIndex = 2;
            // 
            // MaterialSelectWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 477);
            this.Controls.Add(this.flowVertical);
            this.Controls.Add(this.flowLayout);
            this.MinimumSize = new System.Drawing.Size(512, 47);
            this.Name = "MaterialSelectWindow";
            this.Text = "Material Selection";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MaterialSelectWindow_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.MaterialSelectWindow_VisibleChanged);
            this.flowVertical.ResumeLayout(false);
            this.flowVertical.PerformLayout();
            this.flowRow1.ResumeLayout(false);
            this.panelFilter.ResumeLayout(false);
            this.panelColorProfile.ResumeLayout(false);
            this.customFlowLayoutPanel1.ResumeLayout(false);
            this.customFlowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox tbxMaterialFilter;
        private System.Windows.Forms.Label label1;
        private CustomFlowLayoutPanel flowLayout;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.CheckBox cbxIsMultiLayer;
        private System.Windows.Forms.CheckBox cbxIsSideView;
        private System.Windows.Forms.Button btnEditColorProfiles;
        private System.Windows.Forms.ComboBox ddlColorProfile;
        private System.Windows.Forms.Label label2;
        private CustomFlowLayoutPanel flowVertical;
        private System.Windows.Forms.FlowLayoutPanel flowRow1;
        private System.Windows.Forms.Panel panelFilter;
        private System.Windows.Forms.Panel panelColorProfile;
        private CustomFlowLayoutPanel customFlowLayoutPanel1;
        private System.Windows.Forms.SaveFileDialog dlgSave;
    }
}