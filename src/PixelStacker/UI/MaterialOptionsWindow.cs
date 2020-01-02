using PixelStacker.Logic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace PixelStacker.UI
{
    public partial class MaterialOptionsWindow : Form
    {
        private MainForm mainForm;
        private List<MaterialList> refMaterialLists;

        public MaterialOptionsWindow()
        {
            DoubleBuffered = true;
            this.refMaterialLists = new List<MaterialList>();
        }

        public MaterialOptionsWindow(MainForm mainForm)
        {
            this.mainForm = mainForm;
            DoubleBuffered = true;
            this.refMaterialLists = new List<MaterialList>();
            this.SuspendLayout();
            InitializeComponent();
            addMaterialList("Wool");
            addMaterialList("Powder");
            addMaterialList("Concrete");
            addMaterialList("Clay");
            addMaterialList("Glass");
            addMaterialList("Terracotta");
            addMaterialList("Good");
            addMaterialList("Okay");
            addMaterialList("Planks");
            addMaterialList("Stripped");
            addMaterialList("Logs");
            addMaterialList("Solid Ores");
            addMaterialList("Coral");
            addMaterialList("Dead Coral");
            addMaterialList("Ores");
            addMaterialList("Common");
            cbxEnableLayer2.Checked = Options.Get.IsMultiLayer;
            cbxIsSideView.Checked = Options.Get.IsSideView;
            this.ResumeLayout(true);
        }

        public void addMaterialList(string Category)
        {
            if (Materials.List.Any(x => x.Category == Category))
            {
                this.tableLayoutPanel.RowCount = this.tableLayoutPanel.RowCount + 1;
                this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
                var materialList10 = new PixelStacker.UI.MaterialList(Category);
                materialList10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                materialList10.BackColor = System.Drawing.Color.Transparent;
                materialList10.Location = new System.Drawing.Point(3, 3);
                materialList10.MinimumSize = new System.Drawing.Size(0, 100);
                materialList10.Name = "materialList_" + Category.Replace(" ", "");
                materialList10.Size = new System.Drawing.Size(905, 100);
                materialList10.TabIndex = 0;
                this.refMaterialLists.Add(materialList10);
                this.tableLayoutPanel.Controls.Add(materialList10, 0, this.tableLayoutPanel.Controls.Count);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbx = (CheckBox)sender;
            bool isChecked = cbx.CheckState == CheckState.Checked;
            Options.Get.IsMultiLayer = isChecked;
        }

        private async void PixelArtOptionsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Options.Save();
            await TaskManager.Get.StartAsync((token) =>
            {
                Materials.CompileColorMap(token, true);
            });

            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void cbxIsSideView_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbx = (CheckBox)sender;
            bool isChecked = cbx.CheckState == CheckState.Checked;
            Options.Get.IsSideView = isChecked;
            var tols = this.tableLayoutPanel.Controls.OfType<MaterialList>();
            foreach (var ml in tols)
            {
                ml.RepaintTextures();
            }
        }

        private void nbrGridSize_ValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown)sender;
            Options.Get.GridSize = Convert.ToInt32(num.Value);
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

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            this.refMaterialLists.ForEach(ML => {
                ML.setAllChecked(true);
            });
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            this.refMaterialLists.ForEach(ML => {
                ML.setAllChecked(false);
            });

        }
    }
}
