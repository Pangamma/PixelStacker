using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI.Forms
{
    public partial class Credits : Form
    {
        private Label[,] Data = new Label[2, 10];

        public Credits()
        {
            InitializeComponent();
        }

        private void Credits_Load(object sender, EventArgs e)
        {
            this.tableCredits.RowCount = 10;
            int y = 0;
            this[0, y] = "Main Developer";
            this[1, y++] = "Taylor Love";

            this[0, y] = "";
            this[1, y++] = "";

            this[0, y] = "Localizations::";
            this[1, y++] = "";

            this[0, y] = "en-US";
            this[1, y++] = "Taylor Love";

            this[0, y++] = "Some guy";
            this[0, y] = "Localizations:";
            this[0, y++] = "Some guy";
            this[0, y] = "Localizations:";
            this[0, y++] = "Some guy";

        }

        public string this[int x, int y]
        {
            get
            {
                if (y < 0) throw new ArgumentOutOfRangeException(nameof(y), y, "Y must be greater than 0.");
                if (x < 0 || x > 1) throw new ArgumentOutOfRangeException(nameof(x), x, "X must be 0 or 1.");
                if (y < Data.Length) return Data[x, y]?.Text;
                return null;
            }

            set
            {
                if (y < 0) throw new ArgumentOutOfRangeException(nameof(y), y, "Y must be greater than 0.");
                if (x < 0 || x > 1) throw new ArgumentOutOfRangeException(nameof(x), x, "X must be 0 or 1.");
                if (y >= Data.Length)
                {
                    // Resize array
                    var tmp = new Label[2, y + 1];
                    Array.Copy(Data, tmp, tmp.Length);
                    Data = null;
                    Data = tmp;
                }

                if (Data[x,y ] == null)
                {
                    var lbl = new Label();
                    System.Drawing.FontStyle fs = System.Drawing.FontStyle.Regular;

                    if (x == 0) fs |= System.Drawing.FontStyle.Bold;
                    if (value.EndsWith(":")) fs |= FontStyle.Underline;

                    lbl.Font = new System.Drawing.Font("Segoe UI", 9F, fs, System.Drawing.GraphicsUnit.Point);

                    this.tableCredits.Controls.Add(lbl, x, y);
                    Data[x, y] = lbl;
                }
                
                Data[x, y].Text = value;
            }
        }
    }
}
