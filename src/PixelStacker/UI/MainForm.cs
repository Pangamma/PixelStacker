using PixelStacker.Logic.IO.Config;
using System.Windows.Forms;

namespace PixelStacker.UI
{
    public partial class MainForm : Form
    {
        private void SetAllMenubarStatesBasedOnOptions(Options opts)
        {
            this.toggleGridToolStripMenuItem.Checked = opts.ViewerSettings.IsShowGrid;
            this.toggleBorderToolStripMenuItem.Checked = opts.ViewerSettings.IsShowBorder;
            this.horizontalToolStripMenuItem.Checked = !opts.IsSideView;
            this.verticalToolStripMenuItem.Checked = opts.IsSideView;
        }

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

    }
}
