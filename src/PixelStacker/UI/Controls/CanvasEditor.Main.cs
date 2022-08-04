using PixelStacker.EditorTools;
using PixelStacker.Extensions;
using PixelStacker.Logic.CanvasEditor;
using PixelStacker.Logic.CanvasEditor.History;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using PixelStacker.Resources;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
            this.timerBufferedChangeQueue.Interval = 20;
            this.timerPaint.Interval = Constants.DisplayRefreshIntervalMs;
            tsCanvasTools.Renderer = new CustomToolStripButtonRenderer();
            this.BackgroundImage = Resources.UIResources.bg_imagepanel;
            this.DoubleBuffered = true;
            this.ApplyLocalization();

            this.Disposed += CanvasEditor_Disposed;
            AppEvents.OnPrimaryColorChange += this.AppEvents_OnPrimaryColorChange;
        }

        private void CanvasEditor_Load(object sender, System.EventArgs e)
        {
            OnLoadToolstrips();
            this.MainForm = this.ParentForm as MainForm;
            this.Options = this.MainForm.Options;
            this.PanZoomTool = new PanZoomTool(this);
            this.CurrentTool = new PanZoomTool(this);

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


        private ConcurrentQueue<List<RenderRecord>> OnHistoryChangeQueue = new ConcurrentQueue<List<RenderRecord>>();

        public async Task SetCanvas(CancellationToken? worker, RenderedCanvas canvas, PanZoomSettings pz, SpecialCanvasRenderSettings vs)
        {
            this.BackgroundImage = UIResources.bg_imagepanel;

            pz ??= PanZoomSettings.CalculateDefaultPanZoomSettings(canvas.Width, canvas.Height, this.Width, this.Height);
            // possible to use faster math?

            ProgressX.Report(0, "Rendering block plan to viewing window.");
            var painter = await RenderedCanvasPainter.Create(worker, canvas, vs);
            this.Painter = painter;

            this.RepaintRequested = true;
            // DO not set these until ready
            this.Canvas = canvas;
            this.PanZoomSettings = pz;
            await (this.MaterialPickerForm?.SetCanvas(canvas) ?? Task.CompletedTask);
        }
    }
}
