using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
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

        private void lnkFontMaker_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://mvc.lumengaming.com/v/fontmaker");  // Adf.ly
            Process.Start(sInfo);
        }

        private void lnkLumenGaming_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://mvc.lumengaming.com/v/lg");  // Adf.ly
            Process.Start(sInfo);
        }

        private void lnkWoolcityProject_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://mvc.lumengaming.com/v/wc_project");  // Adf.ly
            Process.Start(sInfo);
        }

        private void lnkLinkedIn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://mvc.lumengaming.com/v/linkedin");  // Adf.ly
            Process.Start(sInfo);
        }

        private void lnkDonate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://www.paypal.me/TaylorLove");
            Process.Start(sInfo);
        }

        private void lnkDownload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://www.spigotmc.org/resources/pixelstacker.46812/");  // Adf.ly
            Process.Start(sInfo);
        }
    }
}
