using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.Logic
{
    public static class UIExtensions
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
