using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.Collections.ColorMapper
{
    public class KdColorMapper : IColorMapper
    {

        public MaterialCombination FindBestMatch(Color c)
        {
            throw new NotImplementedException();
        }

        public List<MaterialCombination> FindBestMatches(Color c, int maxMatches)
        {
            throw new NotImplementedException();
        }

        public void SetSeedData(List<MaterialCombination> combos, MaterialPalette mats, bool isSideView)
        {
            throw new NotImplementedException();
        }
    }
}
