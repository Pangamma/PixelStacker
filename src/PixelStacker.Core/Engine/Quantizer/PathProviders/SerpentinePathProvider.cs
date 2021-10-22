using System;
using System.Collections.Generic;
using PixelStacker.Core.Model.Drawing;

namespace PixelStacker.Core.Engine.Quantizer.PathProviders
{
    public class SerpentinePathProvider : IPathProvider
    {
        public IList<PxPoint> GetPointPath(int width, int height)
        {
            bool leftToRight = true;
            List<PxPoint> result = new List<PxPoint>(width * height);

            for (int y = 0; y < height; y++)
            {
                for (int x = leftToRight ? 0 : width - 1; leftToRight ? x < width : x >= 0; x += leftToRight ? 1 : -1)
                {
                    PxPoint point = new PxPoint(x, y);
                    result.Add(point);
                }

                leftToRight = !leftToRight;
            }

            return result;
        }
    }
}
