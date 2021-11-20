using PixelStacker.Logic.Extensions;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PixelStacker.Logic.Collections
{
    public class KDColorTree: KDTree<Color>, IColorMatchProvider
    {
        public KDColorTree(): base(3)
        {
            this.Distance = this.DistanceFunc;
        }

        private double DistanceFunc(double[] arg1, double[] arg2)
        {
            var c1 = Color.FromArgb(255, (int) arg1[0], (int) arg1[1], (int) arg1[2]);
            var c2 = Color.FromArgb(255, (int) arg2[0], (int) arg2[1], (int) arg2[2]);
            return c1.GetColorDistance(c2);
        }

        private List<Color> colors = new List<Color>();

        /// <summary>
        ///   Inserts a value in the tree at the desired position.
        /// </summary>
        /// 
        /// <param name="value">The value to be inserted.</param>
        /// 
        public void Add(Color value)
        {
            var position = new double[] { value.R, value.G, value.B };
            base.AddNode(position).Value = value;
            colors.Add(value);
        }

        public Color FindBestMatch(Color toMatch)
        {
            var d = new double[] { toMatch.R, toMatch.G, toMatch.B };
            var rt = base.Nearest(d);
            return rt.Value;
        }

        public IEnumerable<Color> Nearest(Color position, int neighbors)
        {
            var nodes = base.Nearest(new double[] { position.R, position.G, position.B }, neighbors);
            return nodes.Select(x => x.Node.Value).ToList();
        }
    }
}
