using System;
using System.Collections.Generic;
using PixelStacker.Core.Model.Drawing;

namespace PixelStacker.Core.Engine.Quantizer.PathProviders
{
    public class ReversedPathProvider : IPathProvider
    {
        public IList<PxPoint> GetPointPath(int width, int height)
        {
            List<PxPoint> result = new List<PxPoint>(width * height);

            for (int y = height - 1; y >= 0; y--)
                for (int x = width - 1; x >= 0; x--)
                {
                    PxPoint point = new PxPoint(x, y);
                    result.Add(point);
                }

            return result;
        }
    }
}
