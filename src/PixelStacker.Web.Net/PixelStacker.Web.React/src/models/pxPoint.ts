export interface PxPoint {
    x: number;
    y: number;
}

export class SKSize {
    width: number;
    height: number;
    constructor(w: number, h: number){
        this.width = w;
        this.height = h;
    }
}

export class SKRect  {
    width: number;
    height: number;
    x: number;
    y: number;
    constructor(x: number, y: number, w: number, h: number){
        this.width = w;
        this.height = h;
        this.x = x;
        this.y = y;
    }
}