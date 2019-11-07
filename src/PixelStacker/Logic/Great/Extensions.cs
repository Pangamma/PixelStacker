using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic
{
    public static class Extensions
    {
        public static Size CalculateSize(this Point L, Point R)
        {
            int w = Math.Max(L.X, R.X) - Math.Min(L.X, R.X);
            int h = Math.Max(L.Y, R.Y) - Math.Min(L.Y, R.Y);
            Size size = new Size(Math.Max(1, w), Math.Max(1, h));
            return size;
        }

        /// <summary>
        /// Better than getBrightness. It fixes... "something". 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static float GetBrightnessMagic(this Color c)
        {
            return (c.R * 0.299f + c.G * 0.587f + c.B * 0.114f) / 256f;
        }

        /// <summary>
        /// <para>More convenient than using T.TryParse(string, out T). 
        /// Works with primitive types, structs, and enums.
        /// Tries to parse the string to an instance of the type specified.
        /// If the input cannot be parsed, null will be returned.
        /// </para>
        /// <para>
        /// If the value of the caller is null, null will be returned.
        /// So if you have "string s = null;" and then you try "s.ToNullable...",
        /// null will be returned. No null exception will be thrown. 
        /// </para>
        /// <author>Contributed by Taylor Love (Pangamma)</author>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_self"></param>
        /// <returns></returns>
        public static T? ToNullable<T>(this string p_self) where T : struct
        {
            if (!string.IsNullOrEmpty(p_self))
            {
                var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
                if (converter.IsValid(p_self)) return (T)converter.ConvertFromString(p_self);
                if (typeof(T).IsEnum) { T t; if (Enum.TryParse<T>(p_self, out t)) return t; }
            }

            return null;
        }

        /// <summary>
        /// Disposes image, and handles nulls gracefully.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static void DisposeSafely(this IDisposable src)
        {
            if (src == null) return;
            try { src.Dispose(); } catch { }
        }

    }
}
