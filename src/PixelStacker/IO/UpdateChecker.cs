using Newtonsoft.Json;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.IO
{
    public class UpdateInfo
    {
        public string Version { get; set; }
        public string ChangeLog { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
    }

    public class UpdateChecker
    {
        private static bool IsNewerVersionAvailable(string vA_Newer, string vB_Older)
        {
            if (string.IsNullOrWhiteSpace(vA_Newer)) return false; // Bad NEW version? Better wait for the next one.
            if (string.IsNullOrWhiteSpace(vB_Older)) return true; // Bad version? Yikes. Recommend an upgrade.

            var a_arr = vA_Newer.Split(".", StringSplitOptions.RemoveEmptyEntries);
            var b_arr = vB_Older.Split(".", StringSplitOptions.RemoveEmptyEntries);

            // [1].19.2c
            if (a_arr.Length > 0 && b_arr.Length > 0)
            {
                int mA = a_arr[0].ToNullable<int>() ?? 0;
                int mB = b_arr[0].ToNullable<int>() ?? 0;
                if (mA < mB) return false;
                if (mA > mB) return true;
            }

            // 1.[19].2c
            if (a_arr.Length > 1 && b_arr.Length > 1)
            {
                int mA = a_arr[1].ToNullable<int>() ?? 0;
                int mB = b_arr[1].ToNullable<int>() ?? 0;
                if (mA < mB) return false;
                if (mA > mB) return true;
            }

            // 1.19.[2c]
            if (a_arr.Length > 2 && b_arr.Length > 2)
            {
                int mC = a_arr[2].CompareTo(b_arr[2]);
                if (mC < 0) return true;
            }

            return false;
        }

        public async static Task CheckForUpdates(Options o, CancellationToken cancelToken)
        {
            try
            {
                var settings = o.UpdateSettings;
                //if (settings.LastChecked == null || settings.LastChecked.Value < DateTime.UtcNow.AddHours(-2))
                {
                    ProgressX.Report(75, "Checking for updates");
                    settings.LastChecked = DateTime.UtcNow;

                    bool isAdv = o?.IsAdvancedModeEnabled ?? false;

                    UpdateInfo latestVersion =
                        isAdv ? (await DoGithubRequest())
                        : (await DoSpigotRequest() ?? await DoGithubRequest());

                    o.Save();

                    if (latestVersion?.Version == null)
                    {
                        ProgressX.Report(100, "No updates available.");
                        return;
                    }

                    if (latestVersion.Version == Constants.Version)
                    {
                        ProgressX.Report(100, "You are already using the latest version of PixelStacker");
                        return;
                    }

                    if (!IsNewerVersionAvailable(latestVersion.Version, Constants.Version))
                    {
                        ProgressX.Report(100, "Already using the latest version of PixelStacker");
                        return;
                    }

                    if (latestVersion.Version == settings.SkipNotifyIfVersionIs)
                    {
                        ProgressX.Report(100, "Newest version available is still: " + latestVersion.Version);
                        return;
                    }


                    ProgressX.Report(100, "A new version is available!");
                    var result = MessageBox.Show("A new update for PixelStacker is available. Would you like to download it? Say YES to go to the download page. Say NO to ignore this update.\n\n"
                        + "[" + latestVersion.Title + "]:\n"
                        + latestVersion.ChangeLog, "A new update is available.", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.No)
                    {
                        settings.SkipNotifyIfVersionIs = latestVersion.Version;
                    }
                    else if (result == DialogResult.Yes)
                    {
                        settings.SkipNotifyIfVersionIs = latestVersion.Version;
                        Process.Start("explorer", latestVersion.URL);
                    }

                    o.Save();
                }
                return/* false*/;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }


        private static async Task<string> DoRequest(string URL)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(15);
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", "PixelStacker");
                HttpResponseMessage response = client.GetAsync(URL).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                Console.WriteLine("Failed to check for updates.");
            }

            return null;
        }

        private static async Task<UpdateInfo> DoSpigotRequest()
        {
            string versionNumber = "";
            int pageNum = 0;
            {
                string json2 = await DoRequest("https://api.spigotmc.org/simple/0.2/index.php?action=getResource&id=46812");
                var definition = new { Stats = new { Updates = 0}, Current_Version = "" };
                var parsed = JsonConvert.DeserializeAnonymousType(json2, definition);
                pageNum = 1 + (parsed?.Stats?.Updates ?? 0) / 10;
                versionNumber = parsed.Current_Version;
            }

            string title;
            string msg;
            {
                var definition = new[] { new { title = "An update is here.", message="more details" } };
                string json2 = await DoRequest($"https://api.spigotmc.org/simple/0.2/index.php?action=getResourceUpdates&id=46812&page={pageNum}");
                var parsed = JsonConvert.DeserializeAnonymousType(json2, definition);
                msg = parsed?.LastOrDefault()?.message;
                title = parsed?.LastOrDefault()?.title;
            }

            // 
            // https://api.spigotmc.org/simple/0.2/index.php?action=getResource&id=46812
            // https://api.spigotmc.org/simple/0.2/index.php?action=getResourceUpdates&id=46812&page=1
            //string json = await DoRequest("https://api.spigotmc.org/legacy/update.php?resource=46812/");
            //var parsed = JsonConvert.DeserializeObject<GithubReleaseResponse>(json);
            //string version = parsed.tag_name;
            return new UpdateInfo()
            {
                Version = versionNumber,
                ChangeLog = msg,
                Title = title,
                URL = "https://www.spigotmc.org/resources/pixelstacker.46812/updates"
            };
        }

        private static async Task<UpdateInfo> DoGithubRequest()
        {
            string json = await DoRequest("https://api.github.com/repos/pangamma/PixelStacker/releases?per_page=1");
            var parsed = JsonConvert.DeserializeObject<List<GithubReleaseResponse>>(json)[0];
            string version = parsed.tag_name;
            return new UpdateInfo()
            {
                Version = version,
                Title = parsed.name,
                ChangeLog = parsed.body,
                URL = parsed.html_url
            };
        }
    }

    public class GithubReleaseResponse
    {
#pragma warning disable IDE1006 // Naming Styles
        public string tag_name { get; set; }
        public string body { get; set; }
        public string name { get; set; } // title
        public string html_url { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }
}
