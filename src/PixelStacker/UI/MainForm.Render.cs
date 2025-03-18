using PixelStacker.Extensions;
using PixelStacker.Logic.Engine;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using System;
using System.Threading.Tasks;

namespace PixelStacker.UI
{
    public partial class MainForm
    {
        bool IsCanvasEditorVisible = false;
        private VisiblePanel CurrentlyVisiblePanel { get; set; } = VisiblePanel.Original;
        private enum VisiblePanel
        {
            Original,
            Preprocessed,
            Editor
        }

        /// <summary>
        /// Transforms PanZoomSettings from before to after and modifies panning and zooming along the way.
        /// </summary>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private PanZoomSettings TransformPanZoomSettings(VisiblePanel before, VisiblePanel after)
        {
            // No change detected.
            if (before == after)
            {
                return (before == VisiblePanel.Editor)
                    ? this.canvasEditor.PanZoomSettings
                    : this.imageViewer.PanZoomSettings;
            }

            // Offsets for UI controls present in canvas editor.
            int offsetX = 0, offsetY = 0;
            if ((before == VisiblePanel.Original || before == VisiblePanel.Preprocessed) && after == VisiblePanel.Editor)
            {
                offsetY = -43;
                offsetX = -50;
            }
            else if ((after == VisiblePanel.Original || after == VisiblePanel.Preprocessed) && before == VisiblePanel.Editor)
            {
                offsetY = 43;
                offsetX = 50;
            }

            PanZoomSettings pz = before switch
            {
                VisiblePanel.Original => this.imageViewer.PanZoomSettings,
                VisiblePanel.Preprocessed => this.imageViewer.PanZoomSettings,
                VisiblePanel.Editor => this.canvasEditor.PanZoomSettings,
                _ => throw new Exception("INVALID"),
            };

            var prevSrcWidth = before switch
            {
                VisiblePanel.Original => this.LoadedImage.Width,
                VisiblePanel.Preprocessed => this.PreprocessedImage.Width,
                VisiblePanel.Editor => this.RenderedCanvas.Width,
                _ => throw new Exception("INVALID"),
            };

            var nextSrcWidth = after switch
            {
                VisiblePanel.Original => this.LoadedImage.Width,
                VisiblePanel.Preprocessed => this.PreprocessedImage.Width,
                VisiblePanel.Editor => this.RenderedCanvas.Width,
                _ => throw new Exception("INVALID"),
            };

            var pzWithOffsets = pz.TranslateForNewSize(prevSrcWidth, nextSrcWidth);
            pzWithOffsets.imageX += offsetX;
            pzWithOffsets.imageY += offsetY;
            return pzWithOffsets;
        }

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
                        PanZoomSettings pz = c.TransformPanZoomSettings(CurrentlyVisiblePanel, VisiblePanel.Editor);
                        await c.canvasEditor.SetCanvas(worker, c.RenderedCanvas, pz, new SpecialCanvasRenderSettings(c.Options.ViewerSettings));
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
                    c.ShowImageViewer_PreRenderedImage();
                });
            }));
        }


        public void ShowImageViewer_PreRenderedImage()
        {
            var pz = this.TransformPanZoomSettings(CurrentlyVisiblePanel, VisiblePanel.Preprocessed);
            this.imageViewer.SetImage(this.PreprocessedImage, pz);
            this.canvasEditor.SendToBack();
            this.IsCanvasEditorVisible = false;
            this.CurrentlyVisiblePanel = VisiblePanel.Preprocessed;
            this.TS_SetMenuItemStatesByTagObjects();
        }


        public void ShowImageViewer_OriginalImage()
        {
            var pz = this.TransformPanZoomSettings(CurrentlyVisiblePanel, VisiblePanel.Original);
            this.imageViewer.SetImage(this.LoadedImage, pz);
            this.canvasEditor.SendToBack();
            this.IsCanvasEditorVisible = false;
            this.CurrentlyVisiblePanel = VisiblePanel.Original;
            this.TS_SetMenuItemStatesByTagObjects();
        }

        public void ShowCanvasEditor()
        {
            this.imageViewer.SendToBack();
            this.IsCanvasEditorVisible = true;
            this.CurrentlyVisiblePanel = VisiblePanel.Editor;
            this.TS_SetMenuItemStatesByTagObjects();
        }

        public async void renderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!RateLimit.Check(1, 250)) return;

            var self = this;
            // TODO: Check for memory leaks that occur when exception is thrown and resources are not disposed properly.
            await Task.Run(() => TaskManager.Get.StartAsync(async (worker) =>
            {
                var engine = new RenderCanvasEngine();
                if (!this.ColorMapper.IsSeeded())
                {
                    var validCombos = this.Palette.ToValidCombinationList(Options);
                    this.ColorMapper.SetSeedData(validCombos,
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
                    c.PreprocessedImage = imgPreprocessed;
                    PanZoomSettings pz = c.TransformPanZoomSettings(c.CurrentlyVisiblePanel, VisiblePanel.Editor);
                    await c.canvasEditor.SetCanvas(worker, c.RenderedCanvas, pz, new Logic.IO.Config.SpecialCanvasRenderSettings(c.Options.ViewerSettings));
                });

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
