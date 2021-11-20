using Newtonsoft.Json;
using PixelStacker.Logic.IO.Config;

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
            Properties.Settings.Default.Upgrade();
            string json = Properties.Settings.Default.JSON;
            var rt = JsonConvert.DeserializeObject<Options>(json, SerializerSettings) ?? new Options(this);
            rt.StorageProvider = this;
            return rt;
        }

        public void Save(Options t)
        {
            string json = JsonConvert.SerializeObject(t, SerializerSettings);
            Properties.Settings.Default.JSON = json;
            Properties.Settings.Default.Save();
        }
    }
}
