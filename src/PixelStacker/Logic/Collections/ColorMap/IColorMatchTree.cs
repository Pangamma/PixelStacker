using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.Collections
{
    public interface IColorMatchTree
    {
        void Add(Color value);
        Color FindBestMatch(Color toMatch);
        List<Color> FindBestMatches(Color toMatch, int top);
        void Clear();
        int Count { get; }

        double CalculateDistance(double[] x, double[] y);
    }
}
