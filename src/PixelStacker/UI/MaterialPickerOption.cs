using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PixelStacker.Logic;

namespace PixelStacker.UI
{
    public partial class MaterialPickerOption : UserControl
    {
        private Action<MaterialPickerOption> onClick = (b) => { };
        private Material[] materials = new Material[0];

        public bool IsFocused { get; set; } = false;

        public MaterialPickerOption()
        {
            InitializeComponent();
        }

        public MaterialPickerOption(Action<MaterialPickerOption> onClick, Material[] ms)
        {
            this.onClick = onClick;
            this.materials = ms;
            InitializeComponent();
        }

        private void MaterialPickerOption_MouseEnter(object sender, EventArgs e)
        {
            IsFocused = true;
            this.BackColor = SystemColors.ControlLight;
        }

        private void MaterialPickerOption_MouseLeave(object sender, EventArgs e)
        {
            IsFocused = false;
            this.BackColor = SystemColors.Control;
        }

        private void MaterialPickerOption_Click(object sender, EventArgs e)
        {
            if (onClick != null)
            {
                onClick.Invoke(this);
            }
        }
    }
}
