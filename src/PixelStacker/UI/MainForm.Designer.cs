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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportSchematicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveColorPaletteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.brickToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.squareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allPossibilitiescompactToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allColorsdetailedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleSolidColorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleBorderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.togglePaletteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleProgressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layerFilteringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleLayerFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.up1LayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.down1LayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_PreRenderOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.otherOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_preRender = new System.Windows.Forms.ToolStripMenuItem();
            this.previewSchematicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chineseSimplifiedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.danishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frenchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.germanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.japaneseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.koreanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spanishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.dlgSaveColorPalette = new System.Windows.Forms.SaveFileDialog();
            this.imagePanelMain = new PixelStacker.UI.ImagePanel();
            this.renderedImagePanel = new PixelStacker.UI.RenderedImagePanel();
            this.dlgSaveSettings = new System.Windows.Forms.SaveFileDialog();
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
            this.toolsToolStripMenuItem,
            this.languageToolStripMenuItem,
            this.helpToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.reOpenToolStripMenuItem,
            this.exportSchematicToolStripMenuItem,
            this.saveColorPaletteToolStripMenuItem,
            this.exportSettingsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // reOpenToolStripMenuItem
            // 
            resources.ApplyResources(this.reOpenToolStripMenuItem, "reOpenToolStripMenuItem");
            this.reOpenToolStripMenuItem.Name = "reOpenToolStripMenuItem";
            this.reOpenToolStripMenuItem.Click += new System.EventHandler(this.reOpenToolStripMenuItem_Click);
            // 
            // exportSchematicToolStripMenuItem
            // 
            resources.ApplyResources(this.exportSchematicToolStripMenuItem, "exportSchematicToolStripMenuItem");
            this.exportSchematicToolStripMenuItem.Name = "exportSchematicToolStripMenuItem";
            this.exportSchematicToolStripMenuItem.Click += new System.EventHandler(this.saveMenuClick);
            // 
            // saveColorPaletteToolStripMenuItem
            // 
            this.saveColorPaletteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.graphToolStripMenuItem,
            this.brickToolStripMenuItem,
            this.squareToolStripMenuItem,
            this.allPossibilitiescompactToolStripMenuItem,
            this.allColorsdetailedToolStripMenuItem});
            resources.ApplyResources(this.saveColorPaletteToolStripMenuItem, "saveColorPaletteToolStripMenuItem");
            this.saveColorPaletteToolStripMenuItem.Name = "saveColorPaletteToolStripMenuItem";
            // 
            // graphToolStripMenuItem
            // 
            this.graphToolStripMenuItem.Image = global::PixelStacker.Resources.UIResources.feature_palette_compact_graph;
            resources.ApplyResources(this.graphToolStripMenuItem, "graphToolStripMenuItem");
            this.graphToolStripMenuItem.Name = "graphToolStripMenuItem";
            this.graphToolStripMenuItem.Click += new System.EventHandler(this.graphToolStripMenuItem_Click);
            // 
            // brickToolStripMenuItem
            // 
            this.brickToolStripMenuItem.Image = global::PixelStacker.Resources.UIResources.feature_palette_compact_brick;
            resources.ApplyResources(this.brickToolStripMenuItem, "brickToolStripMenuItem");
            this.brickToolStripMenuItem.Name = "brickToolStripMenuItem";
            this.brickToolStripMenuItem.Click += new System.EventHandler(this.brickToolStripMenuItem_Click);
            // 
            // squareToolStripMenuItem
            // 
            this.squareToolStripMenuItem.Image = global::PixelStacker.Resources.UIResources.feature_palette_compact_square;
            resources.ApplyResources(this.squareToolStripMenuItem, "squareToolStripMenuItem");
            this.squareToolStripMenuItem.Name = "squareToolStripMenuItem";
            this.squareToolStripMenuItem.Click += new System.EventHandler(this.squareToolStripMenuItem_Click);
            // 
            // allPossibilitiescompactToolStripMenuItem
            // 
            this.allPossibilitiescompactToolStripMenuItem.Image = global::PixelStacker.Resources.UIResources.feature_palette_compact_grid;
            resources.ApplyResources(this.allPossibilitiescompactToolStripMenuItem, "allPossibilitiescompactToolStripMenuItem");
            this.allPossibilitiescompactToolStripMenuItem.Name = "allPossibilitiescompactToolStripMenuItem";
            this.allPossibilitiescompactToolStripMenuItem.Click += new System.EventHandler(this.allPossibilitiescompactToolStripMenuItem_Click);
            // 
            // allColorsdetailedToolStripMenuItem
            // 
            this.allColorsdetailedToolStripMenuItem.Image = global::PixelStacker.Resources.UIResources.feature_palette_detailed_grid;
            resources.ApplyResources(this.allColorsdetailedToolStripMenuItem, "allColorsdetailedToolStripMenuItem");
            this.allColorsdetailedToolStripMenuItem.Name = "allColorsdetailedToolStripMenuItem";
            this.allColorsdetailedToolStripMenuItem.Click += new System.EventHandler(this.allColorsdetailedToolStripMenuItem_Click);
            // 
            // exportSettingsToolStripMenuItem
            // 
            this.exportSettingsToolStripMenuItem.BackColor = System.Drawing.Color.PaleTurquoise;
            this.exportSettingsToolStripMenuItem.Name = "exportSettingsToolStripMenuItem";
            resources.ApplyResources(this.exportSettingsToolStripMenuItem, "exportSettingsToolStripMenuItem");
            this.exportSettingsToolStripMenuItem.Click += new System.EventHandler(this.exportSettingsToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            resources.ApplyResources(this.undoToolStripMenuItem, "undoToolStripMenuItem");
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            resources.ApplyResources(this.redoToolStripMenuItem, "redoToolStripMenuItem");
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleGridToolStripMenuItem,
            this.toggleSolidColorsToolStripMenuItem,
            this.toggleBorderToolStripMenuItem,
            this.togglePaletteToolStripMenuItem,
            this.toggleProgressToolStripMenuItem,
            this.layerFilteringToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            resources.ApplyResources(this.viewToolStripMenuItem, "viewToolStripMenuItem");
            // 
            // toggleGridToolStripMenuItem
            // 
            resources.ApplyResources(this.toggleGridToolStripMenuItem, "toggleGridToolStripMenuItem");
            this.toggleGridToolStripMenuItem.Name = "toggleGridToolStripMenuItem";
            this.toggleGridToolStripMenuItem.Click += new System.EventHandler(this.toggleGridToolStripMenuItem_Click);
            // 
            // toggleSolidColorsToolStripMenuItem
            // 
            resources.ApplyResources(this.toggleSolidColorsToolStripMenuItem, "toggleSolidColorsToolStripMenuItem");
            this.toggleSolidColorsToolStripMenuItem.Name = "toggleSolidColorsToolStripMenuItem";
            this.toggleSolidColorsToolStripMenuItem.Click += new System.EventHandler(this.toggleSolidColorsToolStripMenuItem_Click);
            // 
            // toggleBorderToolStripMenuItem
            // 
            resources.ApplyResources(this.toggleBorderToolStripMenuItem, "toggleBorderToolStripMenuItem");
            this.toggleBorderToolStripMenuItem.Name = "toggleBorderToolStripMenuItem";
            this.toggleBorderToolStripMenuItem.Click += new System.EventHandler(this.toggleBorderToolStripMenuItem_Click);
            // 
            // togglePaletteToolStripMenuItem
            // 
            this.togglePaletteToolStripMenuItem.BackColor = System.Drawing.Color.PaleTurquoise;
            resources.ApplyResources(this.togglePaletteToolStripMenuItem, "togglePaletteToolStripMenuItem");
            this.togglePaletteToolStripMenuItem.Name = "togglePaletteToolStripMenuItem";
            this.togglePaletteToolStripMenuItem.Click += new System.EventHandler(this.togglePaletteToolStripMenuItem_Click);
            // 
            // toggleProgressToolStripMenuItem
            // 
            this.toggleProgressToolStripMenuItem.BackColor = System.Drawing.Color.PaleTurquoise;
            this.toggleProgressToolStripMenuItem.Name = "toggleProgressToolStripMenuItem";
            resources.ApplyResources(this.toggleProgressToolStripMenuItem, "toggleProgressToolStripMenuItem");
            this.toggleProgressToolStripMenuItem.Click += new System.EventHandler(this.toggleProgressToolStripMenuItem_Click);
            // 
            // layerFilteringToolStripMenuItem
            // 
            this.layerFilteringToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleLayerFilterToolStripMenuItem,
            this.up1LayerToolStripMenuItem,
            this.down1LayerToolStripMenuItem});
            resources.ApplyResources(this.layerFilteringToolStripMenuItem, "layerFilteringToolStripMenuItem");
            this.layerFilteringToolStripMenuItem.Name = "layerFilteringToolStripMenuItem";
            // 
            // toggleLayerFilterToolStripMenuItem
            // 
            this.toggleLayerFilterToolStripMenuItem.Name = "toggleLayerFilterToolStripMenuItem";
            resources.ApplyResources(this.toggleLayerFilterToolStripMenuItem, "toggleLayerFilterToolStripMenuItem");
            this.toggleLayerFilterToolStripMenuItem.Click += new System.EventHandler(this.toggleLayerFilterToolStripMenuItem_Click);
            // 
            // up1LayerToolStripMenuItem
            // 
            resources.ApplyResources(this.up1LayerToolStripMenuItem, "up1LayerToolStripMenuItem");
            this.up1LayerToolStripMenuItem.Name = "up1LayerToolStripMenuItem";
            this.up1LayerToolStripMenuItem.Click += new System.EventHandler(this.up1LayerToolStripMenuItem_Click);
            // 
            // down1LayerToolStripMenuItem
            // 
            resources.ApplyResources(this.down1LayerToolStripMenuItem, "down1LayerToolStripMenuItem");
            this.down1LayerToolStripMenuItem.Name = "down1LayerToolStripMenuItem";
            this.down1LayerToolStripMenuItem.Click += new System.EventHandler(this.down1LayerToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.mi_PreRenderOptions,
            this.otherOptionsToolStripMenuItem,
            this.mi_preRender,
            this.previewSchematicToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            resources.ApplyResources(this.toolsToolStripMenuItem, "toolsToolStripMenuItem");
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            resources.ApplyResources(this.optionsToolStripMenuItem, "optionsToolStripMenuItem");
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // mi_PreRenderOptions
            // 
            this.mi_PreRenderOptions.Name = "mi_PreRenderOptions";
            resources.ApplyResources(this.mi_PreRenderOptions, "mi_PreRenderOptions");
            this.mi_PreRenderOptions.Click += new System.EventHandler(this.prerenderOptions_Click);
            // 
            // otherOptionsToolStripMenuItem
            // 
            this.otherOptionsToolStripMenuItem.Name = "otherOptionsToolStripMenuItem";
            resources.ApplyResources(this.otherOptionsToolStripMenuItem, "otherOptionsToolStripMenuItem");
            this.otherOptionsToolStripMenuItem.Click += new System.EventHandler(this.otherOptionsToolStripMenuItem_Click);
            // 
            // mi_preRender
            // 
            this.mi_preRender.BackColor = System.Drawing.Color.PaleTurquoise;
            this.mi_preRender.Name = "mi_preRender";
            resources.ApplyResources(this.mi_preRender, "mi_preRender");
            this.mi_preRender.Click += new System.EventHandler(this.mi_preRender_Click);
            // 
            // previewSchematicToolStripMenuItem
            // 
            this.previewSchematicToolStripMenuItem.Name = "previewSchematicToolStripMenuItem";
            resources.ApplyResources(this.previewSchematicToolStripMenuItem, "previewSchematicToolStripMenuItem");
            this.previewSchematicToolStripMenuItem.Click += new System.EventHandler(this.renderSchematicToolStripMenuItem_Click);
            // 
            // languageToolStripMenuItem
            // 
            this.languageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.englishToolStripMenuItem,
            this.chineseSimplifiedToolStripMenuItem,
            this.danishToolStripMenuItem,
            this.frenchToolStripMenuItem,
            this.germanToolStripMenuItem,
            this.japaneseToolStripMenuItem,
            this.koreanToolStripMenuItem,
            this.spanishToolStripMenuItem});
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            resources.ApplyResources(this.languageToolStripMenuItem, "languageToolStripMenuItem");
            // 
            // englishToolStripMenuItem
            // 
            this.englishToolStripMenuItem.Image = global::PixelStacker.Resources.UIResources.country_us;
            resources.ApplyResources(this.englishToolStripMenuItem, "englishToolStripMenuItem");
            this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            this.englishToolStripMenuItem.Click += new System.EventHandler(this.englishToolStripMenuItem_Click);
            // 
            // chineseSimplifiedToolStripMenuItem
            // 
            this.chineseSimplifiedToolStripMenuItem.Image = global::PixelStacker.Resources.UIResources.country_cn;
            resources.ApplyResources(this.chineseSimplifiedToolStripMenuItem, "chineseSimplifiedToolStripMenuItem");
            this.chineseSimplifiedToolStripMenuItem.Name = "chineseSimplifiedToolStripMenuItem";
            this.chineseSimplifiedToolStripMenuItem.Click += new System.EventHandler(this.chineseSimplifiedToolStripMenuItem_Click);
            // 
            // danishToolStripMenuItem
            // 
            this.danishToolStripMenuItem.Image = global::PixelStacker.Resources.UIResources.country_dk;
            resources.ApplyResources(this.danishToolStripMenuItem, "danishToolStripMenuItem");
            this.danishToolStripMenuItem.Name = "danishToolStripMenuItem";
            this.danishToolStripMenuItem.Click += new System.EventHandler(this.danishToolStripMenuItem_Click);
            // 
            // frenchToolStripMenuItem
            // 
            this.frenchToolStripMenuItem.Image = global::PixelStacker.Resources.UIResources.country_fr;
            resources.ApplyResources(this.frenchToolStripMenuItem, "frenchToolStripMenuItem");
            this.frenchToolStripMenuItem.Name = "frenchToolStripMenuItem";
            this.frenchToolStripMenuItem.Click += new System.EventHandler(this.frenchToolStripMenuItem_Click);
            // 
            // germanToolStripMenuItem
            // 
            this.germanToolStripMenuItem.Image = global::PixelStacker.Resources.UIResources.country_de;
            resources.ApplyResources(this.germanToolStripMenuItem, "germanToolStripMenuItem");
            this.germanToolStripMenuItem.Name = "germanToolStripMenuItem";
            this.germanToolStripMenuItem.Click += new System.EventHandler(this.germanToolStripMenuItem_Click);
            // 
            // japaneseToolStripMenuItem
            // 
            this.japaneseToolStripMenuItem.Image = global::PixelStacker.Resources.UIResources.country_jp;
            resources.ApplyResources(this.japaneseToolStripMenuItem, "japaneseToolStripMenuItem");
            this.japaneseToolStripMenuItem.Name = "japaneseToolStripMenuItem";
            this.japaneseToolStripMenuItem.Click += new System.EventHandler(this.japaneseToolStripMenuItem_Click);
            // 
            // koreanToolStripMenuItem
            // 
            this.koreanToolStripMenuItem.Image = global::PixelStacker.Resources.UIResources.country_kr;
            resources.ApplyResources(this.koreanToolStripMenuItem, "koreanToolStripMenuItem");
            this.koreanToolStripMenuItem.Name = "koreanToolStripMenuItem";
            this.koreanToolStripMenuItem.Click += new System.EventHandler(this.koreanToolStripMenuItem_Click);
            // 
            // spanishToolStripMenuItem
            // 
            this.spanishToolStripMenuItem.Image = global::PixelStacker.Resources.UIResources.country_mx;
            resources.ApplyResources(this.spanishToolStripMenuItem, "spanishToolStripMenuItem");
            this.spanishToolStripMenuItem.Name = "spanishToolStripMenuItem";
            this.spanishToolStripMenuItem.Click += new System.EventHandler(this.spanishToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // dlgOpen
            // 
            this.dlgOpen.FileName = "openFileDialog1";
            resources.ApplyResources(this.dlgOpen, "dlgOpen");
            this.dlgOpen.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgOpen_FileOk);
            // 
            // dlgSave
            // 
            this.dlgSave.DefaultExt = "schem";
            resources.ApplyResources(this.dlgSave, "dlgSave");
            this.dlgSave.FilterIndex = 2;
            this.dlgSave.ShowHelp = true;
            this.dlgSave.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgSave_FileOk);
            // 
            // progressBar
            // 
            resources.ApplyResources(this.progressBar, "progressBar");
            this.progressBar.Name = "progressBar";
            // 
            // lblStatus
            // 
            resources.ApplyResources(this.lblStatus, "lblStatus");
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Name = "lblStatus";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // dlgSaveColorPalette
            // 
            this.dlgSaveColorPalette.DefaultExt = "*.png";
            this.dlgSaveColorPalette.ShowHelp = true;
            resources.ApplyResources(this.dlgSaveColorPalette, "dlgSaveColorPalette");
            this.dlgSaveColorPalette.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgSaveColorPalette_FileOk_ColorPalettes);
            // 
            // imagePanelMain
            // 
            resources.ApplyResources(this.imagePanelMain, "imagePanelMain");
            this.imagePanelMain.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.imagePanelMain.Name = "imagePanelMain";
            // 
            // renderedImagePanel
            // 
            resources.ApplyResources(this.renderedImagePanel, "renderedImagePanel");
            this.renderedImagePanel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.renderedImagePanel.Name = "renderedImagePanel";
            // 
            // dlgSaveSettings
            // 
            this.dlgSaveSettings.DefaultExt = "*.json";
            resources.ApplyResources(this.dlgSaveSettings, "dlgSaveSettings");
            this.dlgSaveSettings.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgSaveSettings_FileOk);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.imagePanelMain);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.renderedImagePanel);
            this.Icon = global::PixelStacker.Resources.UIResources.wool;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
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
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private System.Windows.Forms.ToolStripMenuItem previewSchematicToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportSchematicToolStripMenuItem;
        public UI.RenderedImagePanel renderedImagePanel;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleGridToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleSolidColorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleBorderToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog dlgSave;
        public System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ToolStripMenuItem reOpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem togglePaletteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mi_PreRenderOptions;
        private System.Windows.Forms.ToolStripMenuItem mi_preRender;
        private System.Windows.Forms.ToolStripMenuItem toggleProgressToolStripMenuItem;
        public UI.ImagePanel imagePanelMain;
        private System.Windows.Forms.ToolStripMenuItem layerFilteringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleLayerFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem up1LayerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem down1LayerToolStripMenuItem;
        public System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ToolStripMenuItem otherOptionsToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem saveColorPaletteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem graphToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem brickToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allPossibilitiescompactToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allColorsdetailedToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem squareToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog dlgSaveColorPalette;
        private System.Windows.Forms.ToolStripMenuItem exportSettingsToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog dlgSaveSettings;
        private System.Windows.Forms.ToolStripMenuItem languageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem englishToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem danishToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem germanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spanishToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem japaneseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chineseSimplifiedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem koreanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem frenchToolStripMenuItem;
    }
}

