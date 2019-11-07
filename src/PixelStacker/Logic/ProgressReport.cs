using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic
{
    public class ProgressReport
    {
        public string Message { get; set; }
        public int? CurValue { get; set; }
        public int? MaxValue { get; set; }
    }
    public static class ProgressReportExtension
    {
        public static void Report(this IProgress<ProgressReport> p, string message, int? percent = null, int? maxValue = null)
        {
            p.Report(new ProgressReport() { Message = message, CurValue = percent, MaxValue = maxValue });
        }
    }
}
