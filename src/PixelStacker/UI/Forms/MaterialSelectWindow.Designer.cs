
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
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.flowVertical = new System.Windows.Forms.FlowLayoutPanel();
            this.flowRow1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelFilter = new System.Windows.Forms.Panel();
            this.lblFilter = new System.Windows.Forms.Label();
            this.tbxMaterialFilter = new System.Windows.Forms.ComboBox();
            this.panelColorProfile = new System.Windows.Forms.Panel();
            this.btnEditColorProfiles = new System.Windows.Forms.Button();
            this.lblColorProfile = new System.Windows.Forms.Label();
            this.ddlColorProfile = new System.Windows.Forms.ComboBox();
            this.customFlowLayoutPanel1 = new PixelStacker.UI.Controls.CustomFlowLayoutPanel();
            this.cbxIsMultiLayer = new System.Windows.Forms.CheckBox();
            this.cbxRequire2ndLayer = new System.Windows.Forms.CheckBox();
            this.cbxIsSideView = new System.Windows.Forms.CheckBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.materialPanel = new PixelStacker.UI.Controls.CustomPanel();
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
            resources.ApplyResources(this.dlgSave, "dlgSave");
            // 
            // flowVertical
            // 
            this.flowVertical.BackColor = System.Drawing.Color.Transparent;
            this.flowVertical.Controls.Add(this.flowRow1);
            this.flowVertical.Controls.Add(this.customFlowLayoutPanel1);
            resources.ApplyResources(this.flowVertical, "flowVertical");
            this.flowVertical.Name = "flowVertical";
            // 
            // flowRow1
            // 
            resources.ApplyResources(this.flowRow1, "flowRow1");
            this.flowRow1.BackColor = System.Drawing.Color.Transparent;
            this.flowRow1.Controls.Add(this.panelFilter);
            this.flowRow1.Controls.Add(this.panelColorProfile);
            this.flowRow1.Name = "flowRow1";
            // 
            // panelFilter
            // 
            this.panelFilter.BackColor = System.Drawing.SystemColors.Control;
            this.panelFilter.Controls.Add(this.lblFilter);
            this.panelFilter.Controls.Add(this.tbxMaterialFilter);
            resources.ApplyResources(this.panelFilter, "panelFilter");
            this.panelFilter.Name = "panelFilter";
            // 
            // lblFilter
            // 
            resources.ApplyResources(this.lblFilter, "lblFilter");
            this.lblFilter.Name = "lblFilter";
            // 
            // tbxMaterialFilter
            // 
            this.tbxMaterialFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.tbxMaterialFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbxMaterialFilter.FormattingEnabled = true;
            resources.ApplyResources(this.tbxMaterialFilter, "tbxMaterialFilter");
            this.tbxMaterialFilter.Name = "tbxMaterialFilter";
            this.tbxMaterialFilter.TextChanged += new System.EventHandler(this.ddlMaterialSearch_TextChanged);
            // 
            // panelColorProfile
            // 
            this.panelColorProfile.BackColor = System.Drawing.SystemColors.Control;
            this.panelColorProfile.Controls.Add(this.btnEditColorProfiles);
            this.panelColorProfile.Controls.Add(this.lblColorProfile);
            this.panelColorProfile.Controls.Add(this.ddlColorProfile);
            resources.ApplyResources(this.panelColorProfile, "panelColorProfile");
            this.panelColorProfile.Name = "panelColorProfile";
            // 
            // btnEditColorProfiles
            // 
            resources.ApplyResources(this.btnEditColorProfiles, "btnEditColorProfiles");
            this.btnEditColorProfiles.Name = "btnEditColorProfiles";
            this.btnEditColorProfiles.UseVisualStyleBackColor = true;
            this.btnEditColorProfiles.Click += new System.EventHandler(this.btnEditColorProfiles_Click);
            // 
            // lblColorProfile
            // 
            resources.ApplyResources(this.lblColorProfile, "lblColorProfile");
            this.lblColorProfile.Name = "lblColorProfile";
            // 
            // ddlColorProfile
            // 
            this.ddlColorProfile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ddlColorProfile.FormattingEnabled = true;
            this.ddlColorProfile.Items.AddRange(new object[] {
            resources.GetString("ddlColorProfile.Items"),
            resources.GetString("ddlColorProfile.Items1")});
            resources.ApplyResources(this.ddlColorProfile, "ddlColorProfile");
            this.ddlColorProfile.Name = "ddlColorProfile";
            this.ddlColorProfile.SelectedValueChanged += new System.EventHandler(this.ddlColorProfile_SelectedValueChanged);
            // 
            // customFlowLayoutPanel1
            // 
            resources.ApplyResources(this.customFlowLayoutPanel1, "customFlowLayoutPanel1");
            this.customFlowLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.customFlowLayoutPanel1.Controls.Add(this.cbxIsMultiLayer);
            this.customFlowLayoutPanel1.Controls.Add(this.cbxRequire2ndLayer);
            this.customFlowLayoutPanel1.Controls.Add(this.cbxIsSideView);
            this.customFlowLayoutPanel1.Controls.Add(this.lblInfo);
            this.customFlowLayoutPanel1.Name = "customFlowLayoutPanel1";
            this.customFlowLayoutPanel1.OnCommandKey = null;
            // 
            // cbxIsMultiLayer
            // 
            resources.ApplyResources(this.cbxIsMultiLayer, "cbxIsMultiLayer");
            this.cbxIsMultiLayer.Name = "cbxIsMultiLayer";
            this.cbxIsMultiLayer.UseVisualStyleBackColor = true;
            this.cbxIsMultiLayer.CheckedChanged += new System.EventHandler(this.cbxIsMultiLayer_CheckedChanged);
            // 
            // cbxRequire2ndLayer
            // 
            resources.ApplyResources(this.cbxRequire2ndLayer, "cbxRequire2ndLayer");
            this.cbxRequire2ndLayer.Name = "cbxRequire2ndLayer";
            this.cbxRequire2ndLayer.UseVisualStyleBackColor = true;
            this.cbxRequire2ndLayer.CheckedChanged += new System.EventHandler(this.cbxRequire2ndLayer_CheckedChanged);
            // 
            // cbxIsSideView
            // 
            resources.ApplyResources(this.cbxIsSideView, "cbxIsSideView");
            this.cbxIsSideView.Name = "cbxIsSideView";
            this.cbxIsSideView.UseVisualStyleBackColor = true;
            this.cbxIsSideView.CheckedChanged += new System.EventHandler(this.cbxIsSideView_CheckedChanged);
            // 
            // lblInfo
            // 
            resources.ApplyResources(this.lblInfo, "lblInfo");
            this.lblInfo.Name = "lblInfo";
            // 
            // materialPanel
            // 
            resources.ApplyResources(this.materialPanel, "materialPanel");
            this.materialPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.materialPanel.Name = "materialPanel";
            this.materialPanel.OnCommandKey = null;
            // 
            // MaterialSelectWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.materialPanel);
            this.Controls.Add(this.flowVertical);
            this.Name = "MaterialSelectWindow";
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MaterialSelectWindow_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.MaterialSelectWindow_VisibleChanged);
            this.Resize += new System.EventHandler(this.MaterialSelectWindow_Resize);
            this.flowVertical.ResumeLayout(false);
            this.flowVertical.PerformLayout();
            this.flowRow1.ResumeLayout(false);
            this.panelFilter.ResumeLayout(false);
            this.panelFilter.PerformLayout();
            this.panelColorProfile.ResumeLayout(false);
            this.customFlowLayoutPanel1.ResumeLayout(false);
            this.customFlowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

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