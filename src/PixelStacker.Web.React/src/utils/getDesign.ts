// The main function of this file is to take the base64
// data from a PNG (the one w/ palette IDs) and extract
// the design from it. A couple separate functions are
// used to do this.

function getImage(base64Data: string, callback: Function) {
	let imgElement = new Image();
	imgElement.addEventListener("load", () => callback(imgElement));
	imgElement.src = `data:image/png;base64,${base64Data}`;
}
function getPixelData(image: HTMLImageElement): ImageData {
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

// Technically a nested array but who cares
function getIdArray({ width, height, data }: ImageData): number[][] {
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

export function getDesign(base64Image: string, callback: Function) {
	getImage(base64Image, (img: HTMLImageElement) => {
		callback(getIdArray(getPixelData(img)));
	});
}

// // The main function of this file is to take the base64
// // data from a PNG (the one w/ palette IDs) and extract
// // the design from it. A couple separate functions are
// // used to do this.

// function getImage(base64Data: string, callback: Function) {
// 	let imgElement = new Image();
// 	imgElement.addEventListener("load", () => callback(imgElement));
// 	imgElement.src = `data:image/png;base64,${base64Data}`;
// }
// function getPixelData(image: HTMLImageElement): ImageData {
// 	const sampleCanvas = document.createElement("canvas");
// 	const sampleCtx = sampleCanvas.getContext("2d");

// 	// Appease ts
// 	if (sampleCtx === null) return new ImageData(0, 0);

// 	sampleCanvas.width = image.width;
// 	sampleCanvas.height = image.height;

// 	sampleCtx.clearRect(0, 0, image.width, image.height);
// 	sampleCtx.drawImage(image, 0, 0);

// 	return sampleCtx.getImageData(0, 0, image.width, image.height);
// }

// // Technically a nested array but who cares
// function getIdArray({ width, height, data }: ImageData): number[][] {
// 	let output = [];
// 	for (let x = 0; x < width; x++) {
// 		let toAdd = [];
// 		for (let y = 0; y < height; y++) {
// 			let idx = (y * width + x) * 4,
// 				r: number = data[idx],
// 				g: number = data[idx + 1],
// 				b: number = data[idx + 2],
// 				a: number = data[idx + 3];

// 			toAdd.push(r * 256 * 256 + g * 256 + b);
// 		}
// 		output.push(toAdd);
// 	}

// 	return output;
// }

// // this.getImage(image, (el: HTMLImageElement) => {
// // 	let design = getIdArray(this.getPixelData(el));
// // 	console.log(palette, design);
// // });

// export function getDesign(base64Image: string, callback: Function) {
// 	getImage(base64Image, (img: HTMLImageElement) => {
// 		callback(getIdArray(getPixelData(img)));
// 	});
// }
