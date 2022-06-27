using Newtonsoft.Json;
using PixelStacker.Logic.IO.Config;
using System;
using System.Configuration;
using System.Diagnostics;

namespace PixelStacker.IO
{
    public class LocalDataOptionsProvider : IOptionsProvider
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
            string json = JsonConvert.SerializeObject(t, SerializerSettings);
            Properties.Settings.Default.JSON = json;
            Properties.Settings.Default.Save();
        }
    }
}
