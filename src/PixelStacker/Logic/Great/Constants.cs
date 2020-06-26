using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic
{
    public static class Constants
    {
        public const string Version = "1.16.pre.a";
        public const string Website = "https://taylorlove.info/pixelstacker";
        public const int TextureSize = 16;
        public const string RenderedZIndexFilter = "RenderedZIndexFilter";
        public const string ERR_DownsizeYourImage = "Your image is too large and cannot be processed. Please downsize your image using the sizing options or else choose a different image.";
        public const int MAX_HISTORY_SIZE = 20;
        public const float MAX_ZOOM = 200F;
        public const float MIN_ZOOM = 200F;
        public const int SMALL_IMAGE_DIVIDE_SIZE = 2;
        public const int BIG_IMG_MAX_AREA_B4_SPLIT = 100000; // split image into smaller sizes if bigger than this

        // Data version of MC build. Can be found on wiki.
        // TODO: Auto calc this based on selected materials
        // https://minecraft.gamepedia.com/Java_Edition_1.15.2
        // https://minecraft.gamepedia.com/Java_Edition_20w15a
        public const int DataVersion = 2526;

#if FULL_VERSION
        public const bool IsFullVersion = true;
#else
        public const bool IsFullVersion = false;
#endif
    }
}
