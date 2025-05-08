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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            Logic.IO.Config.PanZoomSettings panZoomSettings3 = new Logic.IO.Config.PanZoomSettings();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            reOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            advancedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toggleAdvancedModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            checkSpigotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            checkGithubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            generationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            selectMaterialsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            preprocessingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ditheringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            sizingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            orientationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            horizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            verticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            renderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            preRenderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            colorMatchAlgorithmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            gridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toggleGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            gridOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toggleBorderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            layerFilteringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            showTopLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            showBothLayersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            showBottomLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            visualEnhancementsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toggleShadowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            textureSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            tsTextureSize16 = new System.Windows.Forms.ToolStripMenuItem();
            tsTextureSize32 = new System.Windows.Forms.ToolStripMenuItem();
            tsTextureSize64 = new System.Windows.Forms.ToolStripMenuItem();
            switchPanelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            contributorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            updateSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            chineseSimplifiedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            danishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            dutchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            frenchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            germanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            italianToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            japaneseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            koreanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            spanishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            swedishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            themesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            lightUIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            darkUIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            smoothThemeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            progressBar1 = new System.Windows.Forms.ProgressBar();
            timer1 = new System.Windows.Forms.Timer(components);
            lblProgress = new System.Windows.Forms.Label();
            imageViewer = new PixelStacker.WF.Components.ImageViewer();
            canvasEditor = new CanvasEditor();
            dlgOpen = new System.Windows.Forms.OpenFileDialog();
            dlgSave = new System.Windows.Forms.SaveFileDialog();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, generationToolStripMenuItem, viewToolStripMenuItem, helpToolStripMenuItem, playToolStripMenuItem, languageToolStripMenuItem, themesToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            menuStrip1.Size = new System.Drawing.Size(1000, 33);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { openToolStripMenuItem, reOpenToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem, advancedToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O;
            openToolStripMenuItem.Size = new System.Drawing.Size(285, 34);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // reOpenToolStripMenuItem
            // 
            reOpenToolStripMenuItem.Name = "reOpenToolStripMenuItem";
            reOpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            reOpenToolStripMenuItem.Size = new System.Drawing.Size(285, 34);
            reOpenToolStripMenuItem.Text = "Re-Open";
            reOpenToolStripMenuItem.Click += reOpenToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            saveToolStripMenuItem.Size = new System.Drawing.Size(285, 34);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.S;
            saveAsToolStripMenuItem.Size = new System.Drawing.Size(285, 34);
            saveAsToolStripMenuItem.Text = "Save As";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // advancedToolStripMenuItem
            // 
            advancedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toggleAdvancedModeToolStripMenuItem, checkForUpdatesToolStripMenuItem });
            advancedToolStripMenuItem.Name = "advancedToolStripMenuItem";
            advancedToolStripMenuItem.Size = new System.Drawing.Size(285, 34);
            advancedToolStripMenuItem.Text = "Advanced";
            // 
            // toggleAdvancedModeToolStripMenuItem
            // 
            toggleAdvancedModeToolStripMenuItem.Name = "toggleAdvancedModeToolStripMenuItem";
            toggleAdvancedModeToolStripMenuItem.Size = new System.Drawing.Size(299, 34);
            toggleAdvancedModeToolStripMenuItem.Text = "Enable advanced mode";
            toggleAdvancedModeToolStripMenuItem.Click += toggleAdvancedModeToolStripMenuItem_Click;
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            checkForUpdatesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { checkSpigotToolStripMenuItem, checkGithubToolStripMenuItem });
            checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(299, 34);
            checkForUpdatesToolStripMenuItem.Text = "Check for updates";
            // 
            // checkSpigotToolStripMenuItem
            // 
            checkSpigotToolStripMenuItem.Name = "checkSpigotToolStripMenuItem";
            checkSpigotToolStripMenuItem.Size = new System.Drawing.Size(219, 34);
            checkSpigotToolStripMenuItem.Text = "Check Spigot";
            checkSpigotToolStripMenuItem.Click += checkSpigotToolStripMenuItem_Click;
            // 
            // checkGithubToolStripMenuItem
            // 
            checkGithubToolStripMenuItem.Name = "checkGithubToolStripMenuItem";
            checkGithubToolStripMenuItem.Size = new System.Drawing.Size(219, 34);
            checkGithubToolStripMenuItem.Text = "Check Github";
            checkGithubToolStripMenuItem.Click += checkGithubToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { undoToolStripMenuItem, redoToolStripMenuItem });
            editToolStripMenuItem.Enabled = false;
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new System.Drawing.Size(58, 29);
            editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            undoToolStripMenuItem.Image = Resources.UIResources.undo;
            undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            undoToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z;
            undoToolStripMenuItem.Size = new System.Drawing.Size(219, 34);
            undoToolStripMenuItem.Text = "Undo";
            undoToolStripMenuItem.Click += undoToolStripMenuItem_Click;
            // 
            // redoToolStripMenuItem
            // 
            redoToolStripMenuItem.Image = Resources.UIResources.redo;
            redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            redoToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y;
            redoToolStripMenuItem.Size = new System.Drawing.Size(219, 34);
            redoToolStripMenuItem.Text = "Redo";
            redoToolStripMenuItem.Click += redoToolStripMenuItem_Click;
            // 
            // generationToolStripMenuItem
            // 
            generationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { selectMaterialsToolStripMenuItem, preprocessingToolStripMenuItem, ditheringToolStripMenuItem, sizingToolStripMenuItem, orientationToolStripMenuItem, renderToolStripMenuItem, preRenderToolStripMenuItem, colorMatchAlgorithmToolStripMenuItem });
            generationToolStripMenuItem.Name = "generationToolStripMenuItem";
            generationToolStripMenuItem.Size = new System.Drawing.Size(98, 29);
            generationToolStripMenuItem.Text = "Generate";
            // 
            // selectMaterialsToolStripMenuItem
            // 
            selectMaterialsToolStripMenuItem.Name = "selectMaterialsToolStripMenuItem";
            selectMaterialsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E;
            selectMaterialsToolStripMenuItem.Size = new System.Drawing.Size(311, 34);
            selectMaterialsToolStripMenuItem.Text = "Select Materials";
            selectMaterialsToolStripMenuItem.Click += selectMaterialsToolStripMenuItem_Click;
            // 
            // preprocessingToolStripMenuItem
            // 
            preprocessingToolStripMenuItem.Name = "preprocessingToolStripMenuItem";
            preprocessingToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q;
            preprocessingToolStripMenuItem.Size = new System.Drawing.Size(311, 34);
            preprocessingToolStripMenuItem.Text = "Color Reduction";
            preprocessingToolStripMenuItem.Click += preprocessingToolStripMenuItem_Click;
            // 
            // ditheringToolStripMenuItem
            // 
            ditheringToolStripMenuItem.Name = "ditheringToolStripMenuItem";
            ditheringToolStripMenuItem.Size = new System.Drawing.Size(311, 34);
            ditheringToolStripMenuItem.Text = "Dithering";
            ditheringToolStripMenuItem.Click += preprocessingToolStripMenuItem_Click;
            // 
            // sizingToolStripMenuItem
            // 
            sizingToolStripMenuItem.Name = "sizingToolStripMenuItem";
            sizingToolStripMenuItem.Size = new System.Drawing.Size(311, 34);
            sizingToolStripMenuItem.Text = "Sizing";
            sizingToolStripMenuItem.Click += sizingToolStripMenuItem_Click;
            // 
            // orientationToolStripMenuItem
            // 
            orientationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { horizontalToolStripMenuItem, verticalToolStripMenuItem });
            orientationToolStripMenuItem.Name = "orientationToolStripMenuItem";
            orientationToolStripMenuItem.Size = new System.Drawing.Size(311, 34);
            orientationToolStripMenuItem.Text = "Orientation";
            // 
            // horizontalToolStripMenuItem
            // 
            horizontalToolStripMenuItem.Checked = true;
            horizontalToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            horizontalToolStripMenuItem.Name = "horizontalToolStripMenuItem";
            horizontalToolStripMenuItem.Size = new System.Drawing.Size(196, 34);
            horizontalToolStripMenuItem.Text = "Horizontal";
            horizontalToolStripMenuItem.Click += horizontalToolStripMenuItem_Click;
            // 
            // verticalToolStripMenuItem
            // 
            verticalToolStripMenuItem.Name = "verticalToolStripMenuItem";
            verticalToolStripMenuItem.Size = new System.Drawing.Size(196, 34);
            verticalToolStripMenuItem.Text = "Vertical";
            verticalToolStripMenuItem.Click += verticalToolStripMenuItem_Click;
            // 
            // renderToolStripMenuItem
            // 
            renderToolStripMenuItem.Name = "renderToolStripMenuItem";
            renderToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R;
            renderToolStripMenuItem.Size = new System.Drawing.Size(311, 34);
            renderToolStripMenuItem.Text = "Render";
            renderToolStripMenuItem.Click += renderToolStripMenuItem_Click;
            // 
            // preRenderToolStripMenuItem
            // 
            preRenderToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            preRenderToolStripMenuItem.Name = "preRenderToolStripMenuItem";
            preRenderToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.R;
            preRenderToolStripMenuItem.Size = new System.Drawing.Size(311, 34);
            preRenderToolStripMenuItem.Text = "Pre-Render";
            preRenderToolStripMenuItem.Click += preRenderToolStripMenuItem_Click;
            // 
            // colorMatchAlgorithmToolStripMenuItem
            // 
            colorMatchAlgorithmToolStripMenuItem.BackColor = System.Drawing.Color.Aqua;
            colorMatchAlgorithmToolStripMenuItem.Name = "colorMatchAlgorithmToolStripMenuItem";
            colorMatchAlgorithmToolStripMenuItem.Size = new System.Drawing.Size(311, 34);
            colorMatchAlgorithmToolStripMenuItem.Text = "Color Match Algorithm";
            colorMatchAlgorithmToolStripMenuItem.Click += colorMatchAlgorithmToolStripMenuItem_Click;
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { gridToolStripMenuItem, toggleBorderToolStripMenuItem, layerFilteringToolStripMenuItem, visualEnhancementsToolStripMenuItem, switchPanelsToolStripMenuItem });
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new System.Drawing.Size(65, 29);
            viewToolStripMenuItem.Text = "View";
            // 
            // gridToolStripMenuItem
            // 
            gridToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toggleGridToolStripMenuItem, gridOptionsToolStripMenuItem });
            gridToolStripMenuItem.Name = "gridToolStripMenuItem";
            gridToolStripMenuItem.Size = new System.Drawing.Size(295, 34);
            gridToolStripMenuItem.Text = "Grid";
            // 
            // toggleGridToolStripMenuItem
            // 
            toggleGridToolStripMenuItem.Name = "toggleGridToolStripMenuItem";
            toggleGridToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G;
            toggleGridToolStripMenuItem.Size = new System.Drawing.Size(268, 34);
            toggleGridToolStripMenuItem.Text = "Toggle Grid";
            toggleGridToolStripMenuItem.Click += toggleGridToolStripMenuItem_Click;
            // 
            // gridOptionsToolStripMenuItem
            // 
            gridOptionsToolStripMenuItem.Name = "gridOptionsToolStripMenuItem";
            gridOptionsToolStripMenuItem.Size = new System.Drawing.Size(268, 34);
            gridOptionsToolStripMenuItem.Text = "Grid Options";
            gridOptionsToolStripMenuItem.Click += gridOptionsToolStripMenuItem_Click;
            // 
            // toggleBorderToolStripMenuItem
            // 
            toggleBorderToolStripMenuItem.Name = "toggleBorderToolStripMenuItem";
            toggleBorderToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B;
            toggleBorderToolStripMenuItem.Size = new System.Drawing.Size(295, 34);
            toggleBorderToolStripMenuItem.Text = "Toggle Border";
            toggleBorderToolStripMenuItem.Click += toggleBorderToolStripMenuItem_Click;
            // 
            // layerFilteringToolStripMenuItem
            // 
            layerFilteringToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { showTopLayerToolStripMenuItem, showBothLayersToolStripMenuItem, showBottomLayerToolStripMenuItem });
            layerFilteringToolStripMenuItem.Name = "layerFilteringToolStripMenuItem";
            layerFilteringToolStripMenuItem.Size = new System.Drawing.Size(295, 34);
            layerFilteringToolStripMenuItem.Text = "Layer Filtering";
            // 
            // showTopLayerToolStripMenuItem
            // 
            showTopLayerToolStripMenuItem.Name = "showTopLayerToolStripMenuItem";
            showTopLayerToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up;
            showTopLayerToolStripMenuItem.Size = new System.Drawing.Size(367, 34);
            showTopLayerToolStripMenuItem.Text = "Show Top Layer";
            showTopLayerToolStripMenuItem.Click += showTopLayerToolStripMenuItem_Click;
            // 
            // showBothLayersToolStripMenuItem
            // 
            showBothLayersToolStripMenuItem.Checked = true;
            showBothLayersToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            showBothLayersToolStripMenuItem.Name = "showBothLayersToolStripMenuItem";
            showBothLayersToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Left;
            showBothLayersToolStripMenuItem.Size = new System.Drawing.Size(367, 34);
            showBothLayersToolStripMenuItem.Text = "Show Both Layers";
            showBothLayersToolStripMenuItem.Click += showBothLayersToolStripMenuItem_Click;
            // 
            // showBottomLayerToolStripMenuItem
            // 
            showBottomLayerToolStripMenuItem.Name = "showBottomLayerToolStripMenuItem";
            showBottomLayerToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down;
            showBottomLayerToolStripMenuItem.Size = new System.Drawing.Size(367, 34);
            showBottomLayerToolStripMenuItem.Text = "Show Bottom Layer";
            showBottomLayerToolStripMenuItem.Click += showBottomLayerToolStripMenuItem_Click;
            // 
            // visualEnhancementsToolStripMenuItem
            // 
            visualEnhancementsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toggleShadowsToolStripMenuItem, textureSizeToolStripMenuItem });
            visualEnhancementsToolStripMenuItem.Name = "visualEnhancementsToolStripMenuItem";
            visualEnhancementsToolStripMenuItem.Size = new System.Drawing.Size(295, 34);
            visualEnhancementsToolStripMenuItem.Text = "Visual Enhancements";
            // 
            // toggleShadowsToolStripMenuItem
            // 
            toggleShadowsToolStripMenuItem.Name = "toggleShadowsToolStripMenuItem";
            toggleShadowsToolStripMenuItem.Size = new System.Drawing.Size(246, 34);
            toggleShadowsToolStripMenuItem.Text = "Render Shadows";
            toggleShadowsToolStripMenuItem.Click += toggleShadowsToolStripMenuItem_Click;
            // 
            // textureSizeToolStripMenuItem
            // 
            textureSizeToolStripMenuItem.BackColor = System.Drawing.Color.Aqua;
            textureSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsTextureSize16, tsTextureSize32, tsTextureSize64 });
            textureSizeToolStripMenuItem.Name = "textureSizeToolStripMenuItem";
            textureSizeToolStripMenuItem.Size = new System.Drawing.Size(246, 34);
            textureSizeToolStripMenuItem.Text = "Texture Size";
            // 
            // tsTextureSize16
            // 
            tsTextureSize16.Name = "tsTextureSize16";
            tsTextureSize16.Size = new System.Drawing.Size(134, 34);
            tsTextureSize16.Text = "16";
            tsTextureSize16.Click += tsTextureSize16_Click;
            // 
            // tsTextureSize32
            // 
            tsTextureSize32.Name = "tsTextureSize32";
            tsTextureSize32.Size = new System.Drawing.Size(134, 34);
            tsTextureSize32.Text = "32";
            tsTextureSize32.Click += tsTextureSize32_Click;
            // 
            // tsTextureSize64
            // 
            tsTextureSize64.Name = "tsTextureSize64";
            tsTextureSize64.Size = new System.Drawing.Size(134, 34);
            tsTextureSize64.Text = "64";
            tsTextureSize64.Click += tsTextureSize64_Click;
            // 
            // switchPanelsToolStripMenuItem
            // 
            switchPanelsToolStripMenuItem.Name = "switchPanelsToolStripMenuItem";
            switchPanelsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T;
            switchPanelsToolStripMenuItem.Size = new System.Drawing.Size(295, 34);
            switchPanelsToolStripMenuItem.Text = "Toggle Textures";
            switchPanelsToolStripMenuItem.Click += switchPanelsToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { contributorsToolStripMenuItem, updateSettingsToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new System.Drawing.Size(65, 29);
            helpToolStripMenuItem.Text = "Help";
            helpToolStripMenuItem.Visible = false;
            // 
            // contributorsToolStripMenuItem
            // 
            contributorsToolStripMenuItem.Name = "contributorsToolStripMenuItem";
            contributorsToolStripMenuItem.Size = new System.Drawing.Size(241, 34);
            contributorsToolStripMenuItem.Text = "Contributors";
            contributorsToolStripMenuItem.Click += contributorsToolStripMenuItem_Click;
            // 
            // updateSettingsToolStripMenuItem
            // 
            updateSettingsToolStripMenuItem.Name = "updateSettingsToolStripMenuItem";
            updateSettingsToolStripMenuItem.Size = new System.Drawing.Size(241, 34);
            updateSettingsToolStripMenuItem.Text = "Update Settings";
            // 
            // playToolStripMenuItem
            // 
            playToolStripMenuItem.Name = "playToolStripMenuItem";
            playToolStripMenuItem.Size = new System.Drawing.Size(44, 29);
            playToolStripMenuItem.Text = "▶";
            playToolStripMenuItem.Click += renderToolStripMenuItem_Click;
            // 
            // languageToolStripMenuItem
            // 
            languageToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            languageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { englishToolStripMenuItem, chineseSimplifiedToolStripMenuItem, danishToolStripMenuItem, dutchToolStripMenuItem, frenchToolStripMenuItem, germanToolStripMenuItem, italianToolStripMenuItem, japaneseToolStripMenuItem, koreanToolStripMenuItem, spanishToolStripMenuItem, swedishToolStripMenuItem });
            languageToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("languageToolStripMenuItem.Image");
            languageToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            languageToolStripMenuItem.MergeIndex = 50;
            languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            languageToolStripMenuItem.Size = new System.Drawing.Size(100, 29);
            languageToolStripMenuItem.Text = "English";
            languageToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // englishToolStripMenuItem
            // 
            englishToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            englishToolStripMenuItem.Size = new System.Drawing.Size(269, 34);
            englishToolStripMenuItem.Text = "English";
            englishToolStripMenuItem.Click += englishToolStripMenuItem_Click;
            // 
            // chineseSimplifiedToolStripMenuItem
            // 
            chineseSimplifiedToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            chineseSimplifiedToolStripMenuItem.Name = "chineseSimplifiedToolStripMenuItem";
            chineseSimplifiedToolStripMenuItem.Size = new System.Drawing.Size(269, 34);
            chineseSimplifiedToolStripMenuItem.Text = "Chinese (Simplified)";
            chineseSimplifiedToolStripMenuItem.Click += chineseSimplifiedToolStripMenuItem_Click;
            // 
            // danishToolStripMenuItem
            // 
            danishToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            danishToolStripMenuItem.Name = "danishToolStripMenuItem";
            danishToolStripMenuItem.Size = new System.Drawing.Size(269, 34);
            danishToolStripMenuItem.Text = "Danish";
            danishToolStripMenuItem.Click += danishToolStripMenuItem_Click;
            // 
            // dutchToolStripMenuItem
            // 
            dutchToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            dutchToolStripMenuItem.Name = "dutchToolStripMenuItem";
            dutchToolStripMenuItem.Size = new System.Drawing.Size(269, 34);
            dutchToolStripMenuItem.Text = "Dutch";
            dutchToolStripMenuItem.Click += dutchToolStripMenuItem_Click;
            // 
            // frenchToolStripMenuItem
            // 
            frenchToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            frenchToolStripMenuItem.Name = "frenchToolStripMenuItem";
            frenchToolStripMenuItem.Size = new System.Drawing.Size(269, 34);
            frenchToolStripMenuItem.Text = "French";
            frenchToolStripMenuItem.Click += frenchToolStripMenuItem_Click;
            // 
            // germanToolStripMenuItem
            // 
            germanToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            germanToolStripMenuItem.Name = "germanToolStripMenuItem";
            germanToolStripMenuItem.Size = new System.Drawing.Size(269, 34);
            germanToolStripMenuItem.Text = "German";
            germanToolStripMenuItem.Click += germanToolStripMenuItem_Click;
            // 
            // italianToolStripMenuItem
            // 
            italianToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            italianToolStripMenuItem.Name = "italianToolStripMenuItem";
            italianToolStripMenuItem.Size = new System.Drawing.Size(269, 34);
            italianToolStripMenuItem.Text = "Italian";
            italianToolStripMenuItem.Click += italianToolStripMenuItem_Click;
            // 
            // japaneseToolStripMenuItem
            // 
            japaneseToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            japaneseToolStripMenuItem.Name = "japaneseToolStripMenuItem";
            japaneseToolStripMenuItem.Size = new System.Drawing.Size(269, 34);
            japaneseToolStripMenuItem.Text = "Japanese";
            japaneseToolStripMenuItem.Click += japaneseToolStripMenuItem_Click;
            // 
            // koreanToolStripMenuItem
            // 
            koreanToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            koreanToolStripMenuItem.Name = "koreanToolStripMenuItem";
            koreanToolStripMenuItem.Size = new System.Drawing.Size(269, 34);
            koreanToolStripMenuItem.Text = "Korean";
            koreanToolStripMenuItem.Click += koreanToolStripMenuItem_Click;
            // 
            // spanishToolStripMenuItem
            // 
            spanishToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            spanishToolStripMenuItem.Name = "spanishToolStripMenuItem";
            spanishToolStripMenuItem.Size = new System.Drawing.Size(269, 34);
            spanishToolStripMenuItem.Text = "Spanish";
            spanishToolStripMenuItem.Click += spanishToolStripMenuItem_Click;
            // 
            // swedishToolStripMenuItem
            // 
            swedishToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            swedishToolStripMenuItem.Name = "swedishToolStripMenuItem";
            swedishToolStripMenuItem.Size = new System.Drawing.Size(269, 34);
            swedishToolStripMenuItem.Text = "Swedish";
            swedishToolStripMenuItem.Click += swedishToolStripMenuItem_Click;
            // 
            // themesToolStripMenuItem
            // 
            themesToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            themesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { lightUIToolStripMenuItem, darkUIToolStripMenuItem, smoothThemeToolStripMenuItem });
            themesToolStripMenuItem.Image = Resources.UIResources.color;
            themesToolStripMenuItem.Name = "themesToolStripMenuItem";
            themesToolStripMenuItem.Size = new System.Drawing.Size(101, 29);
            themesToolStripMenuItem.Text = "Theme";
            // 
            // lightUIToolStripMenuItem
            // 
            lightUIToolStripMenuItem.Name = "lightUIToolStripMenuItem";
            lightUIToolStripMenuItem.Size = new System.Drawing.Size(236, 34);
            lightUIToolStripMenuItem.Text = "Light Theme";
            lightUIToolStripMenuItem.Click += lightThemeToolStripMenuItem_Click;
            // 
            // darkUIToolStripMenuItem
            // 
            darkUIToolStripMenuItem.Name = "darkUIToolStripMenuItem";
            darkUIToolStripMenuItem.Size = new System.Drawing.Size(236, 34);
            darkUIToolStripMenuItem.Text = "Dark Theme";
            darkUIToolStripMenuItem.Visible = false;
            darkUIToolStripMenuItem.Click += darkThemeToolStripMenuItem_Click;
            // 
            // smoothThemeToolStripMenuItem
            // 
            smoothThemeToolStripMenuItem.Name = "smoothThemeToolStripMenuItem";
            smoothThemeToolStripMenuItem.Size = new System.Drawing.Size(236, 34);
            smoothThemeToolStripMenuItem.Text = "Smooth Theme";
            smoothThemeToolStripMenuItem.Click += smoothThemeToolStripMenuItem_Click;
            // 
            // progressBar1
            // 
            progressBar1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            progressBar1.Location = new System.Drawing.Point(0, 525);
            progressBar1.Margin = new System.Windows.Forms.Padding(4);
            progressBar1.MarqueeAnimationSpeed = 200;
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(1000, 36);
            progressBar1.TabIndex = 1;
            progressBar1.Value = 1;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Tick += timer1_Tick;
            // 
            // lblProgress
            // 
            lblProgress.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblProgress.AutoSize = true;
            lblProgress.BackColor = System.Drawing.Color.Transparent;
            lblProgress.LiveSetting = System.Windows.Forms.Automation.AutomationLiveSetting.Polite;
            lblProgress.Location = new System.Drawing.Point(6, 530);
            lblProgress.Margin = new System.Windows.Forms.Padding(0);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new System.Drawing.Size(208, 25);
            lblProgress.TabIndex = 2;
            lblProgress.Text = "Progress status message";
            // 
            // imageViewer
            // 
            imageViewer.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            imageViewer.BackgroundImage = (System.Drawing.Image)resources.GetObject("imageViewer.BackgroundImage");
            imageViewer.Location = new System.Drawing.Point(0, 35);
            imageViewer.Margin = new System.Windows.Forms.Padding(5);
            imageViewer.Name = "imageViewer";
            imageViewer.PanZoomSettings = panZoomSettings3;
            imageViewer.Size = new System.Drawing.Size(1000, 491);
            imageViewer.TabIndex = 5;
            // 
            // canvasEditor
            // 
            canvasEditor.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            canvasEditor.BackgroundImage = (System.Drawing.Image)resources.GetObject("canvasEditor.BackgroundImage");
            canvasEditor.Location = new System.Drawing.Point(0, 35);
            canvasEditor.MainForm = null;
            canvasEditor.Margin = new System.Windows.Forms.Padding(5);
            canvasEditor.Name = "canvasEditor";
            canvasEditor.Options = null;
            canvasEditor.PanZoomSettings = null;
            canvasEditor.RepaintRequested = false;
            canvasEditor.Size = new System.Drawing.Size(1000, 491);
            canvasEditor.TabIndex = 6;
            // 
            // dlgOpen
            // 
            dlgOpen.FileName = "openFileDialog1";
            dlgOpen.Filter = "Supported Format|*.png;*.jpeg;*.jpg;*pxlzip;*.webp|png|*.png|Jpeg|*.jpg;*.jpeg|webp|*.webp|PixelStacker Project|*.pxlzip|All Files|*.*";
            dlgOpen.Title = "Open File";
            dlgOpen.FileOk += dlgOpen_FileOk;
            // 
            // dlgSave
            // 
            dlgSave.DefaultExt = "pxlzip";
            dlgSave.Filter = "Schem (1.13+)|*.schem|PNG|*.png|Small PNG|*.sm.png|Block Counts CSV|*.csv|PixelStacker Project|*.pxlzip|Structure Block|*.nbt";
            dlgSave.FileOk += dlgSave_FileOk;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            ClientSize = new System.Drawing.Size(1000, 562);
            Controls.Add(lblProgress);
            Controls.Add(progressBar1);
            Controls.Add(menuStrip1);
            Controls.Add(imageViewer);
            Controls.Add(canvasEditor);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(4);
            Name = "MainForm";
            Text = "MainForm";
            Load += MainForm_Load;
            ResizeBegin += MainForm_ResizeBegin;
            ResizeEnd += MainForm_ResizeEnd;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem visualEnhancementsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem orientationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem horizontalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verticalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renderToolStripMenuItem;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblProgress;
        public WF.Components.ImageViewer imageViewer;
        public CanvasEditor canvasEditor;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private System.Windows.Forms.ToolStripMenuItem reOpenToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog dlgSave;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem switchPanelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ditheringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem swedishToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem themesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contributorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem italianToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleShadowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textureSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsTextureSize16;
        private System.Windows.Forms.ToolStripMenuItem tsTextureSize32;
        private System.Windows.Forms.ToolStripMenuItem tsTextureSize64;
        private System.Windows.Forms.ToolStripMenuItem lightUIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem darkUIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smoothThemeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preRenderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem advancedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleAdvancedModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkSpigotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkGithubToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorMatchAlgorithmToolStripMenuItem;
    }
}