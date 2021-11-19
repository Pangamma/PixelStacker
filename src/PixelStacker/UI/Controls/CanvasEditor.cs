using PixelStacker.EditorTools;
using PixelStacker.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.UI.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PixelStacker.UI.Controls
{
    [ToolboxItemFilter("PixelStacker.UI.CanvasEditor", ToolboxItemFilterType.Require)]
    public partial class CanvasEditor : UserControl
    {
        private void CanvasEditor_Load(object sender, System.EventArgs e)
        {
            OnLoadToolstrips();
            this.MainForm = this.ParentForm as MainForm;
            this.Options = this.MainForm.Options;
            
            this.tbxBrushWidth.Text = this.Options.Tools?.BrushWidth.ToString();
            this.btnMaterialCombination.Image =
                this.Options?.Tools?.PrimaryColor?.GetImage(this.Options?.IsSideView ?? false).SKBitmapToBitmap()
                ?? Resources.Textures.air.SKBitmapToBitmap();
        }
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
