namespace PixelStacker.Logic.Extensions
{
    // Sat: 0...1
    // Hue: 0...360
    // Brightness: 0...1
    public static class ExtendNumbers
    {
        //    public static string ToBaseX(this int original, int baseX)
        //    => ToBaseX(original.ToString(), 10, baseX);



        //    public static string ToBaseX(this string number, int baseOriginal, int baseX)
        //    {

        //    }

        //    public static int FromBaseX(this string number, int baseX)
        //    {

        //    }

        //    /// <summary>
        //    /// An optimized method using an array as buffer instead of 
        //    /// string concatenation. This is faster for return values having 
        //    /// a length > 1.
        //    /// </summary>
        //    private static string IntToStringFast(int value, char[] baseChars)
        //    {
        //        // 32 is the worst cast buffer size for base 2 and int.MaxValue
        //        int i = 32;
        //        char[] buffer = new char[i];
        //        int targetBase = baseChars.Length;

        //        do
        //        {
        //            buffer[--i] = baseChars[value % targetBase];
        //            value = value / targetBase;
        //        }
        //        while (value > 0);

        //        char[] result = new char[32 - i];
        //        Array.Copy(buffer, i, result, 0, 32 - i);

        //        return new string(result);
        //}
    }
}
