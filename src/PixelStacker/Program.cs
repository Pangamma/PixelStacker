using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using PixelStacker.Logic;
using PixelStacker.UI;

namespace PixelStacker
{
    static class Program
    {
        // TODO: Add grasses and dirts
        // TODO: CalculateTextureSize should be a stand-alone method which takes in:
        // - Original image dimensions
        // - Calculated output image dimensions
        // TODO: PanZoomSettings generator based on above inputs ^
        // TODO: Better versioning output format.
        // composter, loom
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
