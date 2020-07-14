using fNbt;
using PixelStacker.Logic.Collections;
using PixelStacker.Logic.Extensions;
using PixelStacker.Properties;
using PixelStacker.UI;
using SimplePaletteQuantizer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic
{

    class PngFormatter
    {

        /* Phases:
            * 1 - Load image
            * 2 - Quantize/Prerender image
            * 3 - Generate blueprint/ map to colors
            * 4 - Generate PNG image (layer 1)
            * 4b - Generate layer 2
            * 4c - Shadows
    * 4d - WorldEdit Origin
    * 4e - Grid
    * 4f - Border
            * 5 - Save to file
         */
        public static async Task writePNG(System.Threading.CancellationToken _worker, string filePath, Bitmap inputImage)
        {
            Console.WriteLine("Resizing image...");
            Bitmap resized = PngFormatter.ResizeAndFormatRawImage(inputImage);
            //inputImage.DisposeSafely();

            Console.WriteLine("Quantizing image...");
            PngFormatter.QuantizeImage(_worker, ref resized);

            Console.WriteLine("Converting pixels to Minecraft materials...");
            BlueprintPA blueprint = await BlueprintPA.GetBluePrintAsync(_worker, resized);
            resized.DisposeSafely();

            Console.WriteLine("Rendering to bitmap...");
            Bitmap rendered = RenderBitmapFromBlueprint(_worker, blueprint, out int? textureSize);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            rendered.Save(filePath, ImageFormat.Png);
        }


        /// <summary>
        /// Step 1 of the process. 
        /// If maxHeight and maxWidth are not provided, they will be taken from the options.
        /// </summary>
        /// <param name="LIM">Your raw input image.</param>
        /// <param name="maxHeight">Taken from options if not given.</param>
        /// <param name="maxWidth">Taken from options if not given.</param>
        /// <returns></returns>
        public static Bitmap ResizeAndFormatRawImage(Bitmap LIM)
        {
            // Let's figure out sizing now.
            int mH = Math.Min(Options.Get.MaxHeight ?? LIM.Height, LIM.Height);
            int mW = Math.Min(Options.Get.MaxWidth ?? LIM.Width, LIM.Width);

            int H = LIM.Height;
            int W = LIM.Width;

            if (mW < mH)
            {
                H = mW * H / W;
                W = mW;
            }
            else
            {
                W = mH * W / H;
                H = mH;
            }

            return LIM.To32bppBitmap(W, H);
        }

        /// <summary>
        /// Step 2 of the process. 
        /// Respects color cache fragment size option as well as various Quantizer options
        /// </summary>
        /// <param name="_worker"></param>
        /// <param name="srcImage">Modified input image. This WILL be modified, and any old data will be disposed.</param>
        /// <returns></returns>
        public static void QuantizeImage(CancellationToken _worker, ref Bitmap srcImage)
        {
            var engine = QuantizerEngine.Get;

            if (Options.Get.PreRender_ColorCacheFragmentSize > 1)
            {
                // We can simplify this so that we cut total color counts down massively.
                srcImage.ToEditStreamParallel(_worker, (int x, int y, Color c) =>
                {
                    int a = (c.A < 32) ? 0 : 255;
                    return Color.FromArgb(a, c.Normalize());
                });
            }


            if (Options.Get.PreRender_IsEnabled)
            {
                Bitmap img = engine.RenderImage(srcImage);
                srcImage.DisposeSafely();
                srcImage = img;
                _worker.SafeThrowIfCancellationRequested();
            }

            //////if (ColorMatcher.Get.ColorToMaterialMap.Count == 0)
            //////{
            //////    TaskManager.SafeReport(0, "Compiling the color map");
            //////    ColorMatcher.Get.CompileColorPalette(_worker, true, Materials.List).GetAwaiter().GetResult();
            //////}

            // Do this step BEFORE setting panzoom to null. Very important.
            //this.PreRenderedImage = img; // RenderedImagePanel.RenderUsingJustTheColorPalette(_worker, img);
        }


        private const byte SHOWN_NONE = 0;
        private const byte SHOWN_TOP = 1;
        private const byte SHOWN_BOTTOM = 2;
        private const byte SHOWN_TOP_AND_BOTTOM = 3;

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

        public static Bitmap RenderBitmapFromBlueprint(CancellationToken? worker, BlueprintPA blueprint, out int? textureSize)
        {
            // TODO: Make sure this value is saved to the render panel instance somehow or else there will be horrible issues
            textureSize = RenderedImagePanel.CalculateTextureSize(blueprint);
            if (textureSize == null) return null;

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
                    //int w = (int) (origW * MainForm.PanZoomSettings.zoomLevel);
                    //int h = (int) (origH * MainForm.PanZoomSettings.zoomLevel);
                    //int zoom = (int) (MainForm.PanZoomSettings.zoomLevel);

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
                    bool _isFrugalAesthetic = Options.Get.IsExtraShadowDepthEnabled && !selectedMaterials.Any();

                    using (Graphics gImg = Graphics.FromImage(bm))
                    {
                        gImg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                        gImg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                        gImg.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;


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

                            for (int x = 0; x < mWidth; x++)
                            {
                                TaskManager.SafeReport(100 * x / mWidth);
                                worker?.SafeThrowIfCancellationRequested();
                                for (int y = 0; y < mHeight; y++)
                                {
                                    int xi = x * textureSize.Value;
                                    int yi = y * textureSize.Value;
                                    //if (xi + MainForm.PanZoomSettings.zoomLevel >= 0 && yi + MainForm.PanZoomSettings.zoomLevel >= 0)
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

        public static void writeBlueprintDirect(string filePath, Schem2Details details)
        {
            bool isv = Options.Get.IsSideView;
            bool isMultiLayer = Options.Get.IsMultiLayer;
            var nbt = new NbtCompound("Schematic");
            nbt.Add(new NbtInt("Version", 2)); // Schematic format version
            nbt.Add(new NbtInt("DataVersion", Constants.DataVersion));

            {
                var metadata = new NbtCompound("Metadata");
                metadata.Add(new NbtString("Name", details.MetaData.Name));
                metadata.Add(new NbtString("Author", details.MetaData.Author));
                metadata.Add(new NbtString("Generator", "PixelStacker (" + Constants.Version + ")"));
                metadata.Add(new NbtString("Generator Website", Constants.Website));
                metadata.Add(new NbtList("RequiredMods", new List<NbtTag>(), NbtTagType.String));
                metadata.Add(new NbtLong("Date", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()));
                metadata.Add(new NbtInt("WEOriginX", 1));
                metadata.Add(new NbtInt("WEOriginY", 1));
                metadata.Add(new NbtInt("WEOriginZ", 1));

                if (details.MetaData.WEOffsetX != null)
                {
                    metadata.Add(new NbtInt("WEOffsetX", details.MetaData.WEOffsetX.Value));
                    metadata.Add(new NbtInt("WEOffsetY", details.MetaData.WEOffsetY.Value));
                    metadata.Add(new NbtInt("WEOffsetZ", details.MetaData.WEOffsetZ.Value));
                }

                nbt.Add(metadata);
            }

            {
                nbt.Add(new NbtIntArray("Offset", new int[] { 0, 0, 0 }));
                nbt.Add(new NbtShort("Width", (short) details.WidthX));
                nbt.Add(new NbtShort("Height", (short) details.HeightY));
                nbt.Add(new NbtShort("Length", (short) details.LengthZ));
            }

            var palette = new Dictionary<string, int>();
            var tileEntities = new List<NbtCompound>();

            //Required.Specifies the main storage array which contains Width *Height * Length entries.Each entry is specified 
            //as a varint and refers to an index within the Palette.The entries are indexed by 
            //x +z * Width + y * Width * Length.
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter buffer = new BinaryWriter(ms))
                {
                    int xMax = details.WidthX;
                    int yMax = details.HeightY;
                    int zMax = details.LengthZ;

                    var r = details.RegionXYZ;
                    for (int y = 0; y < yMax; y++)
                    {
                        for (int z = 0; z < zMax; z++)
                        {
                            for (int x = 0; x < xMax; x++)
                            {
                                var mat = r[x][y][z];
                                string blockKey = mat.GetBlockNameAndData(isv);
                                if (!palette.ContainsKey(blockKey))
                                {
                                    palette.Add(blockKey, palette.Count);
                                }

                                int blockID = palette[blockKey];

                                while ((blockID & -128) != 0)
                                {
                                    buffer.Write((byte) (blockID & 127 | 128));
                                    blockID = (int) ((uint) blockID >> 7);
                                }
                                buffer.Write((byte) blockID);
                            }
                        }
                    }
                }

                // size of block palette in number of bytes needed for the maximum  palette index. Implementations may use
                // this as a hint for the case that the palette data fits within a datatype smaller than a 32 - bit integer
                // that they may allocate a smaller sized array.
                nbt.Add(new NbtInt("PaletteMax", palette.Count));
                var paletteTag = new NbtCompound("Palette");
                var paletteList = palette.OrderBy(kvp => kvp.Value);
                foreach (var kvp in paletteList)
                {
                    paletteTag.Add(new NbtInt(kvp.Key, kvp.Value));
                }

                nbt.Add(paletteTag);
                var blockData = ms.ToArray();
                nbt.Add(new NbtByteArray("BlockData", blockData));
            }

            nbt.Add(new NbtList("TileEntities", NbtTagType.End));

            var serverFile = new NbtFile(nbt);
            serverFile.SaveToFile(filePath, NbtCompression.GZip);
        }
    }
}
