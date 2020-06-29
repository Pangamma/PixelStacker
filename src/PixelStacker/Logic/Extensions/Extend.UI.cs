using System;
using System.Threading;
using System.Windows.Forms;

namespace PixelStacker.Logic.Extensions
{
    public static partial class Extend
    {
        public static TResult InvokeEx<TControl, TResult>(this TControl control, Func<TControl, TResult> func) where TControl : Control
        {
            TResult rt = default(TResult);
            if (!control.IsDisposed)
            {
                rt = control.InvokeRequired
                    ? (TResult)control.Invoke(func, control)
                    : func(control);
            }
            return rt;
        }

        public static void InvokeEx<TControl>(this TControl control, Action<TControl> func) where TControl : Control
        {
            control.InvokeEx(c => { func(c); return c; });
        }

        public static void InvokeEx<TControl>(this TControl control, Action action) where TControl : Control
        {
            control.InvokeEx(c => action());
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

        public static bool SafeIsCancellationRequested(this System.Threading.CancellationToken token)
        {
            if (token == CancellationToken.None) return false;
            if (!token.CanBeCanceled) return false;
            return token.IsCancellationRequested;
        }
    }
}
