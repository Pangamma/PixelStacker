using PixelStacker.Logic.IO.Config;
using PixelStacker.Resources;
using PixelStacker.Resources.Localization;
using System;
using System.Drawing;
using System.Globalization;
using System.Threading;

namespace PixelStacker.UI
{

    public partial class MainForm : ILocalized
    {
        private static Bitmap GetLanguageImage(CultureInfo locale)
        {
            switch (locale.TwoLetterISOLanguageName.ToLowerInvariant())
            {
                case "en": return UIResources.country_us;
                case "es": return UIResources.country_mx;
                case "nl": return UIResources.country_nl;
                case "zh": return UIResources.country_cn;
                case "da": return UIResources.country_dk;
                case "fr": return UIResources.country_fr;
                case "de": return UIResources.country_de;
                case "ja": return UIResources.country_jp;
                case "ko": return UIResources.country_kr;
                case "sv": return UIResources.country_se;
                case "it": return UIResources.country_it;
                default:
                    System.Diagnostics.Debug.WriteLine("Unknown locale detected. " + locale.TwoLetterISOLanguageName);
                    return UIResources.country_us;
            }
        }

        private static string GetLanguageName(CultureInfo locale)
        {
            switch (locale.TwoLetterISOLanguageName.ToLowerInvariant())
            {
                case "en": return global::PixelStacker.Resources.Text.MainMenu_Lang_English;
                case "es": return global::PixelStacker.Resources.Text.MainMenu_Lang_Spanish;
                case "nl": return global::PixelStacker.Resources.Text.MainMenu_Lang_Dutch;
                case "zh": return global::PixelStacker.Resources.Text.MainMenu_Lang_Chinese;
                case "da": return global::PixelStacker.Resources.Text.MainMenu_Lang_Danish;
                case "fr": return global::PixelStacker.Resources.Text.MainMenu_Lang_French;
                case "de": return global::PixelStacker.Resources.Text.MainMenu_Lang_German;
                case "ja": return global::PixelStacker.Resources.Text.MainMenu_Lang_Japanese;
                case "ko": return global::PixelStacker.Resources.Text.MainMenu_Lang_Korean;
                case "sv": return global::PixelStacker.Resources.Text.MainMenu_Lang_Swedish;
                case "it": return global::PixelStacker.Resources.Text.MainMenu_Lang_Italian;
                default:
                    System.Diagnostics.Debug.WriteLine("Unknown locale detected. " + locale.TwoLetterISOLanguageName);
                    return global::PixelStacker.Resources.Text.MainMenu_Lang_English;
            }
        }

        public void ApplyLocalization(CultureInfo locale)
        {
            this.Options.Locale = ResxHelper.GetSupportedLocale(locale.Name);
            locale = CultureInfo.GetCultureInfo(this.Options.Locale);
            Thread.CurrentThread.CurrentUICulture = locale;
            CultureInfo.CurrentUICulture = locale;
            this.ApplyLocalization();
            this.Options.Save();
        }

