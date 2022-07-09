using PixelStacker.Extensions;
using PixelStacker.IO;
using PixelStacker.Logic.Utilities;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

        public void ModifyButtons(Action<ImageButtonData, ImageButton> func)
        {
            this.SuspendLayout();

            foreach (Control control in this.tilePanel.Controls)
            {
                func(control.Tag as ImageButtonData, control as ImageButton);
            }

            this.ResumeLayout();
        }

        /// <summary>
        /// The data item will be stored in the tag field here.
        /// </summary>
        /// <returns></returns>
        public List<ImageButton> GetButtons()
        {
            List<ImageButton> items = new List<ImageButton>();
            foreach (ImageButton control in this.tilePanel.Controls)
            {
                items.Add(control);
            }
            return items;
        }

        public void DoFilterTakeOrderByOperation<TKey>(
            Func<ImageButtonData, bool> filter = null,
            Func<ImageButtonData, TKey> orderBy = null,
            int? take = null
            )
       {
            try
            {
                this.tilePanel.SuspendLayout();

                var allItems = this.GetButtons();
                allItems.ForEach(x => x.Visible = true);
#if DEBUG
                foreach (var item in allItems)
                {
                    if (item.Tag == null)
                        throw new NullReferenceException("The TAG property on the ImageButton is not set. Yikes! Fix it.");
                    if (item.Tag is not ImageButtonData)
                        throw new NullReferenceException("The TAG property on the ImageButton should be of ImageButtonData type.");
                }
#endif

                IEnumerable<ImageButton> remaining = allItems;

                if (orderBy != null)
                {
                    // Re-order everything here, if requested.
                    remaining = allItems.OrderBy(x => orderBy(x.Tag as ImageButtonData));
                    this.tilePanel.ClearControlsQuick();
                    this.tilePanel.AddControlsQuick(remaining.ToArray());
                }

                if (filter != null)
                {
                    foreach (var k in remaining)
                    {
                        if (k.Tag != null && k.Tag is ImageButtonData dat)
                        {
                            bool isVisible = filter(dat);
                            k.Visible = isVisible;
                            //if (isVisible != k.Visible)
                            //{
                            //}
                        }
                    }

                    remaining = remaining.Where(x => x.Tag != null && x.Tag is ImageButtonData dat && filter(dat));
                }

                if (take != null)
                {
                    remaining.Skip(take.Value).ToList().ForEach(x => {
                        x.Visible = false;
                    });

                    remaining = remaining.Take(take.Value);
                }
            } 
            finally
            {
                this.tilePanel.ResumeLayout();
            }
        }

        public void InitializeButtons(IEnumerable<ImageButtonData> data)
        {
            this.tilePanel.ClearControlsQuick();
            this.SuspendLayout();
            var btnElems = new List<ImageButton>();
            foreach(var tileData in data)
            {
                var btn = new ImageButton();
                btn.Size = new Size(80, 80);
                btn.Margin = new Padding(2);

                btn.Image = tileData.Image;
                toolTip1.SetToolTip(btn, tileData.Text);
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
        public string Text { get; set; }
        public object Data { get; set; }
        public T GetData<T>() => (T)Data;
    }
}
