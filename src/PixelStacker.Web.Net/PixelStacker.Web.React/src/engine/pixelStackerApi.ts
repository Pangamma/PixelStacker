import { Constants } from "@/constants";
import DeploymentInfo from "@/models/deplymentInfo";

export class PixelStackerAPI {
    private endpoint: string;
    constructor(apiEndpoint: string = Constants.API_ENDPOINT) {
        this.endpoint = apiEndpoint;
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