import { RenderedCanvas } from "@/models/renderedCanvas";
import { PanZoomSettings } from "@/models/panZoomSettings";
import { CanvasViewerSettings } from "@/models/canvasViewerSettings";
import { PxPoint, SKRect, SKSize } from "@/models/pxPoint";
import { SpriteHelper } from "@/models/spriteHelper";
import { ProgressX } from "@/utils/progressX";
import { Logger } from "@/utils/logger";

type EstimateProp = 'floor' | 'round' | 'ceil';
class Grid2D<T> {
    public data: (T | undefined)[][];
    public width: number;
    public height: number;

    constructor(width: number, height: number, defVal?: T) {
        this.data = [];
        this.width = width;
        this.height = height;
        for (let w = 0; w < width; w++) {
            this.data[w] = [];
            for (let h = 0; h < height; h++) {
                this.data[w][h] = defVal;
            }
        }
    }

    public get = (x: number, y: number) => this.data[x][y];
    public set = (x: number, y: number, val?: T) => this.data[x][y] = val;
    public getLength = (dim: number) => dim == 0 ? this.data.length : this.data[0] != undefined && this.data[0].length || 0;
}

class GridList<T> {
    public toArray(): (T | undefined)[][][] {
        const o: (T | undefined)[][][] = [];
        for (let l = 0; l < this.list.length; l++) {
            o[l] = [];
            const grid = this.list[l];
            for (let x = 0; x < grid.width; x++) {
                o[l][x] = [];
                for (let y = 0; y < grid.height; y++) {
                    o[l][x][y] = grid.get(x, y);
                }
            }
        }

        return o;
    }

    public list: Grid2D<T>[];
    constructor() {
        this.list = [];
    }

    public get = (x: number, y: number, layer: number) => (this.list[layer]).get(x, y);
    public set = (x: number, y: number, layer: number, val?: T) => (this.list[layer]).set(x, y, val);
    public getLayer = (layer: number) => this.list[layer];
    public setLayer = (layer: number, val: Grid2D<T>) => this.list[layer] = val;
    public addLayer = (val: Grid2D<T>) => this.list[this.list.length] = val;
}

const BLOCKS_PER_CHUNK: number = 38;
const TEXTURE_SIZE: number = 16;
const BIG_IMG_MAX_AREA_B4_SPLIT: number = 100000;
const SMALL_IMAGE_DIVIDE_SIZE: number = 2;
export class RenderedCanvasPainter {
    /// <summary>
    /// Calculates a list of arrays that each contain chunk size definitions. Layer 0 would be a 1:1 render.
    /// Layer 1 would be a half scale rendering. 
    /// </summary>
    /// <param name="data">totalSourcePixelsSize</param>
    /// <param name="srs"></param>
    /// <param name="maxLayers"></param>
    /// <returns></returns>
    public static CalculateChunkSizes = (data: SKSize, maxLayers: number) => {
        let scaleDivide = 1;
        const sizesList = new GridList<SKSize>();
        let a = false;
        let b = false;
        let c = false;

        // JUST the pixels in a dest chunk no matter what scale it is at.
        const pixelsPerChunkTile = BLOCKS_PER_CHUNK * TEXTURE_SIZE;
        const MAX_AREA_B4_SPLIT_ADJUSTED = BIG_IMG_MAX_AREA_B4_SPLIT / pixelsPerChunkTile;

        // We run into integer overflows when doing pixelsPerChunk^2. So we use algebra to say
        // W*H*PPC*PPC > MAX_AREA is the same as W*W*PPC > MAX_AREA / PPC
        do {
            let curSizeSet = RenderedCanvasPainter.CalculateChunkSizesForLayer({ width: data.width, height: data.height }, scaleDivide);
            sizesList.addLayer(curSizeSet);
            scaleDivide *= 2;
            maxLayers--;

            // Do not split if one dimension is unable to be split further.
            a = curSizeSet.getLength(0) > 2 && curSizeSet.getLength(1) > 2;

            let bb = (curSizeSet.getLength(0) * curSizeSet.getLength(1)) * pixelsPerChunkTile;
            b = MAX_AREA_B4_SPLIT_ADJUSTED < bb;

            // Do not go on forever
            c = maxLayers > 0;
        } while (a && b && c);

        return sizesList;
    }

