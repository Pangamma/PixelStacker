using PixelStacker.Logic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
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
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += MainForm.OnThreadException;

            SetLocaleByTextureSize(Constants.TextureSize);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static void SetLocaleByTextureSize(int textureSize)
        {
            switch (textureSize)
            {
                case 16:
                    Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-us");
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-us");
                    break;
                case 32:
                    Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ja-jp");
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ja-jp");
                    break;
                case 64:
                    Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ko-kr");
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ko-kr");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
