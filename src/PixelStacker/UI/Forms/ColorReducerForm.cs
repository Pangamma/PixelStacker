using PixelStacker.Resources.Localization;
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

namespace PixelStacker.UI.Forms
{
    public partial class ColorReducerForm : Form, ILocalized
    {
        public ColorReducerForm()
        {
            InitializeComponent();
        }

        public void ApplyLocalization(CultureInfo locale)
        {
            lblInstructions.Text = Resources.Text.ColorReducer_Instructions;
        }
    }
}
