using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.IO.Formatters
{
    public interface IExportFormatter
    {
        Task ExportAsync(string filePath, RenderedCanvas canvas, CancellationToken? worker = null);
    }
}
