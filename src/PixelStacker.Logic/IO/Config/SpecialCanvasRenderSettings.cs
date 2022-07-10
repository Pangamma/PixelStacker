namespace PixelStacker.Logic.IO.Config
{
    public class SpecialCanvasRenderSettings
    {
        public int? ZLayerFilter { get; private set; } = null;
        public bool IsSolidColors { get; private set; } = false;
        /// <summary>
        /// Purely for aesthetic rendering. Assuming no block/material filter is set, any blocks with
        /// only 1 type of material will show up as layer 1 instead of on both layers. So... some 
        /// shadows will appear to give the image more depth.
        /// </summary>
        public bool EnableShadows { get; private set; } = true;

        public SpecialCanvasRenderSettings() { }
        public SpecialCanvasRenderSettings(Options opts)
        {
            this.ZLayerFilter = opts.ViewerSettings.ZLayerFilter;
            this.IsSolidColors = opts.ViewerSettings.IsSolidColors;
            this.EnableShadows = opts.ViewerSettings.IsShadowRenderingEnabled;
        }
    }
}
