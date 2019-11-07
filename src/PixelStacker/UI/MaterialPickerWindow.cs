using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI
{
    public partial class MaterialPickerWindow : Form
    {
        public MaterialPickerWindow()
        {
            InitializeComponent();
        }

        public MaterialPickerWindow AddOption(string labelText, Action a)
        {

            return this;
        }

        /// <summary>
        /// When the control goes off the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaterialPickerWindow_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void materialPickerOption2_Load(object sender, EventArgs e)
        {

        }
    }
}
