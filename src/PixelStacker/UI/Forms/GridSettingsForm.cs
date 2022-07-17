using PixelStacker.Logic.IO.Config;
using SkiaSharp;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PixelStacker.UI.Forms
{
    public partial class GridSettingsForm : Form
    {
        public Options Options { get; set; }
        public GridSettingsForm()
        {
            InitializeComponent();
        }

        private void GridSettingsForm_Load(object sender, EventArgs e)
        {
            var cc = Options?.ViewerSettings.GridColor ?? new SKColor(0,0,0,255);
            btnGridColor.BackColor = Color.FromArgb(cc.Alpha, cc.Red, cc.Green, cc.Blue);
            tbxGridSize.Value = Options?.ViewerSettings.GridSize ?? 16;
        }

        private void btnGridColor_Click(object sender, EventArgs e)
        {
            var result = colorDialog.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                Color c = colorDialog.Color;
                btnGridColor.BackColor = c;
                Options.ViewerSettings.GridColor = new SKColor(c.R, c.G, c.B, c.A);
                Options.Save();
            }
        }

        private void tbxGridSize_ValueChanged(object sender, EventArgs e)
        {
            Options.ViewerSettings.GridSize = (int) tbxGridSize.Value;
            Options.Save();
        }
    }
}
