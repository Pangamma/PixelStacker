﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using PixelStacker.Extensions;
using System.Threading;
using System.Globalization;
using System.Linq;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Collections;
using Google.Cloud.Translation.V2;
using Newtonsoft.Json;
using System.Resources.NetStandard;
using Microsoft.Extensions.Configuration;

namespace PixelStacker.Tools.Generators
{

    [TestClass]
    public class JsonResxMaker
    {
        private string GOOGLE_API_KEY = "";
        private string RootDir = AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { "\\PixelStacker.Tools\\bin\\" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        private string[] OutputLocales = new string[] {
            "ko-kr", "ja-jp", "fr-fr", "de-de", "es-es", "zh-cn", "da-dk", "nl-nl"
        };

        public JsonResxMaker()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<JsonResxMaker>()
                .Build();
            this.GOOGLE_API_KEY = config["GOOGLE_API_KEY"];
        }

        [TestMethod]
        public void Text_Translate()
        {
            this.RipResxIntoJson($@"{RootDir}\PixelStacker.Logic\Resources\{nameof(Resources.Text)}.resx");
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
            string enJsonFilePath = RootDir + "\\PixelStacker.Logic\\Resources\\Localization\\en.json";
            var parsedEnglishKeys = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(enJsonFilePath)) ?? new Dictionary<string, string>();

            var keysToBeRemoved = parsedEnglishKeys.Where(kvp => !keys.ContainsKey(kvp.Key));
            var keysToBeAdded = keys.Where(kvp =>
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
                string jsonFilePath = RootDir + "\\PixelStacker.Logic\\Resources\\Localization\\"+lang+".json";
                if (!File.Exists(jsonFilePath)) File.WriteAllText(jsonFilePath, "{}");
                string existingJson = File.ReadAllText(jsonFilePath);
                Dictionary<string, string> parsed = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(jsonFilePath)) ?? new Dictionary<string, string>();

                foreach(var kvp in keysToBeRemoved)
                {
                    if (parsed.ContainsKey(kvp.Key))
                    {
                        parsed.Remove(kvp.Key);
                    }
                }

                // keysToBeAdded (bc new in english)
                // + keys in english not yet in locale, but allow keys 
                // not yet in english to have THEIR values used as higher priority.
                var keysToBeTranslatedForCurrentLocale = keys.Where(enKvp => !parsed.ContainsKey(enKvp.Key)).ToDictionary(k => k.Key, v => v.Value);
                foreach (var kvp in keysToBeAdded) { keysToBeTranslatedForCurrentLocale[kvp.Key] = kvp.Value; }

                foreach (var kvp in keysToBeTranslatedForCurrentLocale)
                {
                    string translated = GetTranslatedText(kvp.Value, lang);
                    parsed[kvp.Key] = translated;
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
            
            foreach(var kvp in dataNodes.OfType<XmlElement>())
            {
                if (kvp.HasAttribute("name") && kvp.HasChildNodes)
                {
                    string key = kvp.GetAttribute("name");
                    if (key.StartsWith("$")) continue;
                    if (key.StartsWith(">>")) continue;
                    var child = kvp.ChildNodes.OfType<XmlElement>().FirstOrDefault();
                    if (child != null)
                    {
                        string val = child.InnerText;
                        kvps[key] = val;
                    }
                }
            }

            //using (ResXResourceReader rr = ResXResourceReader.FromFileContents(resxFilePath))
            //{
            //    IDictionaryEnumerator di = rr.GetEnumerator();

            //    foreach (DictionaryEntry de in rr)
            //    {
            //        string key = de.Key as string;
            //        string value = de.Value as string;

            //        if (key.StartsWith("$")) continue;
            //        if (key.StartsWith(">>")) continue;

            //        kvps[key] = value;
            //    }
            //}
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