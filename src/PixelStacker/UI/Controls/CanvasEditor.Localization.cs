using PixelStacker.Resources.Localization;

namespace PixelStacker.UI.Controls
{
    public partial class CanvasEditor : ILocalized
    {
        public void ApplyLocalization()
        {
            lblBrushWidth.Text = Resources.Text.Tools_BrushWidth;
            {
                btnPointer.ToolTipText = Resources.Text.Tools_Pointer;
                btnBrush.ToolTipText = Resources.Text.Tools_Brush;
                btnEraser.ToolTipText = Resources.Text.Tools_Eraser;
                btnFill.ToolTipText = Resources.Text.Tools_Fill;
                btnMaterialCombination.ToolTipText = Resources.Text.Combined_Materials;
                btnPencil.ToolTipText = Resources.Text.Tools_Pencil;
                btnPicker.ToolTipText = Resources.Text.Tools_Picker;
                btnWorldEditOrigin.ToolTipText = Resources.Text.Tools_WEOrigin;

                lblBrushWidth.Text = Resources.Text.Tools_BrushWidth;
                setWorldEditOriginHereToolStripMenuItem.Text = global::PixelStacker.Resources.Text.CanvasEditor_SetWorldEditOrigin;
                filterMaterialsToolStripMenuItem.Text = global::PixelStacker.Resources.Text.CanvasEditor_FilterMaterials;

                // Set the text for it.
                this.Tools_SetLayerFilterImage();
                this.MaterialPickerForm?.ApplyLocalization();
            }
        }
    }
}
