using System;
using System.Drawing;

namespace PixelStacker.Extensions
{
    public static partial class Extend
    {
        public static Size CalculateSize(this Point L, Point R)
        {
            int w = Math.Max(L.X, R.X) - Math.Min(L.X, R.X);
            int h = Math.Max(L.Y, R.Y) - Math.Min(L.Y, R.Y);
            Size size = new Size(Math.Max(1, w), Math.Max(1, h));
            return size;
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
                if (typeof(T).IsEnum) { if (Enum.TryParse<T>(p_self, out T t)) return t; }
            }

            return null;
        }

        /// <summary>
        /// Useful when searching. And tbh, only useful in one spot in the entire project anyways. But... I guess the
        /// syntax sugar looks nice. When searching through materials by needle 'R', you don't want to match on EVERY
        /// material containing the letter R. You want to return mateterials that START with letter 'R'. But as soon
        /// as you see 'RE', now you will want to search by using contains, since your search is more specific and
        /// you can handle more results being returned back. That is what this function is for.
        /// </summary>
        /// <param name="p_self"></param>
        /// <param name="needle">Should be lowercase.</param>
        /// <param name="minLengthForContainsSearch"></param>
        /// <returns></returns>
        public static bool StartsWithOrContains(this string p_self, string needle, int minLengthForContainsSearch)
        {
            if (needle.Length >= minLengthForContainsSearch)
            {
                return p_self.ToLowerInvariant().Contains(needle);
            } 
            else
            {
                return p_self.ToLowerInvariant().StartsWith(needle);
            }
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
