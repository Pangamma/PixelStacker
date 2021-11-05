using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.Engine.Dithering
{
    public class FloydSteinbergDitherer
    {
//for each y from top to bottom do
//    for each x from left to right do
//        oldpixel := pixels[x][y]
//        newpixel := find_closest_palette_color(oldpixel)
//        pixels[x][y] := newpixel
//        quant_error := oldpixel - newpixel
//        pixels[x + 1][y    ] := pixels[x + 1][y    ] + quant_error × 7 / 16
//        pixels[x - 1][y + 1] := pixels[x - 1][y + 1] + quant_error × 3 / 16
//        pixels[x    ][y + 1] := pixels[x    ][y + 1] + quant_error × 5 / 16
//        pixels[x + 1][y + 1] := pixels[x + 1][y + 1] + quant_error × 1 / 16
    }
}
