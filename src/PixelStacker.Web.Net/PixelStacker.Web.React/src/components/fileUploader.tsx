import { Constants } from '@/constants';
import { PixelStackerAPI } from '@/engine/pixelStackerApi';
import { MaterialPalette } from '@/models/materialPalette';
import { RenderedCanvas } from '@/models/renderedCanvas';
import { Logger } from '@/utils/logger';
import { ProgressX } from '@/utils/progressX';
import JSZip from 'jszip';
import React from 'react';

interface FileUploaderProps {
    onLoadFileFinish: (isSuccess: boolean, design?: RenderedCanvas) => void;
    onLoadFileError: (e: string) => void;
    onLoadFileStart?: () => void;
    MaxWidth: number;
    MaxHeight: number;
}

export class FileUploader extends React.PureComponent<FileUploaderProps>{
    private refFilePicker: React.RefObject<HTMLInputElement> = React.createRef<HTMLInputElement>();
    private api: PixelStackerAPI = new PixelStackerAPI();
    private zip: JSZip = new JSZip();

    constructor(props: FileUploaderProps) {
        super(props);
    }


    public doPickFile = () => {
        if (!!this.refFilePicker.current) {
            this.refFilePicker.current.click();
        }
    };

    // converts HEIC or other image formats to image/png type
    private readFileAsync = (file: File) => {
        return new Promise<Blob>((resolve, reject) => {
            var reader = new FileReader();

            reader.onloadend = function () {
                const fileType = file.type as string;
                const dataURL: string = reader.result as string;
                const image = new Image();
                image.src = dataURL;

                image.onload = function () {
                    const bm = document.createElement("canvas") as HTMLCanvasElement;

                    let width = image.width;
                    let height = image.height;
                    if ((width > Constants.MAX_WIDTH) || (height > Constants.MAX_HEIGHT)){
                        if (width > height) {
                            height = height * (Constants.MAX_WIDTH / width);
                            width = Constants.MAX_WIDTH;
                        } else {
                            width = width * (Constants.MAX_HEIGHT / height);
                            height = Constants.MAX_HEIGHT;
                        }
                    }

                    bm.width = width;
                    bm.height = height;
                    const ctx = bm.getContext('2d')!;
                    ctx.imageSmoothingEnabled = false;

                    ctx.drawImage(image, 0, 0, width, height);
                    const dataURLConverted = bm.toDataURL("image/png", 100);

                    let arr = dataURLConverted.split(',');
                    if (arr.length === 0) {
                        reject('Failed to load that image.');
                        return;
                    }

                    let mime = arr[0].match(/:(.*?);/)![1];
                    let bstr = window.atob(arr[1]);
                    let n = bstr.length;
                    let u8arr = new Uint8Array(n);
                    while (n--) {
                        u8arr[n] = bstr.charCodeAt(n);
                    }

                    const blob = new Blob([u8arr], { type: mime });

                    resolve(blob);
                }

                image.onerror = function () {
                    reject('There was an error processing your file!');
                };
            }

            reader.onerror = function () {
                reject('There was an error reading the file!');
            }

            reader.readAsDataURL(file);
        });

    }

    // If a pure image is given we will need to call the API to transform it into a
    // pixel zip file.
    private async getPixelZipFromImageOrZipFileAsync(file: File): Promise<JSZip> {

        let pixelZipFile: JSZip;
        if (file?.type.startsWith("image/")) {
            ProgressX.reportUnknownProgress("Preparing to send image to the API...");
            const blobToSend = await this.readFileAsync(file);

            ProgressX.reportUnknownProgress("Awaiting response from API...");

            Logger.debug(`Loading image file of type ${blobToSend.type}`)
            const buffer = await this.api.imageToPxlzipAsync(blobToSend, this.props.MaxWidth, this.props.MaxHeight);

            await ProgressX.reportUnknownProgressAsync("Unzipping PixelStacker Project file...");

            pixelZipFile = await this.zip.loadAsync(buffer);
        } else {
            await ProgressX.reportUnknownProgressAsync("Unzipping PixelStacker Project file...");
            pixelZipFile = await new Promise(resolve => {
                const fr = new FileReader();

                fr.addEventListener("load", async loadEvt => {
                    const buffer = await this.zip.loadAsync(loadEvt.target?.result as ArrayBuffer);
                    resolve(buffer);
                });

                fr.readAsArrayBuffer(file);
            });
        }

        return pixelZipFile;
    }

