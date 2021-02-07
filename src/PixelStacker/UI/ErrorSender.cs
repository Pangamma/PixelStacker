using Newtonsoft.Json;
using PixelStacker.Logic;
using PixelStacker.Logic.Collections;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.WIP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker
{
    public partial class ErrorSender : Form
    {
        public Exception CurrentException { get; set; } = null;

        public ErrorSender()
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

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            string filePath = "pixelstacker-error-report.zip";
            File.Delete(filePath);

            var ex = ErrorReportZipper.SaveError(this.CurrentException, cbxIncludeImage.Checked, filePath);
            if (ex != null)
            {
                MessageBox.Show("Error occurred while saving the exception report. What are the odds!");
            }
            else
            {
                MessageBox.Show("The error report is saved.", "Error report complete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://github.com/Pangamma/PixelStacker/issues");  // Adf.ly
            Process.Start(sInfo);
        }
    }
}
