
namespace PixelStacker.UI
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chineseSimplifiedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.danishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dutchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frenchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.germanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.japaneseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.koreanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spanishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectMaterialsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preprocessingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sizingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleBorderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layerFilteringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showTopLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showBottomLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showBothLayersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shadowRenderingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.skipShadowRenderirngToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addExtraDepthToShadowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orientationToolStripMenuItem = new System.Windows.Forms.ToolStripComboBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.generationToolStripMenuItem,
            this.languageToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(49, 24);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.undoToolStripMenuItem.Text = "Undo";
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.redoToolStripMenuItem.Text = "Redo";
            // 
            // languageToolStripMenuItem
            // 
            this.languageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.englishToolStripMenuItem,
            this.chineseSimplifiedToolStripMenuItem,
            this.danishToolStripMenuItem,
            this.dutchToolStripMenuItem,
            this.frenchToolStripMenuItem,
            this.germanToolStripMenuItem,
            this.japaneseToolStripMenuItem,
            this.koreanToolStripMenuItem,
            this.spanishToolStripMenuItem});
            this.languageToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("languageToolStripMenuItem.Image")));
            this.languageToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.languageToolStripMenuItem.MergeIndex = 50;
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            this.languageToolStripMenuItem.Size = new System.Drawing.Size(30, 24);
            // 
            // englishToolStripMenuItem
            // 
            this.englishToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("englishToolStripMenuItem.Image")));
            this.englishToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            this.englishToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.englishToolStripMenuItem.Text = "English";
            this.englishToolStripMenuItem.Click += new System.EventHandler(this.englishToolStripMenuItem_Click);
            // 
            // chineseSimplifiedToolStripMenuItem
            // 
            this.chineseSimplifiedToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("chineseSimplifiedToolStripMenuItem.Image")));
            this.chineseSimplifiedToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.chineseSimplifiedToolStripMenuItem.Name = "chineseSimplifiedToolStripMenuItem";
            this.chineseSimplifiedToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.chineseSimplifiedToolStripMenuItem.Text = "Chinese (Simplified)";
            this.chineseSimplifiedToolStripMenuItem.Click += new System.EventHandler(this.chineseSimplifiedToolStripMenuItem_Click);
            // 
            // danishToolStripMenuItem
            // 
            this.danishToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("danishToolStripMenuItem.Image")));
            this.danishToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.danishToolStripMenuItem.Name = "danishToolStripMenuItem";
            this.danishToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.danishToolStripMenuItem.Text = "Danish";
            this.danishToolStripMenuItem.Click += new System.EventHandler(this.danishToolStripMenuItem_Click);
            // 
            // dutchToolStripMenuItem
            // 
            this.dutchToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("dutchToolStripMenuItem.Image")));
            this.dutchToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.dutchToolStripMenuItem.Name = "dutchToolStripMenuItem";
            this.dutchToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.dutchToolStripMenuItem.Text = "Dutch";
            this.dutchToolStripMenuItem.Click += new System.EventHandler(this.dutchToolStripMenuItem_Click);
            // 
            // frenchToolStripMenuItem
            // 
            this.frenchToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("frenchToolStripMenuItem.Image")));
            this.frenchToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.frenchToolStripMenuItem.Name = "frenchToolStripMenuItem";
            this.frenchToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.frenchToolStripMenuItem.Text = "French";
            this.frenchToolStripMenuItem.Click += new System.EventHandler(this.frenchToolStripMenuItem_Click);
            // 
            // germanToolStripMenuItem
            // 
            this.germanToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("germanToolStripMenuItem.Image")));
            this.germanToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.germanToolStripMenuItem.Name = "germanToolStripMenuItem";
            this.germanToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.germanToolStripMenuItem.Text = "German";
            this.germanToolStripMenuItem.Click += new System.EventHandler(this.germanToolStripMenuItem_Click);
            // 
            // japaneseToolStripMenuItem
            // 
            this.japaneseToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("japaneseToolStripMenuItem.Image")));
            this.japaneseToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.japaneseToolStripMenuItem.Name = "japaneseToolStripMenuItem";
            this.japaneseToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.japaneseToolStripMenuItem.Text = "Japanese";
            this.japaneseToolStripMenuItem.Click += new System.EventHandler(this.japaneseToolStripMenuItem_Click);
            // 
            // koreanToolStripMenuItem
            // 
            this.koreanToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("koreanToolStripMenuItem.Image")));
            this.koreanToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.koreanToolStripMenuItem.Name = "koreanToolStripMenuItem";
            this.koreanToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.koreanToolStripMenuItem.Text = "Korean";
            this.koreanToolStripMenuItem.Click += new System.EventHandler(this.koreanToolStripMenuItem_Click);
            // 
            // spanishToolStripMenuItem
            // 
            this.spanishToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("spanishToolStripMenuItem.Image")));
            this.spanishToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.spanishToolStripMenuItem.Name = "spanishToolStripMenuItem";
            this.spanishToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.spanishToolStripMenuItem.Text = "Spanish";
            this.spanishToolStripMenuItem.Click += new System.EventHandler(this.spanishToolStripMenuItem_Click);
            // 
            // generationToolStripMenuItem
            // 
            this.generationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectMaterialsToolStripMenuItem,
            this.preprocessingToolStripMenuItem,
            this.sizingToolStripMenuItem,
            this.orientationToolStripMenuItem});
            this.generationToolStripMenuItem.Name = "generationToolStripMenuItem";
            this.generationToolStripMenuItem.Size = new System.Drawing.Size(96, 24);
            this.generationToolStripMenuItem.Text = "Generation";
            // 
            // selectMaterialsToolStripMenuItem
            // 
            this.selectMaterialsToolStripMenuItem.Name = "selectMaterialsToolStripMenuItem";
            this.selectMaterialsToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.selectMaterialsToolStripMenuItem.Text = "Select Materials";
            // 
            // preprocessingToolStripMenuItem
            // 
            this.preprocessingToolStripMenuItem.Name = "preprocessingToolStripMenuItem";
            this.preprocessingToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.preprocessingToolStripMenuItem.Text = "Preprocessing";
            // 
            // sizingToolStripMenuItem
            // 
            this.sizingToolStripMenuItem.Name = "sizingToolStripMenuItem";
            this.sizingToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.sizingToolStripMenuItem.Text = "Sizing";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gridToolStripMenuItem,
            this.toggleBorderToolStripMenuItem,
            this.layerFilteringToolStripMenuItem,
            this.shadowRenderingToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // gridToolStripMenuItem
            // 
            this.gridToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleGridToolStripMenuItem,
            this.gridOptionsToolStripMenuItem});
            this.gridToolStripMenuItem.Name = "gridToolStripMenuItem";
            this.gridToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.gridToolStripMenuItem.Text = "Grid";
            // 
            // toggleGridToolStripMenuItem
            // 
            this.toggleGridToolStripMenuItem.Name = "toggleGridToolStripMenuItem";
            this.toggleGridToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.toggleGridToolStripMenuItem.Size = new System.Drawing.Size(298, 26);
            this.toggleGridToolStripMenuItem.Text = "Toggle Grid";
            // 
            // gridOptionsToolStripMenuItem
            // 
            this.gridOptionsToolStripMenuItem.Name = "gridOptionsToolStripMenuItem";
            this.gridOptionsToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.gridOptionsToolStripMenuItem.Text = "Grid Options";
            // 
            // toggleBorderToolStripMenuItem
            // 
            this.toggleBorderToolStripMenuItem.Name = "toggleBorderToolStripMenuItem";
            this.toggleBorderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.toggleBorderToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.toggleBorderToolStripMenuItem.Text = "Toggle Border";
            // 
            // layerFilteringToolStripMenuItem
            // 
            this.layerFilteringToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showTopLayerToolStripMenuItem,
            this.showBottomLayerToolStripMenuItem,
            this.showBothLayersToolStripMenuItem});
            this.layerFilteringToolStripMenuItem.Name = "layerFilteringToolStripMenuItem";
            this.layerFilteringToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.layerFilteringToolStripMenuItem.Text = "Layer Filtering";
            // 
            // showTopLayerToolStripMenuItem
            // 
            this.showTopLayerToolStripMenuItem.Name = "showTopLayerToolStripMenuItem";
            this.showTopLayerToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.showTopLayerToolStripMenuItem.Text = "Show Top Layer";
            // 
            // showBottomLayerToolStripMenuItem
            // 
            this.showBottomLayerToolStripMenuItem.Name = "showBottomLayerToolStripMenuItem";
            this.showBottomLayerToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.showBottomLayerToolStripMenuItem.Text = "Show Bottom Layer";
            // 
            // showBothLayersToolStripMenuItem
            // 
            this.showBothLayersToolStripMenuItem.Checked = true;
            this.showBothLayersToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showBothLayersToolStripMenuItem.Name = "showBothLayersToolStripMenuItem";
            this.showBothLayersToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.showBothLayersToolStripMenuItem.Text = "Show Both Layers";
            // 
            // shadowRenderingToolStripMenuItem
            // 
            this.shadowRenderingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.skipShadowRenderirngToolStripMenuItem,
            this.addExtraDepthToShadowsToolStripMenuItem});
            this.shadowRenderingToolStripMenuItem.Name = "shadowRenderingToolStripMenuItem";
            this.shadowRenderingToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.shadowRenderingToolStripMenuItem.Text = "Shadow Rendering";
            // 
            // skipShadowRenderirngToolStripMenuItem
            // 
            this.skipShadowRenderirngToolStripMenuItem.Checked = true;
            this.skipShadowRenderirngToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.skipShadowRenderirngToolStripMenuItem.Name = "skipShadowRenderirngToolStripMenuItem";
            this.skipShadowRenderirngToolStripMenuItem.Size = new System.Drawing.Size(279, 26);
            this.skipShadowRenderirngToolStripMenuItem.Text = "Skip shadow rendering";
            // 
            // addExtraDepthToShadowsToolStripMenuItem
            // 
            this.addExtraDepthToShadowsToolStripMenuItem.Checked = true;
            this.addExtraDepthToShadowsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.addExtraDepthToShadowsToolStripMenuItem.Name = "addExtraDepthToShadowsToolStripMenuItem";
            this.addExtraDepthToShadowsToolStripMenuItem.Size = new System.Drawing.Size(279, 26);
            this.addExtraDepthToShadowsToolStripMenuItem.Text = "Add extra depth to shadows";
            // 
            // orientationToolStripMenuItem
            // 
            this.orientationToolStripMenuItem.Items.AddRange(new object[] {
            "Vertical",
            "Horizontal"});
            this.orientationToolStripMenuItem.Name = "orientationToolStripMenuItem";
            this.orientationToolStripMenuItem.Size = new System.Drawing.Size(224, 28);
            this.orientationToolStripMenuItem.Text = "Orientation";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem languageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem englishToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chineseSimplifiedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem danishToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dutchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem frenchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem germanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem japaneseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem koreanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spanishToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectMaterialsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preprocessingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sizingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gridToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleGridToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gridOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleBorderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem layerFilteringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showTopLayerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showBottomLayerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showBothLayersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shadowRenderingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem skipShadowRenderirngToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addExtraDepthToShadowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox orientationToolStripMenuItem;
    }
}