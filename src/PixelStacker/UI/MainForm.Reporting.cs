using PixelStacker.Logic;
using PixelStacker.Logic.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker
{
    partial class MainForm
    {
        /// <summary>
        /// Triggered when exception occurs off of UI thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ErrorSender()
            {
                CurrentException = (Exception) e.ExceptionObject
            });
        }

        /// <summary>
        /// Catches exceptions on the UI thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            if (e.Exception != null)
            {
                var ui = new ErrorSender()
                {
                    CurrentException = e.Exception
                };

                ui.Show();
            }
        }
    }
}
