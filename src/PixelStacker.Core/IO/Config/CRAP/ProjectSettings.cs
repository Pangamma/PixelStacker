using PixelStacker.Core.Model.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Core.IO.Config
{
    public class AllSettings : IProjectSettings, IGlobalSettings
    {
        #region GLOBAL
        /// <inheritdoc/>
        public UpdateSettings UpdateSettings { get; set; } = new UpdateSettings();

        /// <inheritdoc/>
        public string Locale { get; set; } = "en-us";

        /// <inheritdoc/>
        public bool IsAdvancedModeEnabled { get; set; } = false;
        #endregion GLOBAL

        #region Generator Settings / Color Palette

        /// <inheritdoc/>
        public bool IsMultiLayer { get; set; } = true;

        /// <inheritdoc/>
        public bool IsMultiLayerRequired { get; set; } = false;

        /// <inheritdoc/>
        public bool IsSideView { get; set; } = false;
        #endregion

        /// <inheritdoc/>
        public CanvasPreprocessorSettings Preprocessor { get; set; } = new CanvasPreprocessorSettings()
        {
            MaxHeight = null,
            MaxWidth = null,
            RgbBucketSize = 1,
            QuantizerSettings = new QuantizerSettings()
            {
                IsEnabled = false,
                //Algorithm = QuantizerAlgorithm.HslDistinctSelection,
                ColorCache = "Octree search",
                MaxColorCount = 256,
                DitherAlgorithm = "No dithering",
                MaxParallelProcesses = 1
            }
        };
    }

    public interface IGlobalSettings
    {
        /// <summary>
        /// Tracks how long ago an update check was performed, and if user has decided to skip
        /// the current latest version.
        /// </summary>
        public UpdateSettings UpdateSettings { get; set; }

        /// <summary>
        /// The locale for the program
        /// </summary>
        public string Locale { get ; set; }

        /// <summary>
        /// Toggle through the konami code. (up up down down left right left right B A Enter)
        /// Enables advanced materials and other advanced modes.
        /// </summary>
        public bool IsAdvancedModeEnabled { get; set; }
    }
    

    public class GeneratorSettings
    {

    }

    public class ViewSettings
    {
        /// <summary>
        /// Purely for aesthetic rendering. Assuming no block/material filter is set, any blocks with
        /// only 1 type of material will show up as layer 1 instead of on both layers. So... some 
        /// shadows will appear to give the image more depth.
        /// </summary>
        public bool IsArtisticRenderingEnabled { get; set; }
        //{
        //    get => _IsExtraShadowDepthEnabled && IsAdvancedModeEnabled;
        //    set => _IsExtraShadowDepthEnabled = value && IsAdvancedModeEnabled;
        //}
        //private bool _IsExtraShadowDepthEnabled = false;

        /// <summary>
        /// When material filters are enabled, shadows will be rendered to help the viewer 
        /// percieve depth. This helps to visibly separate the different layers from each other.
        /// </summary>
        public bool IsShadowRenderingSkipped { get; set; } = false;
    }

    public interface IProjectSettings
    {
        public bool IsSideView { get; set; }
        /// <summary>
        /// </summary>
        /// <see cref="CanvasPreprocessorSettings"/>
        public CanvasPreprocessorSettings Preprocessor { get; set; }
    }
}
