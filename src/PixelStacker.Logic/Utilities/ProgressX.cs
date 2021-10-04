using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.Utilities
{
    public class ProgressX
    {
        private static object Padlock { get; set; } = new { };

        #region SafeReport
        private static string StatusMessage { get; set; } = null;
        private static int StatusPercent { get; set; } = 0;

        /// <summary>
        /// This is a really dumb way of passing values to upper levels. It will only be called in one place though.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="current"></param>
        /// <param name="setText"></param>
        /// <param name="setPercent"></param>
        public static void UpdateStatus(int min, int max, int current, Action<string> setText, Action<int> setPercent)
        {
            lock (Padlock)
            {
                if (StatusPercent == 100)
                {
                    setPercent(0);
                    //bar.Value = 0;
                }
                else if (StatusPercent != current)
                {
                    int val = Math.Min(max, StatusPercent);
                    val = Math.Max(min, val);
                    setPercent(val > 0 ? val - 1 : val);
                }

                string displayText = $"{StatusPercent}%   {StatusMessage}";
                displayText = (StatusPercent == 0 || StatusPercent == 100) ? StatusMessage : displayText;
                setText(displayText);
            }
        }

        public static void Report(int percent, string status)
        {
            lock (Padlock)
            {
                StatusMessage = status;
                StatusPercent = percent;
            }
        }

        public static void Report(int percent)
        {
            lock (Padlock)
            {
                StatusPercent = percent;
            }
        }

        #endregion
    }
}
