namespace PixelStacker.Core.Extensions
{
    public static class ExtendTasks
    {
        /// <summary>
        /// </summary>
        /// <exception cref="OperationCanceledException">If cancelled.</exception>
        /// <param name="token"></param>
        public static void SafeThrowIfCancellationRequested(this CancellationToken? token)
        {
            if (token.HasValue) token.Value.SafeThrowIfCancellationRequested();
        }

        /// <summary>
        /// </summary>
        /// <exception cref="OperationCanceledException">If cancelled.</exception>
        /// <param name="token"></param>
        public static void SafeThrowIfCancellationRequested(this CancellationToken token)
        {
            if (token == CancellationToken.None) return;
            if (!token.CanBeCanceled) return;
            if (token.IsCancellationRequested) token.ThrowIfCancellationRequested();
        }

        public static bool SafeIsCancellationRequested(this CancellationToken? token)
        {
            if (token.HasValue) token.Value.SafeIsCancellationRequested();
            return false;
        }

        public static bool SafeIsCancellationRequested(this CancellationToken token)
        {
            if (token == CancellationToken.None) return false;
            if (!token.CanBeCanceled) return false;
            return token.IsCancellationRequested;
        }
    }
}