    /// <summary>
    /// When given sizes and a scale, returns an array of the chunk sizes, where the each "chunk size"
    /// represents the size of the bitmap tile to be actually rendered.
    /// </summary>
    /// <param name="srcImageSize">The TOTAL size of the data to render. (width tiles x height tiles)</param>
    /// <param name="scaleDivide">1x = 40 blocks per chunk. 2x = 80 blocks per chunk. And so on. 
    /// But each block will be rendered at half scale. Make sense? Basically this value is used
    /// for down-sizing.</param>
    /// <returns></returns>
    private static CalculateChunkSizesForLayer(srcImageSize: SKSize, scaleDivide: number): Grid2D<SKSize> {
        const srcW = srcImageSize.width;
        const srcH = srcImageSize.height;
        const srcPixelsPerChunk = BLOCKS_PER_CHUNK * scaleDivide;
        const dstPixelsPerChunk = Math.floor(TEXTURE_SIZE * srcPixelsPerChunk / scaleDivide); // 16 * (RenderedCanvasPainter.BlocksPerChunk * N) / N = 6RenderedCanvasPainter.BlocksPerChunk
        const numChunksWide = Math.floor(srcW / srcPixelsPerChunk) + (srcW % srcPixelsPerChunk == 0 ? 0 : 1);
        const numChunksHigh = Math.floor(srcH / srcPixelsPerChunk) + (srcH % srcPixelsPerChunk == 0 ? 0 : 1);
        var sizeSet = new Grid2D<SKSize>(numChunksWide, numChunksHigh);

        // MAX PERFECT WIDTH - ACTUAL WIDTH = difference
        let deltaX = numChunksWide * dstPixelsPerChunk - TEXTURE_SIZE * srcW / scaleDivide;
        let deltaY = numChunksHigh * dstPixelsPerChunk - TEXTURE_SIZE * srcH / scaleDivide;
        for (let x = 0; x < numChunksWide; x++) {
            let dstWidthOfChunk = x < numChunksWide - 1
                ? dstPixelsPerChunk // Very simple. We know if it isn't on the tail we can assume a standard full width.
                : dstPixelsPerChunk - deltaX;
            for (let y = 0; y < numChunksHigh; y++) {
                let dstHeightOfChunk = y < numChunksHigh - 1
                    ? dstPixelsPerChunk // Very simple. We know if it isn't on the tail we can assume a standard full width.
                    : dstPixelsPerChunk - deltaY;

                sizeSet.set(x, y, { width: dstWidthOfChunk, height: dstHeightOfChunk });
            }
        }

        return sizeSet;
    }

