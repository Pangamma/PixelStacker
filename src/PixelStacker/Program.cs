using PixelStacker.IO;
using PixelStacker.Logic.Model;
using PixelStacker.Resources;
using PixelStacker.UI;
using System;
using System.IO;
using System.Windows.Forms;

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

#if !DEBUG
            try
            {
                Application.ThreadException += ErrorReporter.OnThreadException;
                AppDomain.CurrentDomain.UnhandledException += ErrorReporter.OnUnhandledException;
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
#endif
                ResxHelper.InjectIntoTextResx();
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var form = new MainForm();
                ErrorReporter.MF = form;
                //var form = new TestForm();
                Application.Run(form);

#if !DEBUG
            }
            catch (Exception ex)
            {
                byte[] errData = ErrorReporter.SendExceptionInfoToZipBytes(System.Threading.CancellationToken.None, ex, new ErrorReportInfo() { Exception = ex }, false, "Error from main try catch").Result;
                File.WriteAllBytes("pixelstacker-error.zip", errData);
            }
#endif
        }
    }
}
