using PixelStacker.IO;
using PixelStacker.IO.Config;
using PixelStacker.Logic.Collections;
using PixelStacker.Logic.Collections.ColorMapper;
using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        public MainForm()
        {
            this.Options = new LocalDataOptionsProvider().Load();
            this.ColorMapper = new KdTreeMapper();
            this.Palette = MaterialPalette.FromResx();
            InitializeComponent();

            System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(this.Options.Locale ?? "en-us");
            ApplyLocalization(System.Threading.Thread.CurrentThread.CurrentUICulture);
        }
    }
}