    /// <summary>
    /// Initialize the bitmaps by rendering a canvas into image tiles.
    /// </summary>
    /// <returns></returns>
    private static async RenderIntoTilesAsync(data: RenderedCanvas, maxLayers: number) {
        var sizes = RenderedCanvasPainter.CalculateChunkSizes(new SKSize(data.width, data.height), maxLayers);
        var bitmaps = new GridList<CanvasImageSource>();

        let chunksFinishedSoFar = 0;
        let totalChunksToRender = 0;
        for (let si = 0; si < sizes.list.length; si++) {
            const sizeLayer = sizes.getLayer(si);
            totalChunksToRender += sizeLayer.height * sizeLayer.width;
            bitmaps.addLayer(new Grid2D<CanvasImageSource>(sizeLayer.getLength(0), sizeLayer.getLength(1)));
        }
        // TODO: Paint the stuff onto the CanvasImageSOurce.
        // Which type of canvas image source should we use here. Hmmmm. 
        // #region LAYER 0
        const sizeSet = sizes.getLayer(0);
        let scaleDivide = 1;
        let numChunksWide = sizeSet.getLength(0);
        let numChunksHigh = sizeSet.getLength(1);
        let srcPixelsPerChunk = BLOCKS_PER_CHUNK * scaleDivide;
        let dstPixelsPerChunk = TEXTURE_SIZE * srcPixelsPerChunk / scaleDivide;
        let iTask = 0;
        //         Task[] L0Tasks = new Task[sizes[0].Length];
        for (let cW = 0; cW < numChunksWide; cW++) {
            for (let cH = 0; cH < numChunksHigh; cH++) {
                let cWf = cW;
                let cHf = cH;
                const tileSize = sizeSet.get(cW, cH);
                const srcRect = new SKRect(
                    cWf * srcPixelsPerChunk,
                    cHf * srcPixelsPerChunk,
                    Math.floor(tileSize!.width * scaleDivide / TEXTURE_SIZE),
                    Math.floor(tileSize!.height * scaleDivide / TEXTURE_SIZE)
                );


                const dstRect = new SKRect(
                    cWf * dstPixelsPerChunk,
                    cHf * dstPixelsPerChunk,
                    tileSize!.width,
                    tileSize!.height
                );

                //                 L0Tasks[iTask++] = Task.Run(() => {
                var bmToAdd = await RenderedCanvasPainter.CreateLayer0ImageAsync(data, srcRect, dstRect);

                const l0 = bitmaps.getLayer(0);
                l0.set(cWf, cHf, bmToAdd);
                bitmaps.setLayer(0, l0);
                // bitmaps.set(cWf, cHf, 0, bmToAdd);

                // const ctx = bmToAdd.getContext('2d')!;
                // ctx.fillStyle = "5px solid red";
                // ctx.fillRect(2, 3, 100, 200);


                // (document.getElementById('ctxTest') as HTMLCanvasElement).getContext('2d')!.drawImage(bmToAdd, 128, 128);
                let nVal = ++chunksFinishedSoFar;
                await ProgressX.ReportAsync(10 + 90 * nVal / totalChunksToRender, "Rendering data to high-quality canvas tiles...");
            }
        }


        // OTHER LAYERS 2.0

        //    #region OTHER LAYERS
        {

            const pixelsPerHalfChunk: number = TEXTURE_SIZE * BLOCKS_PER_CHUNK / 2;

            for (let l = 1; l < sizes.list.length; l++) {
                const sizeSet: Grid2D<SKSize> = sizes.getLayer(l);
                let scaleDivide = Math.pow(2, l);
                let numChunksWide = sizeSet.getLength(0);
                let numChunksHigh = sizeSet.getLength(1);
                let srcPixelsPerChunk = BLOCKS_PER_CHUNK * scaleDivide;
                let dstPixelsPerChunk = TEXTURE_SIZE * srcPixelsPerChunk / scaleDivide;
                let ssWidth = sizeSet.getLength(0);
                let ssHeight = sizeSet.getLength(1);
                var upperLayer = bitmaps.getLayer(l - 1);
                for (let x = 0; x < ssWidth; x++) {
                    for (let y = 0; y < ssHeight; y++) {
                        const dstSize: SKSize = sizeSet.get(x, y)!;
                        const bm = await this.GetCanvasElemAsync(dstSize.width, dstSize.height);
                        const g = bm.getContext('2d')!;

                        let xUpper = x * 2;
                        let yUpper = y * 2;

                        // TL
                        {
                            const bmToCopy = upperLayer.get(xUpper, yUpper)!;
                            g.drawImage(bmToCopy, 0, 0, (bmToCopy.width as number) / 2, (bmToCopy.height as number) / 2);
                        }

                        // TR
                        if (upperLayer.getLength(0) > xUpper + 1
                            && upperLayer.getLength(1) > yUpper
                        ) {
                            const bmToCopy = bitmaps.get(xUpper + 1, yUpper, l - 1)!;
                            g.drawImage(bmToCopy, pixelsPerHalfChunk, 0, (bmToCopy.width as number) / 2, (bmToCopy.height as number) / 2);
                        }

                        // BL
                        if (upperLayer.getLength(0) > xUpper
                            && upperLayer.getLength(1) > yUpper + 1
                        ) {
                            const bmToCopy = bitmaps.get(xUpper, yUpper + 1, l - 1)!;
                            g.drawImage(bmToCopy, 0, pixelsPerHalfChunk, (bmToCopy.width as number) / 2, (bmToCopy.height as number) / 2);
                        }

                        // BR
                        if (upperLayer.getLength(0) > xUpper + 1
                            && upperLayer.getLength(1) > yUpper + 1
                        ) {
                            const bmToCopy = bitmaps.get(xUpper + 1, yUpper + 1, l - 1)!;
                            g.drawImage(bmToCopy, pixelsPerHalfChunk, pixelsPerHalfChunk, (bmToCopy.width as number) / 2, (bmToCopy.height as number) / 2);
                        }

                        bitmaps.set(x, y, l, bm);
                        await ProgressX.ReportAsync(10 + 90 * ++chunksFinishedSoFar / totalChunksToRender, "Rendering data to low-quality canvas tiles...");
                    }
                }
            }
        }
        //    #endregion OTHER LAYERS

        return bitmaps;
    }


