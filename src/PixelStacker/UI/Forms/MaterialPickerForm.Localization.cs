﻿using PixelStacker.Resources.Localization;
using PixelStacker.Utilities;
using System.Globalization;

namespace PixelStacker.UI.Forms
{
    partial class MaterialPickerForm: ILocalized
    {
        public void ApplyLocalization(CultureInfo locale)
        {
            lblFilter.Text = Resources.Text.Action_Filter;
            tabTop.Text = global::PixelStacker.Resources.Text.Top;
            tabBottom.Text = global::PixelStacker.Resources.Text.Bottom;
            ttTop.ToolTipTitle = global::PixelStacker.Resources.Text.Top_Material;
            ttBottom.ToolTipTitle = global::PixelStacker.Resources.Text.Bottom_Material;
            lblHtmlCode.Text = global::PixelStacker.Resources.Text.RenderedImagePanel_AvgColorCode;
        }
    }
}