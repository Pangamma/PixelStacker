using PixelStacker.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.UI
{
    public partial class MainForm
    {
        public void TS_OnOpenFile()
        {
            string ext = this.loadedImageFilePath.GetFileExtension();
            saveToolStripMenuItem.Enabled = "pxlzip".Contains(ext);
            saveAsToolStripMenuItem.Enabled = true;
        }

        public void TS_OnRenderCanvas()
        {
            string ext = this.loadedImageFilePath?.GetFileExtension() ?? "NOPE";
            saveToolStripMenuItem.Enabled = "pxlzip".Contains(ext);
            saveAsToolStripMenuItem.Enabled = true;
        }
    }
}
