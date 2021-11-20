using PixelStacker.UI.Controls;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            PixelStacker.Logic.IO.Config.PanZoomSettings panZoomSettings2 = new PixelStacker.Logic.IO.Config.PanZoomSettings();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectMaterialsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preprocessingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ditheringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sizingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orientationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.horizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.switchPanelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contributorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.swedishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.swatchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblProgress = new System.Windows.Forms.Label();
            this.imageViewer = new PixelStacker.WF.Components.ImageViewer();
            this.canvasEditor = new PixelStacker.UI.Controls.CanvasEditor();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.generationToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.playToolStripMenuItem,
            this.languageToolStripMenuItem,
            this.swatchToolStripMenuItem});
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
            this.reOpenToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(233, 26);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // reOpenToolStripMenuItem
            // 
            this.reOpenToolStripMenuItem.Enabled = false;
            this.reOpenToolStripMenuItem.Name = "reOpenToolStripMenuItem";
            this.reOpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.reOpenToolStripMenuItem.Size = new System.Drawing.Size(233, 26);
            this.reOpenToolStripMenuItem.Text = "Re-Open";
            this.reOpenToolStripMenuItem.Click += new System.EventHandler(this.reOpenToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(233, 26);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Enabled = false;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(233, 26);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
            this.editToolStripMenuItem.Enabled = false;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(49, 24);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Image = global::PixelStacker.Resources.UIResources.undo;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(179, 26);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Image = global::PixelStacker.Resources.UIResources.redo;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(179, 26);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // generationToolStripMenuItem
            // 
            this.generationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectMaterialsToolStripMenuItem,
            this.preprocessingToolStripMenuItem,
            this.ditheringToolStripMenuItem,
            this.sizingToolStripMenuItem,
            this.orientationToolStripMenuItem,
            this.renderToolStripMenuItem});
            this.generationToolStripMenuItem.Name = "generationToolStripMenuItem";
            this.generationToolStripMenuItem.Size = new System.Drawing.Size(83, 24);
            this.generationToolStripMenuItem.Text = "Generate";
            // 
            // selectMaterialsToolStripMenuItem
            // 
            this.selectMaterialsToolStripMenuItem.Name = "selectMaterialsToolStripMenuItem";
            this.selectMaterialsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.selectMaterialsToolStripMenuItem.Size = new System.Drawing.Size(252, 26);
            this.selectMaterialsToolStripMenuItem.Text = "Select Materials";
            this.selectMaterialsToolStripMenuItem.Click += new System.EventHandler(this.selectMaterialsToolStripMenuItem_Click);
            // 
            // preprocessingToolStripMenuItem
            // 
            this.preprocessingToolStripMenuItem.Name = "preprocessingToolStripMenuItem";
            this.preprocessingToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.preprocessingToolStripMenuItem.Size = new System.Drawing.Size(252, 26);
            this.preprocessingToolStripMenuItem.Text = "Color Reduction";
            this.preprocessingToolStripMenuItem.Click += new System.EventHandler(this.preprocessingToolStripMenuItem_Click);
            // 
            // ditheringToolStripMenuItem
            // 
            this.ditheringToolStripMenuItem.Name = "ditheringToolStripMenuItem";
            this.ditheringToolStripMenuItem.Size = new System.Drawing.Size(252, 26);
            this.ditheringToolStripMenuItem.Text = "Dithering";
            this.ditheringToolStripMenuItem.Click += new System.EventHandler(this.preprocessingToolStripMenuItem_Click);
            // 
            // sizingToolStripMenuItem
            // 
            this.sizingToolStripMenuItem.Name = "sizingToolStripMenuItem";
            this.sizingToolStripMenuItem.Size = new System.Drawing.Size(252, 26);
            this.sizingToolStripMenuItem.Text = "Sizing";
            this.sizingToolStripMenuItem.Click += new System.EventHandler(this.sizingToolStripMenuItem_Click);
            // 
            // orientationToolStripMenuItem
            // 
            this.orientationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.horizontalToolStripMenuItem,
            this.verticalToolStripMenuItem});
            this.orientationToolStripMenuItem.Name = "orientationToolStripMenuItem";
            this.orientationToolStripMenuItem.Size = new System.Drawing.Size(252, 26);
            this.orientationToolStripMenuItem.Text = "Orientation";
            // 
            // horizontalToolStripMenuItem
            // 
            this.horizontalToolStripMenuItem.Checked = true;
            this.horizontalToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.horizontalToolStripMenuItem.Name = "horizontalToolStripMenuItem";
            this.horizontalToolStripMenuItem.Size = new System.Drawing.Size(162, 26);
            this.horizontalToolStripMenuItem.Text = "Horizontal";
            this.horizontalToolStripMenuItem.Click += new System.EventHandler(this.horizontalToolStripMenuItem_Click);
            // 
            // verticalToolStripMenuItem
            // 
            this.verticalToolStripMenuItem.Name = "verticalToolStripMenuItem";
            this.verticalToolStripMenuItem.Size = new System.Drawing.Size(162, 26);
            this.verticalToolStripMenuItem.Text = "Vertical";
            this.verticalToolStripMenuItem.Click += new System.EventHandler(this.verticalToolStripMenuItem_Click);
            // 
            // renderToolStripMenuItem
            // 
            this.renderToolStripMenuItem.Name = "renderToolStripMenuItem";
            this.renderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.renderToolStripMenuItem.Size = new System.Drawing.Size(252, 26);
            this.renderToolStripMenuItem.Text = "Render";
            this.renderToolStripMenuItem.Click += new System.EventHandler(this.renderToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gridToolStripMenuItem,
            this.toggleBorderToolStripMenuItem,
            this.layerFilteringToolStripMenuItem,
            this.shadowRenderingToolStripMenuItem,
            this.switchPanelsToolStripMenuItem});
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
            this.toggleGridToolStripMenuItem.Size = new System.Drawing.Size(222, 26);
            this.toggleGridToolStripMenuItem.Text = "Toggle Grid";
            this.toggleGridToolStripMenuItem.Click += new System.EventHandler(this.toggleGridToolStripMenuItem_Click);
            // 
            // gridOptionsToolStripMenuItem
            // 
            this.gridOptionsToolStripMenuItem.Name = "gridOptionsToolStripMenuItem";
            this.gridOptionsToolStripMenuItem.Size = new System.Drawing.Size(222, 26);
            this.gridOptionsToolStripMenuItem.Text = "Grid Options";
            this.gridOptionsToolStripMenuItem.Click += new System.EventHandler(this.gridOptionsToolStripMenuItem_Click);
            // 
            // toggleBorderToolStripMenuItem
            // 
            this.toggleBorderToolStripMenuItem.Name = "toggleBorderToolStripMenuItem";
            this.toggleBorderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.toggleBorderToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.toggleBorderToolStripMenuItem.Text = "Toggle Border";
            this.toggleBorderToolStripMenuItem.Click += new System.EventHandler(this.toggleBorderToolStripMenuItem_Click);
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
            this.showTopLayerToolStripMenuItem.Size = new System.Drawing.Size(221, 26);
            this.showTopLayerToolStripMenuItem.Text = "Show Top Layer";
            this.showTopLayerToolStripMenuItem.Click += new System.EventHandler(this.showTopLayerToolStripMenuItem_Click);
            // 
            // showBottomLayerToolStripMenuItem
            // 
            this.showBottomLayerToolStripMenuItem.Name = "showBottomLayerToolStripMenuItem";
            this.showBottomLayerToolStripMenuItem.Size = new System.Drawing.Size(221, 26);
            this.showBottomLayerToolStripMenuItem.Text = "Show Bottom Layer";
            this.showBottomLayerToolStripMenuItem.Click += new System.EventHandler(this.showBottomLayerToolStripMenuItem_Click);
            // 
            // showBothLayersToolStripMenuItem
            // 
            this.showBothLayersToolStripMenuItem.Checked = true;
            this.showBothLayersToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showBothLayersToolStripMenuItem.Name = "showBothLayersToolStripMenuItem";
            this.showBothLayersToolStripMenuItem.Size = new System.Drawing.Size(221, 26);
            this.showBothLayersToolStripMenuItem.Text = "Show Both Layers";
            this.showBothLayersToolStripMenuItem.Click += new System.EventHandler(this.showBothLayersToolStripMenuItem_Click);
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
            // switchPanelsToolStripMenuItem
            // 
            this.switchPanelsToolStripMenuItem.Name = "switchPanelsToolStripMenuItem";
            this.switchPanelsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.switchPanelsToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.switchPanelsToolStripMenuItem.Text = "Switch Panels";
            this.switchPanelsToolStripMenuItem.Click += new System.EventHandler(this.switchPanelsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contributorsToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Visible = false;
            // 
            // contributorsToolStripMenuItem
            // 
            this.contributorsToolStripMenuItem.Name = "contributorsToolStripMenuItem";
            this.contributorsToolStripMenuItem.Size = new System.Drawing.Size(174, 26);
            this.contributorsToolStripMenuItem.Text = "Contributors";
            this.contributorsToolStripMenuItem.Click += new System.EventHandler(this.contributorsToolStripMenuItem_Click);
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.Size = new System.Drawing.Size(36, 24);
            this.playToolStripMenuItem.Text = "▶";
            this.playToolStripMenuItem.Click += new System.EventHandler(this.renderToolStripMenuItem_Click);
            // 
            // languageToolStripMenuItem
            // 
            this.languageToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.languageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.englishToolStripMenuItem,
            this.chineseSimplifiedToolStripMenuItem,
            this.danishToolStripMenuItem,
            this.dutchToolStripMenuItem,
            this.frenchToolStripMenuItem,
            this.germanToolStripMenuItem,
            this.japaneseToolStripMenuItem,
            this.koreanToolStripMenuItem,
            this.spanishToolStripMenuItem,
            this.swedishToolStripMenuItem});
            this.languageToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("languageToolStripMenuItem.Image")));
            this.languageToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.languageToolStripMenuItem.MergeIndex = 50;
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            this.languageToolStripMenuItem.Size = new System.Drawing.Size(86, 24);
            this.languageToolStripMenuItem.Text = "English";
            this.languageToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // englishToolStripMenuItem
            // 
            this.englishToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            this.englishToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.englishToolStripMenuItem.Text = "English";
            this.englishToolStripMenuItem.Click += new System.EventHandler(this.englishToolStripMenuItem_Click);
            // 
            // chineseSimplifiedToolStripMenuItem
            // 
            this.chineseSimplifiedToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.chineseSimplifiedToolStripMenuItem.Name = "chineseSimplifiedToolStripMenuItem";
            this.chineseSimplifiedToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.chineseSimplifiedToolStripMenuItem.Text = "Chinese (Simplified)";
            this.chineseSimplifiedToolStripMenuItem.Click += new System.EventHandler(this.chineseSimplifiedToolStripMenuItem_Click);
            // 
            // danishToolStripMenuItem
            // 
            this.danishToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.danishToolStripMenuItem.Name = "danishToolStripMenuItem";
            this.danishToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.danishToolStripMenuItem.Text = "Danish";
            this.danishToolStripMenuItem.Click += new System.EventHandler(this.danishToolStripMenuItem_Click);
            // 
            // dutchToolStripMenuItem
            // 
            this.dutchToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.dutchToolStripMenuItem.Name = "dutchToolStripMenuItem";
            this.dutchToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.dutchToolStripMenuItem.Text = "Dutch";
            this.dutchToolStripMenuItem.Click += new System.EventHandler(this.dutchToolStripMenuItem_Click);
            // 
            // frenchToolStripMenuItem
            // 
            this.frenchToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.frenchToolStripMenuItem.Name = "frenchToolStripMenuItem";
            this.frenchToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.frenchToolStripMenuItem.Text = "French";
            this.frenchToolStripMenuItem.Click += new System.EventHandler(this.frenchToolStripMenuItem_Click);
            // 
            // germanToolStripMenuItem
            // 
            this.germanToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.germanToolStripMenuItem.Name = "germanToolStripMenuItem";
            this.germanToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.germanToolStripMenuItem.Text = "German";
            this.germanToolStripMenuItem.Click += new System.EventHandler(this.germanToolStripMenuItem_Click);
            // 
            // japaneseToolStripMenuItem
            // 
            this.japaneseToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.japaneseToolStripMenuItem.Name = "japaneseToolStripMenuItem";
            this.japaneseToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.japaneseToolStripMenuItem.Text = "Japanese";
            this.japaneseToolStripMenuItem.Click += new System.EventHandler(this.japaneseToolStripMenuItem_Click);
            // 
            // koreanToolStripMenuItem
            // 
            this.koreanToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.koreanToolStripMenuItem.Name = "koreanToolStripMenuItem";
            this.koreanToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.koreanToolStripMenuItem.Text = "Korean";
            this.koreanToolStripMenuItem.Click += new System.EventHandler(this.koreanToolStripMenuItem_Click);
            // 
            // spanishToolStripMenuItem
            // 
            this.spanishToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.spanishToolStripMenuItem.Name = "spanishToolStripMenuItem";
            this.spanishToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.spanishToolStripMenuItem.Text = "Spanish";
            this.spanishToolStripMenuItem.Click += new System.EventHandler(this.spanishToolStripMenuItem_Click);
            // 
            // swedishToolStripMenuItem
            // 
            this.swedishToolStripMenuItem.Name = "swedishToolStripMenuItem";
            this.swedishToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.swedishToolStripMenuItem.Text = "Swedish";
            this.swedishToolStripMenuItem.Click += new System.EventHandler(this.swedishToolStripMenuItem_Click);
            // 
            // swatchToolStripMenuItem
            // 
            this.swatchToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.swatchToolStripMenuItem.Enabled = false;
            this.swatchToolStripMenuItem.Image = global::PixelStacker.Resources.UIResources.color;
            this.swatchToolStripMenuItem.Name = "swatchToolStripMenuItem";
            this.swatchToolStripMenuItem.Size = new System.Drawing.Size(34, 24);
            this.swatchToolStripMenuItem.Click += new System.EventHandler(this.swatchToolStripMenuItem_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(0, 420);
            this.progressBar1.MarqueeAnimationSpeed = 200;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(800, 29);
            this.progressBar1.TabIndex = 1;
            this.progressBar1.Value = 1;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblProgress
            // 
            this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblProgress.AutoSize = true;
            this.lblProgress.BackColor = System.Drawing.Color.Transparent;
            this.lblProgress.LiveSetting = System.Windows.Forms.Automation.AutomationLiveSetting.Polite;
            this.lblProgress.Location = new System.Drawing.Point(5, 424);
            this.lblProgress.Margin = new System.Windows.Forms.Padding(0);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(169, 20);
            this.lblProgress.TabIndex = 2;
            this.lblProgress.Text = "Progress status message";
            // 
            // imageViewer
            // 
            this.imageViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageViewer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("imageViewer.BackgroundImage")));
            this.imageViewer.Location = new System.Drawing.Point(0, 28);
            this.imageViewer.Name = "imageViewer";
            this.imageViewer.PanZoomSettings = panZoomSettings2;
            this.imageViewer.Size = new System.Drawing.Size(800, 393);
            this.imageViewer.TabIndex = 5;
            // 
            // canvasEditor
            // 
            this.canvasEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.canvasEditor.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("canvasEditor.BackgroundImage")));
            this.canvasEditor.Location = new System.Drawing.Point(0, 28);
            this.canvasEditor.MainForm = null;
            this.canvasEditor.Name = "canvasEditor";
            this.canvasEditor.Options = null;
            this.canvasEditor.PanZoomSettings = null;
            this.canvasEditor.Size = new System.Drawing.Size(800, 393);
            this.canvasEditor.TabIndex = 6;
            // 
            // dlgOpen
            // 
            this.dlgOpen.FileName = "openFileDialog1";
            this.dlgOpen.Filter = "Supported Format|*.png;*.jpeg;*.jpg;*pxlzip|png|*.png|Jpeg|*.jpg;*.jpeg|PixelStac" +
    "ker Project|*.pxlzip|All Files|*.*";
            this.dlgOpen.Title = "Open File";
            this.dlgOpen.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgOpen_FileOk);
            // 
            // dlgSave
            // 
            this.dlgSave.DefaultExt = "pxlzip";
            this.dlgSave.Filter = "Schem (1.13+)|*.schem|PNG|*.png|Small PNG|*.sm.png|Schematic|*.schematic|Block Co" +
    "unts CSV|*.csv|PixelStacker Project|*.pxlzip";
            this.dlgSave.FilterIndex = 5;
            this.dlgSave.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgSave_FileOk);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.imageViewer);
            this.Controls.Add(this.canvasEditor);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
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
        private System.Windows.Forms.ToolStripMenuItem orientationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem horizontalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verticalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renderToolStripMenuItem;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblProgress;
        private WF.Components.ImageViewer imageViewer;
        private CanvasEditor canvasEditor;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private System.Windows.Forms.ToolStripMenuItem reOpenToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog dlgSave;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem switchPanelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ditheringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem swedishToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem swatchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contributorsToolStripMenuItem;
    }
}