        public void ApplyLocalization()
        {
            this.MaterialOptions?.ApplyLocalization();
            this.ColorReducerForm?.ApplyLocalization();
            this.canvasEditor?.ApplyLocalization();
            this.MaterialPickerForm?.ApplyLocalization();

            this.Text = global::PixelStacker.Resources.Text.MainForm_Title;
            this.Text = this.Text + " v" + Constants.Version;
#if DEBUG
            this.Text += " (Debug)";
#endif
#if USE_GPU
            this.Text += " [GPU]";
#endif
            this.Text += " x" + (Constants.C_IS_64BIT ? 64 : 32);

            this.themesToolStripMenuItem.Text = global::PixelStacker.Resources.Text.Theme;
            this.darkUIToolStripMenuItem.Text = global::PixelStacker.Resources.Text.Dark_Theme;
            this.lightUIToolStripMenuItem.Text = global::PixelStacker.Resources.Text.Light_Theme;
            this.smoothThemeToolStripMenuItem.Text = global::PixelStacker.Resources.Text.Smooth_Theme;

            this.englishToolStripMenuItem.Image = UIResources.country_us;
            this.chineseSimplifiedToolStripMenuItem.Image = UIResources.country_cn;
            this.koreanToolStripMenuItem.Image = UIResources.country_kr;
            this.japaneseToolStripMenuItem.Image = UIResources.country_jp;
            this.spanishToolStripMenuItem.Image = UIResources.country_mx;
            this.germanToolStripMenuItem.Image = UIResources.country_de;
            this.italianToolStripMenuItem.Image = UIResources.country_it;
            this.danishToolStripMenuItem.Image = UIResources.country_dk;
            this.dutchToolStripMenuItem.Image = UIResources.country_nl;
            this.frenchToolStripMenuItem.Image = UIResources.country_fr;
            this.swedishToolStripMenuItem.Image = UIResources.country_se;


            this.dutchToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Lang_Dutch;
            this.spanishToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Lang_Spanish;
            this.koreanToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Lang_Korean;
            this.japaneseToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Lang_Japanese;
            this.germanToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Lang_German;
            this.frenchToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Lang_French;
            this.danishToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Lang_Danish;
            this.chineseSimplifiedToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Lang_Chinese;
            this.englishToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Lang_English;
            this.swedishToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Lang_Swedish;
            this.languageToolStripMenuItem.Image = MainForm.GetLanguageImage(CultureInfo.CurrentUICulture);
            this.languageToolStripMenuItem.Text = MainForm.GetLanguageName(CultureInfo.CurrentUICulture);

            this.dlgSave.Title = global::PixelStacker.Resources.Text.Action_Save;
            this.dlgOpen.Title = global::PixelStacker.Resources.Text.MainForm_Open_Title;

            //this.aboutToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_About;
            //this.helpToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Help;
            //this.dlgSaveSettings.Title = global::PixelStacker.Resources.Text.MainMenu_SaveSettings;
            //this.dlgSaveColorPalette.Title = global::PixelStacker.Resources.Text.MainMenu_SaveColorPalette;

            //this.languageToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Language;
            //this.previewSchematicToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Render;
            //this.mi_preRender.Text = global::PixelStacker.Resources.Text.MainMenu_PreRender;
            //this.otherOptionsToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_OtherOptions;
            //this.mi_PreRenderOptions.Text = global::PixelStacker.Resources.Text.MainMenu_QuantizerOptions;
            //this.toolsToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Tools;
            //this.down1LayerToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_DownOneLayer;
            //this.toggleProgressToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_ToggleProgress;
            //this.togglePaletteToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_TogglePalette;
            //this.toggleSolidColorsToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_ToggleHideTexture;
            //this.exportSettingsToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_ExportSettings;
            //this.allColorsdetailedToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_CP_AllColors_Detailed;
            //this.allPossibilitiescompactToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_CP_AllColors_Compact;
            //this.squareToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_CP_Square;
            //this.brickToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_CP_Brick;
            //this.graphToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_CP_Graph;
            //this.saveColorPaletteToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_SaveColorPalette;
            //this.exportSchematicToolStripMenuItem.Text = global::PixelStacker.Resources.Text.Action_Save;
            this.switchPanelsToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_ToggleTextures;
            this.visualEnhancementsToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_ShadowRendering;
            this.gridOptionsToolStripMenuItem.Text = global::PixelStacker.Resources.Text.GridOptions;
            this.gridToolStripMenuItem.Text = global::PixelStacker.Resources.Text.Grid;

            this.advancedToolStripMenuItem.Text = global::PixelStacker.Resources.Text.Advanced;
            this.checkForUpdatesToolStripMenuItem.Text =  global::PixelStacker.Resources.Text.CheckForUpdates;
            this.checkGithubToolStripMenuItem.Text =  global::PixelStacker.Resources.Text.CheckGithub;
            this.checkSpigotToolStripMenuItem.Text =  global::PixelStacker.Resources.Text.CheckSpigot;
            this.toggleAdvancedModeToolStripMenuItem.Text = this.Options.IsAdvancedModeEnabled ? Resources.Text.DisableAdvancedMode : Resources.Text.EnableAdvancedMode;
            this.toggleBorderToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_ToggleBorder;
            this.toggleGridToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_ToggleGrid;
            this.viewToolStripMenuItem.Text = global::PixelStacker.Resources.Text.Action_View;
            this.redoToolStripMenuItem.Text = global::PixelStacker.Resources.Text.Action_Redo;
            this.undoToolStripMenuItem.Text = global::PixelStacker.Resources.Text.Action_Undo;
            this.editToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Edit;
            this.generationToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Generate;
            this.selectMaterialsToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_SelectMaterials;
            this.renderToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Render;
            this.preRenderToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_PreRender;

            this.preprocessingToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_ColorReduction;
            this.ditheringToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Dithering;
            this.reOpenToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_ReOpen;

            this.openToolStripMenuItem.Text = global::PixelStacker.Resources.Text.Action_Open;
            this.saveToolStripMenuItem.Text = global::PixelStacker.Resources.Text.Action_Save;
            this.saveAsToolStripMenuItem.Text = global::PixelStacker.Resources.Text.Action_SaveAs;
            this.reOpenToolStripMenuItem.Text = global::PixelStacker.Resources.Text.Action_ReOpen;
            this.fileToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_File;
            this.sizingToolStripMenuItem.Text = global::PixelStacker.Resources.Text.Sizing;

            this.visualEnhancementsToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainForm_TS_VisualEnhancements;
            this.toggleShadowsToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainForm_TS_RenderShadows;
            // Orientation
            this.orientationToolStripMenuItem.Text = global::PixelStacker.Resources.Text.Orientation;
            this.verticalToolStripMenuItem.Text = global::PixelStacker.Resources.Text.Orientation_Vertical;
            this.horizontalToolStripMenuItem.Text = global::PixelStacker.Resources.Text.Orientation_Horizontal;

            // Layer Filters
            this.layerFilteringToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_LayerFiltering;
            this.showTopLayerToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainForm_TS_ShowTopLayer;
            this.showBottomLayerToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainForm_TS_ShowBottomLayer;
            this.showBothLayersToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainForm_TS_ShowBothLayers;
        }

