using PixelStacker.Logic;
using System;
using System.Collections.Generic;
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
        }
    }
}
