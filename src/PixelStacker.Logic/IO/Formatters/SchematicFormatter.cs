using PixelStacker.Logic.Model;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.IO.Formatters
{
    public class SchematicFormatter : IExportFormatter
    {
        public async Task ExportAsync(string filePath, PixelStackerProjectData canvas, CancellationToken? worker = null)
            => File.WriteAllBytes(filePath, await this.ExportAsync(canvas, worker));

        public Task<byte[]> ExportAsync(PixelStackerProjectData canvas, CancellationToken? worker = null)
        {
            throw new NotImplementedException();
        }
    }
}
