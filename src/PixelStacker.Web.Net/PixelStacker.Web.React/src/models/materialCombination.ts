import { Material } from "./material";
import { MaterialList } from "./materials";

export class MaterialCombination {
    public top: Material;
    public bottom: Material;

    public constructor(pxIdBottom: string, pxIdTop?: string) {
        this.bottom = MaterialList.filter(x => x.pixelStackerID == pxIdBottom)[0];
        this.top = MaterialList.filter(x => x.pixelStackerID == pxIdTop)[0] || this.bottom;
    }

    public static fromPixelStackerIDs(pxIdBottom: string, pxIdTop?: string): MaterialCombination {
        const mc = new MaterialCombination(pxIdBottom, pxIdTop);
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
        // if (isSide)
        // {
        //     _GetAverageColorSide ??= SideImage.GetAverageColor();
        //     return _GetAverageColorSide.Value;
        // }
        // else
        // {
        //     _GetAverageColorTop ??= TopImage.GetAverageColor();
        //     return _GetAverageColorTop.Value;
        // }
    }

    public getPxIds(): string {
        if (this.isMultiLayer()) return `${this.bottom.pixelStackerID},${this.top.pixelStackerID}`;
        else return this.bottom.pixelStackerID;
    }
}