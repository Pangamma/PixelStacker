using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PixelStacker.Logic.IO.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PixelStacker.Web.Net.Controllers
{
    public class DeploymentInfoController : BaseApiController
    {
        public static Lazy<DateTimeOffset> DeploymentTime = new Lazy<DateTimeOffset>(() => DateTimeOffset.UtcNow);
        public static Dictionary<string, object> ExtraProperties = new Dictionary<string, object>();


        [HttpGet]
        public JsonResult Index()
        {
            Dictionary<string, object> output = new Dictionary<string, object>();
            TimeZoneInfo tz = this.GetTimeZone();

            output["ProjectAssembly"] = Assembly.GetEntryAssembly().GetName()?.Name ?? "Unknown";

            // Truncate sub-millisecond precision before offsetting by the Unix Epoch to avoid
            // the last digit being off by one for dates that result in negative Unix times
            output["Deployed_UnixTimeMilliseconds"] = GetUnixTimeMilliseconds();
            output["Deployed_Timestamp"] = DeploymentInfoController.DeploymentTime.Value.ToString("yyyy-MM-dd HH:mm:ss") + " (UTC)";


            output["Deployed_Timestamp_PST"] = DeploymentInfoController.DeploymentTime.Value.ToOffset(tz.GetUtcOffset(DateTime.UtcNow)).ToString("yyyy-MM-dd HH:mm:ss") + " (PST)";
            output["Current_Timestamp"] = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " (UTC)";

            TimeSpan deployTime = (DateTimeOffset.UtcNow - DeploymentInfoController.DeploymentTime.Value);
            output["Deployed_Duration"] = string.Format("{0}d {1}h {2}m {3}s", deployTime.Days, deployTime.Hours.ToString().PadLeft(2, '0'), deployTime.Minutes.ToString().PadLeft(2, '0'), deployTime.Seconds.ToString().PadLeft(2, '0'));

            output["PixelStacker_Version"] = Constants.Version;

#if DEBUG
            output["Extra"] = DeploymentInfoController.ExtraProperties;
#endif

            return new JsonResult(output, new System.Text.Json.JsonSerializerOptions()
            {
                MaxDepth = 10,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true
            });
        }

        private TimeZoneInfo _tz = null;

        public static IWebHostEnvironment Environment { get; internal set; }

        private TimeZoneInfo GetTimeZone()
        {
            if (_tz == null)
            {

                try
                {
                    _tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                }
                catch (TimeZoneNotFoundException)
                {
                    try
                    {
                        _tz = TimeZoneInfo.FindSystemTimeZoneById("America/Los_Angeles");
                    }
                    catch (TimeZoneNotFoundException)
                    {
                        _tz = TimeZoneInfo.GetSystemTimeZones().Where(tz => tz.BaseUtcOffset.Hours == -8).FirstOrDefault();
                    }
                }
            }

            return _tz;
        }


        /// <summary>
        /// We need this until we upgrade to 4.6.*
        /// </summary>
        /// <returns></returns>
        private static ulong GetUnixTimeMilliseconds()
        {
            DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - epochStart;
            ulong requestTimeStamp = Convert.ToUInt64(timeSpan.TotalMilliseconds);
            return requestTimeStamp;
        }
    }
}
