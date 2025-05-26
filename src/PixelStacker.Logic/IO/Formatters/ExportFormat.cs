using System;
using PixelStacker.Logic.Model;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using PixelStacker.Logic.IO.Config;

namespace PixelStacker.Logic.IO.Formatters
{
    public enum ExportFormat
    {
        Jpeg,
        JpegPreview,
        Png,
        PngPreview,
        PixelStackerProject,
        BlockCountCsv,
        Schem2,
        Schem3,
        StructureBlock,
    }

    public static class ExportFormatExtensions
    {
        public static IExportFormatter GetFormatter(this ExportFormat format)
        {
            switch (format)
            {
                case ExportFormat.Jpeg:
                    return new JpegFormatter();
                case ExportFormat.Png:
                    return new PngFormatter();
                case ExportFormat.JpegPreview:
                    return new JpegPreviewFormatter();
                case ExportFormat.PngPreview:
                    return new PngPreviewFormatter();
                case ExportFormat.BlockCountCsv:
                    return new BlockCountCsvFormatter();
                case ExportFormat.PixelStackerProject:
                    return new PixelStackerProjectFormatter();
                case ExportFormat.StructureBlock:
                    return new StructureBlockFormatter();
                case ExportFormat.Schem2:
                    return new Schem2Formatter();
                case ExportFormat.Schem3:
                    return new Schem3Formatter();
                default:
                    throw new ArgumentException(nameof(format));
            }
        }

        public static async Task SaveToFile(this IExportFormatter format, string filePath, PixelStackerProjectData canvas, CancellationToken? worker)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            byte[] data = await format.ExportAsync(canvas, worker);
            await File.WriteAllBytesAsync(filePath, data);
        }

        public static async Task SaveToFile(this IExportImageFormatter format, string filePath, PixelStackerProjectData canvas, IReadonlyCanvasViewerSettings srs, CancellationToken? worker)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            byte[] data = await format.ExportAsync(canvas, srs, worker);
            await File.WriteAllBytesAsync(filePath, data);
        }

        public static (string contentType, string fileExt) GetContentTypeData(this ExportFormat format)
        {
            switch (format)
            {
                case ExportFormat.Jpeg:
                case ExportFormat.JpegPreview:
                    return ("image/jpeg", ".jpg");
                case ExportFormat.Png:
                case ExportFormat.PngPreview:
                    return ("image/png", ".png");
                case ExportFormat.BlockCountCsv:
                    return ("application/octet-stream", ".csv");
                case ExportFormat.PixelStackerProject:
                    return ("application/octet-stream", ".pxlzip");
                case ExportFormat.Schem2:
                case ExportFormat.Schem3:
                    return ("application/octet-stream", ".schem");
                case ExportFormat.StructureBlock:
                    return ("application/octet-stream", ".nbt");
                default:
                    throw new ArgumentException(nameof(format));
            }
        }
    }
}
