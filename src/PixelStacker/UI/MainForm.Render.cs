using PixelStacker.Extensions;
using PixelStacker.Logic.Engine;
using PixelStacker.Logic.Utilities;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.UI
{
    public partial class MainForm
    {
        bool IsCanvasEditorVisible = false;
        private void switchPanelsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (IsCanvasEditorVisible) ShowImageViewer();
            else ShowCanvasEditor();
        }

        public void DoPreprocessLoadedImage()
        {
            var self = this;
            // Force a re-render or something.
            Task.Run(() => TaskManager.Get.StartAsync(async (worker) =>
            {
                var engine = new RenderCanvasEngine();
                int? tmpWidth = this.Options.Preprocessor.MaxWidth;
                try
                {
                    this.Options.Preprocessor.MaxWidth = Math.Min(600, this.Options.Preprocessor.MaxWidth ?? 600);
                    var preproc = await engine.PreprocessImageAsync(worker, this.LoadedImage, this.Options.Preprocessor);
                    self.InvokeEx((c) =>
                    {
                        var tmp = c.PreprocessedImage;
                        c.PreprocessedImage = preproc;
                        tmp.DisposeSafely();
                        c.ShowPreviewViewer();
                    });
                }
                finally
                {
                    this.Options.Preprocessor.MaxWidth = tmpWidth;
                }
            }));
        }

        public void ShowPreviewViewer()
        {
            var pz = this.imageViewer.PanZoomSettings;
            this.imageViewer.SetImage(this.PreprocessedImage, pz);
            this.canvasEditor.SendToBack();
            this.IsCanvasEditorVisible = false;
            this.TS_SetMenuItemStatesByTagObjects();
        }


        private void ShowImageViewer()
        {
            var pz = this.canvasEditor.PanZoomSettings;
            this.imageViewer.SetImage(this.LoadedImage, pz);
            this.canvasEditor.SendToBack();
            this.IsCanvasEditorVisible = false;
            this.TS_SetMenuItemStatesByTagObjects();
        }

        private void ShowCanvasEditor()
        {
            var pz = this.imageViewer.PanZoomSettings;
            this.canvasEditor.PanZoomSettings = pz;
            //var canvas = this.RenderedCanvas ?? throw new ArgumentNullException(nameof(RenderedCanvas));
            //var pz = this.imageViewer.PanZoomSettings;
            //this.canvasEditor.SetCanvas(canvas, pz);
            this.imageViewer.SendToBack();
            this.IsCanvasEditorVisible = true;
            this.TS_SetMenuItemStatesByTagObjects();
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
                    this.ColorMapper.SetSeedData(this.Palette.ToValidCombinationList(Options),
                        this.Palette, Options.Preprocessor.IsSideView);
                    worker.ThrowIfCancellationRequested();
                }

                SkiaSharp.SKBitmap imgPreprocessed = await engine.PreprocessImageAsync(worker, this.LoadedImage, this.Options.Preprocessor);
                worker.ThrowIfCancellationRequested();

                // Super dubious and sketchy logic here. Might crash due to cross-context thread access issues
                var canvasThatIsRendered = await engine.RenderCanvasAsync(worker, imgPreprocessed, this.ColorMapper, this.Palette);
                worker.ThrowIfCancellationRequested();
                self.RenderedCanvas = canvasThatIsRendered;
                //self.RenderedCanvas = await engine.PostprocessImageAsync(worker, canvasThatIsRendered, this.ColorMapper, this.Options.Preprocessor);
                //worker.ThrowIfCancellationRequested();

                await self.canvasEditor.SetCanvas(worker, self.RenderedCanvas, this.imageViewer.PanZoomSettings);

                ProgressX.Report(0, "Showing block plan in the viewing window.");
                self.InvokeEx(cc =>
                {
                    cc.ShowCanvasEditor();
                    cc.TS_OnRenderCanvas();
                    cc.TS_SetMenuItemStatesByTagObjects();
                });
            }));
        }
    }
}
