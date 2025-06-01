import { Constants } from "@/constants";
import DeploymentInfo from "@/models/deplymentInfo";
import { Material } from "@/models/material";

export class PixelStackerAPI {
  private endpoint: string;
  constructor(apiEndpoint: string = Constants.API_ENDPOINT) {
    this.endpoint = apiEndpoint;
  }

  public async getSpriteSheet(isSide: boolean, page: number): Promise<HTMLImageElement> {
    const url = `${this.endpoint}/Definitions/GetMaterialSpriteSheet?isSide=${encodeURIComponent(isSide)}&page=${encodeURIComponent(page)}`;

    return new Promise(resolve => {
      const elem = new Image();
      elem.src = url;
      elem.onload = () => {
        resolve(elem);
      }
    });
  }

  public async getMaterialsListAsync(): Promise<Material[]> {
    const reply = await fetch(`${this.endpoint}/Definitions/MaterialList`);
    const data = await reply.json();
    const obj = data as Material[];
    return obj;
  }

  /**
   * @returns { "0": "AIR", "2": "WOOL_00,GLASS_00" }
   */
  public async getMaterialPaletteMap(): Promise<Record<string, string>> {
    const reply = await fetch(`${this.endpoint}/Definitions/MaterialPaletteMap`);
    const data = await reply.json();
    const obj = data as Record<string, string>;
    return obj;
  }

  public async getDeploymentInfoAsync(): Promise<DeploymentInfo> {
    const url = `${this.endpoint}/DeploymentInfo/Index`;
    const response = await fetch(url,
      {
        method: "GET",
        mode: 'cors'
      } as RequestInit);

    // 2) filter on 200 OK
    if (response.status === 200 || response.status === 0) {
      return (await response.json()) as DeploymentInfo;
    } else {
      let obj = await response.text();
      return await Promise.reject(new Error(obj));
    }
  }

  public async imageToPxlzipAsync(image: Blob, maxW: number, maxH: number): Promise<Blob> {
    try {
      let form = new FormData();
      form.append("File", image);

      const qs = new URLSearchParams();
      qs.append("RgbBucketSize", "1");
      qs.append("MaxHeight", `${maxH}`);
      qs.append("MaxWidth", `${maxW}`);
      qs.append("Format", "PixelStackerProject");
      qs.append("EnableDithering", "false");
      qs.append("IsSideView", "false");
      qs.append("IsMultiLayer", "true");

      const url = `${this.endpoint}/Render/ByFileAdvanced?${qs.toString()}`;

      const response = await fetch(url,
        {
          method: "POST",
          mode: 'cors',
          body: form
        } as RequestInit);

      // 2) filter on 200 OK
      if (response.status === 200 || response.status === 0) {
        return await response.blob();
      } else {
        let obj = await response.text();
        return await Promise.reject(new Error(obj));
      }
    } catch (e) {
      return await Promise.reject(e);
    }
  }
}