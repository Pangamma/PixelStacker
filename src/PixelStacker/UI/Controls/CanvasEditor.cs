using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PixelStacker.UI.Controls
{
    [ToolboxItemFilter("PixelStacker.UI.Controls.CanvasEditor", ToolboxItemFilterType.Require)]
    public partial class CanvasEditor : UserControl
    {
    }

    public class CustomToolStripButtonRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            var btn = e.Item as ToolStripButton;
            if (btn != null && btn.Checked)
            {
                Rectangle bounds = new Rectangle(Point.Empty, e.Item.Size);

                using Brush brush = new SolidBrush(Color.FromArgb(255, 191, 221, 245));
                e.Graphics.FillRectangle(brush, bounds);

                using Pen outlinePen = new Pen(Color.FromArgb(255, 1, 121, 215));
                e.Graphics.DrawRectangle(outlinePen, 0,0, e.Item.Size.Width - 1, e.Item.Size.Height - 1);
            }
            else base.OnRenderButtonBackground(e);
        }
    }
}
