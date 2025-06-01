export interface Material {
    pixelStackerID: string;
    label?: string;

    /** The X offset in the spritesheet page */
    spriteX: number;
    /** The Y offset in the spritesheet page */
    spriteY: number;
    /** The index for the sprite sheet page */
    spritePage: number;
}