        private void InitializeLocalization()
        {
            string locale = this.Options.Locale ?? CultureInfo.CurrentUICulture.Name;
            locale = ResxHelper.GetSupportedLocale(locale);
            CultureInfo ci = CultureInfo.GetCultureInfo(locale);
            Thread.CurrentThread.CurrentUICulture = ci;
            Thread.CurrentThread.CurrentCulture = ci;
            CultureInfo.CurrentUICulture = ci;
            CultureInfo.CurrentCulture = ci;

#if !RELEASE
            var testLocaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            testLocaleToolStripMenuItem.Text = "TEST";
            testLocaleToolStripMenuItem.Click += testLocaleToolStripMenuItem_Click;
            languageToolStripMenuItem.DropDownItems.Add(testLocaleToolStripMenuItem);
#endif
        }

        private void italianToolStripMenuItem_Click(object sender, System.EventArgs e)
        => this.ApplyLocalization(CultureInfo.GetCultureInfo("it-it"));

        private void swedishToolStripMenuItem_Click(object sender, System.EventArgs e)
        => this.ApplyLocalization(CultureInfo.GetCultureInfo("sv-se"));

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        => this.ApplyLocalization(CultureInfo.GetCultureInfo("en-us"));

        private void chineseSimplifiedToolStripMenuItem_Click(object sender, EventArgs e)
        => this.ApplyLocalization(CultureInfo.GetCultureInfo("zh-cn"));

        private void dutchToolStripMenuItem_Click(object sender, EventArgs e)
        => this.ApplyLocalization(CultureInfo.GetCultureInfo("nl-nl"));

        private void danishToolStripMenuItem_Click(object sender, EventArgs e)
        => this.ApplyLocalization(CultureInfo.GetCultureInfo("da-dk"));

        private void frenchToolStripMenuItem_Click(object sender, EventArgs e)
        => this.ApplyLocalization(CultureInfo.GetCultureInfo("fr-fr"));

        private void germanToolStripMenuItem_Click(object sender, EventArgs e)
        => this.ApplyLocalization(CultureInfo.GetCultureInfo("de-de"));

        private void japaneseToolStripMenuItem_Click(object sender, EventArgs e)
        => this.ApplyLocalization(CultureInfo.GetCultureInfo("ja-JP"));

        private void koreanToolStripMenuItem_Click(object sender, EventArgs e)
        => this.ApplyLocalization(CultureInfo.GetCultureInfo("ko-KR"));

        private void spanishToolStripMenuItem_Click(object sender, EventArgs e)
        => this.ApplyLocalization(CultureInfo.GetCultureInfo("es-ES"));

        private void testLocaleToolStripMenuItem_Click(object sender, EventArgs e)
        => this.ApplyLocalization(CultureInfo.GetCultureInfo("zu-ZU"));
    }
}
