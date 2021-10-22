using PixelStacker.Core.Extensions;

namespace PixelStacker.Core.Utilities
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
            CancelTokenSource = new CancellationTokenSource();
            CancelToken = CancellationToken.None;
        }

        private static Lazy<TaskManager> _Get = new Lazy<TaskManager>();

        public static TaskManager Get => _Get.Value;
        private CancellationToken CancelToken;
        private CancellationTokenSource CancelTokenSource { get; set; } = null;
        private Task CurrentTask { get; set; } = null;



        #region CANCEL
        public Task<bool> CancelTasks()
        {
            //if (callback == null) ProgressX.Report(0, Text.Operation_Cancelled);
            if (CancelTokenSource == null) { return Task.FromResult(true); }
            if (CancelToken == CancellationToken.None) { return Task.FromResult(true); }
            if (CurrentTask == null) return Task.FromResult(true);
            if (CurrentTask != null && CurrentTask.IsCompleted) return Task.FromResult(true);
            bool askCancel = CancelToken.CanBeCanceled && !CancelToken.IsCancellationRequested;

            try
            {
                if (askCancel)
                    CancelTokenSource.Cancel(true);
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

        //private async Task<T> TryTaskCatchCancelAsync<T>(Task<T> task)
        //{
        //    try { return await task; }
        //    catch (TaskCanceledException) { }
        //    catch (OperationCanceledException) { }
        //    catch (AggregateException aex)
        //    {
        //        if (aex.Flatten().InnerExceptions.Any(x =>
        //            x.GetType() != typeof(TaskCanceledException)
        //            && x.GetType() != typeof(OperationCanceledException))
        //        ) throw;
        //    }

        //    return default(T);
        //}


        #endregion CANCEL
        #region START
        public async Task StartAsync(Func<CancellationToken, Task> task)
        {
            try
            {
                await CancelTasks();
                CancelTokenSource?.DisposeSafely();
                CancelTokenSource = new CancellationTokenSource();
                CancelToken = CancelTokenSource.Token;
                CurrentTask = Task.Run(() => task.Invoke(CancelToken));
                await CurrentTask;
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

        public async Task StartAsync(Action<CancellationToken> task)
        {
            try
            {
                await CancelTasks();
                CancelTokenSource?.DisposeSafely();
                CancelTokenSource = new CancellationTokenSource();
                CancelToken = CancelTokenSource.Token;
                CurrentTask = Task.Run(() => task.Invoke(CancelToken));
                await CurrentTask;
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

        #endregion START
    }
}
