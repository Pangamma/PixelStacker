using PixelStacker.IO;
using PixelStacker.IO.Config;
using PixelStacker.Logic.Engine;
using PixelStacker.Resources;
using PixelStacker.Extensions;
using PixelStacker.Resources.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace PixelStacker
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ResxHelper.InjectIntoTextResx();
            var provider = new LocalDataOptionsProvider();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
    }
}
