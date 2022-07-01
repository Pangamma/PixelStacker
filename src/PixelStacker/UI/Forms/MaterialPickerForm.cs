using PixelStacker.Extensions;
using PixelStacker.IO;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
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
    public partial class MaterialPickerForm : Form
    {

        private MaterialCombination _selectedCombo = null;
        private Options Options;

        public MaterialCombination SelectedCombo { 
            get => this._selectedCombo;
            set {
                this._selectedCombo = value;
                {
                    var img = this._selectedCombo.GetImage(this.Options.IsSideView);
                    var bm = img.SKBitmapToBitmap();
                    imgMaterialsCombined.Image = bm;
                    imgMaterialsCombined.SetTooltip(this._selectedCombo.Top.Label + "\n"+ this._selectedCombo.Bottom.Label, global::PixelStacker.Resources.Text.Combined_Materials);
                }
                {
                    var img = this._selectedCombo.Top.GetImage(this.Options.IsSideView);
                    var bm = img.SKBitmapToBitmap();
                    imgTopMaterial.Image = bm;
                    lblTopMaterial.Text = this._selectedCombo.Top.Label;
                    imgTopMaterial.SetTooltip(this._selectedCombo.Top.Label, global::PixelStacker.Resources.Text.Top_Material);
                }
                {
                    var img = this._selectedCombo.Bottom.GetImage(this.Options.IsSideView);
                    var bm = img.SKBitmapToBitmap();
                    imgBottomMaterial.Image = bm;
                    lblBottomMaterial.Text = this._selectedCombo.Bottom.Label;
                    imgBottomMaterial.SetTooltip(this._selectedCombo.Bottom.Label, global::PixelStacker.Resources.Text.Bottom_Material);
                }
            }
        }


        public MaterialPickerForm(Options opts = null)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            this.Options = opts ?? Options.Get;
#pragma warning restore CS0618 // Type or member is obsolete
            InitializeComponent();
            SelectedCombo = this.Options.Tools.PrimaryColor;
            SelectedCombo ??= MaterialPalette.FromResx()[Constants.MaterialCombinationIDForAir];
            AppEvents.OnPrimaryColorChange += AppEvents_OnPrimaryColorChange;
            this.Disposed += MaterialPickerForm_Disposed;
        }

        private void MaterialPickerForm_Disposed(object sender, EventArgs e)
        {
            AppEvents.OnPrimaryColorChange -= AppEvents_OnPrimaryColorChange;
        }

        private void AppEvents_OnPrimaryColorChange(object sender, OptionsChangeEvent<MaterialCombination> e)
        {
            this.SelectedCombo = this.Options.Tools.PrimaryColor;
        }
    }
}
