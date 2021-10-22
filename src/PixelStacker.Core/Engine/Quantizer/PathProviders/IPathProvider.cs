using System;
using System.Collections.Generic;
using PixelStacker.Core.Model.Drawing;

namespace PixelStacker.Core.Engine.Quantizer.PathProviders
{
    public interface IPathProvider
    {
        /// <summary>
        /// Retrieves the path throughout the image to determine the order in which pixels will be scanned.
        /// </summary>
        IList<PxPoint> GetPointPath(int width, int height);
    }
}
