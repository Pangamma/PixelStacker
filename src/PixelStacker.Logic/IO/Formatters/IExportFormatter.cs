using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.IO.Formatters
{
    public interface IExportImageFormatter : IExportFormatter
    {
        Task<byte[]> ExportAsync(PixelStackerProjectData canvas, IReadonlyCanvasViewerSettings srs, CancellationToken? worker = null);
    }

    public interface IExportFormatter
    {
        //Task ExportAsync(string filePath, PixelStackerProjectData canvas, CancellationToken? worker = null);
        Task<byte[]> ExportAsync(PixelStackerProjectData canvas, CancellationToken? worker = null);


        public static Schem2Details ConvertCanvasToDetails(PixelStackerProjectData canvas)
        {
            bool isv = canvas.IsSideView;
            bool isMultiLayer = canvas.CanvasData.Any(x => canvas.MaterialPalette[x.PaletteID].IsMultiLayer);
            Material[][][] region;
            var details = new Schem2Details();
            var canvasData = canvas.CanvasData;

            int xD, yD, zD;

            if (isv)
            {
                xD = canvasData.Width;
                yD = canvasData.Height;
                zD = isMultiLayer ? 3 : 1;

                if (canvas.WorldEditOrigin != null)
                {
                    details.MetaData.WEOffsetX = -canvas.WorldEditOrigin.X;
                    details.MetaData.WEOffsetY = canvas.WorldEditOrigin.Y - yD;
                    details.MetaData.WEOffsetZ = -zD;
                }
            }
            else
            {
                xD = canvasData.Width;
                yD = isMultiLayer ? 2 : 1;
                zD = canvasData.Height;

                if (canvas.WorldEditOrigin != null)
                {
                    details.MetaData.WEOffsetX = -canvas.WorldEditOrigin.X;
                    details.MetaData.WEOffsetY = 0;
                    details.MetaData.WEOffsetZ = -canvas.WorldEditOrigin.Y;
                }
            }

            region = new Material[xD][][];
            for (int xi = 0; xi < xD; xi++)
            {
                region[xi] = new Material[yD][];
                for (int yi = 0; yi < yD; yi++)
                {
                    region[xi][yi] = new Material[zD];
                }
            }

            details.RegionXYZ = region;
            details.WidthX = xD;
            details.HeightY = yD;
            details.LengthZ = zD;

            if (isv)
            {
                for (int xr = 0; xr < xD; xr++)
                {
                    for (int yr = 0; yr < yD; yr++)
                    {
                        MaterialCombination mm = canvasData[xr, yD - 1 - yr];

                        if (isMultiLayer)
                        {
                            region[xr][yr][0] = mm.Top;
                            region[xr][yr][1] = mm.Bottom;
                            region[xr][yr][2] = mm.Top;
                        }
                        else
                        {
                            region[xr][yr][0] = mm.Bottom;
                        }
                    }
                }
            }
            else
            {
                for (int xr = 0; xr < xD; xr++)
                {
                    for (int zr = 0; zr < zD; zr++)
                    {
                        MaterialCombination mm = canvasData[xr, zr]; // WARN: Maybe this needs to be zD - 1 - zr

                        if (isMultiLayer)
                        {
                            region[xr][0][zr] = mm.Bottom;
                            region[xr][1][zr] = mm.Top;
                        }
                        else
                        {
                            region[xr][0][zr] = mm.Bottom;
                        }
                    }
                }
            }

            return details;
        }

    }

    #region Supporting Models

    public class SchemMetaData
    {
        public string Name { get; set; } = "NameOfSchematic";
        public string Author { get; set; } = "Taylor Love";
        public int WEOriginX => 1;
        public int WEOriginY => 1;
        public int WEOriginZ => 1;
        public int? WEOffsetX { get; set; }
        public int? WEOffsetY { get; set; }
        public int? WEOffsetZ { get; set; }
    }

    public class Schem2Details
    {
        public SchemMetaData MetaData { get; set; }

        // X Y Z
        public Material[][][] RegionXYZ { get; set; }
        public int WidthX { get; set; }
        public int LengthZ { get; set; }
        public int HeightY { get; set; }

        public Schem2Details()
        {
            MetaData = new SchemMetaData();
        }
    }

    public static class ExportFormatterHelper
    {


    }

    #endregion
}
