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

        private static SKBitmap _colorwheel = null;
        public static SKBitmap colorwheel {
            get {
                if (_colorwheel == null)
                    _colorwheel = SKBitmap.Decode((byte[])ResourceManager.GetObject("colorwheel"))
                    .Copy(SKColorType.Rgba8888);
                return _colorwheel;
            }
        }

        private static SKBitmap _elsa = null;
        public static SKBitmap elsa {
            get {
                if (_elsa == null)
                    _elsa = SKBitmap.Decode((byte[])ResourceManager.GetObject("elsa"))
                    .Copy(SKColorType.Rgba8888);
                return _elsa;
            }
        }

        private static SKBitmap _hyper_dimension = null;
        public static SKBitmap hyper_dimension {
            get {
                if (_hyper_dimension == null)
                    _hyper_dimension = SKBitmap.Decode((byte[])ResourceManager.GetObject("hyper-dimension"))
                    .Copy(SKColorType.Rgba8888);
                return _hyper_dimension;
            }
        }

        private static SKBitmap _lighthouse = null;
        public static SKBitmap lighthouse {
            get {
                if (_lighthouse == null)
                    _lighthouse = SKBitmap.Decode((byte[])ResourceManager.GetObject("lighthouse"))
                    .Copy(SKColorType.Rgba8888);
                return _lighthouse;
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

        private static SKBitmap _test_borders = null;
        public static SKBitmap test_borders {
            get {
                if (_test_borders == null)
                    _test_borders = SKBitmap.Decode((byte[])ResourceManager.GetObject("test-borders"))
                    .Copy(SKColorType.Rgba8888);
                return _test_borders;
            }
        }
	}
}
#pragma warning restore IDE1006 // Naming Styles
#endif
