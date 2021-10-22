using PixelStacker.Core.Model;
using PixelStacker.Core.Model.Mats;

namespace PixelStacker.Core.IO.Formatters
{
    public class BlockCountCsvFormatter : IExportFormatter
    {
        public async Task ExportAsync(string filePath, RenderedCanvas canvas, CancellationToken? worker = null)
        {
            Dictionary<Material, int> materialCounts = new Dictionary<Material, int>();
            bool isv = canvas.CanvasData.IsSideView;
            var data = canvas.CanvasData;
            var Palette = canvas.MaterialPalette;
            bool isMultilayer = data.Any(x => Palette[x.PaletteID].IsMultiLayer);

            for (int x = 0; x < data.Width; x++)
            {
                for (int y = 0; y < data.Height; y++)
                {
                }
            }
        }
    }
}
