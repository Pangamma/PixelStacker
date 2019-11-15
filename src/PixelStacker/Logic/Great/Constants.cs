using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic
{
    public static class Constants
    {
        public const string Version = "1.14.4c";
        public const string Website = "https://taylorlove.info/pixelstacker";
        public const int TextureSize = 16;
        public const string RenderedZIndexFilter = "RenderedZIndexFilter";
        public const string ERR_DownsizeYourImage = "Your image is too large and cannot be processed. Please downsize your image using the sizing options or else choose a different image.";

        /// <summary>
        /// R/5, G/5, B/5
        /// </summary>
        public const int ColorFragmentSize = 5;

#if FULL_VERSION
        public const bool IsFullVersion = true;
#else
        public const bool IsFullVersion = false;
#endif
    }
}
