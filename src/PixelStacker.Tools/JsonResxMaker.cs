using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using PixelStacker.Logic.Extensions;
using System.Threading;
using System.Globalization;
using System.Linq;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Resources;
using System.Collections;
using Google.Cloud.Translation.V2;
using Newtonsoft.Json;
using PixelStacker.UI;

namespace PixelStacker.Tools
{

    [TestClass]
    public class JsonResxMaker
    {
        private string GOOGLE_API_KEY = "";
        private string RootDir = AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { "\\PixelStacker.Tools\\bin\\" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        private string[] OutputLocales = new string[] {
            "ko-kr", "ja-jp", "fr-fr", "de-de", "es-es", "zh-cn", "da-dk"
        };

        [TestMethod]
        public void Text_Translate()
        {
            this.RipResxIntoJson($@"{RootDir}\PixelStacker\Resources\{nameof(Resources.Text)}.resx");
        }


        private void RipResxIntoJson(string filePath)
        {
            #region SWAP designer code
            {
                var fileToEdit = filePath.Replace(".resx", ".Designer.cs");
                var fileToEditData = File.ReadAllText(fileToEdit);
                fileToEditData = fileToEditData.Replace(
                    "global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager(\"PixelStacker.Resources.Text\", typeof(Text).Assembly);",
                    "global::System.Resources.ResourceManager temp = new global::PixelStacker.Resources.Localization.ResxHelper();"
                    );
                File.WriteAllText(fileToEdit, fileToEditData);
            }
            #endregion


            var keys = ReadResxIntoDictionary(filePath);
            string enJsonFilePath = RootDir + "\\PixelStacker\\Resources\\Localization\\en.json";
            var parsedEnglishKeys = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(enJsonFilePath)) ?? new Dictionary<string, string>(); ;

            var keysToBeRemoved = parsedEnglishKeys.Where(kvp => !keys.ContainsKey(kvp.Key));
            var keysToBeTranslated = keys.Where(kvp =>
            {
                if (!parsedEnglishKeys.ContainsKey(kvp.Key)) return true;
                if (parsedEnglishKeys[kvp.Key] != kvp.Value) return true;
                return false;
            });

            File.WriteAllText(enJsonFilePath, JsonConvert.SerializeObject(keys, new JsonSerializerSettings()
            {
                Formatting = Newtonsoft.Json.Formatting.Indented
            }));


            foreach(var locale in OutputLocales)
            {
                string lang = locale.Split('-').First();
                string jsonFilePath = RootDir + "\\PixelStacker\\Resources\\Localization\\"+lang+".json";
                string existingJson = File.ReadAllText(jsonFilePath);
                Dictionary<string, string> parsed = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(jsonFilePath)) ?? new Dictionary<string, string>();

                foreach(var kvp in keysToBeRemoved)
                {
                    if (parsed.ContainsKey(kvp.Key))
                    {
                        parsed.Remove(kvp.Key);
                    }
                }


                foreach (var kvp in keysToBeTranslated)
                {
                    if (!parsed.ContainsKey(kvp.Key))
                    {
                        string translated = GetTranslatedText(kvp.Value, lang);
                        parsed[kvp.Key] = translated;
                    }
                }

                File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(parsed, new JsonSerializerSettings()
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented
                }));
            }
        }

        protected Dictionary<string, string> ReadResxIntoDictionary(string resxFilePath)
        {
            Dictionary<string, string> kvps = new Dictionary<string, string>();
            XmlDocument doc = new XmlDocument();
            doc.Load(resxFilePath);
            XmlNodeList dataNodes = doc.SelectNodes("//root/data");
            int totalKeys = dataNodes.Count;

            using (ResXResourceReader rr = new ResXResourceReader(resxFilePath))
            {
                //using (ResXResourceWriter rw = new ResXResourceWriter(outputFilePath))
                {
                    IDictionaryEnumerator di = rr.GetEnumerator();

                    foreach (DictionaryEntry de in rr)
                    {
                        string key = de.Key as string;
                        string value = de.Value as string;

                        if (key.StartsWith("$")) continue;
                        if (key.StartsWith(">>")) continue;

                        kvps[key] = value;
                        //if (!String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(value))
                        //{
                        //    string translatedValue = GetTranslatedText(value, locale);
                        //    rw.AddResource(key, translatedValue);
                        //}
                    }

                    //rw.Generate();
                }
            }
            return kvps;
        }

        private string GetTranslatedText(string text, string localeCode)
        {
            TranslationClient client = TranslationClient.CreateFromApiKey(apiKey: GOOGLE_API_KEY);
            TranslationResult result = client.TranslateText(text, localeCode.Substring(0,2).ToLower());
            return result.TranslatedText;
        }
    }
}
