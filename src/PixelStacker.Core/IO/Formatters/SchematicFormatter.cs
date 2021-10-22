using PixelStacker.Core.Model;

namespace PixelStacker.Core.IO.Formatters
{
    public class SchematicFormatter : IExportFormatter
    {
        public Task ExportAsync(string filePath, RenderedCanvas canvas, CancellationToken? worker = null)
        {
            throw new NotImplementedException();
        }
    }
}
