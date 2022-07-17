using PixelStacker.UI.Forms;

namespace PixelStacker.UI.Controls
{
    public partial class CanvasEditor
    {
        private MaterialPickerForm MaterialPickerForm;

        private void btnMaterialCombination_Click(object sender, System.EventArgs e)
        {
            if (this.MaterialPickerForm == null || this.MaterialPickerForm.IsDisposed)
            {
                this.MaterialPickerForm = new MaterialPickerForm(this.Options);
                this.MaterialPickerForm?.SetCanvas(this.Canvas);
            }

            if (!this.MaterialPickerForm.Visible)
            {
                this.MaterialPickerForm.Show(this);
            }
            else
            {
                this.MaterialPickerForm.Hide();
            }
        }
    }
}
