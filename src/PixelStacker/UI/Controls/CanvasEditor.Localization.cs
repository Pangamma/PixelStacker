using PixelStacker.Resources.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.UI.Controls
{
    public partial class CanvasEditor : ILocalized
    {
        public void ApplyLocalization(CultureInfo locale)
        {
            lblBrushWidth.Text = Resources.Text.Tools_BrushWidth;
            {
                btnBrush.ToolTipText = Resources.Text.Tools_Brush;
                btnEraser.ToolTipText = Resources.Text.Tools_Eraser;
                btnFill.ToolTipText = Resources.Text.Tools_Fill;
                btnMaterialCombination.ToolTipText = Resources.Text.Combined_Materials;
                btnPanZoom.ToolTipText = Resources.Text.Tools_PanZoom;
                btnPencil.ToolTipText = Resources.Text.Tools_Pencil;
                btnPicker.ToolTipText = Resources.Text.Tools_Picker;
                btnWorldEditOrigin.ToolTipText = Resources.Text.Tools_WEOrigin;

                lblBrushWidth.Text = Resources.Text.Tools_BrushWidth;

                // Set the text for it.
                this.Tools_SetLayerFilterImage();
                this.MaterialPickerForm?.ApplyLocalization(locale);
            }
        }
    }
}
