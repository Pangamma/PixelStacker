using Newtonsoft.Json;
using PixelStacker.Resources.Localization;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;

namespace PixelStacker.Resources.Localization
{
    public class ResxHelper : ResourceManager
    {
        /// <summary>
        /// [en] => [[MyFoo_String] => [Value]]
        /// </summary>
        Dictionary<string, Dictionary<string, string>> Content = new Dictionary<string, Dictionary<string, string>>();
        public override string GetString(string name)
            => this.GetString(name, Thread.CurrentThread.CurrentUICulture);

        private Dictionary<string, string> ReadResxJson(string twoDigitLangCode)
        {
            byte[] data = ResxContainer.ResourceManager.GetObject(twoDigitLangCode.ToLower()) as byte[];
            var json = Encoding.UTF8.GetString(data);
            var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return dic ?? new Dictionary<string, string>();
        }

        public override string GetString(string name, CultureInfo culture)
        {
            if (culture == null) culture = CultureInfo.CurrentUICulture;
            string lang = culture.TwoLetterISOLanguageName;

            if (!Content.ContainsKey(lang))
            {
                Content[lang] = ReadResxJson(lang);
            }

            if (Content[lang].TryGetValue(name, out string value))
            {
                if (!string.IsNullOrEmpty(value))
                {
                    return value;
                }
            }

            return Content["en"][name];
        }
    }
}