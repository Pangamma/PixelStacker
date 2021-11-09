using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using System.Windows.Forms;

namespace PixelStacker.UI
{
    public partial class MainForm : Form
    {
        private void toggleGridToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.Options.ViewerSettings.IsShowGrid = !this.Options.ViewerSettings.IsShowGrid;
            toggleGridToolStripMenuItem.Checked = this.Options.ViewerSettings.IsShowGrid;
            this.canvasEditor.Refresh();
        }
        private void toggleBorderToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.Options.ViewerSettings.IsShowBorder = !this.Options.ViewerSettings.IsShowBorder;
            toggleBorderToolStripMenuItem.Checked = this.Options.ViewerSettings.IsShowBorder;
            this.canvasEditor.Refresh();
        }

        private void horizontalToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.Options.IsSideView = false;
            this.horizontalToolStripMenuItem.Checked = !this.Options.IsSideView;
            this.verticalToolStripMenuItem.Checked = this.Options.IsSideView;
        }

        private void verticalToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.Options.IsSideView = true;
            this.horizontalToolStripMenuItem.Checked = !this.Options.IsSideView;
            this.verticalToolStripMenuItem.Checked = this.Options.IsSideView;
        }

        private async void testToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.canvasEditor.Painter.History.AddChange(new Logic.CanvasEditor.History.ChangeRecord() { 
            PaletteIDAfter = 2,
            PaletteIDBefore = 11,
            ChangedPixels = new System.Collections.Generic.List<Logic.Model.PxPoint>()
            {
                new PxPoint(0,0),
                new PxPoint(0,1),
                new PxPoint(1,1),
                new PxPoint(1,2),
                new PxPoint(2,2),
                new PxPoint(2,3),
                new PxPoint(3,3),
            }
            });

            await this.canvasEditor.Painter.DoRenderFromHistoryBuffer();
            this.canvasEditor.Refresh();
        }
    }
}
