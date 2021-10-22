using System.Collections.Generic;
using System.Drawing;

namespace PixelStacker.Logic.Engine.Quantizer.PathProviders
{
    public class ReversedPathProvider : IPathProvider
    {
        public IList<Point> GetPointPath(int width, int height)
        {
            List<Point> result = new List<Point>(width * height);

            for (int y = height - 1; y >= 0; y--)
                for (int x = width - 1; x >= 0; x--)
                {
                    Point point = new Point(x, y);
                    result.Add(point);
                }

            return result;
        }
    }
}
