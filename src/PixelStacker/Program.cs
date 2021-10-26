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
            ResourceHelper.Get();
            //Options opts = new LocalDataOptionsProvider().Load();
            //var model = new CustomGridView();
            //model.Add<Options, bool>(opts => opts.IsSideView);
            //model.Add<Options, bool>(opts => opts.Preprocessor.IsSideView);



            ResxHelper.InjectIntoTextResx();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new MainForm());
        }

    }
}
