#if SKIA_SHARP
#pragma warning disable IDE1006 // Naming Styles
namespace PixelStacker.Resources {
	using System;
	using SkiaSharp;

	public class Shadows {

        private static global::System.Resources.ResourceManager resourceMan;
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (resourceMan is null) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PixelStacker.Resources.Shadows", typeof(Shadows).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        private static SKBitmap _sprite_x16 = null;
        public static SKBitmap sprite_x16 {
            get {
                if (_sprite_x16 == null)
                    _sprite_x16 = SKBitmap.Decode((byte[])ResourceManager.GetObject("sprite-x16"))
                    .Copy(SKColorType.Rgba8888);
                return _sprite_x16;
            }
        }

        private static SKBitmap _sprite_x32 = null;
        public static SKBitmap sprite_x32 {
            get {
                if (_sprite_x32 == null)
                    _sprite_x32 = SKBitmap.Decode((byte[])ResourceManager.GetObject("sprite-x32"))
                    .Copy(SKColorType.Rgba8888);
                return _sprite_x32;
            }
        }

        private static SKBitmap _sprite_x64 = null;
        public static SKBitmap sprite_x64 {
            get {
                if (_sprite_x64 == null)
                    _sprite_x64 = SKBitmap.Decode((byte[])ResourceManager.GetObject("sprite-x64"))
                    .Copy(SKColorType.Rgba8888);
                return _sprite_x64;
            }
        }
	}
}
#pragma warning restore IDE1006 // Naming Styles
#endif
