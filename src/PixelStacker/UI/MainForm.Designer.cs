namespace PixelStacker
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
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.imagePanelMain = new PixelStacker.UI.ImagePanel();
            this.renderedImagePanel = new PixelStacker.UI.RenderedImagePanel();
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
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(779, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.reOpenToolStripMenuItem,
            this.exportSchematicToolStripMenuItem,
            this.saveColorPaletteToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // reOpenToolStripMenuItem
            // 
            this.reOpenToolStripMenuItem.Enabled = false;
            this.reOpenToolStripMenuItem.Name = "reOpenToolStripMenuItem";
            this.reOpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.reOpenToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.reOpenToolStripMenuItem.Text = "Re-Open";
            this.reOpenToolStripMenuItem.Click += new System.EventHandler(this.reOpenToolStripMenuItem_Click);
            // 
            // exportSchematicToolStripMenuItem
            // 
            this.exportSchematicToolStripMenuItem.Enabled = false;
            this.exportSchematicToolStripMenuItem.Name = "exportSchematicToolStripMenuItem";
            this.exportSchematicToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.exportSchematicToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.exportSchematicToolStripMenuItem.Text = "Save";
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
            this.saveColorPaletteToolStripMenuItem.Enabled = false;
            this.saveColorPaletteToolStripMenuItem.Name = "saveColorPaletteToolStripMenuItem";
            this.saveColorPaletteToolStripMenuItem.ShowShortcutKeys = false;
            this.saveColorPaletteToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.saveColorPaletteToolStripMenuItem.Text = "Save color palette";
            // 
            // graphToolStripMenuItem
            // 
            this.graphToolStripMenuItem.Image = global::PixelStacker.Properties.Resources.feature_palette_compact_graph;
            this.graphToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.graphToolStripMenuItem.Name = "graphToolStripMenuItem";
            this.graphToolStripMenuItem.Size = new System.Drawing.Size(270, 62);
            this.graphToolStripMenuItem.Text = "Graph";
            this.graphToolStripMenuItem.Click += new System.EventHandler(this.graphToolStripMenuItem_Click);
            // 
            // brickToolStripMenuItem
            // 
            this.brickToolStripMenuItem.Image = global::PixelStacker.Properties.Resources.feature_palette_compact_brick;
            this.brickToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.brickToolStripMenuItem.Name = "brickToolStripMenuItem";
            this.brickToolStripMenuItem.Size = new System.Drawing.Size(270, 62);
            this.brickToolStripMenuItem.Text = "Brick";
            this.brickToolStripMenuItem.Click += new System.EventHandler(this.brickToolStripMenuItem_Click);
            // 
            // squareToolStripMenuItem
            // 
            this.squareToolStripMenuItem.Image = global::PixelStacker.Properties.Resources.feature_palette_compact_square;
            this.squareToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.squareToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.squareToolStripMenuItem.Name = "squareToolStripMenuItem";
            this.squareToolStripMenuItem.Size = new System.Drawing.Size(270, 62);
            this.squareToolStripMenuItem.Text = "Square";
            this.squareToolStripMenuItem.Click += new System.EventHandler(this.squareToolStripMenuItem_Click);
            // 
            // allPossibilitiescompactToolStripMenuItem
            // 
            this.allPossibilitiescompactToolStripMenuItem.Image = global::PixelStacker.Properties.Resources.feature_palette_compact_grid;
            this.allPossibilitiescompactToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.allPossibilitiescompactToolStripMenuItem.Name = "allPossibilitiescompactToolStripMenuItem";
            this.allPossibilitiescompactToolStripMenuItem.Size = new System.Drawing.Size(270, 62);
            this.allPossibilitiescompactToolStripMenuItem.Text = "All colors (compact)";
            this.allPossibilitiescompactToolStripMenuItem.Click += new System.EventHandler(this.allPossibilitiescompactToolStripMenuItem_Click);
            // 
            // allColorsdetailedToolStripMenuItem
            // 
            this.allColorsdetailedToolStripMenuItem.Image = global::PixelStacker.Properties.Resources.feature_palette_detailed_grid;
            this.allColorsdetailedToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.allColorsdetailedToolStripMenuItem.Name = "allColorsdetailedToolStripMenuItem";
            this.allColorsdetailedToolStripMenuItem.Size = new System.Drawing.Size(270, 62);
            this.allColorsdetailedToolStripMenuItem.Text = "All colors (detailed)";
            this.allColorsdetailedToolStripMenuItem.Click += new System.EventHandler(this.allColorsdetailedToolStripMenuItem_Click);
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
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(179, 26);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(179, 26);
            this.redoToolStripMenuItem.Text = "Redo";
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
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // toggleGridToolStripMenuItem
            // 
            this.toggleGridToolStripMenuItem.Enabled = false;
            this.toggleGridToolStripMenuItem.Name = "toggleGridToolStripMenuItem";
            this.toggleGridToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.toggleGridToolStripMenuItem.Size = new System.Drawing.Size(288, 26);
            this.toggleGridToolStripMenuItem.Text = "Toggle Grid";
            this.toggleGridToolStripMenuItem.Click += new System.EventHandler(this.toggleGridToolStripMenuItem_Click);
            // 
            // toggleSolidColorsToolStripMenuItem
            // 
            this.toggleSolidColorsToolStripMenuItem.Enabled = false;
            this.toggleSolidColorsToolStripMenuItem.Name = "toggleSolidColorsToolStripMenuItem";
            this.toggleSolidColorsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.toggleSolidColorsToolStripMenuItem.Size = new System.Drawing.Size(288, 26);
            this.toggleSolidColorsToolStripMenuItem.Text = "Toggle Hide Texture";
            this.toggleSolidColorsToolStripMenuItem.Click += new System.EventHandler(this.toggleSolidColorsToolStripMenuItem_Click);
            // 
            // toggleBorderToolStripMenuItem
            // 
            this.toggleBorderToolStripMenuItem.Enabled = false;
            this.toggleBorderToolStripMenuItem.Name = "toggleBorderToolStripMenuItem";
            this.toggleBorderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.toggleBorderToolStripMenuItem.Size = new System.Drawing.Size(288, 26);
            this.toggleBorderToolStripMenuItem.Text = "Toggle Border";
            this.toggleBorderToolStripMenuItem.Click += new System.EventHandler(this.toggleBorderToolStripMenuItem_Click);
            // 
            // togglePaletteToolStripMenuItem
            // 
            this.togglePaletteToolStripMenuItem.Enabled = false;
            this.togglePaletteToolStripMenuItem.Name = "togglePaletteToolStripMenuItem";
            this.togglePaletteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.togglePaletteToolStripMenuItem.Size = new System.Drawing.Size(288, 26);
            this.togglePaletteToolStripMenuItem.Text = "Toggle Palette";
            this.togglePaletteToolStripMenuItem.Click += new System.EventHandler(this.togglePaletteToolStripMenuItem_Click);
            // 
            // toggleProgressToolStripMenuItem
            // 
            this.toggleProgressToolStripMenuItem.Name = "toggleProgressToolStripMenuItem";
            this.toggleProgressToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.P)));
            this.toggleProgressToolStripMenuItem.Size = new System.Drawing.Size(288, 26);
            this.toggleProgressToolStripMenuItem.Text = "Toggle Progress";
            this.toggleProgressToolStripMenuItem.Click += new System.EventHandler(this.toggleProgressToolStripMenuItem_Click);
            // 
            // layerFilteringToolStripMenuItem
            // 
            this.layerFilteringToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleLayerFilterToolStripMenuItem,
            this.up1LayerToolStripMenuItem,
            this.down1LayerToolStripMenuItem});
            this.layerFilteringToolStripMenuItem.Enabled = false;
            this.layerFilteringToolStripMenuItem.Name = "layerFilteringToolStripMenuItem";
            this.layerFilteringToolStripMenuItem.Size = new System.Drawing.Size(288, 26);
            this.layerFilteringToolStripMenuItem.Text = "Layer Filtering";
            // 
            // toggleLayerFilterToolStripMenuItem
            // 
            this.toggleLayerFilterToolStripMenuItem.Name = "toggleLayerFilterToolStripMenuItem";
            this.toggleLayerFilterToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.toggleLayerFilterToolStripMenuItem.Size = new System.Drawing.Size(263, 26);
            this.toggleLayerFilterToolStripMenuItem.Text = "Toggle Layer Filter";
            this.toggleLayerFilterToolStripMenuItem.Click += new System.EventHandler(this.toggleLayerFilterToolStripMenuItem_Click);
            // 
            // up1LayerToolStripMenuItem
            // 
            this.up1LayerToolStripMenuItem.Enabled = false;
            this.up1LayerToolStripMenuItem.Name = "up1LayerToolStripMenuItem";
            this.up1LayerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up)));
            this.up1LayerToolStripMenuItem.Size = new System.Drawing.Size(263, 26);
            this.up1LayerToolStripMenuItem.Text = "Up 1 Layer";
            this.up1LayerToolStripMenuItem.Click += new System.EventHandler(this.up1LayerToolStripMenuItem_Click);
            // 
            // down1LayerToolStripMenuItem
            // 
            this.down1LayerToolStripMenuItem.Enabled = false;
            this.down1LayerToolStripMenuItem.Name = "down1LayerToolStripMenuItem";
            this.down1LayerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down)));
            this.down1LayerToolStripMenuItem.Size = new System.Drawing.Size(263, 26);
            this.down1LayerToolStripMenuItem.Text = "Down 1 Layer";
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
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(58, 24);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(275, 26);
            this.optionsToolStripMenuItem.Text = "Material Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // mi_PreRenderOptions
            // 
            this.mi_PreRenderOptions.Name = "mi_PreRenderOptions";
            this.mi_PreRenderOptions.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.mi_PreRenderOptions.Size = new System.Drawing.Size(275, 26);
            this.mi_PreRenderOptions.Text = "Quantizer Options";
            this.mi_PreRenderOptions.Click += new System.EventHandler(this.prerenderOptions_Click);
            // 
            // otherOptionsToolStripMenuItem
            // 
            this.otherOptionsToolStripMenuItem.Name = "otherOptionsToolStripMenuItem";
            this.otherOptionsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.E)));
            this.otherOptionsToolStripMenuItem.Size = new System.Drawing.Size(275, 26);
            this.otherOptionsToolStripMenuItem.Text = "Other Options";
            this.otherOptionsToolStripMenuItem.Click += new System.EventHandler(this.otherOptionsToolStripMenuItem_Click);
            // 
            // mi_preRender
            // 
            this.mi_preRender.Name = "mi_preRender";
            this.mi_preRender.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.R)));
            this.mi_preRender.Size = new System.Drawing.Size(275, 26);
            this.mi_preRender.Text = "Pre-Render";
            this.mi_preRender.Visible = false;
            this.mi_preRender.Click += new System.EventHandler(this.mi_preRender_Click);
            // 
            // previewSchematicToolStripMenuItem
            // 
            this.previewSchematicToolStripMenuItem.Name = "previewSchematicToolStripMenuItem";
            this.previewSchematicToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.previewSchematicToolStripMenuItem.Size = new System.Drawing.Size(275, 26);
            this.previewSchematicToolStripMenuItem.Text = "Render";
            this.previewSchematicToolStripMenuItem.Click += new System.EventHandler(this.renderSchematicToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(133, 26);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // dlgOpen
            // 
            this.dlgOpen.FileName = "openFileDialog1";
            this.dlgOpen.Filter = "png|*.png|Jpeg|*.jpg";
            this.dlgOpen.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgOpen_FileOk);
            // 
            // dlgSave
            // 
            this.dlgSave.DefaultExt = "schem";
            this.dlgSave.Filter = "Schem (1.13+)|*.schem|PNG|*.png|Schematic|*.schematic|Block Counts CSV|*.csv";
            this.dlgSave.ShowHelp = true;
            this.dlgSave.Title = "Save";
            this.dlgSave.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgSave_FileOk);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(0, 490);
            this.progressBar.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(779, 33);
            this.progressBar.TabIndex = 3;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Location = new System.Drawing.Point(16, 496);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.MinimumSize = new System.Drawing.Size(267, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(267, 17);
            this.lblStatus.TabIndex = 6;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // imagePanelMain
            // 
            this.imagePanelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imagePanelMain.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("imagePanelMain.BackgroundImage")));
            this.imagePanelMain.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.imagePanelMain.Location = new System.Drawing.Point(0, 33);
            this.imagePanelMain.Margin = new System.Windows.Forms.Padding(5);
            this.imagePanelMain.Name = "imagePanelMain";
            this.imagePanelMain.Size = new System.Drawing.Size(779, 458);
            this.imagePanelMain.TabIndex = 1;
            // 
            // renderedImagePanel
            // 
            this.renderedImagePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.renderedImagePanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("renderedImagePanel.BackgroundImage")));
            this.renderedImagePanel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.renderedImagePanel.Location = new System.Drawing.Point(0, 33);
            this.renderedImagePanel.Margin = new System.Windows.Forms.Padding(5);
            this.renderedImagePanel.Name = "renderedImagePanel";
            this.renderedImagePanel.Size = new System.Drawing.Size(779, 458);
            this.renderedImagePanel.TabIndex = 2;
            this.renderedImagePanel.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 523);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.imagePanelMain);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.renderedImagePanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Pangamma\'s PixelStacker";
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
    }
}

