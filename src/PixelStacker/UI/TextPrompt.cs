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
    public partial class TextPrompt : Form
    {
        public TextPrompt()
        {
            InitializeComponent();
        }

        public TextPrompt(string promptInstructions)
        {
            InitializeComponent();

            lblPromptText.Text = promptInstructions;
        }

        public string Value
        {
            get { return tbText.Text.Trim(); }
            set { tbText.Text = value; }
        }


        private void BtnSubmitText_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void TextPrompt_Load(object sender, EventArgs e)
        {
            CenterToParent();
        }
    }
}
