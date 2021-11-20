using PixelStacker.IO;
using PixelStacker.Logic.Model;
using PixelStacker.Resources.Localization;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace PixelStacker
{
    public partial class ErrorSender : Form, ILocalized
    {
        public Exception CurrentException { get; set; } = null;
        public ErrorReportInfo ReportInfo { get; internal set; }

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

        private async void btnYes_Click(object sender, EventArgs e)
        {
            string filePath = "pixelstacker-error-report.zip";
            File.Delete(filePath);

            string userComment = "Not implemented yet.";
            var bytes = await ErrorReporter.SendExceptionInfoToZipBytes(System.Threading.CancellationToken.None, this.CurrentException, this.ReportInfo, cbxIncludeImage.Checked, userComment);
            if (bytes == null)
            {
                MessageBox.Show("Error occurred while saving the exception report. What are the odds!");
            }
            else
            {
                File.WriteAllBytes(filePath, bytes);
                MessageBox.Show("The error report is saved.", "Error report complete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://github.com/Pangamma/PixelStacker/issues");  // Adf.ly
            Process.Start(sInfo);
        }

        public void ApplyLocalization(System.Globalization.CultureInfo locale)
        {
            ComponentResourceManager resources = new ComponentResourceManager(this.GetType());
            foreach (Control c in this.Controls)
            {
                resources.ApplyResources(c, c.Name, locale);
            }
        }
    }
}
