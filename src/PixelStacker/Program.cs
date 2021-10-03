using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using PixelStacker.Logic;
using PixelStacker.Logic.Model;
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

            var canvas = new RenderedCanvas() { 
                Width = 5,
                OriginalImage = Resources.Textures.acacia_planks
            };

            string json = JsonConvert.SerializeObject(canvas);
            var canvas2 = JsonConvert.DeserializeObject<RenderedCanvas>(json);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());
        }
    }
}
