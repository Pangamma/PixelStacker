using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.Collections
{
    public interface IColorMapper
    {
        void SetSeedData(List<MaterialCombination> combos, MaterialPalette mats, bool isSideView);
        MaterialCombination FindBestMatch(Color c);
        List<MaterialCombination> FindBestMatches(Color c, int maxMatches);
    }
}
