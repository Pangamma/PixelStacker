using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.IO.Config
{
    public class OptionsChangeEvent<T> : EventArgs
    {
        public T PreviousValue { get; set; }
        public T NewValue { get; set; }
    }

    internal static class AppEvents
    {
        public static void RouteOptionChangeEvent<T>(string methodName, OptionsChangeEvent<T> evt)
        {
            object srcInstance = null;

            var eventDelegate = (MulticastDelegate)typeof(AppEvents)
                .GetField(methodName, BindingFlags.Static | BindingFlags.NonPublic)
                .GetValue(srcInstance);

            if (eventDelegate != null)
            {
                var handlers = eventDelegate.GetInvocationList();
                //Debug.WriteLine($"CHANGE_EVENT=({methodName}), Listeners = {handlers.Length}");
                foreach (var handler in handlers)
                {
                    handler.Method.Invoke(handler.Target, new object[] { srcInstance, evt });
                }
            }
        }

        public static event EventHandler<OptionsChangeEvent<MaterialCombination>> OnPrimaryColorChange;
        public static event EventHandler<OptionsChangeEvent<int>> OnBrushWidthChange;
        public static event EventHandler<OptionsChangeEvent<bool>> IsSideViewChange;
        public static event EventHandler<OptionsChangeEvent<bool>> OnAdvancedModeChange;
        public static event EventHandler<OptionsChangeEvent<string>> OnLocaleChange;
    }
}
