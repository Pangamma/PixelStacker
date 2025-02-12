﻿using System;

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
                case ExportFormat.Schem2:
                    return new Schem2Formatter();
                default:
                    throw new ArgumentException(nameof(format));
            }
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
                    return ("application/octet-stream", ".schem");
                default:
                    throw new ArgumentException(nameof(format));
            }
        }
    }
}
