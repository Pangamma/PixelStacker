using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.Utilities
{
    public class BatchHelper<TInput, TOutput>
    {
        public BatchHelper(Func<IEnumerable<TInput>, Task<List<TOutput>>> manyOpsFunc)
        {
            this.DoManyOperationsFunc = manyOpsFunc;
        }

        public BatchHelper(TimeSpan timeout, int maxPerBatch, Func<IEnumerable<TInput>, Task<List<TOutput>>> manyOpsFunc)
        {
            this.DoManyOperationsFunc = manyOpsFunc;
            this.timeout = timeout;
            this.maxPerBatch = maxPerBatch;
        }

        #region Batching
        // Arbitrary constants
        // You should fetch value from configuration and define sensible defaults
        private readonly int maxPerBatch = 50;
        // I chose a low value so the example wouldn't timeout in .NET Fiddle
        private readonly TimeSpan timeout = TimeSpan.FromMilliseconds(2);

        // Using a separate private object for locking
        private readonly object lockObj = new object();
        // The list of accumulated records to execute in a batch
        private List<TInput> keysToBeFetched = new List<TInput>();
        // The most recent TCS to signal completion when:
        // - the list count reached the threshold
        // - enough time has passed
        private TaskCompletionSource<IEnumerable<TInput>> tcsBatch;

        // A CTS to cancel the timer-based task when the threshold is reached
        // Not strictly necessary, but it reduces resource usage
        private CancellationTokenSource delayCts;
        // The task that will be completed when a batch of records has been dispatched
        private Task<List<TOutput>> dispatchTask;

        private Func<IEnumerable<TInput>, Task<List<TOutput>>> DoManyOperationsFunc { get; }

        // This method doesn't use async/await,
        // because we're not doing an async flow here.
        public Task<TOutput> Invoke(TInput key)
        {
            // TODO: Make it so we do a quick swap on the list reference so we do not block other threads for too long.
            lock (lockObj)
            {
                // When the list of records is empty, set up the next task
                //
                // TaskCompletionSource is just what we need, we'll complete a task
                // not when we've finished some computation, but when we reach some criteria
                //
                // This is the main reason this method doesn't use async/await
                if (keysToBeFetched.Count == 0)
                {
                    // I want the dispatch task to run on the thread pool

                    // In .NET 4.6, there's TaskCreationOptions.RunContinuationsAsynchronously
                    // .NET 4.6
                    //batchTcs = new TaskCompletionSource<IEnumerable<Record>>(TaskCreationOptions.RunContinuationsAsynchronously);
                    //dispatchTask = DispatchRecordsAsync(batchTcs.Task);

                    // Previously, we have to set up a continuation task using the default task scheduler
                    // .NET 4.5.2
                    tcsBatch = new TaskCompletionSource<IEnumerable<TInput>>();
                    var asyncContinuationsTask = tcsBatch.Task
                        .ContinueWith(bt => bt.Result, TaskScheduler.Default);
                    dispatchTask = DispatchOperationsQueue(asyncContinuationsTask);

                    // Create a cancellation token source to be able to cancel the timer
                    //
                    // To be used when we reach the threshold, to release timer resources
                    delayCts = new CancellationTokenSource();
                    Task.Delay(timeout, delayCts.Token)
                        .ContinueWith(
                            dt =>
                            {
                                // When we hit the timer, take the lock and set the batch
                                // task as complete, moving the current records to its result
                                lock (lockObj)
                                {
                                    // Avoid dispatching an empty list of records
                                    //
                                    // Also avoid a race condition by checking the cancellation token
                                    //
                                    // The race would be for the actual timer function to start before
                                    // we had a chance to cancel it
                                    if ((keysToBeFetched.Count > 0) && !delayCts.IsCancellationRequested)
                                    {
                                        tcsBatch.TrySetResult(new List<TInput>(keysToBeFetched));
                                        keysToBeFetched.Clear();
                                    }
                                }
                            },
                            // Since our continuation function is fast, we want it to run
                            // ASAP on the same thread where the actual timer function runs
                            //
                            // Note: this is just a hint, but I trust it'll be favored most of the time
                            TaskContinuationOptions.ExecuteSynchronously);
                    // Remember that we want our batch task to have continuations
                    // running outside the timer thread, since dispatching records
                    // is probably too much work for a timer thread.
                }
                // Actually store the new record somewhere
                keysToBeFetched.Add(key);
                int resultIndex = keysToBeFetched.Count - 1;
                // When we reach the threshold, set the batch task as complete,
                // moving the current records to its result
                //
                // Also, cancel the timer task
                if (keysToBeFetched.Count >= maxPerBatch)
                {
                    tcsBatch.TrySetResult(new List<TInput>(keysToBeFetched));
                    delayCts.Cancel();
                    keysToBeFetched.Clear();
                }
                // Return the last saved dispatch continuation task
                //
                // It'll start after either the timer or the threshold,
                // but more importantly, it'll complete after it dispatches all records
                return dispatchTask.ContinueWith(listTask =>
                {
                    if (resultIndex < 0) return default(TOutput);
                    if (resultIndex >= listTask.Result.Count) return default(TOutput);
                    return listTask.Result[resultIndex];
                });
            }
        }

        #endregion

        protected async Task<List<TOutput>> DispatchOperationsQueue(Task<IEnumerable<TInput>> batchTask)
        {
            var batchedRecords = await batchTask.ConfigureAwait(false);
            return await this.DoManyOperationsFunc(batchedRecords);
        }

        public async Task WaitUntilBatchIsComplete()
        {
            if (dispatchTask == null) return;
            var asyncDispatchCompleteTask = dispatchTask.ContinueWith(dt => dt.Result, TaskScheduler.Default);
            await asyncDispatchCompleteTask;
        }
    }
}