    // Something... somehing... THIS mwethod aint working
    private static async GetCanvasElemAsync(w: number, h: number): Promise<HTMLCanvasElement> {
        return new Promise(resolve => {
            const bm = document.createElement("canvas") as HTMLCanvasElement;
            bm.width = w;
            bm.height = h;
            resolve(bm);
        });
    }

    private static async CreateLayer0ImageAsync(data: RenderedCanvas, srcTile: SKRect, dstTile: SKRect): Promise<HTMLCanvasElement> {
        const bm = await RenderedCanvasPainter.GetCanvasElemAsync(dstTile.width, dstTile.height);

        const canvas = bm.getContext('2d')!;

        // var bm = new SKBitmap((int)dstTile.Width, (int)dstTile.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
        const scaleDivide = Math.floor(dstTile.width / srcTile.width);

        const srcWidth = srcTile.width;
        const srcHeight = srcTile.height;

        // using SKPaint paint = new SKPaint() { BlendMode = SKBlendMode.Src, FilterQuality = SKFilterQuality.None };

        // if (srs.IsSolidColors)
        // {
        //     Parallel.For(0, srcHeight, (y) =>
        //     {
        //         var paintSolid = new SKPaint()
        //         {
        //             BlendMode = SKBlendMode.Src,
        //             FilterQuality = SKFilterQuality.High,
        //             IsAntialias = false,
        //             IsStroke = false, // FILL
        //         };

        //         for (int x = 0; x < srcWidth; x++)
        //         {
        //             var loc = srcTile.Location;
        //             var mc = data.CanvasData[(int)loc.X + x, (int)loc.Y + y];

        //             SKColor toPaint = mc.GetAverageColor(data.IsSideView, srs);

        //             paintSolid.Color = toPaint;
        //             canvas.DrawRect(new SKRect()
        //             {
        //                 Location = new SKPoint(x * TEXTURE_SIZE, y * TEXTURE_SIZE),
        //                 Size = new SKSize(TEXTURE_SIZE, TEXTURE_SIZE)
        //             }, paintSolid);
        //         }
        //     });
        // }
        // else
        // {
        // const c2 = (document.getElementById('ctxTest') as HTMLCanvasElement).getContext('2d')!;

        for (let y = 0; y < srcHeight; y++) {
            for (let x = 0; x < srcWidth; x++) {
                const mcId = data.canvasData[srcTile.x + x][srcTile.y + y];
                const mc = data.palette.getMC(mcId);
                if (!mc) {
                    Logger.warn(`Unknown ID ${mcId}. Skipped.`, mcId);
                    continue;
                }
                // Logger.log(`$x:${x}, y:${y}, mcid=${mcId}`, mc);
                const spriteBottom = await SpriteHelper.GetPageImageAsync(mc.bottom.spritePage, data.isSideView);
                // canvas.fillStyle = '1px solid red';
                // canvas.fillRect(x * TEXTURE_SIZE, y * TEXTURE_SIZE, TEXTURE_SIZE, TEXTURE_SIZE);
                // 11

                // // THIS works very quickly, momentarily, then the change is wiped away when all our tiles
                // get rendered to the panel in the onPaint method.
                // (document.getElementById("ctxTest") as HTMLCanvasElement).getContext("2d")!.drawImage(
                //     // sprite sheet
                //     spriteBottom,
                //     // source area
                //     mc.bottom.spriteX * TEXTURE_SIZE, mc.bottom.spriteY * TEXTURE_SIZE, TEXTURE_SIZE, TEXTURE_SIZE,
                //     // Destination area
                //     (x * TEXTURE_SIZE) + 200, (y * TEXTURE_SIZE) + 200, TEXTURE_SIZE, TEXTURE_SIZE);

                canvas!.drawImage(
                    // sprite sheet
                    spriteBottom,
                    // source area
                    mc.bottom.spriteX * TEXTURE_SIZE, mc.bottom.spriteY * TEXTURE_SIZE, TEXTURE_SIZE, TEXTURE_SIZE,
                    // Destination area
                    x * TEXTURE_SIZE, y * TEXTURE_SIZE, TEXTURE_SIZE, TEXTURE_SIZE);

                if (mc.isMultiLayer()) {
                    const spriteTop = await SpriteHelper.GetPageImageAsync(mc.top.spritePage, data.isSideView);
                    canvas!.drawImage(spriteTop, // sprite sheet
                        // source area
                        mc.top.spriteX * TEXTURE_SIZE, mc.top.spriteY * TEXTURE_SIZE, TEXTURE_SIZE, TEXTURE_SIZE,
                        // Destination area
                        x * TEXTURE_SIZE, y * TEXTURE_SIZE, TEXTURE_SIZE, TEXTURE_SIZE);
                }

                // if (srs.ZLayerFilter == 0) toPaint = mc.Bottom.GetImage(data.IsSideView);
                // else if (srs.ZLayerFilter == 1) toPaint = mc.Top.GetImage(data.IsSideView);
                // else toPaint = mc.GetImage(data.IsSideView);
            }
        }
        // (document.getElementById('ctxTest') as HTMLCanvasElement).getContext('2d')!.drawImage(bm, 128, 128);
        return bm;
        // }

        // return bm;
    }

