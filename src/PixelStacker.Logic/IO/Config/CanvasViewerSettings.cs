using Newtonsoft.Json;
using PixelStacker.Logic.IO.JsonConverters;
using SkiaSharp;
using System;
using System.Drawing;

namespace PixelStacker.Logic.IO.Config
{
    public class CanvasViewerSettings
    {
        /// <summary>
        /// When material filters are enabled, shadows will be rendered to help the viewer 
        /// percieve depth. This helps to visibly separate the different layers from each other.
        /// 
        /// Purely for aesthetic rendering. Assuming no block/material filter is set, any blocks with
        /// only 1 type of material will show up as layer 1 instead of on both layers. So... some 
        /// shadows will appear to give the image more depth.
        /// </summary>
        public bool IsShadowRenderingEnabled { get; set; } = true;
        public int GridSize { get; set; } = 16;

        [JsonConverter(typeof(SKColorJsonTypeConverter))]
        public SKColor GridColor { get; set; } = new SKColor(0, 0, 0);

        public bool IsShowBorder { get; set; } = false;
        public bool IsShowGrid { get; set; } = false;

        public bool IsSolidColors { get; set; } = false;

        [Obsolete(Constants.Obs_TryToRemove)]
        public bool IsColorPalette { get; set; } = false;

        /// <summary>
        /// NULL = show both layers
        /// 0 = show bottom layer
        /// 1 = show top layer
        /// </summary>
        public int? ZLayerFilter { get; set; } = null;
    }
}
