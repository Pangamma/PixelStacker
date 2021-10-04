using PixelStacker.IO.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace PixelStacker.Extensions
{
    public static class ExtendTasks
    {
        /// <summary>
        /// </summary>
        /// <exception cref="System.OperationCanceledException">If cancelled.</exception>
        /// <param name="token"></param>
        public static void SafeThrowIfCancellationRequested(this System.Threading.CancellationToken? token)
        {
            if (token.HasValue) SafeThrowIfCancellationRequested(token.Value);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="System.OperationCanceledException">If cancelled.</exception>
        /// <param name="token"></param>
        public static void SafeThrowIfCancellationRequested(this System.Threading.CancellationToken token)
        {
            if (token == CancellationToken.None) return;
            if (!token.CanBeCanceled) return;
            if (token.IsCancellationRequested) token.ThrowIfCancellationRequested();
        }

        public static bool SafeIsCancellationRequested(this System.Threading.CancellationToken? token)
        {
            if (token.HasValue) SafeIsCancellationRequested(token.Value);
            return false;
        }

        public static bool SafeIsCancellationRequested(this System.Threading.CancellationToken token)
        {
            if (token == CancellationToken.None) return false;
            if (!token.CanBeCanceled) return false;
            return token.IsCancellationRequested;
        }
    }
}
