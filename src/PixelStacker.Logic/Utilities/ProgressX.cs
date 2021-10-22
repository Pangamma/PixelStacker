using System;
using System.Runtime.CompilerServices;

namespace PixelStacker.Logic.Utilities
{
    public interface IDummyArg { }

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
        public static void UpdateStatus(Action<int, string> setValues)
        {
            lock (Padlock)
            {
                int sp = StatusPercent == 100 ? 0 : StatusPercent;
                string displayText = $"{StatusPercent}%   {StatusMessage}";
                string dm = (StatusPercent == 0 || StatusPercent == 100) ? StatusMessage : displayText;
                setValues(sp, dm);
            }
        }

        public static void Report(int percent, string status, IDummyArg arg = null, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = null, [CallerLineNumber] int lineNumber = 0)
        {
            lock (Padlock)
            {
                StatusMessage = status;
                StatusPercent = percent;
                if (percent > 100) StatusPercent = 100;
                if (percent < 0) StatusPercent = 0;
                //System.Diagnostics.Debug.WriteLine($"{percent}% STAT: {methodName}::{lineNumber}");
            }

        }

        public static void Report(int percent, IDummyArg arg = null, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = null, [CallerLineNumber] int lineNumber = 0)
        {
            lock (Padlock)
            {
                StatusPercent = percent;
                if (percent > 100) StatusPercent = 100;
                if (percent < 0) StatusPercent = 0;
                //System.Diagnostics.Debug.WriteLine($"{percent}%: {methodName}::{lineNumber}");
            }
        }

        #endregion
    }
}
