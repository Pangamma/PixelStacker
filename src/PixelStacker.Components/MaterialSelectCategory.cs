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
    public partial class MaterialSelectCategory<TModel> : UserControl where TModel: Control
    {
        public List<TModel> Materials { get; set; } = new List<TModel>();
        public string CategoryName { get => this.cbxEnableAll.Text; set => this.cbxEnableAll.Text = value; }
        public Func<TModel, bool> _IsEnabled { get; set; }
        public Action<TModel, bool> _SetEnabled { get; set; }

        public MaterialSelectCategory()
        {
            InitializeComponent();
        }

        public void RedrawMaterialTiles()
        {
            this.customFlowLayoutPanel1.Controls.Clear();
            this.customFlowLayoutPanel1.Controls.AddRange(this.Materials.ToArray());
        }

        private void cbxEnableAll_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxEnableAll.CheckState == CheckState.Checked)
            {
                this.Materials.ForEach(m => this._SetEnabled(m, true));
                //this.cbxEnableAll.CheckState = CheckState.Unchecked;
            }
            else if (cbxEnableAll.CheckState == CheckState.Unchecked)
            {
                this.Materials.ForEach(m => this._SetEnabled(m, false));
                //this.cbxEnableAll.CheckState = CheckState.Checked;
            }
            else // indeterminate
            {
                int numEnabled = this.Materials.Count(m => this._IsEnabled(m));
                if (numEnabled < this.Materials.Count / 2)
                {
                    this.Materials.ForEach(m => this._SetEnabled(m, false));
                    //this.cbxEnableAll.CheckState = CheckState.Checked;
                }
                else
                {
                    this.Materials.ForEach(m => this._SetEnabled(m, false));
                    //this.cbxEnableAll.CheckState = CheckState.Unchecked;
                }
            }

            this.Refresh();
        }
    }
}
