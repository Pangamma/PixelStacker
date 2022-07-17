using System;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.Utilities
{
    public class DelayThrottle
    {
        private TimeSpan Delay;
        private CancellationTokenSource delayCts;
        private TaskCompletionSource<bool> tcsBatch;
        private object padlock = new object();

        public DelayThrottle(TimeSpan minDelay)
        {
            this.Delay = minDelay;
            this.delayCts = new CancellationTokenSource();
            this.tcsBatch = new TaskCompletionSource<bool>();
        }


        private void Interupt()
        {
            lock (padlock)
            {
                tcsBatch.TrySetResult(false);
                if (!this.delayCts.IsCancellationRequested)
                    this.delayCts.Cancel();
            }
        }

        /// <summary>
        /// Returns TRUE if was able to wait until the end without being interupted.
        /// Returns FALSE if it was interupted.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CanWaitEntireDelayWithoutInteruptions()
        {
            var cts = new CancellationTokenSource();
            var tcs = new TaskCompletionSource<bool>();

            this.Interupt();
            lock (padlock)
            {
                this.delayCts = cts;
                this.tcsBatch = tcs;
            }

            var asyncContinuationsTask = tcs.Task.ContinueWith(bt => bt.Result, TaskScheduler.Default);
            // We let it continue bc we need to do a return to the caller before this delay completes itself.
            await Task.Delay(this.Delay, cts.Token).ContinueWith((Task t) =>
            {
                if (t.Status != TaskStatus.Canceled)
                {
                    lock (padlock)
                    {
                        tcs.TrySetResult(true);
                    }
                }
            }, TaskContinuationOptions.ExecuteSynchronously);

            return await asyncContinuationsTask;
        }
    }
}
