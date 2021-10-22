using System;
using System.Collections.Generic;
using PixelStacker.Core.Model.Drawing;

namespace PixelStacker.Core.Engine.Quantizer.PathProviders
{
    public class StandardPathProvider : IPathProvider
    {
        public IList<PxPoint> GetPointPath(int width, int height)
        {
            List<PxPoint> result = new List<PxPoint>(width * height);

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    PxPoint point = new PxPoint(x, y);
                    result.Add(point);
                }

            return result;
        }
    }
}
