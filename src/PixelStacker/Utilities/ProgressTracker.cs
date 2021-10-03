using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.Utilities
{
    public class ProgressTracker
    {
        private static object Padlock { get; set; } = new { };

        #region SafeReport
        private static string StatusMessage { get; set; } = null;
        private static int StatusPercent { get; set; } = 0;

        public static void UpdateStatus(ProgressBar bar, Label status)
        {
            lock (Padlock)
            {
                if (StatusPercent == 100)
                {
                    bar.Value = 0;
                }
                else if (StatusPercent != bar.Value)
                {
                    int val = Math.Min(bar.Maximum, StatusPercent);
                    val = Math.Max(bar.Minimum, val);
                    bar.Value = val;
                    if (val > 0)
                    {
                        bar.Value = val - 1;
                    }
                }

                string displayText = $"{StatusPercent}%   {StatusMessage}";
                status.Text = (StatusPercent == 0 || StatusPercent == 100) ? StatusMessage : displayText;
            }
        }

        public static void SafeReport(int percent, string status)
        {
            lock (Padlock)
            {
                StatusMessage = status;
                StatusPercent = percent;
            }
        }

        public static void SafeReport(int percent)
        {
            lock (Padlock)
            {
                StatusPercent = percent;
            }
        }

        #endregion
    }
}
