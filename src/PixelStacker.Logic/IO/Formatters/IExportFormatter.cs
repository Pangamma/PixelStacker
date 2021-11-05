using PixelStacker.Logic.Model;
using System.Threading;
using System.Threading.Tasks;
using PixelStacker.Logic.IO.Formatters;

namespace PixelStacker.Logic.IO.Formatters
{
    public interface IExportFormatter
    {
        Task ExportAsync(string filePath, PixelStackerProjectData canvas, CancellationToken? worker = null);
        Task<byte[]> ExportAsync(PixelStackerProjectData canvas, CancellationToken? worker = null);
    }
}
