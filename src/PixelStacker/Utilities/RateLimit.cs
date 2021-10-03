using PixelStacker.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Utilities
{
    public class RateLimit
    {
        public int MaxRequestsPerWindow { get; private set; } = 1;
        public TimeSpan WindowDurationTime { get; private set; } = TimeSpan.FromMilliseconds(50);

        public double NumHits { get; set; } = 0;
        public DateTime LastHit { get; set; } = DateTime.MinValue;

        private static ConcurrentDictionary<string, RateLimit> Limits = new ConcurrentDictionary<string, RateLimit>();

        public RateLimit(int maxPerWindow, double windowDurationMilliseconds, string limiterID)
        {
            this.MaxRequestsPerWindow = maxPerWindow;
            this.WindowDurationTime = TimeSpan.FromMilliseconds(windowDurationMilliseconds);
            if (maxPerWindow < 1) throw new ArgumentOutOfRangeException(nameof(maxPerWindow), $"'{nameof(maxPerWindow)}' must be greater than 0.");
            if (windowDurationMilliseconds < 1) throw new ArgumentOutOfRangeException(nameof(windowDurationMilliseconds), $"'{nameof(windowDurationMilliseconds)}' must be greater than 0.");
        }

        /// <summary>
        /// Returns TRUE if within the window allowable limits.
        /// </summary>
        /// <param name="maxPerWindow"></param>
        /// <param name="windowDurationMilliseconds"></param>
        /// <param name="filePath"></param>
        /// <param name="methodName"></param>
        /// <param name="lineNumber"></param>
        /// <returns></returns>
        public static bool Check(int maxPerWindow, double windowDurationMilliseconds, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = null, [CallerLineNumber] int lineNumber = 0)
        {
            filePath = filePath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
            string key = $"{filePath}::{methodName}::{lineNumber}";

            if (!Limits.TryGetValue(key, out RateLimit limit))
            {
                limit = new RateLimit(maxPerWindow, windowDurationMilliseconds, key);
                Limits[key] = limit;
            }

            if (!limit.IsWithinLimit(true))
            {
                Debug.WriteLine($"[{DateTime.UtcNow.ToLongTimeString()}] {key} WAS CALLED ({limit.NumHits}) MORE THAN {limit.MaxRequestsPerWindow} TIMES PER {limit.WindowDurationTime.TotalSeconds}s.");
                return false;
            }

            return true;
        }

        public bool IsWithinLimit(bool incrementEvenIfCheckWouldDenyUsage)
        {
            // Set up the default if this is the initialization.
            if (this.LastHit == DateTime.MinValue)
            {
                this.LastHit = DateTime.UtcNow;

                // We do +2 here because then for a limit of "1" per X seconds, they will have 
                // to wait another X seconds until they can hit the window again.
                this.NumHits = 2;
                return true;
            }

            // Subtract any hits that have fallen outside the tracking window.
            TimeSpan tsSinceLastHit = (DateTime.UtcNow - this.LastHit);
            double hitsToSubtract = tsSinceLastHit.TotalSeconds * this.MaxRequestsPerWindow / this.WindowDurationTime.TotalSeconds;
            if ((this.NumHits - hitsToSubtract) > MaxRequestsPerWindow)
            {
                if (incrementEvenIfCheckWouldDenyUsage)
                {
                    this.NumHits = Math.Max(this.NumHits - hitsToSubtract, 0);
                    this.LastHit = DateTime.UtcNow; // Increment here just because it affects the hitsToSubtract thing on next call.
                    this.NumHits++;
                }

                // Don't increment on this path bc they weren't actually allowed to go to their endpoint.
                return false;
            }
            else
            {
                this.NumHits = Math.Max(this.NumHits - hitsToSubtract, 0);
                this.LastHit = DateTime.UtcNow; // Increment here just because it affects the hitsToSubtract thing on next call.
                this.NumHits++;
            }

            return true;
        }
    }
}
