using PixelStacker.Logic.Model;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.IO.Formatters
{
    public interface IExportFormatter
    {
        Task ExportAsync(string filePath, RenderedCanvas canvas, CancellationToken? worker = null);
    }

    public interface IExportFormatterWithViewOptions : IExportFormatter
    {
        Task ExportAsync(string filePath, ViewingOptions viewOpts, RenderedCanvas canvas, CancellationToken? worker = null);
    }

    public class ViewingOptions {
        // Contain things like showing the worldedit origin,
        // the grids
        // the shadows
        // the hidden or shown blocks...
    }
}
