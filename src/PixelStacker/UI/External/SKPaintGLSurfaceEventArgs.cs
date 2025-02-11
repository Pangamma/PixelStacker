using SkiaSharp;
using System;
using System.ComponentModel;


// https://github.com/mono/SkiaSharp/blob/02816641132903a8e99b4d43e423dc94b24d1e8f/source/SkiaSharp.Views/SkiaSharp.Views.Shared/SKPaintGLSurfaceEventArgs.cs
namespace PixelStacker.UI.External
{
    public class SKPaintGLSurfaceEventArgs : EventArgs
    {
        public SKPaintGLSurfaceEventArgs(SKSurface surface, GRBackendRenderTarget renderTarget)
            : this(surface, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888)
        {
        }

        public SKPaintGLSurfaceEventArgs(SKSurface surface, GRBackendRenderTarget renderTarget, GRSurfaceOrigin origin, SKColorType colorType)
        {
            Surface = surface;
            BackendRenderTarget = renderTarget;
            ColorType = colorType;
            Origin = origin;
            Info = new SKImageInfo(renderTarget.Width, renderTarget.Height, ColorType);
            RawInfo = Info;
        }

        public SKPaintGLSurfaceEventArgs(SKSurface surface, GRBackendRenderTarget renderTarget, GRSurfaceOrigin origin, SKImageInfo info)
            : this(surface, renderTarget, origin, info, info)
        {
        }

        public SKPaintGLSurfaceEventArgs(SKSurface surface, GRBackendRenderTarget renderTarget, GRSurfaceOrigin origin, SKImageInfo info, SKImageInfo rawInfo)
        {
            Surface = surface;
            BackendRenderTarget = renderTarget;
            ColorType = info.ColorType;
            Origin = origin;
            Info = info;
            RawInfo = rawInfo;
        }

        public SKSurface Surface { get; private set; }

        public GRBackendRenderTarget BackendRenderTarget { get; private set; }

        public SKColorType ColorType { get; private set; }

        public GRSurfaceOrigin Origin { get; private set; }

        public SKImageInfo Info { get; private set; }

        public SKImageInfo RawInfo { get; private set; }
    }
}