    public static CreateAsync = async (data: RenderedCanvas, maxLayers: number = 10): Promise<RenderedCanvasPainter> => {

        var painter = new RenderedCanvasPainter(data);
        // worker ??= CancellationToken.None;
        // worker.SafeThrowIfCancellationRequested();

        const bms = await RenderedCanvasPainter.RenderIntoTilesAsync(data, maxLayers);
        painter.Bitmaps = bms;
        // TODO: Create a whole bunch of tiles, etc
        return painter;
    };

    Data: RenderedCanvas;
    Bitmaps: GridList<CanvasImageSource>;

    constructor(data: RenderedCanvas) {
        this.Data = data;
        this.Bitmaps = new GridList<CanvasImageSource>();
        // History = new SuperHistory(Data);
    }


    public PaintSurface = (canvas: HTMLCanvasElement, pz: PanZoomSettings, vs: CanvasViewerSettings) => {
        const g = canvas.getContext("2d")!;
        const parentControlSize = { width: canvas.width, height: canvas.height };
        g.fillStyle = '5px #000 solid';
        g.fillRect(Math.random() * 1000, Math.random() * 1000, 20, 20);
        g.clearRect(0, 0, canvas.width, canvas.height);
        RenderedCanvasPainter.PaintTilesToView(g, parentControlSize, pz, this.Bitmaps);
        // if (vs.IsShowGrid) DrawGridLines(g, Data, vs, pz);
        // if (vs.IsShowBorder) DrawBorder(g, pz, new SKSize(Data.Width, Data.Height));
        // if (Data.WorldEditOrigin != null) DrawWorldEditOrigin(g, pz, Data.WorldEditOrigin);
    };


