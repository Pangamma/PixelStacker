using System.Collections.Generic;
using System.Drawing;

namespace PixelStacker.Logic.Engine.Quantizer.PathProviders
{
    public interface IPathProvider
    {
        /// <summary>
        /// Retrieves the path throughout the image to determine the order in which pixels will be scanned.
        /// </summary>
        IList<Point> GetPointPath(int width, int height);
    }
}
