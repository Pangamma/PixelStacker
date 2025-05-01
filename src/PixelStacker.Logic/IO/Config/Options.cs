﻿using Newtonsoft.Json;
using PixelStacker.Logic.Engine.Quantizer.Enums;
using PixelStacker.Logic.Extensions;
using PixelStacker.Resources;
using PixelStacker.Resources.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PixelStacker.Logic.IO.Config
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class Options
    {
        [JsonIgnore]
        public IOptionsProvider StorageProvider { get; set; }
        public Options(IOptionsProvider storageProvider)
        {
            StorageProvider = storageProvider;
        }

        public MaterialSelectOptions MaterialOptions { get; set; } = new MaterialSelectOptions();

        /// <summary>
        /// Tracks how long ago an update check was performed, and if user has decided to skip
        /// the current latest version.
        /// </summary>
        public UpdateSettings UpdateSettings { get; set; } = new UpdateSettings();
        public CanvasViewerSettings ViewerSettings { get; set; } = new CanvasViewerSettings();

        public string Locale { get; set; } = ResxHelper.GetSupportedLocale();
        public AppTheme Theme { get; set; } = AppTheme.Light;


        /// <summary>
        /// Which materials are enabled or disabled. TRUE if enabled. The PixelStacker_ID value
        /// is used as the key.
        /// </summary>
        public Dictionary<string, bool> EnableStates { get; set; } = new Dictionary<string, bool>();

        /// <summary>
        /// TRUE if stained glass panes can be layered ontop of regular blocks
        /// </summary>
        public bool IsMultiLayer { get; set; } = true;

        public ToolSettings Tools { get; set; } = new ToolSettings();

        /// <summary>
        /// TRUE if stained glass panes MUST be layered ontop of regular blocks
        /// </summary>
        public bool IsMultiLayerRequired { get; set; } = false;

        [Category("Colors")]
        public bool IsSideView { get; set; } = false;

        /// <summary>
        /// When material filters are enabled, shadows will be rendered to help the viewer 
        /// percieve depth. This helps to visibly separate the different layers from each other.
        /// </summary>
        public bool IsShadowRenderingSkipped { get; set; } = false;

        //public int GridSize { get; set; } = 16;
        //public Color GridColor { get; set; } = Color.Black;
        //public bool Rendered_IsShowGrid { get; set; } = false;
        //public bool Rendered_IsSolidColors { get; set; } = false;
        //public bool Rendered_IsShowBorder { get; set; } = false;
        //public bool Rendered_IsColorPalette { get; set; } = false;
        //public int Rendered_RenderedZIndexToShow { get; set; } = 0;
        //public bool Rendered_IsRenderedZIndexFilteringEnabled { get; set; } = false;

        public CanvasPreprocessorSettings Preprocessor { get; set; } = new CanvasPreprocessorSettings()
        {
            MaxHeight = null,
            MaxWidth = null,
            RgbBucketSize = 1,
            QuantizerSettings = new QuantizerSettings()
            {
                IsEnabled = false,
                Algorithm = QuantizerAlgorithm.WuColor,
                MaxColorCount = 256,
                DitherAlgorithm = "No dithering",
                MaxParallelProcesses = 1
            }
        };

        private static Options _self;
        [Obsolete(Constants.Obs_Static)]
        public static Options GetInMemoryFallback
        {
            get
            {
                if (_self == null)
                {
                    _self = new Options(new MemoryOptionsProvider());
                }
                return _self;
            }
        }

        public bool IsAdvancedModeEnabled { get; set; } = false;

        private string MakeKey(string input)
        {
            return input.Replace(' ', '_');
        }

        public void Save() => StorageProvider.Save(this);

        public void Reset()
        {
            StorageProvider.Save(new Options(StorageProvider));
            _self = null;
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete