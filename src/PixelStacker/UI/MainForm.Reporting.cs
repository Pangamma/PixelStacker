using PixelStacker.Logic.Model;

namespace PixelStacker.UI
{
    public partial class MainForm
    {
        public static ErrorReportInfo GetErrorReportInfo(MainForm mf) => new ErrorReportInfo
        {
            LoadedImage = mf?.LoadedImage,
            PreprocessedImage = mf?.PreprocessedImage,
            Options = mf?.Options,
            RenderedCanvas = mf?.RenderedCanvas,
            ColorMapper = mf?.ColorMapper
        };
    }
}
