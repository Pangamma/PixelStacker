using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using OpenAI;
using OpenAI.Chat;

namespace PixelStacker.CodeGenerator
{

    [TestClass]
    public class JsonResxMaker
    {
        private readonly Translator _translator;
        private string RootDir = AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { "\\PixelStacker.CodeGenerator\\bin\\" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        private string[] OutputLocales = new string[] {
            "da-dk", "de-de", "es-es", "fr-fr",
            "it-it", "ja-jp", "ko-kr", "nl-nl",
            "sv-se", "zh-cn", "zu-zu"
        };

        public JsonResxMaker()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<JsonResxMaker>()
                .Build();
            _translator = new Translator(config["OPENAI_API_KEY"]);
        }


        /// <summary>
        /// Translates any missing keys and adds them to the lang.json files.
        /// </summary>
        [TestMethod]
        [TestCategory("Generators")]
        public async Task Text_Translate()
        {
            await RipResxIntoJsonAsync($@"{RootDir}\PixelStacker.Resources\Text.resx");
        }


        private async Task RipResxIntoJsonAsync(string filePath)
        {
            #region SWAP designer code
            {
                var fileToEdit = filePath.Replace(".resx", ".Designer.cs");
                var fileToEditData = File.ReadAllText(fileToEdit);
                fileToEditData = fileToEditData.Replace(
                    "global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager(\"PixelStacker.Resources.Text\", typeof(Text).Assembly);",
                    "global::System.Resources.ResourceManager temp = new global::PixelStacker.Resources.ResxHelper();"
                    );
                File.WriteAllText(fileToEdit, fileToEditData);
            }
            #endregion


            var keys = ReadResxIntoDictionary(filePath);
            string enJsonFilePath = RootDir + "\\PixelStacker.Resources\\Localization\\en.json";
            var parsedEnglishKeys = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(enJsonFilePath)) ?? new Dictionary<string, string>();

            var keysToBeRemoved = parsedEnglishKeys.Where(kvp => !keys.ContainsKey(kvp.Key));
            var keysToBeAdded = keys.Where(kvp =>
            {
                if (!parsedEnglishKeys.ContainsKey(kvp.Key)) return true;
                if (parsedEnglishKeys[kvp.Key] != kvp.Value) return true;
                return false;
            });

            File.WriteAllText(enJsonFilePath, JsonConvert.SerializeObject(keys.OrderBy(k => k.Key).ToDictionary(k => k.Key, v => v.Value), new JsonSerializerSettings()
            {
                Formatting = Newtonsoft.Json.Formatting.Indented
            }));


            foreach (var locale in OutputLocales)
            {
                string lang = locale.Split('-').First();
                string jsonFilePath = RootDir + "\\PixelStacker.Resources\\Localization\\" + lang + ".json";
                if (!File.Exists(jsonFilePath)) File.WriteAllText(jsonFilePath, "{}");
                string existingJson = File.ReadAllText(jsonFilePath);
                Dictionary<string, string> parsed = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(jsonFilePath)) ?? new Dictionary<string, string>();

                foreach (var kvp in keysToBeRemoved)
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

                string targetLangName = new CultureInfo(locale).EnglishName;
                foreach (var kvp in keysToBeTranslatedForCurrentLocale)
                {
                    string translated = await _translator.TranslateAsync(kvp.Value, "English", targetLangName);
                    parsed[kvp.Key] = translated;
                }

                File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(parsed.OrderBy(k => k.Key).ToDictionary(k => k.Key, v => v.Value), new JsonSerializerSettings()
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented
                }));
            }
        }

        protected static Dictionary<string, string> ReadResxIntoDictionary(string resxFilePath)
        {
            Dictionary<string, string> kvps = new Dictionary<string, string>();
            XmlDocument doc = new XmlDocument();
            doc.Load(resxFilePath);
            XmlNodeList dataNodes = doc.SelectNodes("//root/data");

            foreach (var kvp in dataNodes.OfType<XmlElement>())
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

            return kvps;
        }

    }

    public class Translator
    {
        private readonly ChatClient _chat;

        public Translator(string openAiApiKey)
        {
            if (string.IsNullOrEmpty(openAiApiKey))
            {
                throw new InvalidOperationException("Missing OPENAI_API_KEY in user secrets.");
            }

            var api = new OpenAIClient(openAiApiKey);
            _chat = api.GetChatClient("gpt-4o-mini");
        }

        /// <summary>
        /// Translates text from one language to another using the official OpenAI SDK.
        /// Use "auto" for sourceLang if you don't know the source language.
        /// </summary>
        public async Task<string> TranslateAsync(string text, string sourceLang, string targetLang)
        {
            List<ChatMessage> messages =
            [
                new SystemChatMessage($"Translate from {sourceLang} to {targetLang}. Keep the same tone and emotion. Output only translation."),
                new SystemChatMessage($"Do not translate URLs."),
                new UserChatMessage(text),
            ];

            ChatCompletionOptions options = new()
            {
                ResponseFormat = ChatResponseFormat.CreateTextFormat()
            };

            ChatCompletion completion = await _chat.CompleteChatAsync(messages, options);

            string trans = completion.Content[0].Text;
            if (trans.Contains("sorry") || trans.Contains("I'm sorry, but I can't assist with that."))
            {
                Console.WriteLine("Translation failed or was not possible.");
            }

            return trans;
        }
    }
}
