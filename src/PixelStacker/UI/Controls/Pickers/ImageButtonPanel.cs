using PixelStacker.Extensions;
using PixelStacker.IO;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace PixelStacker.UI.Controls.Pickers
{
    [ToolboxItemFilter("PixelStacker.UI.Controls.Pickers.ImageButtonPanel", ToolboxItemFilterType.Require)]
    public partial class ImageButtonPanel : UserControl
    {
        public event EventHandler<ImageButtonClickEventArgs> TileClicked;

        public Func<Message, Keys, bool> OnCommandKey
        {
            get => this.tilePanel.OnCommandKey;
            set => this.tilePanel.OnCommandKey = value;
        }
        
        public ImageButtonPanel()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public void SetImageButtonData(IEnumerable<ImageButtonData> data)
        {
            // First we REMOVE the combined element.
            //this.materialPanel.Controls.Remove(targetRef.Checkbox);
            //this.materialPanel.Controls.Remove(targetRef.TilePanel);
            this.tilePanel.ClearControlsQuick();

            //// Then we populate the sub groups
            //foreach (var tileGroup in targetRef.Tiles.GroupBy(tr => tr.Material.Category))
            //{
            //    var cRef = this.categoryRefs[tileGroup.Key];
            //    cRef.Tiles.AddRange(tileGroup);
            //    cRef.TilePanel.AddControlsQuick(tileGroup.ToArray());
            //    this.materialPanel.Controls.Add(cRef.Checkbox);
            //    this.materialPanel.Controls.Add(cRef.TilePanel);
            //}

            this.SuspendLayout();
            var btnElems = new List<ImageButton>();
            foreach(var tileData in data)
            {
                var btn = new ImageButton();
                btn.Size = new Size(80, 80);
                btn.Margin = new Padding(2);

                btn.Image = tileData.Image;
                btn.IsChecked = tileData.IsChecked;
                btn.SetTooltip(tileData.Text);
                btn.Tag = tileData;

                btn.Click += Btn_Click;
                btnElems.Add(btn);
            }

            this.tilePanel.AddControlsQuick(btnElems.ToArray());
            this.ResumeLayout();
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            if (this.TileClicked != null)
            {
                if (sender != null && sender is ImageButton ib)
                {
                    if (ib != null && ib.Tag is ImageButtonData dat)
                    {
                        this.TileClicked.Invoke(sender, new ImageButtonClickEventArgs() {
                            ImageButtonData = dat
                        });
                    }
                }
            }
        }
    }

    public class ImageButtonClickEventArgs : EventArgs
    {
        public ImageButtonData ImageButtonData { get; set; }
    }

    public class ImageButtonData {

        // This will not be directly used, and it will never be disposed.
        // Go ahead and use this directly from RESX.
        public SKBitmap Image { get; set; }
        public bool IsChecked { get; set; }
        public string Text { get; set; }
        public object Data { get; set; }
        public T GetData<T>() => (T)Data;
    }
}
