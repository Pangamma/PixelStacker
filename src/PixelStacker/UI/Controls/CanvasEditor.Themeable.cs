using PixelStacker.Extensions;
using PixelStacker.Resources;
using PixelStacker.Resources.Themes;
using PixelStacker.UI.Helpers;
using SkiaSharp;

namespace PixelStacker.UI.Controls
{
    public partial class CanvasEditor
    {
        private void OnThemeChange(object sender, ThemeChangeEventArgs e)
        {
            var curShader = this.bgPaint.Shader;
            this.bgPaint.Shader = SKShader.CreateBitmap(
                ThemeHelper.bg_imagepanel.BitmapToSKBitmap(),
                SKShaderTileMode.Repeat, SKShaderTileMode.Repeat);
            curShader.DisposeSafely();
        }
    }
}
