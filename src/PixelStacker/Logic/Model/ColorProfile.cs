using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.Model
{
    public class ColorProfile
    {
        public string Label { get; set; }
        public Dictionary<string, bool> Materials { get; set; } = new Dictionary<string, bool>();
    }
}
