﻿using System.Windows.Forms;

namespace PixelStacker.UI
{
    public partial class MainForm : Form
    {
        private void MainForm_ResizeBegin(object sender, System.EventArgs e)
        {
            this.canvasEditor.Visible = false;
            this.imageViewer.Visible = false;
            this.canvasEditor.SuspendLayout();
            this.imageViewer.SuspendLayout();
        }

        private void MainForm_ResizeEnd(object sender, System.EventArgs e)
        {
            this.canvasEditor.Visible = true;
            this.imageViewer.Visible = true;
            this.canvasEditor.ResumeLayout(true);
            this.imageViewer.ResumeLayout(true);
        }

        private void preRenderToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            DoPreprocessLoadedImage();
        }
    }
}
