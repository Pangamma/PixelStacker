using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PixelStacker.Logic.Utilities
{
    public class RateLimitException : Exception
    {
        public RateLimitException(string message) : base(message)
        {
        }
    }


    public class RateLimit
    {
        public int MaxRequestsPerWindow { get; private set; } = 1;
        public TimeSpan WindowDurationTime { get; private set; } = TimeSpan.FromMilliseconds(50);

        public double NumHits { get; set; } = 0;
        public DateTime LastHit { get; set; } = DateTime.MinValue;

        private static ConcurrentDictionary<string, RateLimit> Limits = new ConcurrentDictionary<string, RateLimit>();

        public RateLimit(int maxPerWindow, double windowDurationMilliseconds)
        {
            this.MaxRequestsPerWindow = maxPerWindow;
            this.WindowDurationTime = TimeSpan.FromMilliseconds(windowDurationMilliseconds);
            if (maxPerWindow < 1) throw new ArgumentOutOfRangeException(nameof(maxPerWindow), $"'{nameof(maxPerWindow)}' must be greater than 0.");
            if (windowDurationMilliseconds < 1) throw new ArgumentOutOfRangeException(nameof(windowDurationMilliseconds), $"'{nameof(windowDurationMilliseconds)}' must be greater than 0.");
        }

        /// <summary>
        /// Increments and returns current rate.
        /// </summary>
        /// <param name="maxPerWindow"></param>
        /// <param name="windowDurationMilliseconds"></param>
        /// <param name="filePath"></param>
        /// <param name="methodName"></param>
        /// <param name="lineNumber"></param>
        /// <returns></returns>
        public static double GetHitsPerSecond(int maxPerWindow, double windowDurationMilliseconds, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = null, [CallerLineNumber] int lineNumber = 0)
        {
            filePath = filePath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
            string key = $"{filePath}::{methodName}::{lineNumber}";

            if (!Limits.TryGetValue(key, out RateLimit limit))
            {
                limit = new RateLimit(maxPerWindow, windowDurationMilliseconds);
                Limits[key] = limit;
            }

            limit.IsWithinLimit(true);
            return limit.NumHits/ limit.WindowDurationTime.TotalSeconds;
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
                limit = new RateLimit(maxPerWindow, windowDurationMilliseconds);
                Limits[key] = limit;
            }

            if (!limit.IsWithinLimit(true))
            {
                Debug.WriteLine($"[{DateTime.UtcNow.ToLongTimeString()}] {key} WAS CALLED {Math.Floor(limit.NumHits)} TIMES PER {limit.WindowDurationTime.TotalSeconds}s. MAX = {limit.MaxRequestsPerWindow}.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns null if within the window allowable limits.
        /// </summary>
        /// <param name="maxPerWindow"></param>
        /// <param name="windowDurationMilliseconds"></param>
        /// <param name="filePath"></param>
        /// <param name="methodName"></param>
        /// <param name="lineNumber"></param>
        /// <returns></returns>
        public static RateLimitException CheckWithException(int maxPerWindow, double windowDurationMilliseconds, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = null, [CallerLineNumber] int lineNumber = 0)
        {
            filePath = filePath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
            string key = $"{filePath}::{methodName}::{lineNumber}";

            RateLimit limit;
            if (!Limits.TryGetValue(key, out limit))
            {
                limit = new RateLimit(maxPerWindow, windowDurationMilliseconds);
                Limits[key] = limit;
            }

            if (!limit.IsWithinLimit(true))
            {
                Debug.WriteLine($"[{DateTime.UtcNow.ToLongTimeString()}] {key} WAS CALLED ({limit.NumHits}) MORE THAN {limit.MaxRequestsPerWindow} TIMES PER {limit.WindowDurationTime.TotalSeconds}s.");
                return new RateLimitException($"[{DateTime.UtcNow.ToLongTimeString()}] {key} WAS CALLED ({limit.NumHits}) MORE THAN {limit.MaxRequestsPerWindow} TIMES PER {limit.WindowDurationTime.TotalSeconds}s.");
            }

            return null;
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

            // The total recorded number of hits at this moment in time, for this current time window.
            double hitsToSubtract = this.NumHits
                // The fraction of time to remove from current results.
                * (tsSinceLastHit.TotalSeconds / this.WindowDurationTime.TotalSeconds);
            double hitsToSubtract2 = tsSinceLastHit.TotalSeconds * this.MaxRequestsPerWindow / this.WindowDurationTime.TotalSeconds;
            if ((this.NumHits - hitsToSubtract) > MaxRequestsPerWindow)
            {
                if (incrementEvenIfCheckWouldDenyUsage)
                {
                    this.NumHits = 1 + Math.Max(this.NumHits - hitsToSubtract, 0);
                    this.LastHit = DateTime.UtcNow; // Increment here just because it affects the hitsToSubtract thing on next call.
                }

                // Don't increment on this path bc they weren't actually allowed to go to their endpoint.
                return false;
            }
            else
            {
                this.NumHits = 1 + Math.Max(this.NumHits - hitsToSubtract, 0);
                this.LastHit = DateTime.UtcNow; // Increment here just because it affects the hitsToSubtract thing on next call.
            }

            return true;
        }
    }
}
