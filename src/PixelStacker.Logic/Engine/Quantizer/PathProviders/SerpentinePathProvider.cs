using System.Collections.Generic;
using System.Drawing;

namespace PixelStacker.Logic.Engine.Quantizer.PathProviders
{
    public class SerpentinePathProvider : IPathProvider
    {
        public IList<Point> GetPointPath(int width, int height)
        {
            bool leftToRight = true;
            List<Point> result = new List<Point>(width * height);

            for (int y = 0; y < height; y++)
            {
                for (int x = leftToRight ? 0 : width - 1; leftToRight ? x < width : x >= 0; x += leftToRight ? 1 : -1)
                {
                    Point point = new Point(x, y);
                    result.Add(point);
                }

                leftToRight = !leftToRight;
            }

            return result;
        }
    }
}
