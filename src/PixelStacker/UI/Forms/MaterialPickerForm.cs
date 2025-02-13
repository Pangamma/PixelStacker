using PixelStacker.Extensions;
using PixelStacker.Logic.Collections.ColorMapper;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.UI.Controls.Pickers;
using SixLabors.ImageSharp.ColorSpaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using PixelStacker.Logic.Extensions;

namespace PixelStacker.UI.Forms
{
    public partial class MaterialPickerForm : Form
    {
        private Dictionary<string, int> MaterialOrders = new Dictionary<string, int>();
        private MaterialCombination _selectedCombo = null;
        private Options Options;

        public MaterialCombination SelectedCombo
        {
            get => this._selectedCombo;
            set
            {
                if (this._selectedCombo == value)
                {
                    return;
                }

                this._selectedCombo = value;
                {
                    var img = this._selectedCombo.GetImage(this.Options.IsSideView);
                    imgMaterialsCombined.Image = img;
                    toolTip1.SetToolTip(imgMaterialsCombined, this._selectedCombo.Top.Label + "\n" + this._selectedCombo.Bottom.Label);
                    tbxHtmlColorCode.Text = "#" + value.GetAverageColor(this.Options.IsSideView).ToString().Substring(3);
                    //imgMaterialsCombined.SetTooltip(, global::PixelStacker.Resources.Text.Combined_Materials);
                }
                {
                    if (!this._selectedCombo.IsMultiLayer)
                    {
                        var img = global::PixelStacker.Resources.Textures.disabled;
                        imgTopMaterial.Image = img;
                        lblTopMaterial.Text = this._selectedCombo.Top.Label;
                        this.ttTop.SetToolTip(imgTopMaterial, global::PixelStacker.Resources.Text.Nothing);
                    }
                    else
                    {
                        var img = this._selectedCombo.Top.GetImage(this.Options.IsSideView);
                        imgTopMaterial.Image = img;
                        lblTopMaterial.Text = this._selectedCombo.Top.Label;
                        this.ttTop.SetToolTip(imgTopMaterial, this._selectedCombo.Top.Label);
                    }
                }
                {
                    var img = this._selectedCombo.Bottom.GetImage(this.Options.IsSideView);
                    imgBottomMaterial.Image = img;
                    lblBottomMaterial.Text = this._selectedCombo.Bottom.Label;
                    this.ttBottom.SetToolTip(imgBottomMaterial, this._selectedCombo.Bottom.Label);
                }

                {
                    string toFind = value.IsMultiLayer ? value.Top.PixelStackerID : Materials.Air.PixelStackerID;
                    pnlTopMats.ModifyButtons((d, b) =>
                    {
                        b.IsChecked = d.GetData<Material>().PixelStackerID == toFind;
                    });
                }
                {
                    pnlBottomMats.ModifyButtons((d, b) =>
                    {
                        b.IsChecked = d.GetData<Material>().PixelStackerID == value.Bottom.PixelStackerID;
                    });
                }
            }
        }


        [Obsolete("Designer view only", false)]
        public MaterialPickerForm()
        {
            InitializeComponent();
        }


        public MaterialPickerForm(Options opts)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            this.Options = opts ?? Options.GetInMemoryFallback;
#pragma warning restore CS0618 // Type or member is obsolete
            InitializeComponent();
            this.InitializeAutoComplete();
            this.ApplyLocalization();
            SelectedCombo = this.Options.Tools.PrimaryColor;
            SelectedCombo ??= MaterialPalette.FromResx()[Constants.MaterialCombinationIDForAir];

            this.MaterialOrders = Materials.List
                .Select((Material m, int i) => new KeyValuePair<string, int>(m.PixelStackerID, i))
                .ToDictionary(keySelector: x => x.Key, elementSelector: x => x.Value);

            this.InitializeTabs();

            AppEvents.OnUserSampledMaterialEvent += AppEvents_UserSampledMaterial;
            AppEvents.OnAdvancedModeChange += AppEvents_OnAdvancedModeChange;
            this.Disposed += MaterialPickerForm_Disposed;
        }

        private void AppEvents_UserSampledMaterial(object sender, UserSampledMaterialEvent e)
        {
            this.SelectedCombo = e.MaterialCombination;
            UpdateMaterialComboTab();
        }

        private async void AppEvents_OnAdvancedModeChange(object sender, OptionsChangeEvent<bool> e)
        {
            this.InitializeTabs();
            var needle = tbxMaterialFilter.Text.ToLowerInvariant();
            await this.SetSearchFilterAsync(needle, this.pnlBottomMats);
            await this.SetSearchFilterAsync(needle, this.pnlTopMats);
        }

