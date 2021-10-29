#if IMAGE_SHARP
#pragma warning disable IDE1006 // Naming Styles
namespace PixelStacker.Resources {
	using System;
	using SixLabors.ImageSharp;
	using SixLabors.ImageSharp.PixelFormats;

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

        private static Image<Rgba32> _pink_girl = null;
        public static Image<Rgba32> pink_girl {
            get {
                if (_pink_girl == null)
                    _pink_girl = Image.Load((byte[])ResourceManager.GetObject("pink-girl"));
                return _pink_girl;
            }
        }

        private static Image<Rgba32> _psg = null;
        public static Image<Rgba32> psg {
            get {
                if (_psg == null)
                    _psg = Image.Load((byte[])ResourceManager.GetObject("psg"));
                return _psg;
            }
        }

        private static Image<Rgba32> _test_tiling = null;
        public static Image<Rgba32> test_tiling {
            get {
                if (_test_tiling == null)
                    _test_tiling = Image.Load((byte[])ResourceManager.GetObject("test-tiling"));
                return _test_tiling;
            }
        }
	}
}
#pragma warning restore IDE1006 // Naming Styles
#endif
