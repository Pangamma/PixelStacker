using PixelStacker.Extensions;
using PixelStacker.IO.Config;
using PixelStacker.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Options.Save();
            this.Close();
        }
    }
}
