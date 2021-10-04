using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace PixelStacker.Extensions
{
    public static class ExtendUI
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


        public static IEnumerable<ToolStripItem> Flatten(this ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
            {
                if (item is ToolStripDropDownItem toolItem)
                    foreach (ToolStripItem subitem in Flatten(toolItem.DropDownItems))
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
