using PixelStacker.Logic.Model;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.IO.Formatters
{
    public interface IImportFormatter
    {
        Task<RenderedCanvas> ImportAsync(string filePath, CancellationToken? worker = null);
        bool CanImportFile(string filePath);
    }
}
