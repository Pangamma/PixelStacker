using PixelStacker.Logic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker
{
    static class Program
    {
        // TODO: USE HSL DISTINCT SELECTION, OCT TREE(SPEED)/Euclidian(Quality), No dither, 4 parallel, 256 colors
        // TODO: Add option to switch between multiple color palettes easily. (ANd label them)
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            //var fToStrip = @"D:\git\PixelStacker\src\PixelStacker\Resources\Images\UI\shadows\shadow_8_dark.png";
            //var bm = Bitmap.FromFile(fToStrip).To32bppBitmap();
            //bm.ToEditStream(null, (int x, int y, Color c) =>
            //{
            //    var b = c.GetBrightness();
            //    var nAlpha = 255 * (1 - b);
            //    return Color.FromArgb((int)nAlpha, 0, 0, 0);
            //});

            //bm.Save(fToStrip + ".stripped.png");
        }
    }
}
