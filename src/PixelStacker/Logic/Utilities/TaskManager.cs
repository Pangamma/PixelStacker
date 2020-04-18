using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic
{
    public class TaskManager
    {
        private static TaskManager _Get = null;
        public static TaskManager Get { get { if (_Get == null) _Get = new TaskManager(); return _Get; } }

        private CancellationTokenSource CancelTokenSource { get; set; } = null;
        public CancellationToken CancelToken { get; set; } = CancellationToken.None;
        private Task CurrentTask { get; set; } = null;

        private object Padlock { get; set; } = new { };
        private string StatusMessage { get; set; } = null;
        private int StatusPercent { get; set; } = 0;
        public void UpdateStatus(MainForm c)
        {
            lock (Padlock)
            {
                if (StatusPercent == 100)
                {
                    c.progressBar.Value = 0;
                }
                else if (StatusPercent != c.progressBar.Value)
                {
                    int val = Math.Min(c.progressBar.Maximum, StatusPercent);
                    val = Math.Max(c.progressBar.Minimum, val);
                    c.progressBar.Value = val;
                    if (val > 0)
                    {
                        c.progressBar.Value = val - 1;
                    }
                }

                string displayText = $"{StatusPercent}%   {StatusMessage}";
                c.lblStatus.Text = (StatusPercent == 0 || StatusPercent == 100) ? StatusMessage : displayText;
            }
        }

        // Call this first
        public void CancelTasks(Action callback)
        {
            if (callback == null) TaskManager.SafeReport(0, "Operation Cancelled.");
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
                catch (TaskCanceledException)
                {
                }
                catch (AggregateException aex)
                {
                    if (aex.InnerExceptions.Any(x => x.GetType() != typeof(TaskCanceledException)))
                    {
                        throw;
                    }
                }
                callback?.Invoke();
            }
            else if (CurrentTask != null && CurrentTask.IsCompleted)
            {
                callback?.Invoke();
            }
        }

        private void WrapCancelTryCatch(Action<CancellationToken> task)
        {
            try { task(this.CancelToken); }
            catch (OperationCanceledException) { }
            catch (AggregateException aex)
            {
                if (aex.InnerExceptions.Any(x => x.GetType() != typeof(TaskCanceledException)))
                {
                    throw;
                }
            }
        }

        public async Task StartAsync(Action<CancellationToken> task)
        {
            await Task.Run(() => this.CancelTasks(async () => {
                this.CancelTokenSource = new CancellationTokenSource();
                this.CancelToken = this.CancelTokenSource.Token;
                this.CurrentTask = Task.Run(() => {
                    try { task(this.CancelToken); } catch (OperationCanceledException) { } catch (AggregateException aex)
                    {
                        if (aex.InnerExceptions.Any(x => x.GetType() != typeof(TaskCanceledException)))
                        {
                            throw;
                        }
                    }
                }, this.CancelToken);

                try
                {
                    await this.CurrentTask;
                }
                catch (OperationCanceledException) { }
                catch (AggregateException aex)
                {
                    if (aex.InnerExceptions.Any(x => x.GetType() != typeof(TaskCanceledException)))
                    {
                        throw;
                    }
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


        #region SafeReport
        
        public static void SafeReport(int percent, string status)
        {
            lock (TaskManager.Get.Padlock)
            {
                TaskManager.Get.StatusMessage = status;
                TaskManager.Get.StatusPercent = percent;
            }
        }

        public static void SafeReport(int percent)
        {
            lock (TaskManager.Get.Padlock)
            {
                TaskManager.Get.StatusPercent = percent;
            }
        }

        #endregion
    }
}
