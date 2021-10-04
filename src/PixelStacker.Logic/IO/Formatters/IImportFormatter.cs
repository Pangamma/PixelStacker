using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.IO.Formatters
{
    public interface IImportFormatter
    {
        Task<RenderedCanvas> ImportAsync(string filePath, CancellationToken? worker = null);
    }
}
