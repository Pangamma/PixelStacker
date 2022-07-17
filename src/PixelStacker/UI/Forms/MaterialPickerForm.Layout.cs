using PixelStacker.IO;
using System;
using System.Windows.Forms;

namespace PixelStacker.UI.Forms
{
    partial class MaterialPickerForm
    {
        private void MaterialPickerForm_ResizeBegin(object sender, EventArgs e)
        {
            this.pnlBottomMats.SuspendLayout();
            this.pnlTopMats.SuspendLayout();
        }

        private void MaterialPickerForm_ResizeEnd(object sender, EventArgs e)
        {
            this.pnlBottomMats.ResumeLayout();
            this.pnlTopMats.ResumeLayout();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Hide();
                return true;
            }

            KonamiWatcher.ProcessKey(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}