import { MaterialPalette } from "./materialPalette";
import { PxPoint } from "./pxPoint";

// Everything you need to render a design to schematic
export interface RenderedCanvas {
    isSideView: boolean;
    worldEditOrigin?: PxPoint;
    height: number;
    width: number;
    canvasData: number[][];
    palette: MaterialPalette;


    // public bool IsSideView { get; set; }
    // /// <summary>
    // /// True if the user has made any manual edits to the canvas.
    // /// </summary>
    // public bool IsCustomized { get; set; } = false;
    // public PxPoint WorldEditOrigin { get; set; }

    // public int Height => CanvasData.Height;
    // public int Width => CanvasData.Width;

    // [JsonIgnore]
    // public MaterialPalette MaterialPalette { get; set; }

    // [JsonIgnore]
    // public CanvasData CanvasData { get; set; }

    // public bool IsInRange(int x, int y) => CanvasData?.IsInRange(x, y) ?? false;
}