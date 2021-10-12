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
        public Task<bool> CancelTasks()
        {
            //if (callback == null) ProgressX.Report(0, Text.Operation_Cancelled);
            if (this.CancelTokenSource == null) { return Task.FromResult(true); }
            if (this.CancelToken == CancellationToken.None) { return Task.FromResult(true); }
            if (CurrentTask != null && CurrentTask.IsCompleted) { return Task.FromResult(true); }
            bool askCancel = this.CancelToken.CanBeCanceled && !this.CancelToken.IsCancellationRequested;

            try
            {
                if (askCancel)
                    this.CancelTokenSource.Cancel(true);
                // Now we wait it out until done or cancelled.
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

            return Task.FromResult(true);
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

        private void TryTaskCatchCancelAsync(Action task)
        {
            try { task.Invoke(); return; }
            catch (TaskCanceledException) { }
            catch (OperationCanceledException) { }
            catch (AggregateException aex)
            {
                if (aex.Flatten().InnerExceptions.Any(x =>
                    x.GetType() != typeof(TaskCanceledException)
                    && x.GetType() != typeof(OperationCanceledException))
                ) throw;
            }
        }

        private void TryTaskCatchCancelAsync(CancellationToken token, Action<CancellationToken> task)
        {
            try { task.Invoke(token); return; }
            catch (TaskCanceledException) { }
            catch (OperationCanceledException) { }
            catch (AggregateException aex)
            {
                if (aex.Flatten().InnerExceptions.Any(x =>
                    x.GetType() != typeof(TaskCanceledException)
                    && x.GetType() != typeof(OperationCanceledException))
                ) throw;
            }
        }
        #endregion CANCEL

        #region START
        public async Task StartAsync(Action<CancellationToken> task)
        {
            try
            {
                await this.CancelTasks();
                this.CancelTokenSource?.DisposeSafely();
                this.CancelTokenSource = new CancellationTokenSource();
                this.CancelToken = this.CancelTokenSource.Token;
                this.CurrentTask = Task.Run(() => task(this.CancelToken));
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
        }

        public async Task<T> StartAsync<T>(Func<CancellationToken, Task<T>> task)
        {
            await this.CancelTasks();
            this.CancelTokenSource?.DisposeSafely();
            this.CancelTokenSource = new CancellationTokenSource();
            this.CancelToken = this.CancelTokenSource.Token;

            // This will explode.
            this.CurrentTask = TryTaskCatchCancelAsync(task(this.CancelToken));
            return (T)await (Task<T>)this.CurrentTask;
        }

        #endregion START
    }
}
