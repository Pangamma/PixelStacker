using PixelStacker.IO;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Resources.Localization;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace PixelStacker.UI
{
    public partial class SizeForm : Form, ILocalized
    {
        public Options Options { get; }

        public SizeForm()
        {
            InitializeComponent();
        }

        public SizeForm(Options opts)
        {
            InitializeComponent();
            this.Options = opts;
            this.tbxMaxHeight.Text = this.Options.Preprocessor.MaxHeight?.ToString() ?? "";
            this.tbxMaxWidth.Text = this.Options.Preprocessor.MaxWidth?.ToString() ?? "";
            ApplyLocalization(CultureInfo.CurrentUICulture);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }

            KonamiWatcher.ProcessKey(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void ApplyLocalization(CultureInfo locale)
        {
            lblHeight.Text = Resources.Text.OtherOptions_MaxHeight;
            lblWidth.Text = Resources.Text.OtherOptions_MaxWidth;
            lblInstructions.Text = Resources.Text.OtherOptions_Instructions;
            btnSave.Text = Resources.Text.Action_Save;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Options.Preprocessor.MaxHeight = tbxMaxHeight.Text.ToNullable<int>();
            Options.Preprocessor.MaxWidth = tbxMaxWidth.Text.ToNullable<int>();

            if (Options.Preprocessor.MaxHeight.HasValue && Options.Preprocessor.MaxHeight < 1)
                Options.Preprocessor.MaxHeight = null;

            if (Options.Preprocessor.MaxWidth.HasValue && Options.Preprocessor.MaxWidth < 1)
                Options.Preprocessor.MaxWidth = null;

            Options.Save();
            this.Close();
        }
    }
}
