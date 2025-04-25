import { PixelStackerAPI } from "@/engine/pixelStackerApi";
import { MaterialCombination } from "./materialCombination";

export class MaterialPalette {
  private fromPaletteID: Record<number, MaterialCombination>;
  private toPaletteID: Record<string, number>;

  constructor() {
    this.fromPaletteID = {};
    this.toPaletteID = {};
  }

  public static async fromApi(): Promise<MaterialPalette> {
    const output = new MaterialPalette();
    const api = new PixelStackerAPI();

    try {
      const materials = await api.getMaterialsListAsync();
      const map = await api.getMaterialPaletteMap();

      for (const [key, value] of Object.entries(map)) {
        const mats = value.split(',');
        const mc = MaterialCombination.fromPixelStackerIDs(materials, mats[0], mats[1]);
        output.fromPaletteID[parseInt(key, 10)] = mc;
        output.toPaletteID[mc.getPxIds()] = parseInt(key, 10);
      }
    } catch (e) {
      // Logger.error(e);
    }

    return output;
  }

  public static async fromJson(json: string): Promise<MaterialPalette> {
    const output = new MaterialPalette();
    const api = new PixelStackerAPI();

    try {
      const materials = await api.getMaterialsListAsync();
      const map: Record<number, string> = JSON.parse(json);
      for (const [key, value] of Object.entries(map)) {
        const mats = value.split(',');
        const mc = MaterialCombination.fromPixelStackerIDs(materials, mats[0], mats[1]);
        output.fromPaletteID[parseInt(key)] = mc;
        output.toPaletteID[mc.getPxIds()] = parseInt(key);
      }
    } catch (e) {
      // Logger.error(e);
    }

    return output;
  }

  public getMC(paletteID: number): MaterialCombination {
    return this.fromPaletteID[paletteID];
  }

  public getID(mc: MaterialCombination): number {
    return this.toPaletteID[mc.getPxIds()];
  }


  private static _instance: MaterialPalette;
  public static async fromResx(): Promise<MaterialPalette> {
    if (MaterialPalette._instance == undefined) {
      MaterialPalette._instance = await MaterialPalette.fromApi();
    }
    return MaterialPalette._instance;
  }
}