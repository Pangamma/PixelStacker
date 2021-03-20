using PixelStacker.Logic;
using PixelStacker.Logic.Extensions;
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
    public partial class OtherOptionsWindow : Form
    {
        private bool suspendOptionsSave = false;
        public OtherOptionsWindow()
        {
            suspendOptionsSave = true;
            InitializeComponent();
            btnGridColor.BackColor = Options.Get.GridColor;
            this.cbxIsSideView.Checked = Options.Get.IsSideView;
            this.nbrGridSize.Value = Options.Get.GridSize;
            this.cbxIsFrugalWithMaterials.Visible = Options.Get.IsAdvancedModeEnabled;
            this.cbxIsFrugalWithMaterials.Checked = Options.Get.IsExtraShadowDepthEnabled;
            this.cbxSkipShadowRendering.Checked = Options.Get.IsShadowRenderingSkipped;
            this.nbrMaxHeight.Maximum = Options.Get.IsSideView ? Constants.WORLD_HEIGHT : short.MaxValue;

            this.nbrMaxHeight.Value = Math.Min(this.nbrMaxHeight.Maximum, Options.Get.MaxHeight ?? 0);
            this.nbrMaxHeight.Text = nbrMaxHeight.Value.ToString();

            this.nbrMaxWidth.Value = Math.Min(this.nbrMaxWidth.Maximum, Options.Get.MaxWidth ?? 0);
            this.nbrMaxWidth.Text = nbrMaxWidth.Value.ToString();
            suspendOptionsSave = false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }

            MainForm.Self.konamiWatcher.ProcessKey(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void cbxIsSideView_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendOptionsSave) return;
            Options.Get.IsSideView = cbxIsSideView.Checked;

            suspendOptionsSave = true;
            nbrMaxHeight.Value = 0;
            nbrMaxWidth.Value = 0;
            suspendOptionsSave = false;

            if (cbxIsSideView.Checked)
            {
                nbrMaxHeight.Maximum = Constants.WORLD_HEIGHT;
                nbrMaxWidth.Maximum = short.MaxValue;
            }
            else
            {
                nbrMaxHeight.Maximum = short.MaxValue;
                nbrMaxWidth.Maximum = short.MaxValue;
            }

            nbrMaxHeight.Value = Math.Min(nbrMaxHeight.Maximum, Math.Max(0, Options.Get.MaxHeight ?? 0));
            nbrMaxWidth.Value = Math.Min(nbrMaxWidth.Maximum, Math.Max(0, Options.Get.MaxWidth ?? 0));

            MainForm.Self.PreRenderedImage.DisposeSafely();
            MainForm.Self.PreRenderedImage = null;
        }

        private void nbrMaxWidth_ValueChanged(object sender, EventArgs e)
        {
            if (suspendOptionsSave) return;
            int i = Convert.ToInt32(nbrMaxWidth.Value);
            int? val = i > 0 ? i : default(int?);
            Options.Get.MaxWidth = val;
            MainForm.Self.PreRenderedImage.DisposeSafely();
            MainForm.Self.PreRenderedImage = null;

            MainForm.Self.InvokeEx((c) =>
            {
                c.ShowImagePanel();
            });
        }

        private void nbrMaxHeight_ValueChanged(object sender, EventArgs e)
        {
            if (suspendOptionsSave) return;
            int i = Convert.ToInt32(nbrMaxHeight.Value);
            int? val = i > 0 ? i : default(int?);
            Options.Get.MaxHeight = val;
            MainForm.Self.PreRenderedImage.DisposeSafely();
            MainForm.Self.PreRenderedImage = null;

            MainForm.Self.InvokeEx((c) =>
            {
                c.ShowImagePanel();
            });
        }

        private void nbrGridSize_ValueChanged(object sender, EventArgs e)
        {
            if (suspendOptionsSave) return;
            int i = Convert.ToInt32(nbrGridSize.Value);
            Options.Get.GridSize = i;
        }

        private void btnFactoryReset_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(Resources.Text.OtherOptions_ConfirmFactoryReset, Resources.Text.AreYouSure, MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Options.Reset();
                Options.Save();
                this.Close();
            }
        }

        private void OtherOptionsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Options.Save();
        }

        private void btnGridColor_Click(object sender, EventArgs e)
        {
            var result = colorDialogue.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                Color c = colorDialogue.Color;
                btnGridColor.BackColor = c;
                Options.Get.GridColor = c;
                Options.Save();
            }
        }

        private void cbxIsFrugalWithMaterials_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendOptionsSave) return;
            Options.Get.IsExtraShadowDepthEnabled = cbxIsFrugalWithMaterials.Checked;
        }

        private void cbxSkipShadowRendering_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendOptionsSave) return;
            Options.Get.IsShadowRenderingSkipped = cbxSkipShadowRendering.Checked;
        }
    }
}
