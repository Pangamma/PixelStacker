using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PixelStacker.Logic;
using System.Drawing.Drawing2D;

namespace PixelStacker.UI
{
    public partial class MaterialListItemToggle : UserControl
    {
        public bool IsChecked { get; private set; }
        private Material material;

        public MaterialListItemToggle()
        {
            InitializeComponent();
            this.IsChecked = true;
        }

        private void btnToggle_Click(object sender, EventArgs e)
        {
            SetChecked(!this.IsChecked);
        }

        public void SetChecked(bool IsChecked)
        {
            this.IsChecked = IsChecked;
            if (this.material != null)
            {
                this.material.IsEnabled = IsChecked;
                this.btnToggle.Text = IsChecked ? "On" : "Off";
                this.btnToggle.BackColor = IsChecked ? Color.Chartreuse : SystemColors.Control;
            }
        }

        public Material GetMaterial()
        {
            return this.material;
        }

        public MaterialListItemToggle SetMaterial(Material m, bool isSide = false)
        {
            this.material = m;
            this.pictureBox1.Image = isSide ? m.SideImage : m.TopImage;

            SetChecked(m.IsEnabled);

            return this;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //base.OnPaint(e);
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawImage(pictureBox1.Image, 0, 0, 32, 32);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            SetChecked(!this.IsChecked);
        }
    }
}
