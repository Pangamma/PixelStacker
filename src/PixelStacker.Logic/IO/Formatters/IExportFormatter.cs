using PixelStacker.Logic.Model;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.IO.Formatters
{
    public interface IExportFormatter
    {
        Task ExportAsync(string filePath, PixelStackerProjectData canvas, CancellationToken? worker = null);
    }
}
