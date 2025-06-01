import { Material } from "./material";

export class MaterialCombination {
  public top: Material;
  public bottom: Material;

  public constructor(materials: Material[], pxIdBottom: string, pxIdTop?: string) {
    this.bottom = materials.filter(x => x.pixelStackerID == pxIdBottom)[0];
    this.top = materials.filter(x => x.pixelStackerID == pxIdTop)[0] || this.bottom;
  }

  public static fromPixelStackerIDs(materials: Material[], pxIdBottom: string, pxIdTop?: string): MaterialCombination {
    const mc = new MaterialCombination(materials, pxIdBottom, pxIdTop);
    return mc;
  }

  public isMultiLayer(): boolean {
    return this.top.pixelStackerID != this.bottom.pixelStackerID;
  }

  public getImage(isSide: boolean): object {
    return {}; // I have no idea what imgs should be rendered as yet.
  }

  public getAverageColor(isSide: boolean): string {
    return "#fff";
  }

  public getPxIds(): string {
    if (this.isMultiLayer()) return `${this.bottom.pixelStackerID},${this.top.pixelStackerID}`;
    else return this.bottom.pixelStackerID;
  }
}