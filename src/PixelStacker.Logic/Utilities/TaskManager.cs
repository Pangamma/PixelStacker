﻿using PixelStacker.Extensions;
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
        public TaskManager()
        {
            this.CancelTokenSource = new CancellationTokenSource();
            this.CancelToken = CancellationToken.None;
        }

        private static readonly Lazy<TaskManager> _Get = new Lazy<TaskManager>();

        public static TaskManager Get => _Get.Value;
        private CancellationToken CancelToken;
        private CancellationTokenSource CancelTokenSource { get; set; } = null;
        private Task CurrentTask { get; set; } = null;



        #region CANCEL
        public Task<bool> CancelTasks()
        {
            //if (callback == null) ProgressX.Report(0, Text.Operation_Cancelled);
            if (this.CancelTokenSource == null) { return Task.FromResult(true); }
            if (this.CancelToken == CancellationToken.None) { return Task.FromResult(true); }
            if (this.CurrentTask == null) return Task.FromResult(true);
            if (CurrentTask != null && CurrentTask.IsCompleted) return Task.FromResult(true);
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


        //private async Task<T> TryTaskCatchCancelExceptionAsync<T>(Task<T> task)
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
        /// <summary>
        /// Use this method to start tasks that can be cancelled from the helper methods. 
        /// Do non UI thread work with this. Can only be called ONCE at a time. All other
        /// running tasks will be cancelled if you try to use it, so no nesting is allowed.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task StartAsync(Func<CancellationToken, Task> task)
        {
            try
            {
                await this.CancelTasks();
                this.CancelTokenSource?.DisposeSafely();
                this.CancelTokenSource = new CancellationTokenSource();
                this.CancelToken = this.CancelTokenSource.Token;
                this.CurrentTask = Task.Run(() => task.Invoke(this.CancelToken));
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

        public async Task StartAsync(Action<CancellationToken> task)
        {
            try
            {
                await this.CancelTasks();
                this.CancelTokenSource?.DisposeSafely();
                this.CancelTokenSource = new CancellationTokenSource();
                this.CancelToken = this.CancelTokenSource.Token;
                this.CurrentTask = Task.Run(() => task.Invoke(this.CancelToken));
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

        #endregion START
    }
}
