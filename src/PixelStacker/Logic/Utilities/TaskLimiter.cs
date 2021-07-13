using PixelStacker.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic
{
    public class TaskLimiter
    {
        public TimeSpan DelayBeforeStarting { get; private set; } = TimeSpan.FromMilliseconds(50);

        private static Dictionary<string, TaskLimiter> Limits = new Dictionary<string, TaskLimiter>();

        public TaskLimiter(double delayBeforeStarting)
        {
            this.DelayBeforeStarting = TimeSpan.FromMilliseconds(delayBeforeStarting);
            if (delayBeforeStarting < 1) throw new ArgumentOutOfRangeException(nameof(delayBeforeStarting), $"'{nameof(delayBeforeStarting)}' must be greater than 0.");
        }

        /// <summary>
        /// </summary>
        /// <param name="delayBeforeStarting"></param>
        /// <param name="action"></param>
        /// <param name="filePath"></param>
        /// <param name="methodName"></param>
        /// <param name="lineNumber"></param>
        /// <returns>TRUE if allowed to finish</returns>
        public static async Task<bool> TryAsync(double delayBeforeStarting, Action<CancellationToken> action, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = null, [CallerLineNumber] int lineNumber = 0)
            => await TryAsync(delayBeforeStarting, action, $"{filePath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault()}::{methodName}::{lineNumber}");

        public static async Task<bool> TryAsync(double delayBeforeStarting, Action<CancellationToken> action, string key)
        {
            TaskLimiter limit;
            if (!Limits.TryGetValue(key, out limit))
            {
                limit = new TaskLimiter(delayBeforeStarting);
                Limits[key] = limit;
                limit.TaskID = key;
            }

            return await limit.TryAsync(action);
        }

        public static async Task CancelAsync(double delayBeforeStarting, Action<CancellationToken> action, string key)
        {
            if (Limits.TryGetValue(key, out TaskLimiter limit))
            {
                await limit.CancelAsync();
            }
        }













        private CancellationTokenSource CancelTokenSource { get; set; } = null;
        public CancellationToken CancelToken { get; set; } = CancellationToken.None;
        private Task CurrentTask { get; set; } = null;
        public string TaskID { get; private set; }

        // Call this first
        private Task CancelAsync()
        {
            if (this.CancelTokenSource == null) { return Task.CompletedTask; }
            if (this.CancelToken == CancellationToken.None) {  return Task.CompletedTask; }

            // What am I supposed to do with THIS?
            if (!this.CancelToken.CanBeCanceled) 
            { 
                //callbackAfterCancelling?.Invoke();
            }

            // If not yet requested, just cancel and do new task
            // TODO: If already requested... need to somehow cancel other task before assigning this task.
            if (!this.CancelToken.IsCancellationRequested)
            {
                try
                {
                    this.CancelTokenSource.Cancel(true);
                    for (int i = 150; i > 0; i--)
                    {
                        if (CurrentTask?.Wait(10) != false)
                        {
                            break;
                        }
                    }

                    // Unable to continue after cancelling the task. We must wait it out.
                }
                catch (TaskCanceledException) { }
                catch (OperationCanceledException) { }
                catch (AggregateException aex)
                {
                    if (aex.Flatten().InnerExceptions.Any(x =>
                        x.GetType() != typeof(TaskCanceledException)
                        && x.GetType() != typeof(OperationCanceledException))
                    ) throw;
                }

                return Task.CompletedTask;
            }
            else if (CurrentTask != null && CurrentTask.IsCompleted)
            {
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }

        private async Task<bool> TryAsync(Action<CancellationToken> task)
        {
            await this.CancelAsync();
            Debug.WriteLine(this.TaskID + " delaying");
            this.CancelTokenSource = new CancellationTokenSource();
            this.CancelToken = this.CancelTokenSource.Token;
            this.CurrentTask = Task.Delay(this.DelayBeforeStarting, CancelToken);

            bool isCancelled = ! (await TaskManager.Get.TryTaskCatchCancelAsync(this.CurrentTask));
            if (isCancelled)
            {
                Debug.WriteLine(this.TaskID + " cancelled (b4 beginning)");
                return false;
            }

            Debug.WriteLine(this.TaskID + " running...");
            this.CancelTokenSource = new CancellationTokenSource();
            this.CancelToken = this.CancelTokenSource.Token;
            this.CurrentTask = Task.Run(() => task(this.CancelToken), this.CancelToken);

            isCancelled = !(await TaskManager.Get.TryTaskCatchCancelAsync(this.CurrentTask));
            if (isCancelled)
            {
                Debug.WriteLine(this.TaskID + " cancelled (while-in-progress)");
                return false;
            }

            Debug.WriteLine(this.TaskID + " COMPLETE");
            return true;

            //double ms = this.DelayBeforeStarting.TotalMilliseconds;
            //int increment = 10;
            //for (double d = 0; d < ms; d += increment)
            //{
            //    await Task.Delay(increment);
            //}
        }
    }
}
