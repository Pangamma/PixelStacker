using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI.Forms
{
    public partial class CanvasTools : Form
    {
        public CanvasTools()
        {
            InitializeComponent();
            Highlight(this.btnPan);
        }

        public event EventHandler OnClickFill;
        public event EventHandler OnClickPanZoom;
        public event EventHandler OnClickEraser;
        public event EventHandler OnClickPencil;
        public event EventHandler OnClickBrush;
        public event EventHandler OnClickPicker;
        public event EventHandler OnClickWorldEditOrigin;

        private void Highlight(object sender)
        {
            foreach (var otherBtn in this.Controls.OfType<Button>())
            {
                otherBtn.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLightLight;
                otherBtn.BackColor = System.Drawing.SystemColors.ControlLightLight;
                otherBtn.FlatAppearance.BorderSize = 0;
            }

            Button btn = (Button)sender;
            btn.BackColor = Color.FromArgb(255, 191, 221, 245);
            btn.FlatAppearance.BorderColor = Color.FromArgb(255, 1, 121, 215);
            btn.FlatAppearance.BorderSize = 1;
        }

        private void btnFill_Click(object sender, EventArgs e)
        {
            Highlight(sender);
            OnClickFill?.Invoke(sender, e);
        }
        private void btnPanZoom_Click(object sender, EventArgs e)
        {
            Highlight(sender);
            OnClickPanZoom?.Invoke(sender, e);
        }

        private void btnEraser_Click(object sender, EventArgs e)
        {
            Highlight(sender);
            OnClickEraser?.Invoke(sender, e);
        }

        private void btnPencil_Click(object sender, EventArgs e)
        {
            Highlight(sender); 
            OnClickPencil?.Invoke(sender, e);
        }

        private void btnBrush_Click(object sender, EventArgs e)
        {
            Highlight(sender);
            OnClickBrush?.Invoke(sender, e);
        }
        private void btnPicker_Click(object sender, EventArgs e)
        {
            Highlight(sender);
            OnClickPicker?.Invoke(sender, e);
        }

        private void btnWorldEditOrigin_Click(object sender, EventArgs e)
        {
            Highlight(sender);
            OnClickWorldEditOrigin?.Invoke(sender, e);
        }
    }
}
