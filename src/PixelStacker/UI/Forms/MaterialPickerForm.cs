using PixelStacker.Extensions;
using PixelStacker.Logic.Collections.ColorMapper;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.UI.Controls;
using SixLabors.ImageSharp.ColorSpaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using PixelStacker.Logic.Extensions;
using System.Threading.Tasks;
using SkiaSharp;

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
                    pnlTopMats.ImageButtons.ForEach((b) =>
                    {
                        bool shouldCheck = b.GetData<Material>().PixelStackerID == toFind;
                        if (b.IsChecked != shouldCheck)
                        {
                            b.IsChecked = shouldCheck;
                        }
                    });
                }
                {
                    pnlBottomMats.ImageButtons.ForEach((b) =>
                    {
                        bool shouldCheck = b.GetData<Material>().PixelStackerID == value.Bottom.PixelStackerID;
                        if (b.IsChecked != shouldCheck)
                        {
                            b.IsChecked = shouldCheck;
                        }
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
            this.Refresh();
        }

        private async void AppEvents_OnAdvancedModeChange(object sender, OptionsChangeEvent<bool> e)
        {
            this.InitializeTabs();
            var needle = tbxMaterialFilter.Text.ToLowerInvariant();
            await this.SetSearchFilterAsync(needle, this.pnlBottomMats, GetImageButtonData_Lower());
            await this.SetSearchFilterAsync(needle, this.pnlTopMats, GetImageButtonData_Upper());
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

            pnlSimilarCombinations.ImageButtons = GetImageButtonData_Both();
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

        public void pnlTopMats_TileHover(object sender, ImageButtonClickEventArgs e)
        {
            var m = e.ImageButtonData?.GetData<Material>();
            string text = m?.Label ?? this._selectedCombo.Top.Label;
            if (text != lblTopMaterial.Text)
            {
                lblTopMaterial.Text = text;
            }
        }

        public void pnlBottomMats_TileHover(object sender, ImageButtonClickEventArgs e)
        {
            var m = e.ImageButtonData?.GetData<Material>();
            string text = m?.Label ?? this._selectedCombo.Bottom.Label;
            if (text != lblBottomMaterial.Text)
            {
                lblBottomMaterial.Text = text;
            }
        }

        public void pnlSimilarCombinations_TileHover(object sender, ImageButtonClickEventArgs e)
        {
            var m = e.ImageButtonData?.GetData<MaterialCombination>();
            {
                string text = m?.Bottom.Label ?? this._selectedCombo.Bottom.Label;
                if (text != lblBottomMaterial.Text)
                {
                    lblBottomMaterial.Text = text;
                }
            }
            {
                string text = m?.Top.Label ?? this._selectedCombo.Top.Label;
                if (text != lblTopMaterial.Text)
                {
                    lblTopMaterial.Text = text;
                }
            }
        }

        public void pnlTopMats_TileClicked(object sender, ImageButtonClickEventArgs e)
        {
            var m = e.ImageButtonData.GetData<Material>();
            var pc = this.Options.Tools.PrimaryColor;
            var bottom = pc.Bottom;
            var top = m;

            var mats = MaterialPalette.FromResx();
            var baze = mats.GetMaterialCombinationByMaterials(m, m);
            var newColor = MaterialCombination.GetMcToPaintWith(ZLayer.Top, mats, baze, pc);
            this.SelectedCombo = newColor;
            this.Options.Tools.PrimaryColor = newColor;
            this.Options.Save();
        }

        public void pnlSimilarCombinations_TileClicked(object sender, ImageButtonClickEventArgs e)
        {
            var m = e.ImageButtonData.GetData<MaterialCombination>();
            this.Options.Tools.PrimaryColor = m;
            this.SelectedCombo = this.Options.Tools.PrimaryColor;
            this.Options.Save();
        }

        public void pnlBottomMats_TileClicked(object sender, ImageButtonClickEventArgs e)
        {
            var m = e.ImageButtonData.GetData<Material>();
            var pc = this.Options.Tools.PrimaryColor;
            var mats = MaterialPalette.FromResx();

            var baze = mats.GetMaterialCombinationByMaterials(m, m);
            var newColor = MaterialCombination.GetMcToPaintWith(ZLayer.Bottom, mats, baze, pc);
            this.SelectedCombo = newColor;
            this.Options.Tools.PrimaryColor = newColor;
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

        private async void timerFilterRefresher_Tick(object sender, EventArgs e)
        {
            if (filterNeedsRefresh)
            {
                filterNeedsRefresh = false;
                var needle = tbxMaterialFilter.Text.ToLowerInvariant();
                await this.SetSearchFilterAsync(needle, this.pnlBottomMats, GetImageButtonData_Lower());
                await this.SetSearchFilterAsync(needle, this.pnlTopMats, GetImageButtonData_Upper());
            }
        }
    }
}
