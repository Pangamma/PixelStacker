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

        private static Image<Rgba32> _colorwheel = null;
        public static Image<Rgba32> colorwheel {
            get {
                if (_colorwheel == null)
                    _colorwheel = Image.Load((byte[])ResourceManager.GetObject("colorwheel"));
                return _colorwheel;
            }
        }

        private static Image<Rgba32> _elsa = null;
        public static Image<Rgba32> elsa {
            get {
                if (_elsa == null)
                    _elsa = Image.Load((byte[])ResourceManager.GetObject("elsa"));
                return _elsa;
            }
        }

        private static Image<Rgba32> _hyper_dimension = null;
        public static Image<Rgba32> hyper_dimension {
            get {
                if (_hyper_dimension == null)
                    _hyper_dimension = Image.Load((byte[])ResourceManager.GetObject("hyper-dimension"));
                return _hyper_dimension;
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

        private static Image<Rgba32> _test_borders = null;
        public static Image<Rgba32> test_borders {
            get {
                if (_test_borders == null)
                    _test_borders = Image.Load((byte[])ResourceManager.GetObject("test-borders"));
                return _test_borders;
            }
        }
	}
}
#pragma warning restore IDE1006 // Naming Styles
#endif
