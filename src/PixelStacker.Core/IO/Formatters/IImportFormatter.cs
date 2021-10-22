using PixelStacker.Core.Model;

namespace PixelStacker.Core.IO.Formatters
{
    public interface IImportFormatter
    {
        Task<RenderedCanvas> ImportAsync(string filePath, CancellationToken? worker = null);
        bool CanImportFile(string filePath);
    }
}
