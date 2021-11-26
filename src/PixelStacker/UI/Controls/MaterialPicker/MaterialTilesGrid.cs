using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI.Controls.MaterialPicker
{
    public partial class MaterialTilesGrid : UserControl
    {
        public MaterialTilesGrid()
        {
            InitializeComponent();
        }

        private List<MaterialPickerTile> _Tiles = new List<MaterialPickerTile>();
        public List<MaterialPickerTile> Tiles
        {
            get => _Tiles;
            set
            {
                this.flowLayoutPanel1.Controls.Clear();
                this.flowLayoutPanel1.Controls.AddRange(value.OfType<Control>().ToArray());
            }
        }


        public event EventHandler<PixelStacker.UI.Events.CommandKeyEvent> OnCommandKey;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            IO.KonamiWatcher.ProcessKey(keyData);

            //if (keyData == Keys.Escape)
            //{
            //    this.Close();
            //    return true;
            //}

            if (OnCommandKey != null)
            {
                var evt = new PixelStacker.UI.Events.CommandKeyEvent(msg, keyData);
                OnCommandKey(this, evt);
                if (evt.IsHandled)
                {
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
