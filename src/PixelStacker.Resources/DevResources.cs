using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Resources
{
    public class ResourceHelper
    {
        public static Lazy<System.Reflection.Assembly> Assembly = new Lazy<System.Reflection.Assembly>(() => typeof(ResourceHelper).Assembly);
        public static Lazy<HashSet<string>> ResourceNames { get; set; } = new Lazy<HashSet<string>>(() => new HashSet<string>(Assembly.Value.GetManifestResourceNames()));

        public static void Get()
        {
            var foo = ResourceNames.Value;
            foreach(var bar in foo)
            {
                if (bar.EndsWith(".png"))
                {
                    var barInfo = Assembly.Value.GetManifestResourceInfo(bar);
                    var barStream = Assembly.Value.GetManifestResourceStream(bar);
                    var skBitmap = SKBitmap.Decode(barStream);
                }
            }
        }
    }
}
