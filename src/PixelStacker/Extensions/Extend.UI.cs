using PixelStacker.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace PixelStacker.Extensions
{
    public static class ExtendUI
    {
        public static async Task<TResult> InvokeEx<TControl, TResult>(this TControl control, Func<TControl, Task<TResult>> func) where TControl : Control
        {
            TResult rt = default(TResult);
            if (!control.IsDisposed)
            {
                var tsk = control.InvokeRequired
                    ? (Task<TResult>)control.Invoke(func, control)
                    : func(control);
                rt = await tsk;
            }
            return rt;
        }

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

        public static void ModifyRecursive(this ToolStripItem menuStripItem, Action<ToolStripItem> action)
        {
            action(menuStripItem);
            if (menuStripItem is ToolStripMenuItem mi)
            {
                if (mi.HasDropDownItems)
                {
                    IEnumerable<ToolStripItem> items = mi.DropDownItems.Flatten();
                    foreach (var item in items)
                    {
                        action(item);
                    }
                }
            }
        }


        public static void ModifyRecursive(this ToolStrip menuStrip, Action<ToolStripItem, MainFormTags> action)
        {
            foreach (ToolStripItem menuStripItem in menuStrip.Items)
            {
                ModifyRecursive(menuStripItem, action);
            }
        }

        public static void ModifyRecursive(this ToolStripItem menuStripItem, Action<ToolStripItem, MainFormTags> action)
        {
            action(menuStripItem, menuStripItem.Tag as MainFormTags);
            if (menuStripItem is ToolStripMenuItem mi)
            {
                if (mi.HasDropDownItems)
                {
                    IEnumerable<ToolStripItem> items = mi.DropDownItems.Flatten();
                    foreach (var item in items)
                    {
                        action(item, item.Tag as MainFormTags);
                    }
                }
            }
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

        public static T[] ToArray<T>(this ControlCollection self) where T : Control
        {
            int i = 0;
            T[] t = new T[self.Count];
            foreach(var item in self)
            {
                t[i++] = (T)item;
            }
            return t;
        }
    }
}
