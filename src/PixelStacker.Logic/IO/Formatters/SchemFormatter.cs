using PixelStacker.Logic.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.IO.Formatters
{
    public class SchemFormatter : IExportFormatter
    {
        public Task ExportAsync(string filePath, PixelStackerProjectData canvas, CancellationToken? worker = null)
        {
            throw new NotImplementedException();
        }
    }
}
