using PixelStacker.Logic.IO.Config;
using PixelStacker.Resources;
using PixelStacker.Resources.Localization;
using PixelStacker.Resources.Themes;
using System;
using System.Drawing;
using System.Globalization;
using System.Threading;

namespace PixelStacker.UI
{

    public partial class MainForm
    {
        public static AppTheme Theme { get; set; } = AppTheme.Light;
        private void smoothThemeToolStripMenuItem_Click(object sender, System.EventArgs e) => this.SetTheme(AppTheme.Smooth);
        private void darkThemeToolStripMenuItem_Click(object sender, System.EventArgs e) => this.SetTheme(AppTheme.Dark);
        private void lightThemeToolStripMenuItem_Click(object sender, System.EventArgs e) => this.SetTheme(AppTheme.Light);

        private void SetTheme(AppTheme theme)
        {
            this.Options.Theme = theme;
            ThemeManager.Theme = theme;
            this.ApplyCurrentTheme();
            this.Options.Save();
        }

        private void ApplyCurrentTheme()
        {
            var theme = ThemeManager.Theme;
            this.smoothThemeToolStripMenuItem.Checked = theme == AppTheme.Smooth;
            this.darkUIToolStripMenuItem.Checked = theme == AppTheme.Dark;
            this.lightUIToolStripMenuItem.Checked = theme == AppTheme.Light;
        }
    }
}
