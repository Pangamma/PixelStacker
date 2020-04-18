using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PixelStacker.Logic;
using PixelStacker.Properties;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;
using PixelStacker.Logic.WIP;
using PixelStacker.Logic.Extensions;
using PixelStacker.Resources;
using PixelStacker.Logic.Collections;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace PixelStacker.UI
{
    public partial class RenderedImagePanel : UserControl
    {
        private const byte SHOWN_NONE = 0;
        private const byte SHOWN_TOP = 1;
        private const byte SHOWN_BOTTOM = 2;
        private const byte SHOWN_TOP_AND_BOTTOM = 3;

        public int xOrigin = 0;
        public int yOrigin = 0;
        private BlueprintPA image;


        /// <summary>
        /// Should contain: 
        /// 0 = 1/1 size, when viewing at zoom(tex)+ to zoom(tex * 0.75) 
        /// 1 = 1/2 size
        /// 2 = 1/4 size
        /// 3 = 1/8 size
        /// </summary>
        private List<Bitmap> renderedImages = new List<Bitmap>();
        //private Bitmap renderedImage;
        //private Bitmap renderedImageSmallSize;

        private string materialToAddOrRemove_1 { get; set; } = null;
        private string materialToAddOrRemove_2 { get; set; } = null;
        private int CalculatedTextureSize { get; set; } = Constants.TextureSize;

        private Rectangle? gridMaskClip = null;
        private Point initialDragPoint;
        private bool IsDragging = false;

        public RenderedImagePanel()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public async Task<bool> ForceReRender()
        {
            await TaskManager.Get.StartAsync((token) =>
            {
                Bitmap _renderedImage = RenderedImagePanel.RenderBitmapFromBlueprint(token, image, out int? textureSize);
                CalculatedTextureSize = textureSize ?? Constants.TextureSize;

                this.ClearAndDisposeRenderedImages();
                if (_renderedImage == null) return;
                var images = new List<Bitmap>();
                images.Add(_renderedImage);
                int w = _renderedImage.Width;
                int h = _renderedImage.Height;
                int a = w * h;

                while (a > Constants.BIG_IMG_MAX_AREA_B4_SPLIT)
                {
                    w /= Constants.SMALL_IMAGE_DIVIDE_SIZE;
                    h /= Constants.SMALL_IMAGE_DIVIDE_SIZE;
                    if (w == 0 || h == 0) break;
                    a = w * h;
                    _renderedImage = _renderedImage.To32bppBitmap(w, h);
                    images.Add(_renderedImage);
                }

                this.renderedImages = images; // le quick swap

                TaskManager.SafeReport(0, "Finished.");
                this.InvokeEx((c) =>
                {
                    c.Refresh();
                });
            });

            return true;
        }

        private static bool isShaded(int current, int adjacent)
        {
            // top and bottom = no shade
            if (current == SHOWN_TOP_AND_BOTTOM)
            {
                return false;
            }

            // air by top = shade
            // air by bottom = shade
            // air by top and bottom = shade
            if (current == SHOWN_NONE)
            {
                if (adjacent != SHOWN_NONE)
                {
                    return true;
                }
            }

            // isBottomCoveredByInvisibleTop and by top
            if (current == SHOWN_BOTTOM)
            {
                if (adjacent == SHOWN_TOP || adjacent == SHOWN_TOP_AND_BOTTOM)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// WILL CONSUME THE INPUT IMAGE AND MODIFY IT IN PLACE
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="blueprint"></param>
        /// <returns></returns>
        public static Bitmap RenderPlaceholderBitmapFromBlueprint(CancellationToken worker, Bitmap blueprint)
        {
            int mWidth = blueprint.Width;
            int mHeight = blueprint.Height;

            if (ColorMatcher.Get.ColorToMaterialMap.Count == 0)
            {
                TaskManager.SafeReport(0, "Compiling the color map");
                ColorMatcher.Get.CompileColorPalette(worker, true, Materials.List).GetAwaiter().GetResult();
            }

            int numPixelsProcessed = 0;
            int numPixelsTotal = mWidth * mHeight;
            blueprint.ToEditStream(worker, (int x, int y, Color c) =>
            {

                Color cFromPalette = ColorMatcher.Get.FindBestMatch(c);


                if (cFromPalette.A == 0)
                {
                    return Materials.Air.getAverageColor(true);
                }

                Interlocked.Increment(ref numPixelsProcessed);
                TaskManager.SafeReport(100 * numPixelsProcessed / numPixelsTotal, "Rendering low-rez preview to give the illusion of a faster program.");

                return cFromPalette;
            });
            TaskManager.SafeReport(100, "");
            return blueprint;
        }

        public class Coordinate
        {
            public int x { get; set; }
            public int y { get; set; }
            public int z { get; set; }
            // TODO: This is going in a dict, we don't need to override Equals or GetHashCode do we?

            public override bool Equals(object obj)
            {
                if (obj is Coordinate c)
                    return c.x == x && c.y == y && c.z == z;
                return false;
            }

            public override int GetHashCode()
            {
                return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
            }
        }



        public static Bitmap RenderBitmapFromBlueprint(CancellationToken? worker, BlueprintPA blueprint, out int? textureSize)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            // TODO: Make sure this value is saved to the render panel instance somehow or else there will be horrible issues
            textureSize = CalculateTextureSize(blueprint);
            if (textureSize == null) return null;

            if (blueprint != null)
            {
                try
                {
                    TaskManager.SafeReport(0, "Converting blueprint to bitmap");
                    bool isSelectiveLayerViewEnabled = Options.Get.IsEnabled(Constants.RenderedZIndexFilter, false);
                    bool isMaterialFilterViewEnabled = Options.Get.SelectedMaterialFilter.Any();
                    bool isSide = Options.Get.IsSideView;
                    double origW = blueprint.Width;
                    double origH = blueprint.Height;
                    int w = (int)(origW * MainForm.PanZoomSettings.zoomLevel);
                    int h = (int)(origH * MainForm.PanZoomSettings.zoomLevel);
                    int zoom = (int)(MainForm.PanZoomSettings.zoomLevel);


                    SolidBrush brush = new SolidBrush(Color.Black);
                    Pen pen = new Pen(brush);

                    bool isMaterialIncludedInFilter = true;
                    int mWidth = blueprint.Width;
                    int mHeight = blueprint.Height;
                    int mDepth = Options.Get.IsMultiLayer ? 2 : 1;

                    int calcW = mWidth * textureSize.Value;
                    int calcH = mHeight * textureSize.Value;
                    Bitmap bm = new Bitmap(
                        width: calcW,
                        height: calcH,
                        format: PixelFormat.Format32bppArgb);

                    var selectedMaterials = Options.Get.SelectedMaterialFilter.AsEnumerable().ToList(); // clone
                    bool _IsSolidColors = Options.Get.Rendered_IsSolidColors;
                    bool _IsColorPalette = Options.Get.Rendered_IsColorPalette;
                    bool _IsMultiLayer = Options.Get.IsMultiLayer;
                    bool _isSkipShadowRendering = Options.Get.IsShadowRenderingSkipped;
                    int _RenderedZIndexToShow = Options.Get.Rendered_RenderedZIndexToShow;
                    bool _isFrugalAesthetic = Options.Get.IsFrugalWithMaterials && !selectedMaterials.Any();


                    using (Graphics gImg = Graphics.FromImage(bm))
                    {
                        gImg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                        gImg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                        gImg.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;


                        #region Regular
                        for (int z = 0; z < mDepth; z++)
                        {
                            TaskManager.SafeReport(0, "Rendering to materials display... (Layer " + z + ")");

                            if (isSelectiveLayerViewEnabled)
                            {
                                if (z != _RenderedZIndexToShow)
                                {
                                    continue;
                                }
                            }

                            for (int x = 0; x < mWidth; x++)
                            {
                                TaskManager.SafeReport(100 * x / mWidth);
                                worker?.SafeThrowIfCancellationRequested();
                                for (int y = 0; y < mHeight; y++)
                                {
                                    int xi = x * textureSize.Value;
                                    int yi = y * textureSize.Value;
                                    if (xi + MainForm.PanZoomSettings.zoomLevel >= 0 && yi + MainForm.PanZoomSettings.zoomLevel >= 0)
                                    {
                                        Material m = blueprint.GetMaterialAt(x, y, z, !_isFrugalAesthetic);

                                        if (isMaterialFilterViewEnabled)
                                        {
                                            string blockId = m.PixelStackerID;
                                            isMaterialIncludedInFilter = Options.Get.SelectedMaterialFilter.Any(xm => xm == blockId);
                                        }

                                        if (m.BlockID != 0)
                                        {
                                            if (_IsSolidColors)
                                            {
                                                if (isMaterialIncludedInFilter)
                                                {
                                                    brush.Color = blueprint.GetColor(x, y);
                                                    gImg.FillRectangle(brush, xi, yi, textureSize.Value, textureSize.Value);
                                                }
                                            }
                                            else if (_IsColorPalette)
                                            {
                                                if (isMaterialIncludedInFilter)
                                                {
                                                    brush.Color = blueprint.GetColor(x, y);
                                                    gImg.DrawImage(m.getImage(isSide), xi, yi, textureSize.Value, textureSize.Value);
                                                    gImg.FillRectangle(brush, xi, yi, textureSize.Value / 2, textureSize.Value / 2);
                                                    brush.Color = Color.Black;
                                                    gImg.DrawRectangle(pen, xi, yi, textureSize.Value / 2, textureSize.Value / 2);
                                                }
                                            }
                                            else
                                            {
                                                if (isMaterialIncludedInFilter)
                                                {
                                                    gImg
                                                        .DrawImage(m.getImage(isSide), xi, yi, textureSize.Value, textureSize.Value);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        watch.Stop();
                        Console.WriteLine($"Rendered bitmap from blueprint in {watch.ElapsedMilliseconds}ms");
                        #region SHADOW_NEW
                        if (!_isSkipShadowRendering)
                        {
                            Bitmap bmShadeSprites = ShadowHelper.GetSpriteSheet(Constants.TextureSize);

                            Bitmap bmShadow = new Bitmap(
                            width: calcW,
                            height: calcH,
                            format: PixelFormat.Format32bppArgb);

                            byte[,] shadowMap = new byte[mWidth, mHeight];
                            {
                                #region Initialize shadow map (booleans basically)
                                TaskManager.SafeReport(0, "Rendering shader map");
                                for (int xShadeMap = 0; xShadeMap < mWidth; xShadeMap++)
                                {
                                    TaskManager.SafeReport(100 * xShadeMap / mWidth);
                                    worker?.SafeThrowIfCancellationRequested();

                                    for (int yShadeMap = 0; yShadeMap < mHeight; yShadeMap++)
                                    {
                                        Material mBottom = blueprint.GetMaterialAt(xShadeMap, yShadeMap, 0, true);
                                        bool isBottomShown = mBottom.BlockID != 0 && (selectedMaterials.Count == 0 || selectedMaterials.Any(xm => xm == mBottom.PixelStackerID));

                                        Material mTop = blueprint.GetMaterialAt(xShadeMap, yShadeMap, 1, !_isFrugalAesthetic);
                                        bool isTopShown = mTop.BlockID != 0 && (selectedMaterials.Count == 0 || selectedMaterials.Any(xm => xm == mTop.PixelStackerID));

                                        if (isTopShown && isBottomShown)
                                        {
                                            shadowMap[xShadeMap, yShadeMap] = SHOWN_TOP_AND_BOTTOM;
                                        }
                                        else if (isTopShown)
                                        {
                                            shadowMap[xShadeMap, yShadeMap] = SHOWN_TOP;
                                        }
                                        else if (isBottomShown)
                                        {
                                            shadowMap[xShadeMap, yShadeMap] = SHOWN_BOTTOM;
                                        }
                                        else
                                        {
                                            shadowMap[xShadeMap, yShadeMap] = SHOWN_NONE;
                                        }
                                    }
                                }
                                #endregion

                                using (Graphics gShadow = Graphics.FromImage(bmShadow))
                                {
                                    gShadow.CompositingMode = CompositingMode.SourceOver; // over is slower but better...
                                    gShadow.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                                    gShadow.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                                    gShadow.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

                                    var brushTransparentCover = new SolidBrush(Color.FromArgb(40, 127, 127, 127));
                                    {
                                        TaskManager.SafeReport(0, "Rendering shadows");
                                        for (int x = 0; x < mWidth; x++)
                                        {
                                            TaskManager.SafeReport(100 * x / mWidth);
                                            worker?.SafeThrowIfCancellationRequested();
                                            for (int y = 0; y < mHeight; y++)
                                            {
                                                int xi = x * textureSize.Value;
                                                int yi = y * textureSize.Value;

                                                bool isTopShown = shadowMap[x, y] == SHOWN_TOP || shadowMap[x, y] == SHOWN_TOP_AND_BOTTOM;
                                                bool isBottomShown = shadowMap[x, y] == SHOWN_BOTTOM || shadowMap[x, y] == SHOWN_TOP_AND_BOTTOM;
                                                bool isBottomCoveredByInvisibleTop = isBottomShown && !isTopShown;

                                                // The thing that makes it slightly less saturated on bottom layer
                                                if (isBottomCoveredByInvisibleTop && _IsMultiLayer)
                                                {
                                                    gShadow.FillRectangle(brushTransparentCover, xi, yi, textureSize.Value, textureSize.Value);
                                                }

                                                if (isTopShown && isBottomShown)
                                                {
                                                    continue; // No shade required
                                                }

                                                // AIR block (or block we aint rendering)
                                                if (!isTopShown)
                                                {
                                                    ShadeFrom sFrom = ShadeFrom.EMPTY;
                                                    bool isBlockTop = y > 0 && isShaded(shadowMap[x, y], shadowMap[x, y - 1]);
                                                    bool isBlockLeft = x > 0 && isShaded(shadowMap[x, y], shadowMap[x - 1, y]);
                                                    bool isBlockRight = x < mWidth - 1 && isShaded(shadowMap[x, y], shadowMap[x + 1, y]);
                                                    bool isBlockBottom = (y < mHeight - 1 && isShaded(shadowMap[x, y], shadowMap[x, y + 1]));
                                                    bool isBlockTopLeft = (y > 0 && x > 0 && isShaded(shadowMap[x, y], shadowMap[x - 1, y - 1]));
                                                    bool isBlockTopRight = (y > 0 && x < mWidth - 1 && isShaded(shadowMap[x, y], shadowMap[x + 1, y - 1]));
                                                    bool isBlockBottomLeft = (y < mHeight - 1 && x > 0 && isShaded(shadowMap[x, y], shadowMap[x - 1, y + 1]));
                                                    bool isBlockBottomRight = (y < mHeight - 1 && x < mWidth - 1 && isShaded(shadowMap[x, y], shadowMap[x + 1, y + 1]));

                                                    if (isBlockTop) sFrom |= ShadeFrom.T;
                                                    if (isBlockLeft) sFrom |= ShadeFrom.L;
                                                    if (isBlockRight) sFrom |= ShadeFrom.R;
                                                    if (isBlockBottom) sFrom |= ShadeFrom.B;
                                                    if (isBlockTopLeft) sFrom |= ShadeFrom.TL;
                                                    if (isBlockTopRight) sFrom |= ShadeFrom.TR;
                                                    if (isBlockBottomLeft) sFrom |= ShadeFrom.BL;
                                                    if (isBlockBottomRight) sFrom |= ShadeFrom.BR;

                                                    var shadeImg = ShadowHelper.GetSpriteIndividual(Constants.TextureSize, sFrom);
                                                    gShadow.DrawImage(image: shadeImg, xi, yi, textureSize.Value, textureSize.Value);

                                                }
                                            }
                                        }
                                    }

                                    brushTransparentCover.Dispose();
                                }

                                gImg.CompositingMode = CompositingMode.SourceOver;
                                gImg.DrawImage(bmShadow, 0, 0, calcW, calcH);
                            }
                        }

                        #endregion
                        brush.DisposeSafely();
                        pen.DisposeSafely();

                    }


                    return bm;
                }
                catch (Exception ex)
                {
                    blueprint = null;
                }
            }

            return null;
        }

        public static Bitmap RenderBitmapFromBlueprintParallel(CancellationToken? worker, BlueprintPA blueprint, out int? textureSize)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            // TODO: Make sure this value is saved to the render panel instance somehow or else there will be horrible issues
            textureSize = CalculateTextureSize(blueprint);
            if (textureSize == null) return null;

            int textureSizeRef = textureSize.Value; // We can't take an out var into a thread

            if (blueprint != null)
            {
                try
                {

                    TaskManager.SafeReport(0, "Preparing canvas for textures");
                    bool isSelectiveLayerViewEnabled = Options.Get.IsEnabled(Constants.RenderedZIndexFilter, false);
                    bool isMaterialFilterViewEnabled = Options.Get.SelectedMaterialFilter.Any();
                    bool isSide = Options.Get.IsSideView;
                    double origW = blueprint.Width;
                    double origH = blueprint.Height;
                    int w = (int)(origW * MainForm.PanZoomSettings.zoomLevel);
                    int h = (int)(origH * MainForm.PanZoomSettings.zoomLevel);
                    int zoom = (int)(MainForm.PanZoomSettings.zoomLevel);

                    SolidBrush brush = new SolidBrush(Color.Black);
                    Pen pen = new Pen(brush);

                    bool isMaterialIncludedInFilter = true;
                    int mWidth = blueprint.Width;
                    int mHeight = blueprint.Height;
                    int mDepth = Options.Get.IsMultiLayer ? 2 : 1;

                    int calcW = mWidth * textureSize.Value;
                    int calcH = mHeight * textureSize.Value;
                    TaskManager.SafeReport(20, "Preparing canvas for textures");

                    Bitmap bm = new Bitmap(
                        width: calcW,
                        height: calcH,
                        format: PixelFormat.Format32bppArgb);

                    TaskManager.SafeReport(50, "Preparing canvas for textures");
                    var selectedMaterials = Options.Get.SelectedMaterialFilter.AsEnumerable().ToList(); // clone
                    bool _IsSolidColors = Options.Get.Rendered_IsSolidColors;
                    bool _IsColorPalette = Options.Get.Rendered_IsColorPalette;
                    bool _IsMultiLayer = Options.Get.IsMultiLayer;
                    bool _isSkipShadowRendering = Options.Get.IsShadowRenderingSkipped;
                    int _RenderedZIndexToShow = Options.Get.Rendered_RenderedZIndexToShow;
                    bool _isFrugalAesthetic = Options.Get.IsFrugalWithMaterials && !selectedMaterials.Any();

                    //Get the bitmap data
                    var bitmapData = bm.LockBits(
                        new Rectangle(0, 0, bm.Width, bm.Height),
                        ImageLockMode.ReadWrite,
                        bm.PixelFormat
                    );

                    // The real smart thing would be to just load every texture they have selected first
                    // Instead of the long struggle I had trying to cache them as they came
                    var materials = Materials.List.Where(m2 => m2.IsEnabled && m2.Category != "Air").Distinct();
                    ConcurrentDictionary<Material, BitmapData> materialDataMap = new ConcurrentDictionary<Material, BitmapData>();

                    foreach (var m in materials)
                    {
                        var image = m.getImage(isSide);
                        var data = image.LockBits(
                        new Rectangle(0, 0, image.Width, image.Height),
                        ImageLockMode.ReadOnly,
                        image.PixelFormat
                        );
                        materialDataMap[m] = data;
                    }

                    unsafe
                    {
                        int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bm.PixelFormat) / 8;
                        int heightInPixels = bitmapData.Height;
                        int widthInBytes = bitmapData.Width * bytesPerPixel;
                        byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                        #region Regular
                        for (int z = 0; z < mDepth; z++)
                        {
                            TaskManager.SafeReport(0, "Applying textures... (Layer " + z + ")");

                            if (isSelectiveLayerViewEnabled)
                            {
                                if (z != _RenderedZIndexToShow)
                                {
                                    continue;
                                }
                            }
                            int numProcessed = 0;
                            int numPixels = mWidth * mHeight;
                            Parallel.For(0, numPixels, pixel =>
                            {
                                int x = pixel % mWidth;
                                int y = pixel / mWidth;
                                TaskManager.SafeReport(100 * numProcessed / numPixels);
                                Interlocked.Increment(ref numProcessed);
                                worker?.SafeThrowIfCancellationRequested();
                                int xi = x * textureSizeRef;
                                int yi = y * textureSizeRef;
                                if (xi + MainForm.PanZoomSettings.zoomLevel >= 0 && yi + MainForm.PanZoomSettings.zoomLevel >= 0)
                                {
                                    Material m = blueprint.GetMaterialAt(x, y, z, !_isFrugalAesthetic);

                                    if (isMaterialFilterViewEnabled)
                                    {
                                        string blockId = m.PixelStackerID;
                                        isMaterialIncludedInFilter = Options.Get.SelectedMaterialFilter.Any(xm => xm == blockId);
                                    }

                                    if (m.BlockID != 0)
                                    {
                                        // Which we use here
                                        int ptrX = xi * bytesPerPixel;
                                        var data = materialDataMap[m];
                                        byte* dataPtrFirstPixel = (byte*)data.Scan0;
                                        byte* currentLineBase = PtrFirstPixel + (yi * bitmapData.Stride);
                                        int bytesPerMaterialPixel = System.Drawing.Bitmap.GetPixelFormatSize(data.PixelFormat) / 8;


                                        if (_IsSolidColors)
                                        {
                                            if (isMaterialIncludedInFilter)
                                            {
                                                var color = blueprint.GetColor(x, y);

                                                //gImg.FillRectangle(brush, xi, yi, textureSize.Value, textureSize.Value);
                                                // Fill rectangle implementation
                                                for (int y1 = 0; y1 < textureSizeRef; y1++)
                                                {
                                                    byte* currentLine = PtrFirstPixel + ((yi + y1) * bitmapData.Stride);
                                                    for (int x1 = 0; x1 < textureSizeRef; x1++)
                                                    {
                                                        currentLine[ptrX + x1 * bytesPerPixel] = color.B;
                                                        currentLine[ptrX + x1 * bytesPerPixel + 1] = color.G;
                                                        currentLine[ptrX + x1 * bytesPerPixel + 2] = color.R;
                                                        if (color.A > 0) // IDK I'm paranoid after all the alpha issues I had
                                                            currentLine[ptrX + x1 * bytesPerPixel + 3] = color.A;
                                                        else
                                                            currentLine[ptrX + x1 * bytesPerPixel + 3] = (byte)255;
                                                    }
                                                }
                                            }
                                        }
                                        else if (_IsColorPalette)
                                        {
                                            if (isMaterialIncludedInFilter)
                                            {
                                                // Per the previous implementation, this one is interesting...
                                                // We need to draw the texture
                                                // Then put a square of the appropriate color from 0,0 to textureSizeRef/2
                                                // Then put a black border around that square
                                                var color = blueprint.GetColor(x, y);

                                                Color targetColor;
                                                for (int y1 = 0; y1 < textureSizeRef; y1++)
                                                {
                                                    byte* currentLine = PtrFirstPixel + ((yi + y1) * bitmapData.Stride);
                                                    byte* currentDataLine = dataPtrFirstPixel + y1 * data.Stride;
                                                    for (int x1 = 0; x1 < textureSizeRef; x1++)
                                                    {
                                                        if (x1 < textureSizeRef / 2 && y1 < textureSizeRef / 2) // If it's in the upper-left quadrant, color
                                                        {
                                                            targetColor = color;
                                                        }
                                                        else if (x1 == 0 || y1 == 0 || x1 == textureSizeRef / 2 || y1 == textureSizeRef / 2) // If it's exactly on a border
                                                        {
                                                            // Black for the border
                                                            targetColor = Color.Black;
                                                        }
                                                        else
                                                        {
                                                            // To prevent storing this in a color and back out again, we're going to duplicate the code a bit
                                                            // It doesn't save us a lot, but it's not nothing, and it costs nothing
                                                            var dataPtrX = x1 * bytesPerPixel;
                                                            currentLine[ptrX + x1 * bytesPerPixel] = currentDataLine[dataPtrX];
                                                            currentLine[ptrX + x1 * bytesPerPixel + 1] = currentDataLine[dataPtrX + 1];
                                                            currentLine[ptrX + x1 * bytesPerPixel + 2] = currentDataLine[dataPtrX + 2];
                                                            if(data.PixelFormat.HasAlpha())
                                                                currentLine[ptrX + x1 * bytesPerPixel + 3] = currentDataLine[dataPtrX + 2];
                                                            else
                                                                currentLine[ptrX + x1 * bytesPerPixel + 3] = (byte)255;
                                                            // Note that we can't use our nice func to draw this material because it's a custom one
                                                            // And it's faster to not draw the entire material if it would be occluded anyway
                                                            // We're also skipping any blending because there shouldn't be anything drawn under these.  I think.
                                                        }
                                                        currentLine[ptrX + x1 * bytesPerPixel] = color.B;
                                                        currentLine[ptrX + x1 * bytesPerPixel + 1] = color.G;
                                                        currentLine[ptrX + x1 * bytesPerPixel + 2] = color.R;
                                                        currentLine[ptrX + x1 * bytesPerPixel + 3] = color.A;
                                                    }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            if (isMaterialIncludedInFilter)
                                            {
                                                DrawImage(data, bitmapData, xi, yi, textureSizeRef, bytesPerPixel);
                                            }
                                        }
                                    }
                                }


                            });
                        }
                        #endregion
                        // We'll unlock here and let shadows be their own thing still
                        bm.UnlockBits(bitmapData);
                        foreach (var m in materials)
                        {
                            m.getImage(isSide).UnlockBits(materialDataMap[m]);
                        }
                        watch.Stop();
                        Console.WriteLine($"Rendered bitmap from blueprint in {watch.ElapsedMilliseconds}ms");

                        using (Graphics gImg = Graphics.FromImage(bm))
                        {
                            #region SHADOW_NEW
                            if (!_isSkipShadowRendering)
                            {
                                Bitmap bmShadeSprites = ShadowHelper.GetSpriteSheet(Constants.TextureSize);

                                Bitmap bmShadow = new Bitmap(
                                width: calcW,
                                height: calcH,
                                format: PixelFormat.Format32bppArgb);

                                byte[,] shadowMap = new byte[mWidth, mHeight];
                                {
                                    #region Initialize shadow map (booleans basically)
                                    TaskManager.SafeReport(0, "Calculating shadow placement map");
                                    for (int xShadeMap = 0; xShadeMap < mWidth; xShadeMap++)
                                    {
                                        TaskManager.SafeReport(100 * xShadeMap / mWidth);
                                        worker?.SafeThrowIfCancellationRequested();

                                        for (int yShadeMap = 0; yShadeMap < mHeight; yShadeMap++)
                                        {
                                            Material mBottom = blueprint.GetMaterialAt(xShadeMap, yShadeMap, 0, true);
                                            bool isBottomShown = mBottom.BlockID != 0 && (selectedMaterials.Count == 0 || selectedMaterials.Any(xm => xm == mBottom.PixelStackerID));

                                            Material mTop = blueprint.GetMaterialAt(xShadeMap, yShadeMap, 1, !_isFrugalAesthetic);
                                            bool isTopShown = mTop.BlockID != 0 && (selectedMaterials.Count == 0 || selectedMaterials.Any(xm => xm == mTop.PixelStackerID));

                                            if (isTopShown && isBottomShown)
                                            {
                                                shadowMap[xShadeMap, yShadeMap] = SHOWN_TOP_AND_BOTTOM;
                                            }
                                            else if (isTopShown)
                                            {
                                                shadowMap[xShadeMap, yShadeMap] = SHOWN_TOP;
                                            }
                                            else if (isBottomShown)
                                            {
                                                shadowMap[xShadeMap, yShadeMap] = SHOWN_BOTTOM;
                                            }
                                            else
                                            {
                                                shadowMap[xShadeMap, yShadeMap] = SHOWN_NONE;
                                            }
                                        }
                                    }
                                    #endregion

                                    using (Graphics gShadow = Graphics.FromImage(bmShadow))
                                    {
                                        gShadow.CompositingMode = CompositingMode.SourceOver; // over is slower but better...
                                        gShadow.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                                        gShadow.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                                        gShadow.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

                                        var brushTransparentCover = new SolidBrush(Color.FromArgb(40, 127, 127, 127));
                                        {
                                            TaskManager.SafeReport(0, "Rendering shadows");
                                            for (int x = 0; x < mWidth; x++)
                                            {
                                                TaskManager.SafeReport(100 * x / mWidth);
                                                worker?.SafeThrowIfCancellationRequested();
                                                for (int y = 0; y < mHeight; y++)
                                                {
                                                    int xi = x * textureSize.Value;
                                                    int yi = y * textureSize.Value;

                                                    bool isTopShown = shadowMap[x, y] == SHOWN_TOP || shadowMap[x, y] == SHOWN_TOP_AND_BOTTOM;
                                                    bool isBottomShown = shadowMap[x, y] == SHOWN_BOTTOM || shadowMap[x, y] == SHOWN_TOP_AND_BOTTOM;
                                                    bool isBottomCoveredByInvisibleTop = isBottomShown && !isTopShown;

                                                    // The thing that makes it slightly less saturated on bottom layer
                                                    if (isBottomCoveredByInvisibleTop && _IsMultiLayer)
                                                    {
                                                        gShadow.FillRectangle(brushTransparentCover, xi, yi, textureSize.Value, textureSize.Value);
                                                    }

                                                    if (isTopShown && isBottomShown)
                                                    {
                                                        continue; // No shade required
                                                    }

                                                    // AIR block (or block we aint rendering)
                                                    if (!isTopShown)
                                                    {
                                                        ShadeFrom sFrom = ShadeFrom.EMPTY;
                                                        bool isBlockTop = y > 0 && isShaded(shadowMap[x, y], shadowMap[x, y - 1]);
                                                        bool isBlockLeft = x > 0 && isShaded(shadowMap[x, y], shadowMap[x - 1, y]);
                                                        bool isBlockRight = x < mWidth - 1 && isShaded(shadowMap[x, y], shadowMap[x + 1, y]);
                                                        bool isBlockBottom = (y < mHeight - 1 && isShaded(shadowMap[x, y], shadowMap[x, y + 1]));
                                                        bool isBlockTopLeft = (y > 0 && x > 0 && isShaded(shadowMap[x, y], shadowMap[x - 1, y - 1]));
                                                        bool isBlockTopRight = (y > 0 && x < mWidth - 1 && isShaded(shadowMap[x, y], shadowMap[x + 1, y - 1]));
                                                        bool isBlockBottomLeft = (y < mHeight - 1 && x > 0 && isShaded(shadowMap[x, y], shadowMap[x - 1, y + 1]));
                                                        bool isBlockBottomRight = (y < mHeight - 1 && x < mWidth - 1 && isShaded(shadowMap[x, y], shadowMap[x + 1, y + 1]));

                                                        if (isBlockTop) sFrom |= ShadeFrom.T;
                                                        if (isBlockLeft) sFrom |= ShadeFrom.L;
                                                        if (isBlockRight) sFrom |= ShadeFrom.R;
                                                        if (isBlockBottom) sFrom |= ShadeFrom.B;
                                                        if (isBlockTopLeft) sFrom |= ShadeFrom.TL;
                                                        if (isBlockTopRight) sFrom |= ShadeFrom.TR;
                                                        if (isBlockBottomLeft) sFrom |= ShadeFrom.BL;
                                                        if (isBlockBottomRight) sFrom |= ShadeFrom.BR;

                                                        var shadeImg = ShadowHelper.GetSpriteIndividual(Constants.TextureSize, sFrom);
                                                        gShadow.DrawImage(image: shadeImg, xi, yi, textureSize.Value, textureSize.Value);

                                                    }
                                                }
                                            }
                                        }

                                        brushTransparentCover.Dispose();
                                    }

                                    gImg.CompositingMode = CompositingMode.SourceOver;
                                    gImg.DrawImage(bmShadow, 0, 0, calcW, calcH);
                                }
                            }
                        }

                        #endregion
                        brush.DisposeSafely();
                        pen.DisposeSafely();

                    }

                    return bm;
                }
                catch (Exception ex)
                {
                    blueprint = null;
                }
            }

            return null;
        }

        /// <summary>
        /// Draws a material onto target at the X,Y coordinates
        /// Wants textureSize and bytesPerPixel to avoid re-querying them unnecessarily
        /// And same for the data, you should lock and expect to call this multiple times within a lock
        /// </summary>
        private static unsafe void DrawImage(BitmapData materialData, BitmapData targetBitmapData, int x, int y, int textureSize, int bytesPerPixel)
        {
            x *= bytesPerPixel; // Get x in reference to bytes
            byte* PtrFirstPixel = (byte*)targetBitmapData.Scan0;
            byte* dataPtrFirstPixel = (byte*)materialData.Scan0;
            byte* currentLineBase = PtrFirstPixel + (y * targetBitmapData.Stride);
            int bytesPerMaterialPixel = System.Drawing.Bitmap.GetPixelFormatSize(materialData.PixelFormat) / 8;
            bool hasAlpha = materialData.PixelFormat.HasAlpha(); // This is a method so... better cache the result cuz it may be computational
            for (int y1 = 0; y1 < textureSize; y1++)
            {
                byte* currentLine = PtrFirstPixel + ((y + y1) * targetBitmapData.Stride);
                byte* currentDataLine = dataPtrFirstPixel + y1 * materialData.Stride;
                for (int x1 = 0; x1 < textureSize; x1++)
                {
                    var dataPtrX = x1 * bytesPerMaterialPixel;

                    int alpha = currentDataLine[dataPtrX + 3];
                    if (hasAlpha && alpha != 255) // Alpha blend
                    {
                        // out = alpha * new + (1 - alpha) * old
                        // I'm not sure if storing these has any benefit except to make the formula slightly cleaner
                        // But they're just bytes, they shouldn't affect the overhead much
                        var B = currentLine[x + x1 * bytesPerPixel];
                        var G = currentLine[x + x1 * bytesPerPixel + 1];
                        var R = currentLine[x + x1 * bytesPerPixel + 2];
                        var A = currentLine[x + x1 * bytesPerPixel + 3];
                        double normalAlpha = alpha / 255d;
                        currentLine[x + x1 * bytesPerPixel] = (byte)(normalAlpha * currentDataLine[dataPtrX] + (1 - normalAlpha) * B);
                        currentLine[x + x1 * bytesPerPixel + 1] = (byte)(normalAlpha * currentDataLine[dataPtrX + 1] + (1 - normalAlpha) * G);
                        currentLine[x + x1 * bytesPerPixel + 2] = (byte)(normalAlpha * currentDataLine[dataPtrX + 2] + (1 - normalAlpha) * R);
                        currentLine[x + x1 * bytesPerPixel + 3] = (byte)A; // Our resulting alpha should always be the original, we're blending it ourselves
                    }
                    else // These are separated so that we don't have to read current value in the more common case that it has no alpha
                    {
                        currentLine[x + x1 * bytesPerPixel] = currentDataLine[dataPtrX];
                        currentLine[x + x1 * bytesPerPixel + 1] = currentDataLine[dataPtrX + 1];
                        currentLine[x + x1 * bytesPerPixel + 2] = currentDataLine[dataPtrX + 2];
                        currentLine[x + x1 * bytesPerPixel + 3] = (byte)255;
                    }
                }
            }
        }

        private static int? CalculateTextureSize(BlueprintPA image)
        {
            // Calculate texture size so we can handle large images.
            if (image == null) return null;

            int calculatedTextureSize = Constants.TextureSize;
            int bytesInSrcImage = (image.Width * image.Height * 32 / 8); // Still need to multiply by texture size (4 bytes per pixel / 8 bits per byte = 4 bytes)

            int safetyMultiplier = 2; // Want to be able to store N of these things in memory

            bool isSuccess = false;

            do
            {
                if (image.Width * calculatedTextureSize >= 30000 || image.Height * calculatedTextureSize >= 30000)
                {
                    calculatedTextureSize = Math.Max(1, calculatedTextureSize - 2);
                    continue;
                }

                int totalPixels = (image.Width + 1) * image.Height * calculatedTextureSize * calculatedTextureSize * 4;
                if (totalPixels >= int.MaxValue || totalPixels < 0)
                {
                    calculatedTextureSize = Math.Max(1, calculatedTextureSize - 2);
                    continue;
                }

                try
                {
                    int numMegaBytes = bytesInSrcImage // pixels in base image * bytes per pixel
                        * calculatedTextureSize * calculatedTextureSize // size of texture tile squared 
                        / 1024 / 1024       // convert to MB
                        * safetyMultiplier  // Multiply by safety buffer to plan for a bunch of these layers.
                        ;

                    if (numMegaBytes > 0)
                    {
                        using (var memoryCheck = new System.Runtime.MemoryFailPoint(numMegaBytes))
                        {
                        }
                    }

                    isSuccess = true;
                }
                catch (InsufficientMemoryException)
                {
                    calculatedTextureSize = Math.Max(1, calculatedTextureSize - 2);
                }
            } while (isSuccess == false && calculatedTextureSize > 1);

            if (!isSuccess)
            {
                MessageBox.Show(Constants.ERR_DownsizeYourImage);
                MainForm.Self.PreRenderedImage.DisposeSafely();
                MainForm.Self.PreRenderedImage = null;
                return null;
            }

            return calculatedTextureSize;
        }

        /// <summary>
        /// Sets rendered images to null also disposes all images then clears the list.
        /// </summary>
        private void ClearAndDisposeRenderedImages()
        {
            var toClear = this.renderedImages;
            this.renderedImages = null;
            toClear.ForEach(x => x.DisposeSafely());
            toClear.Clear();
        }


        public void SetBluePrint(BlueprintPA src, Bitmap renderedImage, int? textureSize)
        {
            this.image = src;

            #region Calculate size
            this.CalculatedTextureSize = textureSize ?? Constants.TextureSize;
            this.ClearAndDisposeRenderedImages();
            TaskManager.SafeReport(100, "Creating large image tiles.");
            var images = new List<Bitmap>();
            {
                int w = renderedImage.Width;
                int h = renderedImage.Height;
                int a = w * h;
                images.Add(renderedImage);

                while (a > Constants.BIG_IMG_MAX_AREA_B4_SPLIT)
                {
                    w /= Constants.SMALL_IMAGE_DIVIDE_SIZE;
                    h /= Constants.SMALL_IMAGE_DIVIDE_SIZE;
                    if (w == 0 || h == 0) break;
                    a = w * h;
                    renderedImage = renderedImage.To32bppBitmap(w, h);
                    images.Add(renderedImage);
                }
            }

            this.renderedImages = images; // le quick swap
            #endregion

            bool preserveZoom = (MainForm.PanZoomSettings != null);
            if (!preserveZoom)
            {
                var settings = new PanZoomSettings()
                {
                    initialImageX = 0,
                    initialImageY = 0,
                    imageX = 0,
                    imageY = 0,
                    zoomLevel = 0,
                    maxZoomLevel = Constants.MAX_ZOOM,
                    minZoomLevel = Constants.MIN_ZOOM
                };

                int mWidth = src.Width;
                int mHeight = src.Height;

                double wRatio = (double)Width / mWidth;
                double hRatio = (double)Height / mHeight;
                if (hRatio < wRatio)
                {
                    settings.zoomLevel = hRatio;
                    settings.imageX = (Width - (int)(mWidth * hRatio)) / 2;
                }
                else
                {
                    settings.zoomLevel = wRatio;
                    settings.imageY = (Height - (int)(mHeight * wRatio)) / 2;
                }

                int numICareAbout = Math.Max(mWidth, mHeight);
                settings.minZoomLevel = (100.0D / numICareAbout);
                if (settings.minZoomLevel > 1.0D)
                {
                    settings.minZoomLevel = 1.0D;
                }

                MainForm.PanZoomSettings = settings;
            }

            TaskManager.SafeReport(100, "Image tiling complete. Blueprint set.");
            Refresh();
        }

        public void SaveToPNG(string filename)
        {
            if (this.renderedImages == null || this.renderedImages.Count == 0 || this.image == null)
            {
                return;
            }

            bool isCompact = Options.Get.Rendered_IsSolidColors && !Options.Get.Rendered_IsShowGrid && !Options.Get.Rendered_IsColorPalette;
            int blockWidth = isCompact ? 1 : CalculatedTextureSize;
            var renderedImage = this.renderedImages[0];
            int W = renderedImage.Width; if (isCompact) { W /= CalculatedTextureSize; }
            int H = renderedImage.Height; if (isCompact) { H /= CalculatedTextureSize; }
            using (Bitmap bm = new Bitmap(W, H, PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bm))
                {
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.SmoothingMode = SmoothingMode.None;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

                    g.DrawImage(image: renderedImage,
                        x: 0, y: 0,
                        width: W,
                        height: H);

                    using (SolidBrush brush = new SolidBrush(Color.Black))
                    {
                        using (Pen pen = new Pen(brush))
                        {
                            if (Options.Get.Rendered_IsShowBorder)
                            {
                                int indexShift = isCompact ? 1 : 0;
                                Point pp2 = new Point(indexShift, indexShift);
                                using (Pen penWE = new Pen(Color.FromArgb(127, 0, 0, 0), blockWidth))
                                {
                                    penWE.Alignment = PenAlignment.Inset;
                                    g.DrawRectangle(penWE, pp2.X, pp2.Y, (int)(bm.Width) - indexShift, (int)(bm.Height) - indexShift);
                                }
                            }

                            if (Constants.IsFullVersion)
                            {
                                brush.Color = Color.Red;
                                Point wePoint = getPointOnPanel(image.WorldEditOrigin);
                                g.FillRectangle(brush, image.WorldEditOrigin.X * blockWidth, image.WorldEditOrigin.Y * blockWidth, blockWidth, blockWidth);
                            }

                            if ((Options.Get.Rendered_IsShowGrid) && (MainForm.PanZoomSettings.zoomLevel >= 0.5D))
                            {
                                // This doesn't work because it actually only works on the current viewing window.
                                //drawGridMask(g, Options.Get.GridSize, Options.Get.GridColor);
                                if (this.gridMaskClip != null)
                                {
                                    Point tL = new Point(this.gridMaskClip.Value.Left * blockWidth, this.gridMaskClip.Value.Top * blockWidth);
                                    Point bR = new Point(this.gridMaskClip.Value.Right * blockWidth, this.gridMaskClip.Value.Bottom * blockWidth);
                                    g.ExcludeClip(new Rectangle(tL, tL.CalculateSize(bR)));
                                    using (Pen p = new Pen(Color.FromArgb(128, Options.Get.GridColor)))
                                    {
                                        g.FillRectangle(p.Brush, 0, 0, bm.Width, bm.Height);
                                    }
                                    g.ResetClip();
                                }

                                int gridSize = Options.Get.GridSize;
                                int gridWidth = GetGridWidth();
                                int numHorizBlocks = (this.image.Width / gridSize);
                                int numVertBlocks = (this.image.Height / gridSize);
                                using (Pen p = new Pen(Color.Black, gridWidth))
                                {
                                    p.Alignment = PenAlignment.Inset;

                                    g.DrawLine(p, 0, 0, bm.Width, 0);
                                    g.DrawLine(p, 0, bm.Height, bm.Width, bm.Height);
                                    g.DrawLine(p, 0, 0, 0, bm.Height);
                                    g.DrawLine(p, bm.Width, 0, bm.Width, bm.Height);

                                    for (int x = 0; x < numHorizBlocks; x++)
                                    {
                                        g.DrawLine(p, x * gridSize * CalculatedTextureSize, 0, x * gridSize * CalculatedTextureSize, bm.Height * CalculatedTextureSize);
                                    }

                                    for (int y = 0; y < numVertBlocks; y++)
                                    {
                                        g.DrawLine(p, 0, y * gridSize * CalculatedTextureSize, bm.Width, y * gridSize * CalculatedTextureSize);
                                    }
                                }

                                gridWidth = 2;
                                using (Pen p = new Pen(Color.FromArgb(40, 0, 0, 0), gridWidth))
                                {
                                    p.Alignment = PenAlignment.Inset;

                                    for (int x = 0; x < bm.Width / CalculatedTextureSize; x++)
                                    {
                                        g.DrawLine(p, x * CalculatedTextureSize, 0, x * CalculatedTextureSize, bm.Height);
                                    }

                                    for (int y = 0; y < bm.Height / CalculatedTextureSize; y++)
                                    {
                                        g.DrawLine(p, 0, y * CalculatedTextureSize, bm.Width, y * CalculatedTextureSize);
                                    }
                                }
                            }
                        }
                    }
                }

                bm.Save(filename, ImageFormat.Png);
            }
        }

        private int GetGridWidth()
        {
            int zoom = (int)MainForm.PanZoomSettings.zoomLevel;
            if (zoom > 70) return 8;
            if (zoom > 60) return 7;
            if (zoom > 50) return 6;
            if (zoom > 35) return 5;
            if (zoom > 25) return 4;
            if (zoom > 15) return 3;
            if (zoom > 8) return 2;
            if (zoom >= 0) return 1;
            return 1;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            double zoom = (MainForm.PanZoomSettings?.zoomLevel ?? 1);
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.SmoothingMode = SmoothingMode.None;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            var blueprint = this.image;

            if (blueprint != null && MainForm.PanZoomSettings != null)
            {
                if (MainForm.PanZoomSettings.zoomLevel < 1.0D)
                {
                    g.InterpolationMode = InterpolationMode.Low;
                    g.CompositingQuality = CompositingQuality.HighSpeed;
                    g.SmoothingMode = SmoothingMode.HighSpeed;
                }
                else
                {
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.CompositingQuality = CompositingQuality.HighSpeed;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                }

                SolidBrush brush = new SolidBrush(Color.Black);

                if (this.renderedImages != null && this.renderedImages.Count > 0 && MainForm.PanZoomSettings != null)
                {
                    Bitmap toUse = this.renderedImages[0];
                    int divideAmount = 1;
                    int i = 1;
                    while (zoom <= 10.0D / divideAmount / Constants.SMALL_IMAGE_DIVIDE_SIZE && i < this.renderedImages.Count)
                    {
                        toUse = this.renderedImages[i];
                        divideAmount *= Constants.SMALL_IMAGE_DIVIDE_SIZE;
                        i++;
                    }

                    Point pStart = getPointOnImage(new Point(0, 0), EstimateProp.Floor);
                    Point fStart = getPointOnPanel(pStart);
                    pStart.X *= CalculatedTextureSize; pStart.X /= divideAmount;
                    pStart.Y *= CalculatedTextureSize; pStart.Y /= divideAmount;

                    Point pEnd = getPointOnImage(new Point(this.Width, this.Height), EstimateProp.Ceil);
                    Point fEnd = getPointOnPanel(pEnd);
                    pEnd.X *= CalculatedTextureSize; pEnd.X /= divideAmount;
                    pEnd.Y *= CalculatedTextureSize; pEnd.Y /= divideAmount;

                    Rectangle rectSRC = new Rectangle(pStart, pStart.CalculateSize(pEnd));
                    Rectangle rectDST = new Rectangle(fStart, fStart.CalculateSize(fEnd));

                    try
                    {
                        int numMegaBytes = 32 / 8 * CalculatedTextureSize * CalculatedTextureSize * image.Height * image.Width / 1024 / 1024;
                        if (numMegaBytes > 0)
                        {
                            using (var memoryCheck = new System.Runtime.MemoryFailPoint(numMegaBytes))
                            {
                                g.DrawImage(image: toUse,
                                    srcRect: rectSRC,
                                    destRect: rectDST,
                                    srcUnit: GraphicsUnit.Pixel);
                            }
                        }
                        else if ((image.Height * image.Width) < 2000)
                        {
                            g.DrawImage(image: toUse,
                                srcRect: rectSRC,
                                destRect: rectDST,
                                srcUnit: GraphicsUnit.Pixel);
                        }
                    }
                    catch (InsufficientMemoryException)
                    {
                        MainForm.Self.PreRenderedImage.DisposeSafely();
                        MainForm.Self.PreRenderedImage = null;
                        CalculatedTextureSize = Math.Max(1, CalculatedTextureSize - 2);
                        if (CalculatedTextureSize <= 2)
                        {
                            MessageBox.Show("Not enough memory. Please try again or reduce image size if problem continues.");
                        }
                        else
                        {
                            bool dummyBool = ForceReRender().Result;
                        }
                        return;
                    }
                    catch (System.InvalidOperationException)
                    {
                        MainForm.Self.PreRenderedImage.DisposeSafely();
                        MainForm.Self.PreRenderedImage = null;
                        return;
                    }
                }

                if (Options.Get.Rendered_IsShowBorder)
                {
                    if (MainForm.PanZoomSettings != null)
                    {
                        Point pp2 = getPointOnPanel(new Point(0, 0));
                        using (Pen penWE = new Pen(Color.FromArgb(127, 0, 0, 0), (int)zoom))
                        {
                            penWE.Alignment = PenAlignment.Inset;
                            g.DrawRectangle(penWE, pp2.X, pp2.Y, (int)(blueprint.Width * MainForm.PanZoomSettings.zoomLevel), (int)(blueprint.Height * MainForm.PanZoomSettings.zoomLevel));
                        }
                    }
                }

                if (Constants.IsFullVersion)
                {
                    brush.Color = Color.Red;
                    if (MainForm.PanZoomSettings != null)
                    {
                        Point wePoint = getPointOnPanel(image.WorldEditOrigin);
                        g.FillRectangle(brush, wePoint.X, wePoint.Y, (int)zoom + 1, (int)zoom + 1);
                    }
                }

                if ((Options.Get.Rendered_IsShowGrid) && (MainForm.PanZoomSettings.zoomLevel >= 0.5D))
                {
                    drawGridMask(g, Options.Get.GridSize, Options.Get.GridColor);
                    drawGrid(g, Options.Get.GridSize, Options.Get.GridColor);
                    if (MainForm.PanZoomSettings.zoomLevel > 5) drawGrid(g, 1, Color.FromArgb(40, Options.Get.GridColor));
                }
            }
        }

        #region Mouse Events
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Delta != 0)
            {
                Point panelPoint = e.Location;
                Point imagePoint = this.getPointOnImage(panelPoint, EstimateProp.Round);
                if (e.Delta < 0)
                {
                    MainForm.PanZoomSettings.zoomLevel *= 0.65;
                }
                else
                {
                    MainForm.PanZoomSettings.zoomLevel *= 1.50;
                }
                this.restrictZoom();
                MainForm.PanZoomSettings.imageX = ((int)Math.Round(panelPoint.X - imagePoint.X * MainForm.PanZoomSettings.zoomLevel));
                MainForm.PanZoomSettings.imageY = ((int)Math.Round(panelPoint.Y - imagePoint.Y * MainForm.PanZoomSettings.zoomLevel));
                this.Refresh();
            }
        }

        private void ImagePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.initialDragPoint = e.Location;
                MainForm.PanZoomSettings.initialImageX = MainForm.PanZoomSettings.imageX;
                MainForm.PanZoomSettings.initialImageY = MainForm.PanZoomSettings.imageY;
                this.Cursor = new Cursor(UIResources.cursor_handclosed.GetHicon());
                this.IsDragging = true;
            }
        }

        private void ImagePanel_MouseUp(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            this.IsDragging = false;
        }

        private void ImagePanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDragging)
            {
                //EditorPanel.this.fitToSize = false;
                Point point = e.Location;
                MainForm.PanZoomSettings.imageX = MainForm.PanZoomSettings.initialImageX - (this.initialDragPoint.X - point.X);
                MainForm.PanZoomSettings.imageY = MainForm.PanZoomSettings.initialImageY - (this.initialDragPoint.Y - point.Y);
            }
            Refresh();
        }
        #endregion 

        public Point getPointOnImage(Point pointOnPanel, EstimateProp prop)
        {
            if (prop == EstimateProp.Ceil)
            {
                return new Point((int)Math.Ceiling((pointOnPanel.X - MainForm.PanZoomSettings.imageX) / MainForm.PanZoomSettings.zoomLevel), (int)Math.Ceiling((pointOnPanel.Y - MainForm.PanZoomSettings.imageY) / MainForm.PanZoomSettings.zoomLevel));
            }
            if (prop == EstimateProp.Floor)
            {
                return new Point((int)Math.Floor((pointOnPanel.X - MainForm.PanZoomSettings.imageX) / MainForm.PanZoomSettings.zoomLevel), (int)Math.Floor((pointOnPanel.Y - MainForm.PanZoomSettings.imageY) / MainForm.PanZoomSettings.zoomLevel));
            }
            return new Point((int)Math.Round((pointOnPanel.X - MainForm.PanZoomSettings.imageX) / MainForm.PanZoomSettings.zoomLevel), (int)Math.Round((pointOnPanel.Y - MainForm.PanZoomSettings.imageY) / MainForm.PanZoomSettings.zoomLevel));
        }

        public Point getPointOnPanel(Point pointOnImage)
        {
            if (pointOnImage == null)
            {
#if !RELEASE
                throw new ArgumentNullException("pointOnImage is null. Please report this to Pangamma along with the stacktrace!");
#else
                return new Point(0, 0);
#endif
            }

            if (MainForm.PanZoomSettings == null)
            {
#if !RELEASE
                throw new ArgumentNullException("PanZoomSettings are not set. So weird!");
#else
                return new Point(0, 0);
#endif
            }

            return new Point((int)Math.Round(pointOnImage.X * MainForm.PanZoomSettings.zoomLevel + MainForm.PanZoomSettings.imageX), (int)Math.Round(pointOnImage.Y * MainForm.PanZoomSettings.zoomLevel + MainForm.PanZoomSettings.imageY));
        }

        private void restrictZoom()
        {
            MainForm.PanZoomSettings.zoomLevel = (MainForm.PanZoomSettings.zoomLevel < MainForm.PanZoomSettings.minZoomLevel ? MainForm.PanZoomSettings.minZoomLevel : MainForm.PanZoomSettings.zoomLevel > MainForm.PanZoomSettings.maxZoomLevel ? MainForm.PanZoomSettings.maxZoomLevel : MainForm.PanZoomSettings.zoomLevel);
        }

        private void drawGridMask(Graphics g, int gridSize, Color c)
        {
            if (this.gridMaskClip == null) return;
            if (MainForm.PanZoomSettings == null) return;
            Point tL = new Point(this.gridMaskClip.Value.Left, this.gridMaskClip.Value.Top);
            Point bR = new Point(this.gridMaskClip.Value.Right, this.gridMaskClip.Value.Bottom);
            tL = this.getPointOnPanel(tL);
            bR = this.getPointOnPanel(bR);
            g.ExcludeClip(new Rectangle(tL, tL.CalculateSize(bR)));
            using (Pen p = new Pen(Color.FromArgb(128, Options.Get.GridColor)))
            {
                g.FillRectangle(p.Brush, 0, 0, this.Width, this.Height);
            }
            g.ResetClip();
        }

        private void drawGrid(Graphics g, int gridSize, Color c)
        {
            int numHorizBlocks = (this.image.Width / gridSize);
            int numVertBlocks = (this.image.Height / gridSize);
            Pen p = new Pen(c, gridSize == 1 ? 2 : GetGridWidth());
            g.DrawLine(p, MainForm.PanZoomSettings.imageX, MainForm.PanZoomSettings.imageY, MainForm.PanZoomSettings.imageX, getRoundedZoomDistance(MainForm.PanZoomSettings.imageY, this.image.Height));
            g.DrawLine(p, MainForm.PanZoomSettings.imageX, getRoundedZoomDistance(MainForm.PanZoomSettings.imageY, this.image.Height), getRoundedZoomDistance(MainForm.PanZoomSettings.imageX, this.image.Width), getRoundedZoomDistance(MainForm.PanZoomSettings.imageY, this.image.Height));
            g.DrawLine(p, getRoundedZoomDistance(MainForm.PanZoomSettings.imageX, this.image.Width), getRoundedZoomDistance(MainForm.PanZoomSettings.imageY, this.image.Height), getRoundedZoomDistance(MainForm.PanZoomSettings.imageX, this.image.Width), MainForm.PanZoomSettings.imageY);
            g.DrawLine(p, getRoundedZoomDistance(MainForm.PanZoomSettings.imageX, this.image.Width), MainForm.PanZoomSettings.imageY, MainForm.PanZoomSettings.imageX, MainForm.PanZoomSettings.imageY);
            for (int x = 0; x <= numHorizBlocks; x++)
            {
                g.DrawLine(p, getRoundedZoomX(x, gridSize), MainForm.PanZoomSettings.imageY, getRoundedZoomX(x, gridSize), getRoundedZoomDistance(MainForm.PanZoomSettings.imageY, this.image.Height));
            }
            for (int y = 0; y <= numVertBlocks; y++)
            {
                g.DrawLine(p, MainForm.PanZoomSettings.imageX, getRoundedZoomY(y, gridSize), getRoundedZoomDistance(MainForm.PanZoomSettings.imageX, this.image.Width), getRoundedZoomY(y, gridSize));
            }
            p.Dispose();
        }

        private Point getShowingStart()
        {
            Point showingStart = getPointOnImage(new Point(0, 0), EstimateProp.Floor);
            showingStart.X = (showingStart.X < 0 ? 0 : showingStart.X);
            showingStart.Y = (showingStart.Y < 0 ? 0 : showingStart.Y);
            return showingStart;
        }

        private Point getShowingEnd(double origW, double origH)
        {
            Point showingEnd = getPointOnImage(new Point(Width, Height), EstimateProp.Ceil);
            showingEnd.X = (showingEnd.X > origW ? (int)origW : showingEnd.X);
            showingEnd.Y = (showingEnd.Y > origH ? (int)origH : showingEnd.Y);
            return showingEnd;
        }


        private int getRoundedZoomDistance(int x, int deltaX)
        {
            return (int)Math.Round(x + deltaX * MainForm.PanZoomSettings.zoomLevel);
        }

        private int getRoundedZoomX(int val, int blockSize)
        {
            return (int)Math.Floor(MainForm.PanZoomSettings.imageX + val * blockSize * MainForm.PanZoomSettings.zoomLevel);
        }

        private int getRoundedZoomY(int val, int blockSize)
        {
            return (int)Math.Floor(MainForm.PanZoomSettings.imageY + val * blockSize * MainForm.PanZoomSettings.zoomLevel);
        }

        public enum EstimateProp
        {
            Floor, Ceil, Round
        }

        private void RenderedImagePanel_DoubleClick(object sender, MouseEventArgs e)
        {
            if (Constants.IsFullVersion)
            {
                if (e.Button == MouseButtons.Left)
                {
                    RenderedImagePanel panel = (RenderedImagePanel)sender;
                    Point loc = getPointOnImage(e.Location, EstimateProp.Floor);
                    image.WorldEditOrigin = loc;
                    Refresh();
                }
            }
        }

        private void RenderedImagePanel_MaterialFilter_Remove_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                var toolMenuItem = sender as ToolStripMenuItem;
                bool isOp1 = toolMenuItem.Name == nameof(btnRemoveMat0_Filter);
                string matLabel = isOp1 ? this.materialToAddOrRemove_1 : this.materialToAddOrRemove_2;

                if (Options.Get.SelectedMaterialFilter.Count == 0)
                {
                    var items = new List<string>();
                    items.AddRange(Materials.List.Where(x => x.BlockID != 0).Select(x => x.PixelStackerID));
                    Options.Get.SelectedMaterialFilter = items;
                }

                Options.Get.SelectedMaterialFilter = Options.Get.SelectedMaterialFilter.Except(new List<string>() { matLabel }).ToList();

                Options.Save();
                this.InvokeEx(c => c.ForceReRender());
            }
        }

        private void RenderedImagePanel_MaterialFilter_Add_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                var toolMenuItem = sender as ToolStripMenuItem;
                bool isOp1 = toolMenuItem.Name == nameof(btnAddMat0_Filter);
                string matLabel = isOp1 ? this.materialToAddOrRemove_1 : this.materialToAddOrRemove_2;

                // use concat incase some thing somewhere is iterating over the list. This way we
                // avoid concurrent modification issues.
                Options.Get.SelectedMaterialFilter = Options.Get.SelectedMaterialFilter
                    .Concat(new List<string>() { matLabel }).ToList();

                Options.Save();
                this.InvokeEx(c => c.ForceReRender());
            }
        }



        private void RenderedImagePanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                Point loc = getPointOnImage(e.Location, EstimateProp.Floor);
                int gs = Options.Get.GridSize;
                int xMin = (loc.X / gs) * gs;
                int xMax = xMin + gs;
                int yMin = (loc.Y / gs) * gs;
                int yMax = yMin + gs;
                var newGridMaskClip = new Rectangle(new Point(xMin, yMin), new Point(xMin, yMin).CalculateSize(new Point(xMax, yMax)));

                // If they click it again, we disable it.
                if (this.gridMaskClip != null)
                {
                    if (this.gridMaskClip.Value.X == newGridMaskClip.X)
                    {
                        if (this.gridMaskClip.Value.Y == newGridMaskClip.Y)
                        {
                            this.gridMaskClip = null;
                            Refresh();
                            return;
                        }
                    }
                }

                this.gridMaskClip = newGridMaskClip;
                Refresh();
            }
            else if (e.Button == MouseButtons.Right)
            {
                Point loc = getPointOnImage(e.Location, EstimateProp.Floor);
                if (loc.X < 0 || loc.Y < 0 || loc.X > this.image.BlocksMap.GetLength(0) - 1 || loc.Y > this.image.BlocksMap.GetLength(1) - 1)
                {
                    return; // out of range
                }
                Color cFromPreRender = MainForm.Self.PreRenderedImage.GetPixelSafely(loc.X, loc.Y);
                Color cFromBlueprint = this.image.GetColor(loc.X, loc.Y);

                #region Coordinates (XYZ)
                if (Options.Get.IsSideView)
                {
                    this.ts_xyz.Text = $"XYZ : ({loc.X}, {(this.image.Height - loc.Y)}, *)";
                }
                else
                {
                    this.ts_xyz.Text = $"XYZ : ({loc.X}, *, {loc.Y})";
                }
                #endregion

                Material[] ms = this.image.GetMaterialsAt(loc.X, loc.Y);

                #region Material Filter
                removeAllToolStripMenuItem.Enabled = Options.Get.SelectedMaterialFilter.Any();

                btnAddMat0_Filter.Visible = ms.Length > 0;
                btnRemoveMat0_Filter.Visible = ms.Length > 0;
                if (ms.Length > 0)
                {
                    this.materialToAddOrRemove_1 = ms[0].PixelStackerID;
                    string matName = ms[0].Label.Replace("zz", "");
                    btnAddMat0_Filter.Text = $"Add '{matName}'";
                    btnRemoveMat0_Filter.Text = $"Remove '{matName}'";

                    btnAddMat0_Filter.Visible = !Options.Get.SelectedMaterialFilter.Contains(ms[0].PixelStackerID);
                    btnRemoveMat0_Filter.Visible = Options.Get.SelectedMaterialFilter.Contains(ms[0].PixelStackerID)
                        || !Options.Get.SelectedMaterialFilter.Any();
                }

                btnAddMat1_Filter.Visible = ms.Length > 1;
                btnRemoveMat1_Filter.Visible = ms.Length > 1;
                if (ms.Length > 1)
                {
                    this.materialToAddOrRemove_2 = ms[1].PixelStackerID;
                    string matName = ms[1].Label.Replace("zz", "");
                    btnAddMat1_Filter.Text = $"Add '{matName}'";
                    btnRemoveMat1_Filter.Text = $"Remove '{matName}'";

                    btnAddMat1_Filter.Visible = !Options.Get.SelectedMaterialFilter.Contains(ms[1].PixelStackerID);
                    btnRemoveMat1_Filter.Visible = Options.Get.SelectedMaterialFilter.Contains(ms[1].PixelStackerID)
                        || !Options.Get.SelectedMaterialFilter.Any();
                }
                #endregion

                #region Material name
                this.ts_MaterialName.Text = $"Materials: \r\n{string.Join("\r\n", ms.Select(x => x.Label.Replace("zz", "")))}";
                #endregion

                #region Average color
                this.averageColorCodeToolStripMenuItem.Text = "AvgColor: #" + cFromBlueprint.R.ToString("X2") + cFromBlueprint.G.ToString("X2") + cFromBlueprint.B.ToString("X2");
                this.rGBAToolStripMenuItem.Text = $"RGBA: ({cFromBlueprint.R}, {cFromBlueprint.G}, {cFromBlueprint.B}, {cFromBlueprint.A})";
                #endregion

                #region Replacements list
                List<Color> matches = ColorMatcher.Get.FindBestMatches(cFromPreRender, Math.Min(ColorMatcher.Get.ColorToMaterialMap.Keys.Count, 40));
                var singleLayerItems = matches.Where(mi =>
                {
                    if (ColorMatcher.Get.ColorToMaterialMap.TryGetValue(mi, out Material[] miMats))
                    {
                        if (miMats.Length == 1)
                        {
                            return true;
                        }
                    }

                    return false;
                });

                matches = singleLayerItems.Concat(matches.Except(singleLayerItems)).ToList();


                this.replaceMenuItems_1.DropDownItems.Clear();
                this.replaceMenuItems_2.DropDownItems.Clear();
                this.replaceMenuItems_3.DropDownItems.Clear();
                this.replaceMenuItems_4.DropDownItems.Clear();

                int size = 64;
                for (int matchIndex = 0; matchIndex < matches.Count; matchIndex++)
                {
                    var match = matches[matchIndex];

                    if (ColorMatcher.Get.ColorToMaterialMap.TryGetValue(match, out Material[] materials))
                    {
                        Bitmap exampleThumbnail = new Bitmap(
                            width: size,
                            height: size,
                            format: PixelFormat.Format32bppArgb);

                        using (Graphics g = Graphics.FromImage(exampleThumbnail))
                        {
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

                            for (int z = 0; z < materials.Length; z++)
                            {
                                var mat = materials[z];
                                var matPic = mat.getImage(Options.Get.IsSideView);

                                try
                                {
                                    lock (matPic)
                                    {
                                        g.DrawImage(matPic, 0, 0, size, size);
                                    }
                                }
                                catch
                                {
                                }
                            }

                            var menuToAddTo = matchIndex < 10 ? this.replaceMenuItems_1
                                    : matchIndex < 20 ? this.replaceMenuItems_2
                                    : matchIndex < 30 ? this.replaceMenuItems_3
                                    : this.replaceMenuItems_4
                                    ;


                            menuToAddTo.DropDownItems.Add(
                            new ToolStripMenuItem(match.Name, exampleThumbnail, (object ttsender, EventArgs evtArgs) =>
                            {
                                this.ReplaceMaterialWithMaterial(cFromBlueprint, match);
                            })
                            {
                                AutoSize = true,
                                ImageScaling = ToolStripItemImageScaling.None
                            });
                        }
                    }
                }

                #endregion

                this.contextMenu.Show(Cursor.Position);
            }
        }

        private void ReplaceMaterialWithMaterial(Color before, Color after)
        {
            int mWidth = this.image.Width;
            int mHeight = this.image.Height;

            #region Analyze changes to be made
            ChangeRecord record = new ChangeRecord()
            {
                Before = before,
                After = after,
                ChangedPixels = new List<Point>()
            };

            for (int x = 0; x < mWidth; x++)
            {
                for (int y = 0; y < mHeight; y++)
                {
                    Color cReplace = Color.FromArgb(image.BlocksMap[x, y]);
                    if (cReplace == before)
                    {
                        record.ChangedPixels.Add(new Point(x, y));
                    }
                }
            }
            #endregion

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            MainForm.Self.History.AddChange(record);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private void clearMaterialFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options.Get.SelectedMaterialFilter = new List<string>();
            Options.Save();
            this.InvokeEx(c => c.ForceReRender());
        }

        private void addAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var items = new List<string>();
            items.AddRange(Materials.List.Where(x => x.BlockID != 0).Select(x => x.PixelStackerID));
            lock (Options.Get.SelectedMaterialFilter)
            {
                Options.Get.SelectedMaterialFilter = items;
            }
            Options.Save();
            this.InvokeEx(c => c.ForceReRender());
        }
    }
}