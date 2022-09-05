export class CanvasViewerSettings {
    // GridSize: number = 16;

    // [JsonConverter(typeof(SKColorJsonTypeConverter))]
    // public SKColor GridColor { get; set; } = new SKColor(0, 0, 0);
    // public bool IsShowGrid { get; set; } = false;

    IsShowBorder: boolean = false;
    IsSolidColors: boolean = false;

    // /// <summary>
    // /// undefined = show both layers
    // /// 0 = show bottom layer
    // /// 1 = show top layer
    // /// </summary>
    // ZLayerFilter?: number = undefined;
}