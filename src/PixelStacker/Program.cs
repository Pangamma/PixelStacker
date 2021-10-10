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
using PixelStacker.UI;

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
            var options = provider.Load();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = new MaterialSelectWindow(options);
            Application.Run(form);
            //Application.Run(new Form1());
        }
    }
}
