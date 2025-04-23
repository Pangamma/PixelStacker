using Newtonsoft.Json;
using PixelStacker.Logic.IO.Config;
using System;
using System.Configuration;
using System.Diagnostics;

namespace PixelStacker.IO
{

    public class WinFormsOptionsProvider : IOptionsProvider
    {
        private static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
        };

        public Options Load()
        {
            try
            {
                //Properties.Settings.Default.Upgrade();
                string json = Properties.Settings.Default.JSON;
                var rt = JsonConvert.DeserializeObject<Options>(json, SerializerSettings);
                rt ??= new Options(this);
                rt.StorageProvider = this;
                return rt;
            } 
            catch(ConfigurationErrorsException ex)
            {
                Debug.WriteLine("Failed to load config file. Resetting config and starting fresh instead.");
                Debug.WriteLine(ex);
            }

            return new Options(this);
        }

        public void Save(Options t)
        {
            Options old = this.Load();
            FireChangeEvents(old, t, opts => opts.Tools.PrimaryColor, nameof(AppEvents.OnPrimaryColorChange));
            FireChangeEvents(old, t, opts => opts.Tools.BrushWidth, nameof(AppEvents.OnBrushWidthChange));
            FireChangeEvents(old, t, opts => opts.IsAdvancedModeEnabled, nameof(AppEvents.OnAdvancedModeChange));
            FireChangeEvents(old, t, opts => opts.IsSideView, nameof(AppEvents.IsSideViewChange));

            string json = JsonConvert.SerializeObject(t, SerializerSettings);
            Properties.Settings.Default.JSON = json;
            Properties.Settings.Default.Save();
        }

        private void FireChangeEvents<T>(Options old, Options nw, Func<Options, T> func, string nameOfEvent)
        {
#if FAIL_FAST
            var ex = RateLimit.CheckWithException(20, 1000, methodName: nameOfEvent);
            if (ex != null)
                throw ex;
#endif

            T oVal = default;
            T nVal = default;
            try { oVal = func(old); } catch (NullReferenceException) { }
            try { nVal = func(nw); } catch (NullReferenceException) { }

            if (!object.Equals(oVal, nVal))
            {
                AppEvents.PublishEvent(nameOfEvent, new OptionsChangeEvent<T>()
                {
                    NewValue = nVal,
                    PreviousValue = oVal,
                });
            }
        }
    }
}
