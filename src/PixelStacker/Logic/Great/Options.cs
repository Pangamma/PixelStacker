using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic
{
    public class Options
    {
        public UpdateSettings UpdateSettings { get; set; } = new UpdateSettings();
        public Dictionary<string,bool> EnableStates { get; set; } = new Dictionary<string, bool>();
        public Dictionary<string, string> CustomValues { get; set; } = new Dictionary<string, string>();
        public bool IsMultiLayer { get; set; } = true;
        public bool IsSideView { get; set; } = false;
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
        public static Options Get
        {
            get
            { 
                if (_self == null)
                {
                    Properties.Settings.Default.Upgrade();
                    string json = Properties.Settings.Default.JSON;
                    _self = JsonConvert.DeserializeObject<Options>(json) ?? new Options();
                }
                return _self;
            }
        }

        private string makeKey(string input)
        {
            return input.Replace(' ', '_');
        }

        public bool IsEnabled(string key, bool defVal = false)
        {
            key = makeKey(key);
            if (EnableStates.ContainsKey(key))
            {
                return EnableStates[key];
            }
            return defVal;
        }

        public void SetEnabled(string key, bool val)
        {
            key = makeKey(key);
            EnableStates[key] = val;
        }

        private void SetValue<T>(string key, T val) where T : struct
        {
            key = makeKey(key);
            CustomValues[key] = val.ToString();
        }

        private T? GetValue<T>(string key) where T : struct
        {
            key = makeKey(key);
            if (CustomValues.ContainsKey(key))
            {
                var val = CustomValues[key].ToNullable<T>();
                return val;
            }

            return null;
        }

        public static void Save()
        {
            string json = JsonConvert.SerializeObject(Options.Get);
            Properties.Settings.Default.JSON = json;
            Properties.Settings.Default.Save();
        }

        public static void Reset()
        {
            Properties.Settings.Default.JSON = "{}";
            Properties.Settings.Default.Save();
            _self = null;
        }
    }
}
