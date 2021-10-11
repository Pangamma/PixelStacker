using PixelStacker.Extensions;
using PixelStacker.Resources;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.Utilities
{
    /// <summary>
    /// THREAD TASK QUEUES:
    /// 1. Compile Color Palette
    /// 2. Preprocessing Image
    /// 3. Rendering blueprints
    /// 4. Rendering blueprints to PNG
    /// 5. Saving blueprints as schem/PNG
    /// </summary>
    public class TaskManager
    {
        #region SafeReport
        [Obsolete("Use the Progress.Report call instead.")]
        public static void SafeReport(int percent, string status) => ProgressX.Report(percent, status);

        [Obsolete("Use the Progress.Report call instead.")]
        public static void SafeReport(int percent) => ProgressX.Report(percent);

        #endregion


        public TaskManager()
        {
            this.CancelTokenSource = new CancellationTokenSource();
            this.CancelToken = CancellationToken.None;
        }

        private static Lazy<TaskManager> _Get = new Lazy<TaskManager>();
        private CancellationToken CancelToken;

        public static TaskManager Get => _Get.Value;

        private CancellationTokenSource CancelTokenSource { get; set; } = null;
        //public CancellationToken CancelToken { get; set; } = CancellationToken.None;
        private Task CurrentTask { get; set; } = null;



        #region CANCEL
        public async Task<bool> CancelTasks()
        {
            //if (callback == null) ProgressX.Report(0, Text.Operation_Cancelled);
            if (this.CancelTokenSource == null) { return true; }
            if (this.CancelToken == CancellationToken.None) { return true; }
            if (!this.CancelToken.CanBeCanceled) { return true; }
            if (CurrentTask != null && CurrentTask.IsCompleted) { return true; }

            // If not yet requested, just cancel and do new task
            // TODO: If already requested... need to somehow cancel other task before assigning this task.
            if (!this.CancelToken.IsCancellationRequested)
            {
                await this.TryTaskCatchCancelAsync(() =>
                {
                    // Unable to continue after cancelling the task. We must wait it out.
                    this.CancelTokenSource.Cancel(true);
                    if (CurrentTask != null)
                    {
                        for (int i = 15; i > 0; i--)
                        {
                            if (CurrentTask.Wait(50) != false)
                            {
                                break;
                            }
                        }
                    }
                });

                return true;
            }

            return false;
        }

        private async Task<T> TryTaskCatchCancelAsync<T>(Task<T> task)
        {
            try { return await task; }
            catch (TaskCanceledException) { }
            catch (OperationCanceledException) { }
            catch (AggregateException aex)
            {
                if (aex.Flatten().InnerExceptions.Any(x =>
                    x.GetType() != typeof(TaskCanceledException)
                    && x.GetType() != typeof(OperationCanceledException))
                ) throw;
            }

            return default(T);
        }

        private async Task<bool> TryTaskCatchCancelAsync(Task task)
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

        private Task<bool> TryTaskCatchCancelAsync(Action task)
        {
            try { task.Invoke(); return Task.FromResult(true); }
            catch (TaskCanceledException) { }
            catch (OperationCanceledException) { }
            catch (AggregateException aex)
            {
                if (aex.Flatten().InnerExceptions.Any(x =>
                    x.GetType() != typeof(TaskCanceledException)
                    && x.GetType() != typeof(OperationCanceledException))
                ) throw;
            }

            return Task.FromResult(false);
        }
        #endregion CANCEL

        #region START
        public async Task StartAsync(Action<CancellationToken> task)
        {
            await this.CancelTasks();
            this.CancelTokenSource?.DisposeSafely();
            this.CancelTokenSource = new CancellationTokenSource();
            this.CancelToken = this.CancelTokenSource.Token;
            this.CurrentTask = TryTaskCatchCancelAsync(Task.Run(() => task(this.CancelToken)));
            await this.CurrentTask;
        }

        public async Task<T> StartAsync<T>(Func<CancellationToken, Task<T>> task)
        {
            await this.CancelTasks();
            this.CancelTokenSource?.DisposeSafely();
            this.CancelTokenSource = new CancellationTokenSource();
            this.CancelToken = this.CancelTokenSource.Token;
            var tmp = TryTaskCatchCancelAsync(task(this.CancelToken));
            this.CurrentTask = tmp;
            return await tmp;
        }

        #endregion START
    }
}
