using PixelStacker.EditorTools;
using PixelStacker.Extensions;
using PixelStacker.Logic.CanvasEditor;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using PixelStacker.Resources.Themes;
using PixelStacker.UI.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.UI.Controls
{
    public partial class CanvasEditor
    {
        private object Padlock = new { };
        public RenderedCanvasPainter Painter;

        public RenderedCanvas Canvas { get; private set; }
        public PanZoomSettings PanZoomSettings { get; set; }


        public MainForm MainForm { get; set; }

        public Options Options { get; set; }

        public CanvasEditor()
        {
            InitializeComponent();
            this.timerBufferedChangeQueue.Interval = 2 * Constants.DisplayRefreshIntervalMs;
            this.timerPaint.Interval = Constants.DisplayRefreshIntervalMs;
            tsCanvasTools.Renderer = new CustomToolStripButtonRenderer();
            this.BackgroundImage = Resources.UIResources.bg_imagepanel;
            this.DoubleBuffered = true;
            this.ApplyLocalization();
            this.Disposed += CanvasEditor_Disposed;
            AppEvents.OnPrimaryColorChange += this.AppEvents_OnPrimaryColorChange;
            ThemeManager.OnThemeChange += this.OnThemeChange;
            this.OnThemeChange(null, ThemeManager.Theme);
        }


        private void CanvasEditor_Load(object sender, System.EventArgs e)
        {
            OnLoadToolstrips();
            this.MainForm = this.ParentForm as MainForm;
            this.Options = this.MainForm.Options;
            this.PanZoomTool = new PointerTool(this);
            this.CurrentTool = new PointerTool(this);

            this.tbxBrushWidth.Text = this.Options.Tools?.BrushWidth.ToString();
            using var img = this.Options?.Tools?.PrimaryColor?.GetImage(this.Options?.IsSideView ?? false).SKBitmapToBitmap()
                ?? Resources.Textures.barrier.SKBitmapToBitmap();
            this.btnMaterialCombination.Image = img.Resize(64, 64);
        }

        private void CanvasEditor_Disposed(object sender, System.EventArgs e)
        {
            AppEvents.OnPrimaryColorChange -= this.AppEvents_OnPrimaryColorChange;
        }

        private void AppEvents_OnPrimaryColorChange(object sender, OptionsChangeEvent<MaterialCombination> e)
        {
            MaterialCombination mcAfter = Options.Tools.PrimaryColor;
            var skimg = mcAfter.GetImage(Options.IsSideView);
            using var img = skimg.SKBitmapToBitmap();
            this.btnMaterialCombination.Image = img.Resize(64, 64);

            btnMaterialCombination.ToolTipText = mcAfter.Top.Label + ", " + mcAfter.Bottom.Label;
        }

        public async Task SetCanvas(CancellationToken? worker, RenderedCanvas canvas, Func<PanZoomSettings> pzFunc, IReadonlyCanvasViewerSettings vs)
        {
            this.BackgroundImage = ThemeHelper.bg_imagepanel;
            ProgressX.Report(0, "Rendering block plan to viewing window.");
            this.Canvas = canvas;

            //this.PanZoomSettings = pz;
            var self = this;
            // Don't do heavy things on UI thread.
            await Task.Run(async () =>
            {
                worker?.ThrowIfCancellationRequested();
                var painter = await RenderedCanvasPainter.Create(worker, canvas, vs);
                await self.InvokeEx(async c =>
                {
                    var pz = pzFunc() ?? PanZoomSettings.CalculateDefaultPanZoomSettings(canvas.Width, canvas.Height, this.Width, this.Height);
                    c.PanZoomSettings = pz;
                    c.Painter = painter;
                    c.RepaintRequested = true;
                    // Do not set these until ready

                    await (c.MaterialPickerForm?.SetCanvas(canvas) ?? Task.CompletedTask);
                });
            });
        }
    }
}
