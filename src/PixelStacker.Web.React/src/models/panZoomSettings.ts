export interface PanZoomSettings {
    /// <summary>
    /// Used when measuring drag distances. Starts as equal to imageX 
    /// or imageY, and is then compared to "current drag point" with a
    /// UI element of some kind.
    /// </summary>
    initialImageX: number; // 0

    /// <summary>
    /// Used when measuring drag distances. Starts as equal to imageX 
    /// or imageY, and is then compared to "current drag point" with a
    /// UI element of some kind.
    /// </summary>
    initialImageY: number; // 0

    imageY: number; // 0
    imageX: number; // 0

    /// <summary>
    /// Current zoom level. Acts as a multiplier for tile size.
    /// float/double
    /// </summary>
    zoomLevel: number; // 0

    /// <summary>
    /// Prevent zooming IN further than this
    /// float/double
    /// default should be 100.0
    /// </summary>
    maxZoomLevel: number; // 100.0

    /// <summary>
    /// Prevent zooming further away than this
    /// float/double
    /// </summary>
    minZoomLevel: number; // 0.0

    // public PanZoomSettings Clone()
    // {
    //     return new PanZoomSettings()
    //     {
    //         initialImageX = this.initialImageX,
    //         initialImageY = this.initialImageY,
    //         imageX = this.imageX,
    //         imageY = this.imageY,
    //         zoomLevel = this.zoomLevel,
    //         maxZoomLevel = this.maxZoomLevel,
    //         minZoomLevel = this.minZoomLevel
    //     };
    // }

    // public PanZoomSettings() { }

    // public PanZoomSettings TranslateForNewSize(int srcWidth, int srcHeight, int viewerWidth, int viewerHeight)
    // {
    //     var settings = this.Clone();
    //     // TODO the imageX and imageY and zoomLevel
    //     settings.minZoomLevel = Math.Min(1.0D, (100.0D / Math.Max(srcWidth, srcHeight)));

    //     return settings;
    // }

    // /// <summary>
    // /// TAKEN FROM... VIEWER PANEL
    // /// </summary>
    // /// <param name="srcWidth"></param>
    // /// <param name="srcHeight"></param>
    // /// <param name="viewerWidth"></param>
    // /// <param name="viewerHeight"></param>
    // /// <returns></returns>
    // public static PanZoomSettings CalculateDefaultPanZoomSettings(int srcWidth, int srcHeight, int viewerWidth, int viewerHeight)
    // {
    //     var settings = new PanZoomSettings()
    //     {
    //         initialImageX = 0,
    //         initialImageY = 0,
    //         imageX = 0,
    //         imageY = 0,
    //         zoomLevel = 0,
    //         maxZoomLevel = Constants.MAX_ZOOM,
    //         minZoomLevel = Constants.MIN_ZOOM
    //     };

    //     double wRatio = (double)viewerWidth / srcWidth;
    //     double hRatio = (double)viewerHeight / srcHeight;
    //     if (hRatio < wRatio)
    //     {
    //         settings.zoomLevel = hRatio;
    //         settings.imageX = (viewerWidth - (int)(srcWidth * hRatio)) / 2;
    //     }
    //     else
    //     {
    //         settings.zoomLevel = wRatio;
    //         settings.imageY = (viewerHeight - (int)(srcHeight * wRatio)) / 2;
    //     }

    //     int numICareAbout = Math.Max(srcWidth, srcHeight);
    //     settings.minZoomLevel = (100.0D / numICareAbout);
    //     if (settings.minZoomLevel > 1.0D)
    //     {
    //         settings.minZoomLevel = 1.0D;
    //     }

    //     return settings;
    // }
}

export const translatePanZoomSettingsForNewSize
    = (original: PanZoomSettings, srcWidth: number, srcHeight: number, viewerWidth: number, viewerHeight: number) => {
        var settings = { ...original };
        // TODO the imageX and imageY and zoomLevel
        settings.minZoomLevel = Math.min(1.0, (100.0 / Math.max(srcWidth, srcHeight)));
        return settings;
    }

export const getDefaultPanZoomSettings = () => ({
    initialImageX: 0,
    initialImageY: 0,
    imageY: 5,
    imageX: 20,
    zoomLevel: 4,
    maxZoomLevel: 200.0,
    minZoomLevel: 1,
} as PanZoomSettings);
/// <summary>
/// TAKEN FROM... VIEWER PANEL
/// </summary>
/// <param name="srcWidth"></param>
/// <param name="srcHeight"></param>
/// <param name="viewerWidth"></param>
/// <param name="viewerHeight"></param>
/// <returns></returns>
export const calculateDefaultPanZoomSettings = (srcWidth: number, srcHeight: number, viewerWidth: number, viewerHeight: number) => {
    var settings =
    {
        initialImageX: 0,
        initialImageY: 0,
        imageX: 0,
        imageY: 0,
        zoomLevel: 0,
        maxZoomLevel: 200,
        minZoomLevel: 200
    };

    let wRatio = viewerWidth / srcWidth;
    let hRatio = viewerHeight / srcHeight;
    if (hRatio < wRatio) {
        settings.zoomLevel = hRatio;
        settings.imageX = (viewerWidth - Math.floor(srcWidth * hRatio)) / 2;
    }
    else {
        settings.zoomLevel = wRatio;
        settings.imageY = (viewerHeight - Math.floor(srcHeight * wRatio)) / 2;
    }

    let numICareAbout = Math.max(srcWidth, srcHeight);
    settings.minZoomLevel = (100 / numICareAbout);
    if (settings.minZoomLevel > 1) {
        settings.minZoomLevel = 1;
    }

    return settings;
};