using PixelStacker.Extensions;
using PixelStacker.IO.Config;
using PixelStacker.Logic;
using PixelStacker.Logic.Engine;
using PixelStacker.Logic.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.UI
{
    public partial class MainForm
    {
        private RenderedCanvasPainter CanvasPainter;

        private void ShowImageViewer()
        {
            //this.imageViewer.SetImage(this.LoadedImage, null);
            this.canvasEditor.SendToBack();
        }

        private void ShowCanvasEditor()
        {
            //var canvas = this.RenderedCanvas ?? throw new ArgumentNullException(nameof(RenderedCanvas));
            //var pz = this.imageViewer.PanZoomSettings;
            //this.canvasEditor.SetCanvas(canvas, pz);
            this.imageViewer.SendToBack();
        }

        private async void renderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!RateLimit.Check(1, 250)) return;

            var self = this;
            // TODO: Check for memory leaks that occur when exception is thrown and resources are not disposed properly.
            await Task.Run(() => TaskManager.Get.StartAsync(async (worker) =>
            {
                var engine = new RenderCanvasEngine();
                if (!this.ColorMapper.IsSeeded())
                {
                    this.ColorMapper.SetSeedData(this.Palette.ToCombinationList()
                        .Where(mc => mc.Bottom.IsVisibleF(Options) && mc.Top.IsVisibleF(Options))
                        .Where(mc => mc.Bottom.IsEnabledF(Options) && mc.Top.IsEnabledF(Options))
                        //.Where(mc => !mc.IsMultiLayer)
                        //.Where(mc => Options.IsMultiLayerRequired ? mc.IsMultiLayer : true)
                        //.Where(mc => Options.IsMultiLayer ? true : !mc.IsMultiLayer)
                        .ToList(), this.Palette, Options.Preprocessor.IsSideView);
                    worker.ThrowIfCancellationRequested();
                }

                Bitmap imgPreprocessed = await engine.PreprocessImageAsync(worker, this.LoadedImage, this.Options.Preprocessor);
                worker.ThrowIfCancellationRequested();

                // Super dubious and sketchy logic here. Might crash due to cross-context thread access issues
                self.RenderedCanvas = await engine.RenderCanvasAsync(worker, ref imgPreprocessed, this.ColorMapper, this.Palette);
                worker.ThrowIfCancellationRequested();
                await self.canvasEditor.SetCanvas(worker, self.RenderedCanvas, this.imageViewer.PanZoomSettings);

                ProgressX.Report(0, "Showing block plan in the viewing window.");
                self.InvokeEx(cc =>
                {
                    cc.ShowCanvasEditor();
                });
            }));
        }
    }
}
