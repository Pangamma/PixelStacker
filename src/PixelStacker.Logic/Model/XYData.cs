using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.Model
{
    public class XYData<x, y, t>
    {
        public x X { get; set; }
        public y Y { get; set; }
        public t Data { get; set; }
    }
}
