using PixelStacker.Logic;
using PixelStacker.Logic.Engine;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.UI
{
    public partial class MainForm
    {

        private async void renderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Task.Run(() => TaskManager.Get.StartAsync(async (worker) =>
            {
                var engine = new RenderCanvasEngine();
                if (!this.ColorMapper.IsSeeded())
                {
                    this.ColorMapper.SetSeedData(this.Palette.ToCombinationList()
                            .Where(mc => Options.IsMultiLayerRequired ? mc.IsMultiLayer : true)
                            .Where(mc => mc.Bottom.IsEnabledF(Options) && mc.Top.IsEnabledF(Options))
                            .Where(mc => Options.IsMultiLayer ? true : !mc.IsMultiLayer)
                            .ToList(), this.Palette, Options.Preprocessor.IsSideView);
                    worker.ThrowIfCancellationRequested();
                }

                var imgPreprocessed = await engine.PreprocessImageAsync(worker, this.LoadedImage, this.Options.Preprocessor);
                worker.ThrowIfCancellationRequested();

                this.RenderedCanvas = await engine.RenderCanvasAsync(worker, ref imgPreprocessed, this.ColorMapper, this.Palette);
                worker.ThrowIfCancellationRequested();

                ProgressX.Report(0, "Rendering block plan to viewing window.");
                var splitmap = await RenderedCanvasSplitmap.Create(worker, this.RenderedCanvas);
                worker.ThrowIfCancellationRequested();
            }));
        }
    }
}
