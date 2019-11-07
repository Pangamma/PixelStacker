using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.Logic
{
    public class UpdateSettings
    {
        public DateTime? LastChecked { get; set; } = null;
        public string SkipNotifyIfVersionIs { get; set; } = Constants.Version;
    }

    public class UpdateChecker
    {
        public async static void CheckForUpdates()
        {
            await Task.Delay(1000);
            var settings = Options.Get.UpdateSettings;
            if (settings.LastChecked == null || settings.LastChecked.Value < DateTime.UtcNow.AddHours(2))
            {
                settings.LastChecked = DateTime.UtcNow;
                string latestVersion = DoRequest($"https://taylorlove.info/pixelstacker/update-check.php?v={Constants.Version}")
                    ?? DoRequest("https://api.spigotmc.org/legacy/update.php?resource=46812/");

                if (latestVersion == null) return;
                if (latestVersion == Constants.Version) return /*false*/;
                if (latestVersion == settings.SkipNotifyIfVersionIs) return /*false*/;

                var result = MessageBox.Show("A new update for PixelStacker is available. Would you like to download it? Say YES to go to the download page. Say NO to ignore this update.", "A new update is available.", MessageBoxButtons.YesNo,MessageBoxIcon.Information);
                if (result == DialogResult.No)
                {
                    settings.SkipNotifyIfVersionIs = latestVersion;
                }
                else if (result == DialogResult.Yes)
                {
                    settings.SkipNotifyIfVersionIs = latestVersion;
                    ProcessStartInfo sInfo = new ProcessStartInfo("https://www.spigotmc.org/resources/pixelstacker.46812/updates");
                    Process.Start(sInfo);
                }

                Options.Save();
            }

            return/* false*/;
        }


        private static string DoRequest(string URL)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(15);
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(URL).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    //T t = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to check for updates.");
            }

            return null;
        }

    }
}
