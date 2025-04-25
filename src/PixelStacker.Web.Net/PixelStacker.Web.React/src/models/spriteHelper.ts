import { PixelStackerAPI } from "@/engine/pixelStackerApi";

const imgCache: Record<string, HTMLImageElement> = {};
export class SpriteHelper {
  private static sideImages: Record<number, HTMLImageElement> = {};
  private static topImages: Record<number, HTMLImageElement> = {};

  public static async GetPageImageAsync(page: number, isSide: boolean): Promise<HTMLImageElement> {
    const cacheKey = `${page}--${isSide}`;
    if (!!imgCache[cacheKey])
      return imgCache[cacheKey];

    const api = new PixelStackerAPI();
    const img = await api.getSpriteSheet(isSide, page);
    imgCache[cacheKey] = img;
    return img;
  }
}
