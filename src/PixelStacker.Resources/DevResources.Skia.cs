#if SKIA_SHARP
#pragma warning disable IDE1006 // Naming Styles
namespace PixelStacker.Resources {
	using System;
	using SkiaSharp;

	public class DevResources {

        private static global::System.Resources.ResourceManager resourceMan;
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (resourceMan is null) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PixelStacker.Resources.DevResources", typeof(DevResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        private static SKBitmap _pink_girl = null;
        public static SKBitmap pink_girl {
            get {
                if (_pink_girl == null)
                    _pink_girl = SKBitmap.Decode((byte[])ResourceManager.GetObject("pink-girl"))
                    .Copy(SKColorType.Rgba8888);
                return _pink_girl;
            }
        }

        private static SKBitmap _psg = null;
        public static SKBitmap psg {
            get {
                if (_psg == null)
                    _psg = SKBitmap.Decode((byte[])ResourceManager.GetObject("psg"))
                    .Copy(SKColorType.Rgba8888);
                return _psg;
            }
        }

        private static SKBitmap _test_tiling = null;
        public static SKBitmap test_tiling {
            get {
                if (_test_tiling == null)
                    _test_tiling = SKBitmap.Decode((byte[])ResourceManager.GetObject("test-tiling"))
                    .Copy(SKColorType.Rgba8888);
                return _test_tiling;
            }
        }
	}
}
#pragma warning restore IDE1006 // Naming Styles
#endif
