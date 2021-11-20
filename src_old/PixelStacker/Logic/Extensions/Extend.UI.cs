using System;
using System.Collections.Generic;
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


        public static IEnumerable<ToolStripItem> Flatten(this ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
            {
                if (item is ToolStripDropDownItem)
                    foreach (ToolStripItem subitem in
                        Flatten(((ToolStripDropDownItem) item).DropDownItems))
                        yield return subitem;
                yield return item;
            }
        }

        /// <summary>
        /// This method is the problem. Why though...
        /// </summary>
        /// <param name="self"></param>
        /// <param name="toAdd"></param>
        public static void AddControlsQuick(this Control self, Control[] toAdd)
        {
            if (toAdd.Length == 0) return;
            self.SuspendLayout();
            self.Controls.AddRange(toAdd);
            self.ResumeLayout();
        }

        public static void ClearControlsQuick(this Control self)
        {
            if (self.Controls.Count == 0) return;
            self.SuspendLayout();
            self.Controls.Clear();
            self.ResumeLayout();
        }
    }
}
