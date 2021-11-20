using System.Collections.Generic;
using System.Drawing;

namespace PixelStacker.Logic.Collections
{
    public interface IColorMatchProvider
    { 
        /// <summary>
        ///   Gets the number of elements contained in this
        ///   tree. This is also the number of tree nodes.
        /// </summary>
        ///
        int Count { get; }

        /// <summary>
        ///   Inserts a value in the tree at the desired position.
        /// </summary>
        /// 
        /// <param name="position">A double-vector with the same number of elements as dimensions in the tree.</param>
        /// <param name="value">The value to be inserted.</param>
        /// 
        void Add(Color c);


        /// <summary>
        ///   Removes all nodes from this tree.
        /// </summary>
        void Clear();

        /// <summary>
        ///   Retrieves a fixed number of nearest points to a given point.
        /// </summary>
        ///
        /// <param name="position">The queried point.</param>
        /// <param name="neighbors">The number of neighbors to retrieve.</param>
        ///
        /// <returns>A list of neighbor points, ordered by distance.</returns>
        ///
        IEnumerable<Color> Nearest(Color position, int neighbors);

        /// <summary>
        ///   Retrieves the nearest point to a given point.
        /// </summary>
        /// <param name="position">The queried point.</param>
        Color FindBestMatch(Color position);
    }
}
