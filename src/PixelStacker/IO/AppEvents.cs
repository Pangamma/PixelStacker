using PixelStacker.EditorTools;
using PixelStacker.Logic.Model;
using System;
using System.Reflection;

namespace PixelStacker.Logic.IO.Config
{
    public class OptionsChangeEvent<T> : EventArgs
    {
        public T PreviousValue { get; set; }
        public T NewValue { get; set; }
    }

    /// <summary>
    /// Occurs when user manually samples a color using one of the tools.
    /// </summary>
    public class UserSampledMaterialEvent : EventArgs
    {
        public MaterialCombination MaterialCombination { get; set; }
        public bool IsLeftClick { get; set; } = false;
    }


    internal static class AppEvents
    {
        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodName">Example: OnPrimaryColorChange</param>
        /// <param name="evt"></param>
        public static void PublishEvent<E>(string methodName, E evt) where E : EventArgs
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
        public static event EventHandler<UserSampledMaterialEvent> OnUserSampledMaterialEvent;
    }
}
