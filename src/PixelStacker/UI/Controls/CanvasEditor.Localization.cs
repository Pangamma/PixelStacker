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
                btnWorldEditOrigin.ToolTipText = Resources.Text.Tools_WEOrigin;
            }
        }
    }
}
