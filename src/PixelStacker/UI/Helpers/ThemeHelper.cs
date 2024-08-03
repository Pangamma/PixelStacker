using PixelStacker.Extensions;
using PixelStacker.Resources.Themes;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.UI.Helpers
{
    internal class ThemeHelper
    {
        public static System.Drawing.Bitmap bg_imagepanel
        {
            get
            {
                AppTheme theme = ThemeManager.Theme;
                switch (theme)
                {
                    case AppTheme.Smooth:
                        return global::PixelStacker.Resources.UIResources.bg_imagepanel_smooth;
                    case AppTheme.Dark:
                        return global::PixelStacker.Resources.UIResources.bg_imagepanel_dark;
                    case AppTheme.Light:
                    default:
                        return global::PixelStacker.Resources.UIResources.bg_imagepanel;
                }
            }
        }
}
}
