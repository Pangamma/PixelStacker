using PixelStacker.IO;
using PixelStacker.UI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI.Forms
{
    public partial class TestForm : Form
    {
        private SizeForm form;
        public SnapManager SnapManager { get; }

        public TestForm()
        {
            InitializeComponent();
        }
    }
}
