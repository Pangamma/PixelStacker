namespace PixelStacker.Logic.IO.Config
{
    public static class Constants
    {
        public const int DisplayRefreshIntervalMs = 10;
        public const string Version = "1.20.4d";
        public const string Website = "https://taylorlove.info/pixelstacker";
        [System.Obsolete("Switch to using DefaultTextureSize, bc that is what this should represent.", true)]
        public const int TextureSize = 16;
        public const int DefaultTextureSize = 16;
        public const int MaxPageSizeForRenders = int.MaxValue;

        //public const string ERR_DownsizeYourImage = "Your image is too large and cannot be processed. Please downsize your image using the sizing options or else choose a different image.";
        public const int MAX_HISTORY_SIZE = 20;
        public const float MAX_ZOOM = 200F;
        public const float MIN_ZOOM = 200F;
        public const int SMALL_IMAGE_DIVIDE_SIZE = 2;
        public const int WORLD_HEIGHT = 1024;
        public const int BIG_IMG_MAX_AREA_B4_SPLIT = 51200; // split image into smaller sizes if bigger than this

        public const string Obs_TryToRemove = "Try to remove this. Seems useless.";
        public const string Obs_AsyncPreferred = "Switch to async";
        public const string Obs_Static = "Avoid static properties. We want to have one instance per web user.";

        // Data version of MC build. Can be found on wiki.
        // TODO: Auto calc this based on selected materials
        // https://minecraft.fandom.com/wiki/Java_Edition_1.18.1
        public const int DataVersion = 3117;
        public const int MaterialCombinationIDForAir = 0;
        public const int BlockID_Unavailable = 166; // barrier

        public const string CatGlass = "Glass";

#if USE_GPU
        public const bool C_USE_GPU = true;
#else
        public const bool C_USE_GPU = false;
#endif
        public static readonly bool C_IS_64BIT = (System.IntPtr.Size == 8);
#if DEBUG
        public const bool IsDevMode = true;
#else
        public const bool IsDevMode = false;
#endif
    }
}
