using PixelStacker.Logic;
using PixelStacker.Logic.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI
{
    partial class MainForm: ILocalized
    {
        public void ApplyLocalization(CultureInfo locale)
        {
            Options.Get.Locale = locale.Name;
            Options.Save();
            Thread.CurrentThread.CurrentUICulture = locale;
            this.renderedImagePanel?.ApplyLocalization(locale);
            this.MaterialOptions?.ApplyLocalization(locale);

            this.Text = global::PixelStacker.Resources.Text.MainForm_Title;
            this.Text = this.Text + " v" + Constants.Version;
#if !RELEASE
            this.Text += " (Debug)";
#endif
            if (!Constants.IsFullVersion)
            {
#pragma warning disable CS0162 // Unreachable code detected
                this.Text += " (Free Version)";
#pragma warning restore CS0162 // Unreachable code detected
            }


            this.dlgSaveSettings.Title = global::PixelStacker.Resources.Text.MainMenu_SaveSettings;
            this.dlgSaveColorPalette.Title = global::PixelStacker.Resources.Text.MainMenu_SaveColorPalette;
            this.dlgSave.Title = global::PixelStacker.Resources.Text.Action_Save;
            this.dlgOpen.Title = global::PixelStacker.Resources.Text.MainForm_Open_Title;
            this.aboutToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_About;
            this.helpToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Help;
            this.dutchToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Lang_Dutch;
            this.spanishToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Lang_Spanish;
            this.koreanToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Lang_Korean;
            this.japaneseToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Lang_Japanese;
            this.germanToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Lang_German;
            this.frenchToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Lang_French;
            this.danishToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Lang_Danish;
            this.chineseSimplifiedToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Lang_Chinese;
            this.englishToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Lang_English;
            this.languageToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Language;
            this.previewSchematicToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Render;
            this.mi_preRender.Text = global::PixelStacker.Resources.Text.MainMenu_PreRender;
            this.otherOptionsToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_OtherOptions;
            this.mi_PreRenderOptions.Text = global::PixelStacker.Resources.Text.MainMenu_QuantizerOptions;
            this.optionsToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_MaterialOptions;
            this.toolsToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Tools;
            this.down1LayerToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_DownOneLayer;
            this.up1LayerToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_UpOneLayer;
            this.toggleLayerFilterToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_ToggleLayerFilter;
            this.layerFilteringToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_LayerFiltering;
            this.toggleProgressToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_ToggleProgress;
            this.togglePaletteToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_TogglePalette;
            this.toggleBorderToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_ToggleBorder;
            this.toggleSolidColorsToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_ToggleHideTexture;
            this.toggleGridToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_ToggleGrid;
            this.viewToolStripMenuItem.Text = global::PixelStacker.Resources.Text.Action_View;
            this.redoToolStripMenuItem.Text = global::PixelStacker.Resources.Text.Action_Redo;
            this.undoToolStripMenuItem.Text = global::PixelStacker.Resources.Text.Action_Undo;
            this.editToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_Edit;
            this.exportSettingsToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_ExportSettings;
            this.allColorsdetailedToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_CP_AllColors_Detailed;
            this.allPossibilitiescompactToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_CP_AllColors_Compact;
            this.squareToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_CP_Square;
            this.brickToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_CP_Brick;
            this.graphToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_CP_Graph;
            this.saveColorPaletteToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_SaveColorPalette;
            this.exportSchematicToolStripMenuItem.Text = global::PixelStacker.Resources.Text.Action_Save;
            this.reOpenToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_ReOpen;
            this.openToolStripMenuItem.Text = global::PixelStacker.Resources.Text.Action_Open;
            this.fileToolStripMenuItem.Text = global::PixelStacker.Resources.Text.MainMenu_File;
        }

        private void InitializeThreadLocale()
        {
            try
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Options.Get.Locale);
            }
            catch (Exception)
            {
                Options.Get.Locale = "en-us";
            }
        }

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
    }
}
