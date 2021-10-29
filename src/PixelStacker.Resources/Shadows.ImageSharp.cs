#if IMAGE_SHARP
#pragma warning disable IDE1006 // Naming Styles
namespace PixelStacker.Resources {
	using System;
	using SixLabors.ImageSharp;
	using SixLabors.ImageSharp.PixelFormats;

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

        private static Image<Rgba32> _sprite_x16 = null;
        public static Image<Rgba32> sprite_x16 {
            get {
                if (_sprite_x16 == null)
                    _sprite_x16 = Image.Load((byte[])ResourceManager.GetObject("sprite_x16"));
                return _sprite_x16;
            }
        }

        private static Image<Rgba32> _sprite_x32 = null;
        public static Image<Rgba32> sprite_x32 {
            get {
                if (_sprite_x32 == null)
                    _sprite_x32 = Image.Load((byte[])ResourceManager.GetObject("sprite_x32"));
                return _sprite_x32;
            }
        }

        private static Image<Rgba32> _sprite_x64 = null;
        public static Image<Rgba32> sprite_x64 {
            get {
                if (_sprite_x64 == null)
                    _sprite_x64 = Image.Load((byte[])ResourceManager.GetObject("sprite_x64"));
                return _sprite_x64;
            }
        }
	}
}
#pragma warning restore IDE1006 // Naming Styles
#endif
