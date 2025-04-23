using PixelStacker.Logic.Extensions;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PixelStacker.UI.Controls
{
    public partial class CanvasEditor
    {
        private void OnLoadToolstrips()
        {
            Toolbox_OnClickPointer(btnPointer, null);
            toolstrip_LayoutStyleChanged(tsCanvasTools, null);
            Tools_SetLayerFilterImage();
        }

        internal void Tools_SetLayerFilterImage()
        {
            if (this.Options != null)
            {
                var z = this.Options.Tools.ZLayerFilter;
                if (z == Logic.IO.Config.ZLayer.Both)
                {
                    btnPaintLayerFilter.ToolTipText = global::PixelStacker.Resources.Text.CanvasEditor_ZFilter_Both;
                    btnPaintLayerFilter.Image = global::PixelStacker.Resources.UIResources.iso2_both_layers;
                }
                else if (z == Logic.IO.Config.ZLayer.Top)
                {
                    btnPaintLayerFilter.ToolTipText = global::PixelStacker.Resources.Text.CanvasEditor_ZFilter_Top;
                    btnPaintLayerFilter.Image = global::PixelStacker.Resources.UIResources.iso2_top_layer;
                }
                else
                {
                    btnPaintLayerFilter.ToolTipText = global::PixelStacker.Resources.Text.CanvasEditor_ZFilter_Bottom;
                    btnPaintLayerFilter.Image = global::PixelStacker.Resources.UIResources.iso2_bottom_layer;
                }
            }
        }

        private void btnPaintLayerFilter_Click(object sender, System.EventArgs e)
        {
            var z = this.Options.Tools.ZLayerFilter;
            if (z == Logic.IO.Config.ZLayer.Both)
            {
                z = Logic.IO.Config.ZLayer.Top;
            }
            else if (z == Logic.IO.Config.ZLayer.Top)
            {
                z = Logic.IO.Config.ZLayer.Bottom;
            }
            else
            {
                z = Logic.IO.Config.ZLayer.Both;
            }

            this.Options.Tools.ZLayerFilter = z;
            this.Options.Save();

            Tools_SetLayerFilterImage();
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
                    btn.Size = new System.Drawing.Size(40, 40);
                }
                else if (ts.LayoutStyle.HasFlag(ToolStripLayoutStyle.HorizontalStackWithOverflow))
                {
                    ts.ImageScalingSize = new Size(20, 20);
                    btn.Size = new System.Drawing.Size(27, 27);
                    btn.Margin = new Padding(0, 5, 0, 5);
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
