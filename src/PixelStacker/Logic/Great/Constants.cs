using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic
{
    public static class Constants
    {
        public const string Version = "1.14.4b";
        public const string Website = "https://taylorlove.info/pixelstacker";
        public const int TextureSize = 16;
        public const string RenderedZIndexFilter = "RenderedZIndexFilter";
        public const string ERR_DownsizeYourImage = "Your image is too large and cannot be processed. Please downsize your image using the sizing options or else choose a different image.";

#if FULL_VERSION
        public const bool IsFullVersion = true;
#else
        public const bool IsFullVersion = false;
#endif
    }
}
