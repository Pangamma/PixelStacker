using Newtonsoft.Json;
using PixelStacker.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace PixelStacker.IO.Config
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class Options
    {
        /// <summary>
        /// Tracks how long ago an update check was performed, and if user has decided to skip
        /// the current latest version.
        /// </summary>
        public UpdateSettings UpdateSettings { get; set; } = new UpdateSettings();

        public string Locale { get; set; } = "en-us";

        /// <summary>
        /// Which materials are enabled or disabled. TRUE if enabled. The PixelStacker_ID value
        /// is used as the key.
        /// </summary>
        public Dictionary<string, bool> EnableStates { get; set; } = new Dictionary<string, bool>();

        [Obsolete("Avoid this.", false)]
        public Dictionary<string, string> CustomValues { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// TRUE if stained glass panes can be layered ontop of regular blocks
        /// </summary>
        public bool IsMultiLayer { get; set; } = true;

        /// <summary>
        /// TRUE if stained glass panes MUST be layered ontop of regular blocks
        /// </summary>
        public bool IsMultiLayerRequired { get; set; } = false;

        [Obsolete]
        public bool IsSideView
        {
            get => this.PreprocessorSettings.IsSideView;
            set => this.PreprocessorSettings.IsSideView = value;
        }

        /// <summary>
        /// Purely for aesthetic rendering. Assuming no block/material filter is set, any blocks with
        /// only 1 type of material will show up as layer 1 instead of on both layers. So... some 
        /// shadows will appear to give the image more depth.
        /// </summary>
        public bool IsExtraShadowDepthEnabled
        {
            get => _IsExtraShadowDepthEnabled && this.IsAdvancedModeEnabled;
            set => _IsExtraShadowDepthEnabled = value && this.IsAdvancedModeEnabled;
        }
        private bool _IsExtraShadowDepthEnabled = false;

        /// <summary>
        /// When material filters are enabled, shadows will be rendered to help the viewer 
        /// percieve depth. This helps to visibly separate the different layers from each other.
        /// </summary>
        public bool IsShadowRenderingSkipped { get; set; } = false;


        /// <summary>
        /// R/5, G/5, B/5
        /// The value all RGB values should be divided by to achieve awesome truncating.
        /// Basically, 251 would become 250. This is used by the Color cache size
        /// settings dropdown in the pre-render/quantizer options menu.
        /// </summary>
        public int PreRender_ColorCacheFragmentSize { get; set; } = 1;

        public int GridSize { get; set; } = 16;
        public Color GridColor { get; set; } = Color.Black;

        public bool PreRender_IsEnabled { get; set; } = false;
        public string PreRender_Algorithm { get; set; } = "HSL distinct selection";
        public string PreRender_ColorCache { get; set; } = "Octree search";
        public int PreRender_ColorCount { get; set; } = 256;
        public string PreRender_Dither { get; set; } = "No dithering";
        public int PreRender_Parallel { get; set; } = 4;

        public bool Rendered_IsShowGrid { get; set; } = false;
        public bool Rendered_IsSolidColors { get; set; } = false;
        public bool Rendered_IsShowBorder { get; set; } = false;
        public bool Rendered_IsColorPalette { get; set; } = false;
        public int Rendered_RenderedZIndexToShow { get; set; } = 0;
        public bool Rendered_IsRenderedZIndexFilteringEnabled { get; set; } = false;

        /// <summary>
        /// TRUE is a vertical build. FALSE is a horizontal build.
        /// </summary>
        public CanvasPreprocessorSettings PreprocessorSettings { get; set; } = new CanvasPreprocessorSettings()
        {
            IsSideView = false,
            MaxHeight = null,
            MaxWidth = null
        };

        [Obsolete]
        [JsonIgnore]
        public int? MaxWidth
        {
            get
            {
                int? val = (IsSideView) ? this.GetValue<int>("MaxWidthSideView") : this.GetValue<int>("MaxWidthTopView");
                val = (val == 0) ? null : val;
                return val;
            }
            set
            {
                if (IsSideView) this.SetValue<int>("MaxWidthSideView", value ?? 0);
                else this.SetValue<int>("MaxWidthTopView", value ?? 0);
            }
        }

        [Obsolete]
        [JsonIgnore]
        public int? MaxHeight
        {
            get
            {
                int? val = (IsSideView) ? this.GetValue<int>("MaxHeightSideView") : this.GetValue<int>("MaxHeightTopView");
                val = (val == 0) ? null : val;
                return val;
            }
            set
            {
                if (IsSideView) this.SetValue<int>("MaxHeightSideView", value ?? 0);
                else this.SetValue<int>("MaxHeightTopView", value ?? 0);
            }
        }

        private static Options _self;
        [Obsolete(Constants.Obs_Static)]
        public static Options Get
        {
            get
            {
                if (_self == null)
                {
                    _self = StorageProvider.Load() ?? new Options();
                }
                return _self;
            }
        }

        //internal static Options Import(string filePath)
        //{
        //    string json = File.ReadAllText(filePath);
        //    _self = JsonConvert.DeserializeObject<Options>(json) ?? new Options();
        //    return _self;
        //}

        //internal static void Export(string filePath)
        //{
        //    string json = JsonConvert.SerializeObject(Options._self ?? new Options(), Formatting.Indented);
        //    File.WriteAllText(filePath, json);
        //}

        /// <summary>
        /// Contains stuff like "AIR_00"
        /// </summary>
        public List<string> SelectedMaterialFilter { get; set; } = new List<string>();
        public bool IsAdvancedModeEnabled { get; set; } = false;

        private string MakeKey(string input)
        {
            return input.Replace(' ', '_');
        }

        private void SetValue<T>(string key, T val) where T : struct
        {
            key = MakeKey(key);
            CustomValues[key] = val.ToString();
        }

        private T? GetValue<T>(string key) where T : struct
        {
            key = MakeKey(key);
            if (CustomValues.ContainsKey(key))
            {
                var val = CustomValues[key].ToNullable<T>();
                return val;
            }

            return null;
        }

        public static void Save() => StorageProvider.Save(Options.Get);

        public static void Reset()
        {
            StorageProvider.Save(new Options());
            _self = null;
        }

        public static void Load(Options o)
        {
            StorageProvider.Save(o);
            _self = o;
        }

        public static IStorage<Options> StorageProvider { get; set; } = new MemoryStorage<Options>();
    }

    public class UpdateSettings
    {
        public DateTime? LastChecked { get; set; } = null;
        public string SkipNotifyIfVersionIs { get; set; } = Constants.Version;
    }
}
#pragma warning restore CS0618 // Type or member is obsolete