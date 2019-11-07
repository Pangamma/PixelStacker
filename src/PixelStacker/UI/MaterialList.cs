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
using PixelStacker.Properties;
using static System.Windows.Forms.ListViewItem;

namespace PixelStacker.UI
{
    public partial class MaterialList : UserControl
    {
        public string Category { get; set; }
        public MaterialList(string category = "Wool")
        {
            //TODO: Optimize by avoiding _Load renders.
            Category = category;
            DoubleBuffered = true;
            InitializeComponent();
            btnToggleAll.Text = Options.Get.IsEnabled("btnToggleAll_" + this.Category, true) ? "Disable All" : "Enable All";
            {
                lblCategory.Text = Category;
                var toggles = Materials.List.Where(m => m.Category == Category).OrderBy(x => x.Label).Select(m => new MaterialListItemToggle().SetMaterial(m, Options.Get.IsSideView)).ToArray<MaterialListItemToggle>();
                flowLayoutPanel1.Controls.AddRange(toggles);
            }
        }

        public void RepaintTextures()
        {
            foreach (var m in flowLayoutPanel1.Controls.OfType<MaterialListItemToggle>())
            {
                m.SetMaterial(m.GetMaterial(), Options.Get.IsSideView);
            }
        }

        private void MaterialList_Load(object sender, EventArgs e)
        {
            
        }

        private void btnToggleAll_Click(object sender, EventArgs e)
        {
            bool shouldDisable = btnToggleAll.Text == "Disable All";
            btnToggleAll.Text = shouldDisable ? "Enable All" : "Disable All";
            Options.Get.SetEnabled("btnToggleAll_" + this.Category, !shouldDisable);
            foreach (MaterialListItemToggle toggle in this.flowLayoutPanel1.Controls.OfType<MaterialListItemToggle>())
            {
                toggle.SetChecked(!shouldDisable);
            }
        }
    }
}
