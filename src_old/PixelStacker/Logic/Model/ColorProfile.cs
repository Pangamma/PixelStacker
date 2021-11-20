using System.Collections.Generic;

namespace PixelStacker.Logic.Model
{
    public class ColorProfile
    {
        public string Label { get; set; }
        public Dictionary<string, bool> Materials { get; set; } = new Dictionary<string, bool>();
    }
}
