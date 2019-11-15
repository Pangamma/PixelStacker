using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PixelStacker.Logic;

namespace PixelStacker.UI
{
    public partial class ColorPaletteSaveOptions : Form
    {
        private Action<ColorPaletteStyle> onChoice;

        public ColorPaletteSaveOptions()
        {
            this.onChoice = null;
            InitializeComponent();
        }

        public ColorPaletteSaveOptions(Action<ColorPaletteStyle> onChoice)
        {
            this.onChoice = onChoice;
            InitializeComponent();
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnGrid_Click(object sender, EventArgs e)
        {
            this.onChoice(ColorPaletteStyle.CompactGrid);
            this.Close();
        }

        private void btnCompactBrick_Click(object sender, EventArgs e)
        {
            this.onChoice(ColorPaletteStyle.CompactBrick);
            this.Close();
        }

        private void btnCompactGraph_Click(object sender, EventArgs e)
        {
            this.onChoice(ColorPaletteStyle.CompactGraph);
            this.Close();
        }

        private void btnFullGrid_Click(object sender, EventArgs e)
        {
            this.onChoice(ColorPaletteStyle.DetailedGrid);
            this.Close();
        }
    }
}