    private onFileChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
        if (!!e.target.files) {
            if (e.target.files.length === 1) {
                if (!!this.props.onLoadFileStart) {
                    this.props.onLoadFileStart();
                }

                try {
                    let file = e.target.files[0];

                    if (!file) {
                        this.props.onLoadFileError("Pick a file!");
                        return;
                    }

                    ProgressX.reportUnknownProgress("Processing file...");

                    // If a pure image is given we will need to call the API to transform it into a
                    // pixel zip file. This also takes care of weird image formats.
                    let pixelZipFile: JSZip = await this.getPixelZipFromImageOrZipFileAsync(file);
                    const design = await this.getRenderedCanvasBlueprintFromPixelZipFile(pixelZipFile);
                    if (!!this.props.onLoadFileFinish) {
                        this.props.onLoadFileFinish(true, design);
                    }
                } catch (e) {
                    this.props.onLoadFileError((e as Error).message);
                    this.props.onLoadFileFinish(false, undefined);
                    Logger.error(e);
                }
            }
        }

        e.target.value = '';
        return false;
    }

    private async getRenderedCanvasBlueprintFromPixelZipFile(zipFile: JSZip): Promise<RenderedCanvas> {
        const files = zipFile.files;
        Logger.debug(`Loading PXLZIP file with content`, files);
        await ProgressX.reportUnknownProgressAsync("Processing PixelStacker Project...");
        // TODO Probably *PRETTY* important--Make sure these files exist!
        // Either that, or only accept .pxlzip and not .zip
        let paletteJson = await files["palette.json"].async("string");
        // paletteJson = JSON.parse(paletteJson);
        const materialPalette = MaterialPalette.fromJson(paletteJson);

        let canvasB64 = await files["canvas-data.png"].async("base64");
        const imgElem = await this.getImageElementAsync(canvasB64);
        const canvasImgData: ImageData = this.getPixelDataFromImageElement(imgElem);
        const idArray = this.getIdArray(canvasImgData);

        const design: RenderedCanvas = {
            canvasData: idArray,
            isSideView: false,
            palette: materialPalette,
            width: idArray.length,
            height: idArray[0].length
        };

        return design;
    }

    // Technically a nested array but who cares
    private getIdArray({ width, height, data }: ImageData): number[][] {
        let output = [];
        for (let x = 0; x < width; x++) {
            let toAdd = [];
            for (let y = 0; y < height; y++) {
                let idx = (y * width + x) * 4,
                    r: number = data[idx],
                    g: number = data[idx + 1],
                    b: number = data[idx + 2],
                    a: number = data[idx + 3];

                toAdd.push(r * 256 * 256 + g * 256 + b);
            }
            output.push(toAdd);
        }

        return output;
    }

    // The main function of this file is to take the base64
    // data from a PNG (the one w/ palette IDs) and extract
    // the design from it. A couple separate functions are
    // used to do this.

    private getImageElementAsync(base64Data: string): Promise<HTMLImageElement> {
        return new Promise<HTMLImageElement>(resolve => {
            let imgElement = new Image();
            imgElement.addEventListener("load", () => resolve(imgElement));
            imgElement.src = `data:image/png;base64,${base64Data}`;
        });
    }

    private getPixelDataFromImageElement(image: HTMLImageElement): ImageData {
        const sampleCanvas = document.createElement("canvas");
        const sampleCtx = sampleCanvas.getContext("2d");

        // Appease ts
        if (sampleCtx === null) return new ImageData(0, 0);

        sampleCanvas.width = image.width;
        sampleCanvas.height = image.height;

        sampleCtx.clearRect(0, 0, image.width, image.height);
        sampleCtx.drawImage(image, 0, 0);

        return sampleCtx.getImageData(0, 0, image.width, image.height);
    }

    public render(): React.ReactNode {
        return (
            <form
                style={{ display: 'none' }}
                encType='multipart/form-data'
                method='POST'
            >
                <input
                    type="file"
                    name='File'
                    capture={false}
                    style={{ display: 'none' }}
                    // accept="image/*;"
                    accept=".png,.jpg,.jpeg,.pxlzip,image/*;capture=camera"
                    ref={this.refFilePicker}
                    onChange={this.onFileChange}
                />
            </form>
        );
    }
}