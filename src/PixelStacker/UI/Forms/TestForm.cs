using PixelStacker.IO;
using PixelStacker.Logic.Model;
using PixelStacker.UI.Controls.Pickers;
using PixelStacker.UI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI.Forms
{
    public partial class TestForm : Form
    {
        public SnapManager SnapManager { get; }

        public TestForm()
        {
            InitializeComponent();
            var items = Materials.List.Where(x => x.Category != "Glass")
                .Select(x => new ImageButtonData() { 
                Image = x.GetImage(false),
                IsChecked = false,
                Text = x.Label,
                Data = x
            }).ToList();

            items.Insert(0, new ImageButtonData() { 
                Text = "Nothing",
                Data = null,
                Image = Resources.Textures.disabled,
                IsChecked = false
            });

            this.imageButtonPanel1.TileClicked += ImageButtonPanel1_TileClicked;
            this.imageButtonPanel1.SetImageButtonData(items);
        }

        private void ImageButtonPanel1_TileClicked(object sender, ImageButtonClickEventArgs e)
        {
            Debug.WriteLine(e.ImageButtonData.Text);
        }
    }
}
