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

            ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));

            var allMenuItems = this.menuStrip1.Items.Flatten().ToList();
            foreach(var c in allMenuItems)
            {
                resources.ApplyResources(c, c.Name, locale);
            }

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
