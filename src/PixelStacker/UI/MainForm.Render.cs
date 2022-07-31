using PixelStacker.Extensions;
using PixelStacker.Logic.Engine;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Utilities;
using System;
using System.Threading.Tasks;

namespace PixelStacker.UI
{
    public partial class MainForm
    {
        bool IsCanvasEditorVisible = false;
        private async void switchPanelsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {

            this.Options.ViewerSettings.IsSolidColors = !this.Options.ViewerSettings.IsSolidColors;
            this.Options.Save();
            this.TS_SetAllMenubarStatesBasedOnOptions(this.Options);
            if (this.IsCanvasEditorVisible && this.RenderedCanvas != null)
            {
                var self = this;
                await Task.Run(() => TaskManager.Get.StartAsync(async (worker) =>
                {
                    await self.InvokeEx(async c =>
                    {
                        await c.canvasEditor.SetCanvas(worker, c.RenderedCanvas, c.canvasEditor.PanZoomSettings, new SpecialCanvasRenderSettings(c.Options.ViewerSettings));
                        c.ShowCanvasEditor();
                    });

                }));
            }
        }

        public void DoPreprocessLoadedImage()
        {
            var self = this;
            // Force a re-render or something.
            Task.Run(() => TaskManager.Get.StartAsync(async (worker) =>
            {
                var engine = new RenderCanvasEngine();
                var preproc = await engine.PreprocessImageAsync(worker, this.LoadedImage, this.Options.Preprocessor);
                self.InvokeEx((c) =>
                {
                    var tmp = c.PreprocessedImage;
                    c.PreprocessedImage = preproc;
                    tmp.DisposeSafely();
                    c.ShowPreviewViewer();
                });
            }));
        }

        public void ShowPreviewViewer()
        {
            var pz = this.imageViewer.PanZoomSettings;
            this.imageViewer.SetImage(this.PreprocessedImage, pz);
            this.canvasEditor.SendToBack();
            this.IsCanvasEditorVisible = false;
            this.TS_SetMenuItemStatesByTagObjects();
            //progressBar1.Visible = true;
            //lblProgress.Visible = true;
        }


        private void ShowImageViewer()
        {
            var pz = this.canvasEditor.PanZoomSettings;
            this.imageViewer.SetImage(this.LoadedImage, pz);
            this.canvasEditor.SendToBack();
            this.IsCanvasEditorVisible = false;
            this.TS_SetMenuItemStatesByTagObjects();
            //progressBar1.Visible = true;
            //lblProgress.Visible = true;
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
            //progressBar1.Visible = false;
            //lblProgress.Visible = false;
            this.TS_SetMenuItemStatesByTagObjects();
        }

        internal async void renderToolStripMenuItem_Click(object sender, EventArgs e)
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
                        this.Palette, Options.IsSideView);
                    worker.ThrowIfCancellationRequested();
                }

                SkiaSharp.SKBitmap imgPreprocessed = await engine.PreprocessImageAsync(worker, this.LoadedImage, this.Options.Preprocessor);
                worker.ThrowIfCancellationRequested();

                // Super dubious and sketchy logic here. Might crash due to cross-context thread access issues
                var canvasThatIsRendered = await engine.RenderCanvasAsync(worker, imgPreprocessed, this.ColorMapper, this.Palette);
                worker.ThrowIfCancellationRequested();

                await self.InvokeEx(async c =>
                {
                    c.RenderedCanvas = canvasThatIsRendered;

                    await c.canvasEditor.SetCanvas(worker, c.RenderedCanvas, null, new Logic.IO.Config.SpecialCanvasRenderSettings(c.Options.ViewerSettings));
                });
                //self.RenderedCanvas = canvasThatIsRendered;

                //var pz = self.imageViewer.PanZoomSettings;
                //var pz2 = pz.TranslateForNewSize(canvasThatIsRendered.Width, canvasThatIsRendered.Height, self.canvasEditor.Width, self.canvasEditor.Height);

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
