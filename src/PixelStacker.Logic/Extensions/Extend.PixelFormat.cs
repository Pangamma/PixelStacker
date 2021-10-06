using PixelStacker.IO.Config;
using System;
using System.Drawing.Imaging;

namespace PixelStacker.Extensions
{
    /// <summary>
    /// The utility extender class.
    /// </summary>
    public static partial class Extend
    {

        [Obsolete(Constants.Obs_TryToRemove)]
        /// <summary>
        /// Gets the friendly name of the pixel format.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <returns></returns>
        public static string GetFriendlyName(this PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format1bppIndexed:
                    return "Indexed (2 colors)";

                case PixelFormat.Format4bppIndexed:
                    return "Indexed (16 colors)";

                case PixelFormat.Format8bppIndexed:
                    return "Indexed (256 colors)";

                case PixelFormat.Format16bppGrayScale:
                    return "Grayscale (65536 shades)";

                case PixelFormat.Format16bppArgb1555:
                    return "Highcolor + Alpha mask (32768 colors)";

                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                    return "Highcolor (65536 colors)";

                case PixelFormat.Format24bppRgb:
                    return "Truecolor (24-bit)";

                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                    return "Truecolor + Alpha (32-bit)";

                case PixelFormat.Format32bppRgb:
                    return "Truecolor (32-bit)";

                case PixelFormat.Format48bppRgb:
                    return "Truecolor (48-bit)";

                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    return "Truecolor + Alpha (64-bit)";

                default:
                    string message = string.Format("A pixel format '{0}' not supported!", pixelFormat);
                    throw new NotSupportedException(message);
            }
        }

        /// <summary>
        /// Determines whether the specified pixel format is indexed.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <returns>
        /// 	<c>true</c> if the specified pixel format is indexed; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIndexed(this PixelFormat pixelFormat)
        {
            return (pixelFormat & PixelFormat.Indexed) == PixelFormat.Indexed;
        }

        [Obsolete(Constants.Obs_TryToRemove)]
        /// <summary>
        /// Determines whether the specified pixel format is supported.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <returns>
        /// 	<c>true</c> if the specified pixel format is supported; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSupported(this PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format1bppIndexed:
                case PixelFormat.Format4bppIndexed:
                case PixelFormat.Format8bppIndexed:
                case PixelFormat.Format16bppArgb1555:
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                case PixelFormat.Format24bppRgb:
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                case PixelFormat.Format48bppRgb:
                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    return true;

                default:
                    return false;
            }
        }
    }
}


