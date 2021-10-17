using PixelStacker.Extensions;
using PixelStacker.IO;
using PixelStacker.IO.Config;
using PixelStacker.Logic.Collections;
using PixelStacker.Logic.Collections.ColorMapper;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using PixelStacker.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI
{
    public partial class MainForm : Form
    {
        private Options Options;
        private IColorMapper ColorMapper;
        private MaterialPalette Palette;
        public Bitmap LoadedImage { get; private set; } = UIResources.psg.To32bppBitmap();
        private RenderedCanvas RenderedCanvas;

        public MainForm()
        {
            this.Options = new LocalDataOptionsProvider().Load();
            this.ColorMapper = new KdTreeMapper();
            this.Palette = MaterialPalette.FromResx();

            InitializeComponent();
            this.imageViewer.SetImage(this.LoadedImage);
            ShowImageViewer();
            this.lblProgress.Parent = this.progressBar1;
            this.lblProgress.BackColor = Color.Transparent;


            // Localization
            System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(this.Options.Locale ?? "en-us");
            ApplyLocalization(System.Threading.Thread.CurrentThread.CurrentUICulture);
        }

        [DebuggerStepThrough]
        private void timer1_Tick(object sender, EventArgs e)
        {
            ProgressX.UpdateStatus((val, str) =>
            {
                if (progressBar1.Value != val)
                {
                    progressBar1.Value = val;
                }

                if (lblProgress.Text != str)
                {
                    lblProgress.Text = str;
                }
            });
        }
    }
}
