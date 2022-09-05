const imgCache: Record<string, HTMLImageElement> = {};
function getImgAsync(url: string) : Promise<HTMLImageElement> {
    if (!!imgCache[url])
        return Promise.resolve(imgCache[url]);
    return new Promise(resolve => {
        const elem = new Image();
        elem.src = url;
        elem.onload = () => {
          imgCache[url] = elem;
          resolve(elem);
        }
    });
}

export class SpriteHelper {
    private static sideImages: Record<number, HTMLImageElement> = {};
    private static topImages: Record<number, HTMLImageElement> = {};

    public static async GetPageImageAsync(page: number, isSide: boolean): Promise<HTMLImageElement> {
        return await getImgAsync(`./assets/img/${(isSide ? 'side' : 'top')}-sprite-${page}.png`);
    }
}
