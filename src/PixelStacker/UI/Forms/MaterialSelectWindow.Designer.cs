
using PixelStacker.UI.Controls;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MaterialSelectWindow));
            dlgSave = new System.Windows.Forms.SaveFileDialog();
            flowVertical = new System.Windows.Forms.FlowLayoutPanel();
            flowRow1 = new System.Windows.Forms.FlowLayoutPanel();
            panelFilter = new System.Windows.Forms.Panel();
            lblFilter = new System.Windows.Forms.Label();
            tbxMaterialFilter = new System.Windows.Forms.ComboBox();
            panelColorProfile = new System.Windows.Forms.Panel();
            btnEditColorProfiles = new System.Windows.Forms.Button();
            lblColorProfile = new System.Windows.Forms.Label();
            ddlColorProfile = new System.Windows.Forms.ComboBox();
            customFlowLayoutPanel1 = new CustomFlowLayoutPanel();
            cbxIsMultiLayer = new System.Windows.Forms.CheckBox();
            cbxRequire2ndLayer = new System.Windows.Forms.CheckBox();
            cbxIsSideView = new System.Windows.Forms.CheckBox();
            lblInfo = new System.Windows.Forms.Label();
            materialPanel = new CustomPanel();
            flowVertical.SuspendLayout();
            flowRow1.SuspendLayout();
            panelFilter.SuspendLayout();
            panelColorProfile.SuspendLayout();
            customFlowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // dlgSave
            // 
            dlgSave.DefaultExt = "json";
            resources.ApplyResources(dlgSave, "dlgSave");
            // 
            // flowVertical
            // 
            flowVertical.BackColor = System.Drawing.Color.Transparent;
            flowVertical.Controls.Add(flowRow1);
            flowVertical.Controls.Add(customFlowLayoutPanel1);
            resources.ApplyResources(flowVertical, "flowVertical");
            flowVertical.Name = "flowVertical";
            // 
            // flowRow1
            // 
            resources.ApplyResources(flowRow1, "flowRow1");
            flowRow1.BackColor = System.Drawing.Color.Transparent;
            flowRow1.Controls.Add(panelFilter);
            flowRow1.Controls.Add(panelColorProfile);
            flowRow1.Name = "flowRow1";
            // 
            // panelFilter
            // 
            panelFilter.BackColor = System.Drawing.SystemColors.Control;
            panelFilter.Controls.Add(lblFilter);
            panelFilter.Controls.Add(tbxMaterialFilter);
            resources.ApplyResources(panelFilter, "panelFilter");
            panelFilter.Name = "panelFilter";
            // 
            // lblFilter
            // 
            resources.ApplyResources(lblFilter, "lblFilter");
            lblFilter.Name = "lblFilter";
            // 
            // tbxMaterialFilter
            // 
            tbxMaterialFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            tbxMaterialFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            tbxMaterialFilter.FormattingEnabled = true;
            resources.ApplyResources(tbxMaterialFilter, "tbxMaterialFilter");
            tbxMaterialFilter.Name = "tbxMaterialFilter";
            tbxMaterialFilter.TextChanged += ddlMaterialSearch_TextChanged;
            // 
            // panelColorProfile
            // 
            panelColorProfile.BackColor = System.Drawing.SystemColors.Control;
            panelColorProfile.Controls.Add(btnEditColorProfiles);
            panelColorProfile.Controls.Add(lblColorProfile);
            panelColorProfile.Controls.Add(ddlColorProfile);
            resources.ApplyResources(panelColorProfile, "panelColorProfile");
            panelColorProfile.Name = "panelColorProfile";
            // 
            // btnEditColorProfiles
            // 
            resources.ApplyResources(btnEditColorProfiles, "btnEditColorProfiles");
            btnEditColorProfiles.Name = "btnEditColorProfiles";
            btnEditColorProfiles.UseVisualStyleBackColor = true;
            btnEditColorProfiles.Click += btnEditColorProfiles_Click;
            // 
            // lblColorProfile
            // 
            resources.ApplyResources(lblColorProfile, "lblColorProfile");
            lblColorProfile.Name = "lblColorProfile";
            // 
            // ddlColorProfile
            // 
            ddlColorProfile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            ddlColorProfile.FormattingEnabled = true;
            ddlColorProfile.Items.AddRange(new object[] { resources.GetString("ddlColorProfile.Items"), resources.GetString("ddlColorProfile.Items1") });
            resources.ApplyResources(ddlColorProfile, "ddlColorProfile");
            ddlColorProfile.Name = "ddlColorProfile";
            ddlColorProfile.SelectedValueChanged += ddlColorProfile_SelectedValueChanged;
            // 
            // customFlowLayoutPanel1
            // 
            resources.ApplyResources(customFlowLayoutPanel1, "customFlowLayoutPanel1");
            customFlowLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            customFlowLayoutPanel1.Controls.Add(cbxIsMultiLayer);
            customFlowLayoutPanel1.Controls.Add(cbxRequire2ndLayer);
            customFlowLayoutPanel1.Controls.Add(cbxIsSideView);
            customFlowLayoutPanel1.Controls.Add(lblInfo);
            customFlowLayoutPanel1.Name = "customFlowLayoutPanel1";
            customFlowLayoutPanel1.OnCommandKey = null;
            // 
            // cbxIsMultiLayer
            // 
            resources.ApplyResources(cbxIsMultiLayer, "cbxIsMultiLayer");
            cbxIsMultiLayer.Name = "cbxIsMultiLayer";
            cbxIsMultiLayer.UseVisualStyleBackColor = true;
            cbxIsMultiLayer.CheckedChanged += cbxIsMultiLayer_CheckedChanged;
            // 
            // cbxRequire2ndLayer
            // 
            resources.ApplyResources(cbxRequire2ndLayer, "cbxRequire2ndLayer");
            cbxRequire2ndLayer.Name = "cbxRequire2ndLayer";
            cbxRequire2ndLayer.UseVisualStyleBackColor = true;
            cbxRequire2ndLayer.CheckedChanged += cbxRequire2ndLayer_CheckedChanged;
            // 
            // cbxIsSideView
            // 
            resources.ApplyResources(cbxIsSideView, "cbxIsSideView");
            cbxIsSideView.Name = "cbxIsSideView";
            cbxIsSideView.UseVisualStyleBackColor = true;
            cbxIsSideView.CheckedChanged += cbxIsSideView_CheckedChanged;
            // 
            // lblInfo
            // 
            resources.ApplyResources(lblInfo, "lblInfo");
            lblInfo.Name = "lblInfo";
            // 
            // materialPanel
            // 
            resources.ApplyResources(materialPanel, "materialPanel");
            materialPanel.BackColor = System.Drawing.SystemColors.Control;
            materialPanel.Name = "materialPanel";
            materialPanel.OnCommandKey = null;
            // 
            // MaterialSelectWindow
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(materialPanel);
            Controls.Add(flowVertical);
            Name = "MaterialSelectWindow";
            ShowInTaskbar = false;
            FormClosing += MaterialSelectWindow_FormClosing;
            VisibleChanged += MaterialSelectWindow_VisibleChanged;
            Resize += MaterialSelectWindow_Resize;
            flowVertical.ResumeLayout(false);
            flowVertical.PerformLayout();
            flowRow1.ResumeLayout(false);
            panelFilter.ResumeLayout(false);
            panelFilter.PerformLayout();
            panelColorProfile.ResumeLayout(false);
            customFlowLayoutPanel1.ResumeLayout(false);
            customFlowLayoutPanel1.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox tbxMaterialFilter;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.CheckBox cbxIsMultiLayer;
        private System.Windows.Forms.CheckBox cbxIsSideView;
        private System.Windows.Forms.Button btnEditColorProfiles;
        private System.Windows.Forms.ComboBox ddlColorProfile;
        private System.Windows.Forms.Label lblColorProfile;
        private System.Windows.Forms.FlowLayoutPanel flowVertical;
        private System.Windows.Forms.FlowLayoutPanel flowRow1;
        private System.Windows.Forms.Panel panelFilter;
        private System.Windows.Forms.Panel panelColorProfile;
        private CustomFlowLayoutPanel customFlowLayoutPanel1;
        private System.Windows.Forms.SaveFileDialog dlgSave;
        private System.Windows.Forms.CheckBox cbxRequire2ndLayer;
        private CustomPanel materialPanel;
        private System.Windows.Forms.Label lblFilter;
    }
}