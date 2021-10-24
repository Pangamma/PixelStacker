using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.IO.Formatters
{
    public class SvgFormatter : IExportFormatter
    {
        public async Task ExportAsync(string filePath, PixelStackerProjectData canvas, CancellationToken? worker)
        {
            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"<svg xmlns='http://www.w3.org/2000/svg' width='{canvas.PreprocessedImage.Width * 16}' height='{canvas.PreprocessedImage.Height * 16}'>");
                sb.AppendLine("<style>");
                sb.AppendLine(".mat-tile { height: 16px; width: 16px; }");

                HashSet<int> UniquePaletteIDs = new HashSet<int>();
                foreach(var tile in canvas.CanvasData)
                {
                    if (!UniquePaletteIDs.Contains(tile.PaletteID))
                    {
                        UniquePaletteIDs.Add(tile.PaletteID);
                        sb.Append($" .mc{tile.PaletteID} {{");
                        var bmTile = canvas.MaterialPalette[tile.PaletteID].GetImage(canvas.IsSideView);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            bmTile.Save(ms, ImageFormat.Png);
                            byte[] byteImage = ms.ToArray();
                            var SigBase64 = Convert.ToBase64String(byteImage); // Get Base64
                            sb.Append($"background: url('data:image/png;base64,{SigBase64}');");
                        }
                            
                        sb.AppendLine("}");
                    }
                }
                
                sb.AppendLine("</style>");
                foreach (var tile in canvas.CanvasData)
                {
                    sb.AppendLine($"<image class='mat-tile mc{tile.PaletteID}' width='16' height='16' />");
                }
                //<linearGradient id='gradient'><stop offset='10%' stop-color='#F00'/><stop offset='90%' stop-color='#fcc'/> </linearGradient><rect fill='url(#gradient)' x='0' y='0' width='100%' height = '100%' />

                sb.AppendLine("</svg>");

                string str = sb.ToString();
                await File.WriteAllTextAsync(filePath, str);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