        private void MaterialPickerForm_Disposed(object sender, EventArgs e)
        {
            AppEvents.OnUserSampledMaterialEvent -= AppEvents_UserSampledMaterial;
            AppEvents.OnAdvancedModeChange -= AppEvents_OnAdvancedModeChange;
        }

        
        private void UpdateMaterialComboTab()
        {
            if (this.SelectedCombo == null)
            {
                return;
            }

            int MAX_PULL = 50;
            bool isv = this.Options.IsSideView;
            var c = this.SelectedCombo.GetAverageColor(isv);

            List<MaterialCombination> mats = MaterialPalette.FromResx()
                .ToCombinationList().Where(x => !x.Bottom.IsAdvanced).ToList();

            var singleLayers = mats.Where(x => !x.IsMultiLayer).OrderBy(x => x.GetAverageColor(isv).GetColorDistance(c)).Take(MAX_PULL/2+10);
            var doubleLayers = mats.Where(x => x.IsMultiLayer).OrderBy(x => x.GetAverageColor(isv).GetColorDistance(c)).Take(MAX_PULL/2+10);

            mats = singleLayers.Union(doubleLayers)
                .OrderBy(x => c.GetAverageColorDistance(x.GetColorsInImage(isv)))
                .Take(MAX_PULL)
                .ToList();

            List<ImageButtonData> items = new List<ImageButtonData>();
            items.AddRange(mats.Select(x => new ImageButtonData()
            {
                Data = x,
                Image = x.GetImage(isv),
                Text = x.ToString(),
            }));

            pnlSimilarCombinations.InitializeButtons(items);
        }

        private void imgTopMaterial_Click(object sender, EventArgs e)
        {
            imgTopMaterial.IsChecked = true;
            imgBottomMaterial.IsChecked = false;
            tcMaterials.SelectedIndex = 0;
        }

        private void imgBottomMaterial_Click(object sender, EventArgs e)
        {
            imgTopMaterial.IsChecked = false;
            imgBottomMaterial.IsChecked = true;
            tcMaterials.SelectedIndex = 1;
        }

        private void TabControlMaterials_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tcMaterials.SelectedIndex)
            {
                case 0: // Top
                    imgTopMaterial.IsChecked = true;
                    imgBottomMaterial.IsChecked = false;
                    tbxMaterialFilter.Enabled = true;
                    break;
                case 1: // Bottom
                    imgTopMaterial.IsChecked = false;
                    imgBottomMaterial.IsChecked = true;
                    tbxMaterialFilter.Enabled = true;
                    break;
                case 2: // Both
                    imgTopMaterial.IsChecked = true;
                    imgBottomMaterial.IsChecked = true;
                    tbxMaterialFilter.Enabled = false;
                    UpdateMaterialComboTab();
                    break;
                default:
                    throw new IndexOutOfRangeException("Unexpected tab index was requested. " +
                    "Weird! Add logic for the new tab index. Thanks.");
            }

        }

        private void pnlTopMats_TileClicked(object sender, ImageButtonClickEventArgs e)
        {
            var m = e.ImageButtonData.GetData<Material>();
            var pc = this.Options.Tools.PrimaryColor;

            var mats = MaterialPalette.FromResx();
            if (m.PixelStackerID == "AIR")
            {
                this.Options.Tools.PrimaryColor = mats.GetMaterialCombinationByMaterials(pc.Bottom, pc.Bottom);
                this.SelectedCombo = this.Options.Tools.PrimaryColor;
                this.Options.Save();
                return;
            }

            if (pc.Bottom.PixelStackerID == "AIR")
            {
                // Sorry. No changes allowed. We require something on the bottom layer if the top layer is to contain something.
                //this.Options.Tools.PrimaryColor = mats.GetMaterialCombinationByMaterials(pc.Bottom, pc.Bottom);
                //this.Options.Save();
                return;
            }

            this.Options.Tools.PrimaryColor = mats.GetMaterialCombinationByMaterials(pc.Bottom, m);
            this.SelectedCombo = this.Options.Tools.PrimaryColor;
            this.Options.Save();
        }

        private void pnlSimilarCombinations_TileClicked(object sender, ImageButtonClickEventArgs e)
        {
            var m = e.ImageButtonData.GetData<MaterialCombination>();
            this.Options.Tools.PrimaryColor = m;
            this.SelectedCombo = this.Options.Tools.PrimaryColor;
            this.Options.Save();
        }

        private void pnlBottomMats_TileClicked(object sender, ImageButtonClickEventArgs e)
        {
            var m = e.ImageButtonData.GetData<Material>();
            var pc = this.Options.Tools.PrimaryColor;
            var mats = MaterialPalette.FromResx();
            if (m.PixelStackerID == "AIR")
            {
                this.Options.Tools.PrimaryColor = mats[Constants.MaterialCombinationIDForAir];
                this.SelectedCombo = this.Options.Tools.PrimaryColor;
                this.Options.Save();
                return;
            }

            // If it is air, or not glass.
            if (pc.Top.PixelStackerID == "AIR" || !pc.Top.IsGlassOrLayer2Block)
            {
                this.Options.Tools.PrimaryColor = mats.GetMaterialCombinationByMaterials(m, m);
                this.SelectedCombo = this.Options.Tools.PrimaryColor;
                this.Options.Save();
                return;
            }

            this.Options.Tools.PrimaryColor = mats.GetMaterialCombinationByMaterials(m, pc.Top);
            this.SelectedCombo = this.Options.Tools.PrimaryColor;
            this.Options.Save();
        }

        private void MaterialPickerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }
    }
}
