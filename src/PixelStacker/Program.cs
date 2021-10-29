using System;
using System.Windows.Forms;
using PixelStacker.UI;
using PixelStacker.Resources;
using PixelStacker.UI.Forms;
using PixelStacker.Logic.IO.Config;
using PixelStacker.IO;

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
            //ResxHelper.InjectIntoTextResx();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new MainForm());
        }

    }
}
