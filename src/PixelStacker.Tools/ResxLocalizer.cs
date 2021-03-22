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
using PixelStacker.Logic;
using System.Xml;
using System.Resources;
using System.Collections;
using Google.Cloud.Translation.V2;
using PixelStacker.UI;

namespace PixelStacker.Tools
{

    [TestClass]
    [Obsolete("I will keep this file handy, since it is an automatic resx localizer. But it is no longer used in this project.")]
    public class ResxLocalizer
    {
        private bool SKIP_EXISTING_LOCALES = false;
        private string GOOGLE_API_KEY = "<INSERT API KEY HERE>";
        private string RootDir = AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { "\\PixelStacker.Tools\\bin\\" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        private string[] OutputLocales = new string[] {
            "ko-kr", "ja-jp", "fr-fr", "de-de", "es-es", "zh-cn", "da-dk"
        };

        [TestMethod]
        public void Text_Translate()
        {
            Assert.Fail();
            this.TranslateFile($@"{RootDir}\PixelStacker\Resources\{nameof(Resources.Text)}.resx", this.OutputLocales);
        }

        [TestMethod]
        public void TranslateALL()
        {
            var files = Directory.GetFiles(RootDir, "*.resx", SearchOption.AllDirectories);
            for(int fi = 0; fi < files.Length; fi++)
            {
                for (int li = 0; li < OutputLocales.Length; li++)
                {
                    FileInfo fii = new FileInfo(files[fi]);
                    if (fii.Name.Split('.').Length == 2)
                    {
                        Console.WriteLine($"Translating {fii.Name} to {OutputLocales[li]}.");

                        TranslateFile(files[fi], OutputLocales[li]);
                    }
                }
            }
        }

        [Obsolete("I will keep this file handy, since it is an automatic resx localizer. But it is no longer used in this project.")]
        private void TranslateFile(string filePath, string[] locales)
        {
            foreach(var locale in locales)
            {
                TranslateFile(filePath, locale);
            }
        }

        [Obsolete("I will keep this file handy, since it is an automatic resx localizer. But it is no longer used in this project.")]
        protected void TranslateFile(string resxFilePath, string locale)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(resxFilePath);
            XmlNodeList dataNodes = doc.SelectNodes("//root/data");
            int totalKeys = dataNodes.Count;

            using (ResXResourceReader rr = new ResXResourceReader(resxFilePath))
            {
                // string outputFilePath = String.Format("{0}.{1}.{2}", Path.GetFileNameWithoutExtension(resxFilePath), language.ToString().Substring(0, 2).ToLower(), Path.GetExtension(resxFilePath));
                string outputFilePath = resxFilePath.Replace("resx", "") + locale.ToString().Substring(0, 2).ToLower() + ".resx";
                if (File.Exists(outputFilePath) && SKIP_EXISTING_LOCALES) return;

                using (ResXResourceWriter rw = new ResXResourceWriter(outputFilePath))
                {
                    IDictionaryEnumerator di = rr.GetEnumerator();

                    foreach (DictionaryEntry de in rr)
                    {
                        string key = de.Key as string;
                        string value = de.Value as string;

                        if (key.StartsWith("$")) continue;
                        if (key.StartsWith(">>")) continue;

                        if (!String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(value))
                        {
                            string translatedValue = GetTranslatedText(value, locale);
                            rw.AddResource(key, translatedValue);
                        }
                    }

                    rw.Generate();
                }
            }

        }

        private string GetTranslatedText(string text, string localeCode)
        {
            TranslationClient client = TranslationClient.CreateFromApiKey(apiKey: GOOGLE_API_KEY);
            TranslationResult result = client.TranslateText(text, localeCode.Substring(0,2).ToLower());
            return result.TranslatedText;
        }
    }
}
