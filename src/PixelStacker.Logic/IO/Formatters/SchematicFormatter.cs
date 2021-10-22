using PixelStacker.Logic.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.IO.Formatters
{
    public class SchematicFormatter : IExportFormatter
    {
        public Task ExportAsync(string filePath, RenderedCanvas canvas, CancellationToken? worker = null)
        {
            throw new NotImplementedException();
        }
    }
}
