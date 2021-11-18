using PixelStacker.Logic.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI.Controls
{
    public partial class CanvasEditor
    {
        private void OnLoadToolstrips()
        {
            toolstrip_LayoutStyleChanged(tsCanvasTools, null);
        }

        private void toolstrip_LayoutStyleChanged(object sender, System.EventArgs e)
        {
            ToolStrip ts = sender as ToolStrip;
            foreach (ToolStripButton btn in ts.Items)
            {
                btn.AutoSize = false;
                btn.ImageAlign = ContentAlignment.MiddleCenter;
                if (ts.LayoutStyle.HasFlag(ToolStripLayoutStyle.VerticalStackWithOverflow))
                {
                    ts.ImageScalingSize = new Size(32, 32);
                    btn.Margin = new Padding(5, 0, 5, 0);
                    //btn.ImageScaling = ToolStripItemImageScaling.None;
                    btn.Size = new System.Drawing.Size(40, 40);
                }
                else if (ts.LayoutStyle.HasFlag(ToolStripLayoutStyle.HorizontalStackWithOverflow))
                {
                    ts.ImageScalingSize = new Size(20, 20);
                    btn.Size = new System.Drawing.Size(27, 27);
                    btn.Margin = new Padding(0, 5, 0, 5);
                    //btn.ImageScaling = ToolStripItemImageScaling.SizeToFit;
                }
                btn.ImageScaling = ToolStripItemImageScaling.SizeToFit;
            }
        }

        private void btnBrushWidthAdd_Click(object sender, System.EventArgs e)
        {
            if (Options?.Tools != null)
            {
                Options.Tools.BrushWidth = Math.Min(500, 1 + (tbxBrushWidth.Text.ToNullable<int>() ?? 1));
                tbxBrushWidth.Text = Options.Tools.BrushWidth.ToString();
                Options.Save();
            }
        }

        private void btnBrushWidthMinus_Click(object sender, System.EventArgs e)
        {

            if (Options?.Tools != null)
            {
                Options.Tools.BrushWidth = Math.Max(1, -1 + (tbxBrushWidth.Text.ToNullable<int>() ?? 1));
                tbxBrushWidth.Text = Options.Tools.BrushWidth.ToString();
                Options.Save();
            }
        }

        private void tbxBrushWidth_TextChanged(object sender, System.EventArgs e)
        {
            if (Options?.Tools != null)
            {
                Options.Tools.BrushWidth = tbxBrushWidth.Text.ToNullable<int>() ?? 1;
                Options.Save();
            }
        }

        private void tbxBrushWidth_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

    }
}
