using PixelStacker.Logic.Utilities;
using PixelStacker.Resources;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Utilities
{
    [Obsolete("Use the one in logic instead.")]
    public class TaskManager
    {
        private static TaskManager _Get = null;
        public static TaskManager Get { get { if (_Get == null) _Get = new TaskManager(); return _Get; } }
        private CancellationTokenSource CancelTokenSource { get; set; } = null;
        public CancellationToken CancelToken { get; set; } = CancellationToken.None;
        private Task CurrentTask { get; set; } = null;


        #region SafeReport
        [Obsolete("Use the Progress.Report call instead.")]
        public static void SafeReport(int percent, string status) => ProgressX.Report(percent, status);

        [Obsolete("Use the Progress.Report call instead.")]
        public static void SafeReport(int percent) => ProgressX.Report(percent);

        #endregion


        // Call this first
        public void CancelTasks(Action callback)
        {
            if (callback == null) ProgressX.Report(0, Text.Operation_Cancelled);
            if (this.CancelTokenSource == null) { callback?.Invoke(); return; }
            if (this.CancelToken == CancellationToken.None) { callback?.Invoke(); return; }
            if (!this.CancelToken.CanBeCanceled) { callback?.Invoke(); }

            // If not yet requested, just cancel and do new task
            // TODO: If already requested... need to somehow cancel other task before assigning this task.
            if (!this.CancelToken.IsCancellationRequested)
            {
                try
                {
                    this.CancelTokenSource.Cancel(true);
                    for (int i = 15; i > 0; i--)
                    {
                        if (CurrentTask?.Wait(100) != false)
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
                callback?.Invoke();
            }
            else if (CurrentTask != null && CurrentTask.IsCompleted)
            {
                callback?.Invoke();
            }
        }


        public async Task<bool> TryTaskCatchCancelAsync(Task task)
        {
            try { await task; return true; }
            catch (TaskCanceledException) { }
            catch (OperationCanceledException) { }
            catch (AggregateException aex)
            {
                if (aex.Flatten().InnerExceptions.Any(x =>
                    x.GetType() != typeof(TaskCanceledException)
                    && x.GetType() != typeof(OperationCanceledException))
                ) throw;
            }

            return false;
        }
        public bool TryTaskCatchCancelSync(Action task)
        {
            try { task(); return true; }
            catch (TaskCanceledException) { }
            catch (OperationCanceledException) { }
            catch (AggregateException aex)
            {
                if (aex.Flatten().InnerExceptions.Any(x =>
                    x.GetType() != typeof(TaskCanceledException)
                    && x.GetType() != typeof(OperationCanceledException))
                ) throw;
            }

            return false;
        }

        public async Task StartAsync(Action<CancellationToken> task)
        {
            await Task.Run(() => this.CancelTasks(async () =>
            {
                this.CancelTokenSource = new CancellationTokenSource();
                this.CancelToken = this.CancelTokenSource.Token;
                this.CurrentTask = Task.Run(() =>
                {
                    try { task(this.CancelToken); }
                    catch (TaskCanceledException) { }
                    catch (OperationCanceledException) { }
                    catch (AggregateException aex)
                    {
                        if (aex.Flatten().InnerExceptions.Any(x =>
                            x.GetType() != typeof(TaskCanceledException)
                            && x.GetType() != typeof(OperationCanceledException))
                        ) throw;
                    }
                }, this.CancelToken);

                try
                {
                    await this.CurrentTask;
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
            }));
        }

        /// <summary>
        /// Does not dispose the token!
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        private CancellationTokenSource CancelAndRenewCancellationToken(CancellationTokenSource original)
        {
            if (original?.Token != null)
            {
                if (original.Token.CanBeCanceled)
                {
                    original.Cancel();
                }
            }

            original = new CancellationTokenSource();

            return original;
        }
    }
}
