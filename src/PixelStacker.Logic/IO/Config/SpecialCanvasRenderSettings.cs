
using System;

namespace PixelStacker.Logic.IO.Config
{
    public class SpecialCanvasRenderSettings
    {
        public int? ZLayerFilter { get; set; } = null;
        public bool IsSolidColors { get; set; } = false;
        /// <summary>
        /// Purely for aesthetic rendering. Assuming no block/material filter is set, any blocks with
        /// only 1 type of material will show up as layer 1 instead of on both layers. So... some 
        /// shadows will appear to give the image more depth.
        /// </summary>
        public bool EnableShadows { get; set; } = false;
        public int TextureSize { get; set; } = Constants.DefaultTextureSize;

        [Newtonsoft.Json.JsonIgnore]
        public readonly int BlocksPerChunk;

        public SpecialCanvasRenderSettings()
        {
            BlocksPerChunk = 38;
            TextureSize = Constants.DefaultTextureSize;
        }

        [Obsolete($"Prefer the option that directly takes a {nameof(CanvasViewerSettings)} object.")]
        public SpecialCanvasRenderSettings(Options opts)
        {
            this.ZLayerFilter = opts.ViewerSettings.ZLayerFilter;
            this.IsSolidColors = opts.ViewerSettings.IsSolidColors;
            this.EnableShadows = opts.ViewerSettings.IsShadowRenderingEnabled;
            this.TextureSize = opts.ViewerSettings.TextureSize;
            if (this.EnableShadows == false)
                this.TextureSize = Constants.DefaultTextureSize;
            this.BlocksPerChunk = this.TextureSize == 16 ? 38
           : this.TextureSize == 32 ? 19
           : 10;
        }

        public SpecialCanvasRenderSettings(CanvasViewerSettings viewerSettings)
        {
            this.ZLayerFilter = viewerSettings.ZLayerFilter;
            this.IsSolidColors = viewerSettings.IsSolidColors;
            this.EnableShadows = viewerSettings.IsShadowRenderingEnabled;
            this.TextureSize = viewerSettings.TextureSize;

            // We don't notice resolution unless shadows are turned on.
            if (this.EnableShadows == false)
                this.TextureSize = Constants.DefaultTextureSize;

            this.BlocksPerChunk = this.TextureSize == 16 ? 38
           : this.TextureSize == 32 ? 19
           : 10;
        }
    }
}

