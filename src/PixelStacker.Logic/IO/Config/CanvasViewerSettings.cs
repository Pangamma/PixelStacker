using Newtonsoft.Json;
using PixelStacker.Logic.IO.JsonConverters;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace PixelStacker.Logic.IO.Config
{
    public interface IReadonlyCanvasViewerSettings
    {
        [Newtonsoft.Json.JsonIgnore]
        public int BlocksPerChunk { get; }

        /// <summary>
        /// When material filters are enabled, shadows will be rendered to help the viewer 
        /// percieve depth. This helps to visibly separate the different layers from each other.
        /// 
        /// Purely for aesthetic rendering. Assuming no block/material filter is set, any blocks with
        /// only 1 type of material will show up as layer 1 instead of on both layers. So... some 
        /// shadows will appear to give the image more depth.
        /// </summary>
        public bool IsShadowRenderingEnabled { get; }

        [Newtonsoft.Json.JsonIgnore]
        /// <summary>
        /// If empty, show everything.
        /// If not empty, only show what is contained within the filter set.
        /// If full, show nothing.
        /// </summary>
        public ISet<string> VisibleMaterialsFilter { get; }

        /// <summary>
        /// Whether to completely hide filtered out materials or if they should
        /// still be rendered but with some saturation adjustments.
        /// </summary>
        public bool ShowFilteredMaterials { get; }


        public int GridSize { get; }

        [JsonConverter(typeof(SKColorJsonTypeConverter))]
        public SKColor GridColor { get; }

        public bool IsShowBorder { get; }
        public bool IsShowGrid { get; }

        public bool IsSolidColors { get; }


        /// <summary>
        /// NULL = show both layers
        /// 0 = show bottom layer
        /// 1 = show top layer
        /// </summary>
        public int? ZLayerFilter { get; }

        public int TextureSize { get; }
    }

    public class CanvasViewerSettings: IReadonlyCanvasViewerSettings
    {
        /// <summary>
        /// When material filters are enabled, shadows will be rendered to help the viewer 
        /// percieve depth. This helps to visibly separate the different layers from each other.
        /// 
        /// Purely for aesthetic rendering. Assuming no block/material filter is set, any blocks with
        /// only 1 type of material will show up as layer 1 instead of on both layers. So... some 
        /// shadows will appear to give the image more depth.
        /// </summary>
        public virtual bool IsShadowRenderingEnabled { get; set; } = true;

        /// <summary>
        /// If empty, show everything.
        /// If not empty, only show what is contained within the filter set.
        /// If full, show nothing.
        /// </summary>
        //[Newtonsoft.Json.JsonIgnore]
        public ISet<string> VisibleMaterialsFilter { get; set; } = new HashSet<string>();
        public bool ShowFilteredMaterials { get; set; } = true;

        public int GridSize { get; set; } = 16;

        [JsonConverter(typeof(SKColorJsonTypeConverter))]
        public SKColor GridColor { get; set; } = new SKColor(0, 0, 0);

        public bool IsShowBorder { get; set; } = false;
        public bool IsShowGrid { get; set; } = false;

        public bool IsSolidColors { get; set; } = false;

        /// <summary>
        /// NULL = show both layers
        /// 0 = show bottom layer
        /// 1 = show top layer
        /// </summary>
        public int? ZLayerFilter { get; set; } = null;

        public int _textureSize { get; set; } = Constants.DefaultTextureSize;
        public int TextureSize
        {
            get => _textureSize;
            set
            {
                _textureSize
                = value == 16 ? 16
                : value == 32 ? 32
                : value == 64 ? 64
                : 16;
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        public int BlocksPerChunk => _textureSize == 16 ? 38
               : _textureSize == 32 ? 19
               : 10;


        public CanvasViewerSettings()
        {
            TextureSize = Constants.DefaultTextureSize;
        }

        /// <summary>
        /// Creates a read-only copy of the toClone object.
        /// </summary>
        /// <param name="toClone"></param>
        public CanvasViewerSettings(CanvasViewerSettings toClone)
        {
            this.IsShadowRenderingEnabled = toClone.IsShadowRenderingEnabled;
            this.ZLayerFilter = toClone.ZLayerFilter;
            this.VisibleMaterialsFilter = toClone.VisibleMaterialsFilter.ToImmutableHashSet();
            this.ShowFilteredMaterials = toClone.ShowFilteredMaterials;
            this.TextureSize = toClone.TextureSize;
            this.GridSize = toClone.GridSize;
            this.GridColor = toClone.GridColor;
            this.GridSize = toClone.GridSize;
            this.IsShowBorder = toClone.IsShowBorder;
            this.IsShowGrid = toClone.IsShowGrid;
            this.IsSolidColors = toClone.IsSolidColors;
        }

        public IReadonlyCanvasViewerSettings ToReadonlyClone()
        {
            return (IReadonlyCanvasViewerSettings) new CanvasViewerSettings(this);
        }

    }
}
