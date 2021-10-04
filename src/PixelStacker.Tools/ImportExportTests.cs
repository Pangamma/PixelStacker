using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixelStacker.Logic.Model;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PixelStacker.IO.Formatters;
using PixelStacker.Extensions;
using System.Text;
using PixelStacker.Resources;

namespace PixelStacker.Tools
{
    [TestClass]
    public class ImportExportTests
    {
        private RenderedCanvas Canvas;
        public ImportExportTests()
        {
            string json = Encoding.UTF8.GetString(DataResources.materialPalette);
            MaterialPalette mat = JsonConvert.DeserializeObject<MaterialPalette>(json);
            var img = UIResources.weird_intro.To32bppBitmap();
            Canvas = new RenderedCanvas()
            {
                MaterialPalette = mat,
                OriginalImage = img,
                WorldEditOrigin = new System.Drawing.Point(0, img.Height - 1),
                CanvasData = new CanvasData(mat, new int[img.Width, img.Height])
            };

            Canvas.CanvasData[0, 0] = mat[55];
            Canvas.CanvasData[0, 1] = mat[55];
            Canvas.CanvasData[0, 2] = mat[55];
            Canvas.CanvasData[0, 3] = mat[55];
            Canvas.CanvasData[0, 4] = mat[55];
            Canvas.CanvasData[0, 5] = mat[55];
        }

        [TestMethod]
        [TestCategory("IO")]
        public async Task IE_PixelStackerProjectFormat()
        {
            var formatter = new PixelStackerProjectFormatter();
            await formatter.ExportAsync("test.zip", Canvas, null);
            var canv = await formatter.ImportAsync("test.zip", null);
            Assert.AreEqual(Canvas.WorldEditOrigin, canv.WorldEditOrigin);
            Assert.AreEqual(JsonConvert.SerializeObject(Canvas.MaterialPalette), JsonConvert.SerializeObject(canv.MaterialPalette));
            Assert.AreEqual(Canvas.OriginalImage.Height, canv.OriginalImage.Height);
        }
    }
}
