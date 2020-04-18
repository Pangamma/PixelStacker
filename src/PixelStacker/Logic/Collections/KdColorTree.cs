using PixelStacker.Logic.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.Collections
{
    public class KDColorTree: KDTreePS<Color>
    {
        public KDColorTree(): base(3)
        {
        }

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
        }

        public Color FindBestMatch(Color toMatch)
        {
            var rt = base.Nearest(new double[] { toMatch.R, toMatch.G, toMatch.B }, 10)
                .OrderBy(x => x.Node.Value.GetColorDistance(toMatch))
                .Select(x => x.Node.Value)
                .FirstOrDefault();

            return rt;
        }
    }
}