    /// <summary>
    /// Paint the rendered canvas onto the SKCanvas view.
    /// </summary>
    /// <param name="g"></param>
    /// <param name="parentControlSize"></param>
    /// <param name="pz"></param>
    private static PaintTilesToView(g: CanvasRenderingContext2D, parentControlSize: SKSize, pz: PanZoomSettings, bitmaps: GridList<CanvasImageSource>) {
        //  #region GET BITMAP SET
        //             if (bitmaps == null || bitmaps.Count == 0 || padlocks == null || padlocks.Count == 0)
        //             {
        // #if FAIL_FAST
        //                 throw new Exception("BAD STATE. RenderToView is called before view is ready.");
        // #else
        //                 return;
        // #endif
        //             }


        // TODO: ALl of this.
        let toUse = bitmaps.getLayer(0);
        let divideAmount = 1;
        let i = 1;
        while (pz.zoomLevel <= 10.0 / divideAmount / SMALL_IMAGE_DIVIDE_SIZE && i < bitmaps.list.length) {
            toUse = bitmaps.getLayer(i);
            divideAmount *= SMALL_IMAGE_DIVIDE_SIZE;
            i++;
        }
        // #endregion GET BITMAP SET

        let texSize = TEXTURE_SIZE;
        let BlocksPerChunk = BLOCKS_PER_CHUNK;
        // The count of ORIGINAL SOURCE pixels in a FULL chunk.
        let srcPixelsPerChunk = BlocksPerChunk * divideAmount;

        let srcLocationOfPanelTL = RenderedCanvasPainter.GetPointOnImage({ x: 0, y: 0 }, pz, 'floor');
        let srcLocationOfPanelBR = RenderedCanvasPainter.GetPointOnImage({ x: parentControlSize.width, y: parentControlSize.height }, pz, 'floor');

        //     // Figure out min and max chunk indexes for faster iteration.
        let minXIndex = Math.floor(srcLocationOfPanelTL.x / srcPixelsPerChunk);
        let minYIndex = Math.floor(srcLocationOfPanelTL.y / srcPixelsPerChunk);
        let maxXIndex = Math.ceil(srcLocationOfPanelBR.x / srcPixelsPerChunk);
        let maxYIndex = Math.ceil(srcLocationOfPanelBR.y / srcPixelsPerChunk);

        // Prevent out of bounds exceptions and clip it to only what is actually visible and renderable.
        let maxX = toUse.getLength(0) - 1;
        let maxY = toUse.getLength(1) - 1;
        minXIndex = Math.min(Math.max(minXIndex, 0), maxX);
        minYIndex = Math.min(Math.max(minYIndex, 0), maxY);
        maxXIndex = Math.min(Math.max(maxXIndex, 0), maxX);
        maxYIndex = Math.min(Math.max(maxYIndex, 0), maxY);

        for (let xChunk = minXIndex; xChunk <= maxXIndex; xChunk++) {
            for (let yChunk = minYIndex; yChunk <= maxYIndex; yChunk++) {


                // lock(lockSetToUse[xChunk, yChunk])
                const bmToPaint = toUse.get(xChunk, yChunk);
                if (!bmToPaint) continue;
                const bmW = bmToPaint.width as number;
                const bmH = bmToPaint.height as number;
                const pnlStart = RenderedCanvasPainter.GetPointOnPanel({ x: xChunk * srcPixelsPerChunk, y: yChunk * srcPixelsPerChunk }, pz);
                const pnlEnd = RenderedCanvasPainter.GetPointOnPanel({
                    x: (xChunk * srcPixelsPerChunk) + (bmW * divideAmount / texSize),
                    y: (yChunk * srcPixelsPerChunk) + (bmH * divideAmount / texSize)
                }, pz);

                if (!bmToPaint)
                    throw new Error('Really??');

                g.drawImage(bmToPaint,
                    0, 0, bmW, bmH,
                    // x, y (dest)
                    Math.min(pnlEnd.x, pnlStart.x), Math.min(pnlEnd.y, pnlStart.y),
                    // width dest
                    Math.abs(pnlEnd.x - pnlStart.x),
                    // height (dest)
                    Math.abs(pnlEnd.y - pnlStart.y)
                );
            }
        }
    }

    /// <summary>
    /// = (panelX - offsetX) / zoomLevel
    /// </summary>
    /// <param name="pointOnPanel"></param>
    /// <param name="pz"></param>
    /// <param name="prop"></param>
    /// <returns></returns>
    public static GetPointOnImage(pointOnPanel: PxPoint, pz: PanZoomSettings, prop: EstimateProp): PxPoint {
        if (prop == 'ceil') {
            return {
                x: Math.ceil((pointOnPanel.x - pz.imageX) / pz.zoomLevel),
                y: Math.ceil((pointOnPanel.y - pz.imageY) / pz.zoomLevel)
            };
        }
        if (prop == 'floor') {
            return {
                x: Math.floor((pointOnPanel.x - pz.imageX) / pz.zoomLevel),
                y: Math.floor((pointOnPanel.y - pz.imageY) / pz.zoomLevel)
            };
        }

        return {
            x: Math.round((pointOnPanel.x - pz.imageX) / pz.zoomLevel),
            y: Math.round((pointOnPanel.y - pz.imageY) / pz.zoomLevel)
        };
    }

    /// <summary>
    /// (imgX * zoom) + offsetX
    /// </summary>
    /// <param name="pointOnImage"></param>
    /// <param name="pz"></param>
    /// <returns></returns>
    public static GetPointOnPanel(pointOnImage: PxPoint, pz: PanZoomSettings): PxPoint {
        if (pz == undefined) {
            // #if FAIL_FAST
            // throw new ArgumentNullException("PanZoomSettings are not set. So weird!");
            // #else
            return { x: 0, y: 0 };
            // #endif
        }

        return {
            x: Math.round(pointOnImage.x * pz.zoomLevel + pz.imageX),
            y: Math.round(pointOnImage.y * pz.zoomLevel + pz.imageY)
        };
    }
}