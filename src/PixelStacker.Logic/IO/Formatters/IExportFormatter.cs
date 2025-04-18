using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.IO.Formatters
{
    public interface IExportImageFormatter : IExportFormatter
    {
        Task<byte[]> ExportAsync(PixelStackerProjectData canvas, IReadonlyCanvasViewerSettings srs, CancellationToken? worker = null);
    }

    public interface IExportFormatter
    {
        Task ExportAsync(string filePath, PixelStackerProjectData canvas, CancellationToken? worker = null);
        Task<byte[]> ExportAsync(PixelStackerProjectData canvas, CancellationToken? worker = null);
    }
}
