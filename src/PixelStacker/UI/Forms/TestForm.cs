using PixelStacker.Logic.Model;
using PixelStacker.UI.Controls.Pickers;
using PixelStacker.UI.Helpers;
using System.Data;
using System.Diagnostics;
using System.Linq;
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
                Text = x.Label,
                Data = x
            }).ToList();

            items.Insert(0, new ImageButtonData() { 
                Text = "Nothing",
                Data = null,
                Image = Resources.Textures.disabled,
            });

            this.imageButtonPanel1.TileClicked += ImageButtonPanel1_TileClicked;
            this.imageButtonPanel1.InitializeButtons(items);
        }

        private void ImageButtonPanel1_TileClicked(object sender, ImageButtonClickEventArgs e)
        {
            Debug.WriteLine(e.ImageButtonData.Text);
        }
    }
}
