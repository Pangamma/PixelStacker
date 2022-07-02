using PixelStacker.Resources.Localization;
using PixelStacker.Utilities;
using System.Globalization;

namespace PixelStacker.UI.Forms
{
    partial class MaterialPickerForm: ILocalized
    {
        public void ApplyLocalization(CultureInfo locale)
        {
            lblFilter.Text = Resources.Text.Action_Filter;
            ddlColorPool.ValueMember = nameof(ComboBoxItem.Value);
            ddlColorPool.DisplayMember = nameof(ComboBoxItem.Text);
            ddlColorPool.Items.Clear();
            ddlColorPool.Items.AddRange(new object[] {
                new ComboBoxItem("ALL", "All Materials"),
                new ComboBoxItem("FROM_CANVAS", "From Canvas"),
                new ComboBoxItem("FROM_A_TO_B", "From A to B"),
                new ComboBoxItem("CLOSEST_TO_A", "Closest to A"),
            });
            if (ddlColorPool.SelectedIndex < 0) ddlColorPool.SelectedIndex = 0;
        }
    }
}