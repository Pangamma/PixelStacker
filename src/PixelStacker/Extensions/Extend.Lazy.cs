using PixelStacker.IO.Config;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace PixelStacker.Extensions
{
    public static partial class ExtendLazy
    {
        private static ConditionalWeakTable<object, IDictionary<string, object>> Instances = new ConditionalWeakTable<object, IDictionary<string, object>>();
        public static R LazyValue<T, R>(this T obj, Func<R> factory, [CallerMemberName] string prop = "")
        {
            R result = default(R);
            if (!ReferenceEquals(obj, null))
            {
                if (!Instances.TryGetValue(obj, out var cache))
                {
                    cache = new ConcurrentDictionary<string, object>();
                    Instances.Add(obj, cache);
                }

                if (!cache.TryGetValue(prop, out var cached))
                {
                    cache[prop] = (result = factory());
                }
                else
                {
                    result = (R)cached;
                }
            }

            return result;
        }
    }
}
