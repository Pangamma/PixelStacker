using PixelStacker.Logic.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.IO.Formatters
{
    public class BlockCountCsvFormatter : IExportFormatter
    {
        public async Task ExportAsync(string filePath, PixelStackerProjectData canvas, CancellationToken? worker = null)
        {
            Dictionary<Material, int> materialCounts = new Dictionary<Material, int>();
            bool isv = canvas.IsSideView;
            var data = canvas.CanvasData;
            var Palette = canvas.MaterialPalette;
            bool isMultilayer = data.Any(x => Palette[x.PaletteID].IsMultiLayer);

            for (int x = 0; x < data.Width; x++)
            {
                for (int y = 0; y < data.Height; y++)
                {
                    var mc = data[x, y];
                    
                }
            }
            //int xM = this.LoadedBlueprint.Mapper.GetXLength(isv);
            //int yM = this.LoadedBlueprint.Mapper.GetYLength(isv);
            //int zM = this.LoadedBlueprint.Mapper.GetZLength(isv);
            //for (int x = 0; x < xM; x++)
            //{
            //    for (int y = 0; y < yM; y++)
            //    {
            //        for (int z = 0; z < zM; z++)
            //        {
            //            Material m = this.LoadedBlueprint.Mapper.GetMaterialAt(isv, x, y, z);
            //            if (m != Materials.Air)
            //            {
            //                if (!materialCounts.ContainsKey(m))
            //                {
            //                    materialCounts.Add(m, 0);
            //                }

            //                materialCounts[m] = materialCounts[m] + 1;
            //            }
            //        }
            //    }
            //}

            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine("\"Material\",\"Block Count\",\"Full Stacks needed\"");
            //sb.AppendLine("\"Total\"," + materialCounts.Values.Sum());

            //foreach (var kvp in materialCounts.OrderByDescending(x => x.Value))
            //{
            //    sb.AppendLine($"\"{kvp.Key.GetBlockNameAndData(isv).Replace("\"", "\"\"")}\",{kvp.Value},{kvp.Value / 64} stacks and {kvp.Value % 64} remaining blocks");
            //}

            //File.WriteAllText(fName, sb.ToString());
        }
    }
}
